using BusinessLogic.DTOs;
using BusinessLogic.DTOs.User;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace New.Controllers
{
    public class AdminController : Controller
    {
        public readonly IAdmin _admin;
        public AdminController(IAdmin admin) 
        {
            _admin=admin;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _admin.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            await _admin.Delete(id);

            return RedirectToAction(nameof(Index), "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var seach = await _admin.GetId(id);

            //якщо знайдено, то надсилаємо його на View
            if (seach != null)
            {                
                return View(seach);
            }

            //якщо не знайдено, то помилка
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDto dto)
        {
            await _admin.Edit(dto);

            //після виправлення переходить на метод індекс контролера
            return RedirectToAction(nameof(Index), "Admin");
        }
    }
}
