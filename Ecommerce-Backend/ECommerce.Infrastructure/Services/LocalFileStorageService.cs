using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IHostEnvironment _env;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public LocalFileStorageService(IHostEnvironment env) => _env = env;

        public async Task<string> SaveFileAsync(Stream fileStream, string originalFileName, string subFolder)
        {
            if (fileStream == null || fileStream.Length == 0)
                throw new ArgumentException("File stream is empty.");

            if (fileStream.Length > MaxFileSize)
                throw new ArgumentException($"File size exceeds the maximum allowed ({MaxFileSize / 1024 / 1024} MB).");

            var ext = Path.GetExtension(originalFileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
                throw new ArgumentException($"File extension '{ext}' is not allowed. Allowed: {string.Join(", ", AllowedExtensions)}");

            // Determine the root path (wwwroot)
            var rootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
            var uploadFolder = Path.Combine(rootPath, "uploads", subFolder);
            Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var output = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(output);
            }

            // Return relative URL (e.g., /uploads/products/abc.jpg)
            return $"/uploads/{subFolder}/{fileName}";
        }

        public void DeleteFile(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl)) return;

            var rootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
            // The relative URL starts with '/' – remove it to get a relative path
            var filePath = Path.Combine(rootPath, relativeUrl.TrimStart('/'));
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}