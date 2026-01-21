using System.Diagnostics;
using CongratulatorWeb.Data;
using CongratulatorWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CongratulatorWeb.Controllers
{
    public class HomeController(ILogger<HomeController> logger, AppDbContext context) : Controller
    {
        public IActionResult Index()
        {
            var today = DateTime.Today;
            var allPeople = context.People.ToList();
            var upcomingBirthdays = allPeople
                .Where(p => p.NextBirthday >= today && p.NextBirthday <= today.AddDays(30))
                .OrderBy(p => p.NextBirthday)
                .ToList();

            ViewBag.Today = today;

            return View(upcomingBirthdays);
        }
    }
}
