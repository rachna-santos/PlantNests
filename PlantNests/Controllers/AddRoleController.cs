using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantNests.Models;

namespace PlantNests.Controllers
{
    public class AddRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyDbContext _context;

        public AddRoleController(RoleManager<IdentityRole> roleManager,MyDbContext context)
        {
             _roleManager = roleManager;    
            _context= context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
         
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddRole model)
        {

            IdentityRole identityRole = new IdentityRole
            {
                Name = model.RoleName,
            };
            IdentityResult result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
