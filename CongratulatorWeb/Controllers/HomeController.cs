using System.Diagnostics;
using CongratulatorWeb.Data;
using CongratulatorWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CongratulatorWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;
            var allPeople = _appDbContext.People.ToList();
            var upcomingBirthdays = allPeople
                .Where(p => p.NextBirthday >= today && p.NextBirthday <= today.AddDays(60))
                .OrderBy(p => p.NextBirthday)
                .ToList();

            ViewBag.Today = today;

            return View(upcomingBirthdays);
        }
    }
}
