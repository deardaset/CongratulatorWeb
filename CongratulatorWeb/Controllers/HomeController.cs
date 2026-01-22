using System.Diagnostics;
using System.Threading.Tasks;
using CongratulatorWeb.Data;
using CongratulatorWeb.Interfaces;
using CongratulatorWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CongratulatorWeb.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IPersonRepository repository) : Controller
    {
        public async Task<IActionResult> Index(string? sortBy)
        {
            var today = DateTime.Today;
            var upcomingBirthdays = await repository.UpcomingBirthdaysAsync(today, sortBy);
            ViewBag.SortBy = sortBy;
            ViewBag.Today = today;

            return View(upcomingBirthdays);
        }
    }
}
