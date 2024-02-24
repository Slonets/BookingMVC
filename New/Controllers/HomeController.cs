using BusinessLogic.DTOs;
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
        [HttpGet]
        public async Task< IActionResult> Index()
        {
            return View(await _building.GetAll());
        }
        [HttpGet]
        public async Task<IActionResult> DetailCard(int id)
        {
            return View(await _building.GetId(id));
        }

        [HttpGet]
        public async Task<IActionResult> AllInformation()
        {
            return View(await _building.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> AllFlat()
        {
            return View(await _building.AllFlat());
        }

        [HttpGet]
        public async Task<IActionResult> AllHouse()
        {
            return View(await _building.AllHouse());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BuildingDto buildingDto, List<IFormFile> Image)
        {
            var r = this.Request;

            await _building.Create(buildingDto);

            return RedirectToAction(nameof(Index), "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
