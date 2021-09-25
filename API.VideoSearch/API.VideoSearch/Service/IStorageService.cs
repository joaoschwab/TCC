using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.VideoSearch.Service
{
    public interface IStorageService
    {
        void Upload(IFormFile formfile);

        Task<byte[]> Read(string filename);
    }
}
