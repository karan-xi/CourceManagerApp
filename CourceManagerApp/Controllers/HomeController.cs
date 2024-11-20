using CourceManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourceManagerApp.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            string firstVisit = Request.Cookies["FirstVisit"];

            if (string.IsNullOrEmpty(firstVisit))
            {
                // If the cookie doesn't exist, set it with the current timestamp
                firstVisit = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                Response.Cookies.Append("FirstVisit", firstVisit, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMonths(12) // Cookie valid for 1 year
                });

                ViewBag.Message = "Welcome to your first visit!";
            }
            else
            {
                ViewBag.Message = $"You first started using the app on {firstVisit}.";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
