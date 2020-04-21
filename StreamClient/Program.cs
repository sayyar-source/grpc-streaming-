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
            await StreamFile(client, @"C:\Users\istech\Pictures\buf.jpeg");
            Console.WriteLine("Done!");
            Console.ReadLine();

        }
        private static async Task StreamFile(FilesClient client, string filePath)
        {

            using Stream source = File.OpenRead(filePath);
            using var call = client.SendFileStream();

            byte[] buffer = new byte[2048];
            int bytesRead;

            int c = 0;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {

                await call.RequestStream.WriteAsync(new Chunk { Content = Google.Protobuf.ByteString.CopyFrom(buffer) });

                await Task.Delay(100);
                Console.WriteLine(c++);
            }

            await call.RequestStream.CompleteAsync();
        }
    }
    
}
