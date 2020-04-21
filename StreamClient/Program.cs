using Google.Protobuf;
using Grpc.Net.Client;
using GrpcServiceStream;
using System;
using System.IO;
using System.Threading.Tasks;
using static GrpcServiceStream.Files;

namespace StreamClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

          
            var client = new Files.FilesClient(channel);

            Console.WriteLine("Sending");
            await SendFile(client, @"D:\protocol_buffers.png");
            Console.WriteLine("Done!");
            Console.ReadLine();

        }
        private static async Task SendFile(FilesClient client, string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;
                buffer = new byte[length];
                int count;
                int sum = 0;

                //System.IO.File.ReadAllBytes()

                while ((count = await fileStream.ReadAsync(buffer, sum, length - sum)) > 0)
                    sum += count;
            }
            finally
            {
                fileStream.Close();
            }

            var result = await client.SendFileAsync(new Chunk
            {
                Content = ByteString.CopyFrom(buffer)
            });

            Console.WriteLine(result.Success);

        }
    }
}
