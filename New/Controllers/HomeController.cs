using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using New.Models;
using System.Diagnostics;

namespace New.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBuilding _building;

        public HomeController(IBuilding building)
        {
            _building = building;
        }

        public async Task< IActionResult> Index()
        {
            return View(await _building.GetAll());
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
