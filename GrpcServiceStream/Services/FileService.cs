using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceStream.Services
{
    public class FileService:Files.FilesBase
    {
        private readonly ILogger<FileService> _logger;
        private readonly IWebHostEnvironment _webenv;
        public FileService(ILogger<FileService> logger, IWebHostEnvironment webenv)
        {
            _logger = logger;
            _webenv = webenv;
        }
        public override async Task<SendResult> SendFile(Chunk request, ServerCallContext context)
        {
            var content = request.Content.ToArray();
            await System.IO.File.WriteAllBytesAsync(_webenv.ContentRootPath + "/Files/" + "pic1.jpg", content);
            return new SendResult { Success = true };
        }
    }
}
