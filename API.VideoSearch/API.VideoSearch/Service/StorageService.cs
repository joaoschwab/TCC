using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.VideoSearch.Service
{
    public class StorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        public StorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public void Upload(IFormFile formfile)
        {
            var containerName = _configuration.GetSection("Storage:ContainerName").Value;

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(formfile.FileName);

            using var stream = formfile.OpenReadStream();
            blobClient.Upload(stream, true);
            
        }

        public async Task<byte[]> Read(string filename)
        {
            var containerName = _configuration.GetSection("Storage:ContainerName").Value;
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(filename);

            var fileDownload =  await blobClient.DownloadAsync();
            using(MemoryStream ms = new MemoryStream())
            {
                await fileDownload.Value.Content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
