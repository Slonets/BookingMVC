using AutoMapper;
using BusinessLogic.BookingServices;
using BusinessLogic.DTOs.User;
using BusinessLogic.Helpers;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace New.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper, UserManager<UserEntity> userManager)
        {
            _accountService = accountService;
            _mapper = mapper;
            _userManager = userManager;
        }        

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistedDto registedDto)
        {
            await _accountService.Registration(registedDto);

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            var result = await _accountService.Login(loginDto);
            if (!result)
            {
                ModelState.AddModelError("","Дані вказано не вірно!");
                return View(loginDto); //відобраємо туж саму сторінку 
            }
            // Успішній віхід перекидає на сторінку Home
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();

            return RedirectToAction(nameof(Index), "Home");
        }
    }        
}
