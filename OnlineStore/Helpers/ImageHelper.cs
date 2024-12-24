using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;

namespace OnlineStore.Helpers
{
    public class ImageHelper : IImageHelper // Generic image helper.
    {
        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            string guid = Guid.NewGuid().ToString(); // Globally unique identifier string.
            string fileName = $"{guid}.jpg"; // Prevent file name repetition.

            string path = Path.Combine(
                Directory.GetCurrentDirectory(), // Application directory.
                $"wwwroot\\images\\{folder}", // File directory within application.
                fileName // File name.
            );

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream); // Save file to path.
            }

            return $"~/images/{folder}/{fileName}"; // File path to save to database.
        }
    }
}
