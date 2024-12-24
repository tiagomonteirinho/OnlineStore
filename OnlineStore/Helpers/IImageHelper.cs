using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OnlineStore.Helpers
{
    public interface IImageHelper // Add helper interfaces to services at Startup.cs to be instantiated.
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
