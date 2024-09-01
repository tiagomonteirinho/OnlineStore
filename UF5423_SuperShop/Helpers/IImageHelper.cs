using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UF5423_SuperShop.Helpers
{
    public interface IImageHelper // Add helper interfaces to services at Startup.cs to be instantiated.
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
