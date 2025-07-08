using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Projects()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetResume()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "CV Alfie Siddons.pdf");

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var fileName = Path.GetFileName(filePath);
                return File(fileBytes, "application/pdf", fileName);
            }

            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
