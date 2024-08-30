using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantNests.Models;
using System.Security.Claims;

namespace PlantNests.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyDbContext _context;


        public AdminController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager,MyDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager; 
            _context=context;

        }
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();

            var registrations = users.Select(user => new Registration
            {
                UserName = user.UserName,
                Email = user.Email,
                ContactNumber = user.ContactNumber,
                Password=user.PasswordHash,
                // Map other properties as needed
            }).ToList();

            return View(registrations);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roles = _roleManager.Roles.Select(r => new AddRole
            {
                //RoleId = r.Id,
                RoleName = r.Name
            }).ToList();
            ViewBag.roles = roles;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Registration model)
        {
            if (!ModelState.IsValid)
            {
                var role = _roleManager.Roles.Select(r => new AddRole
                {
                    RoleName = r.Name
                }).ToList();
                ViewBag.roles = role;
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                ContactNumber = model.ContactNumber,
                PasswordHash = model.Password,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
                if (roleExists)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid role name.");
                }
            }

            var roles = _roleManager.Roles.Select(r => new AddRole
            {
                RoleName = r.Name
            }).ToList();
            ViewBag.roles = roles;

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login userModel)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userModel.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, userModel.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user); // Get user role

                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role)); // Add role claims
                    }

                    var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password");
                    return View();
                }
            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Admin");
        }
    }
}
