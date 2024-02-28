using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Data;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using New.Models;
using System.Diagnostics;

namespace New.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBuilding _building;
        private readonly UserManager<UserEntity> _userManager;
        private readonly BookingDbContext _dataContext;

        public HomeController( IBuilding building, UserManager<UserEntity> userManager, BookingDbContext dataContext)
        {
            _building = building;
            _userManager = userManager; 
            _dataContext = dataContext;
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
            var viewOfTheHouse = _dataContext.ViewOfTheHouse
                .Select(x => new { Value = x.Id, Text = x.Name })
                .ToList();

            var typeOfSale = _dataContext.TypeOfSale
               .Select(x => new { Value = x.Id, Text = x.Name })
               .ToList();          



            var model = new BuildingCreateDto
            {
                ViewOfTheHouseList = new SelectList(viewOfTheHouse, "Value", "Text"),
                TypeOfSaleList = new SelectList(typeOfSale, "Value", "Text")                
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BuildingCreateDto create, List<IFormFile> Image)
        {
            var r = this.Request;

            await _building.Create(create);

            return RedirectToAction(nameof(Index), "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> MyAdvertisement(int id)
        {
            var value = _building.MyAdvertisement(id);
            return View(value);
        }
    }
}
