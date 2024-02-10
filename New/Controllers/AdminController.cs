using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}
