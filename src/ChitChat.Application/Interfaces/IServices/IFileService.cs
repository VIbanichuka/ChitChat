using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile image);
        void RemoveImage(string imageUrl);
        Task<string> UpdateImageAsync(IFormFile image, string imageUrl);
    }
}
