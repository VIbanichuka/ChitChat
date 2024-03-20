using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Application.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            CheckIfImageFileIsNull(image);
            var fileName = await CreateImage(image);
            return fileName;
        }

        public void RemoveImage(string imageUrl)
        {
            CheckImageUrlIsNullOrWhiteSpace(imageUrl);
            DeleteImageIfExists(imageUrl);
        }

        public async Task<string> UpdateImageAsync(IFormFile image, string imageUrl)
        { 
            CheckImageUrlIsNullOrWhiteSpace(imageUrl);
            DeleteImageIfExists(imageUrl);
            CheckIfImageFileIsNull(image);
            var fileName = await CreateImage(image);
            return fileName;
        }

        private void CheckIfImageFileIsNull(IFormFile image) 
        {
            if (image.FileName == null)
            {
                throw new ArgumentNullException(nameof(image), "You haven't included a file");
            }
        }

        private void CheckImageUrlIsNullOrWhiteSpace(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException(nameof(imageUrl), "This file doesn't exist");
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString()
                + Path.GetExtension(fileName);
        }

        private string GetImagePath(string fileName)
        {
            var path = @"ChitChat\src\ChitChat.App\chitchat.app.client\src\assets\";
            return Path.Combine(path, fileName);
        }

        private void DeleteImageIfExists(string imageUrl)
        {
            var imagePath = GetImagePath(imageUrl);

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        private async Task<string> CreateImage(IFormFile image)
        {
            var fileName = GetUniqueFileName(image.FileName);

            var path = GetImagePath(fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}
