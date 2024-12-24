using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")] // Define action view for specific error code.
        public IActionResult Error404() // (404 Not Found)
        {
            return View();
        }
    }
}
