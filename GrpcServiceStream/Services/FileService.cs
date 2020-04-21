using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceStream.Services
{
    public class FileService : Files.FilesBase
    {
        private readonly ILogger<FileService> _logger;
        private readonly IWebHostEnvironment _webenv;
        public FileService(ILogger<FileService> logger, IWebHostEnvironment webenv)
        {
            _logger = logger;
            _webenv = webenv;
        }
        public override async Task<SendResult> SendFileStream(IAsyncStreamReader<Chunk> requestStream, ServerCallContext context)
        {
            var fileName = _webenv.ContentRootPath + "/Files/" + "proto.jpg";
            using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            int c = 0;

            try
            {
                await foreach (var chunk in requestStream.ReadAllAsync())
                {

                    fs.Write(chunk.Content.ToArray(), 0, chunk.Content.Length);

                    await Task.Delay(200);
                    Console.WriteLine(c++);
                }
            }
            finally
            {

                fs.Close();
            }


            return new SendResult { Success = true };
        }
    }
}