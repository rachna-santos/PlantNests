using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PlantNests.Migrations;
using PlantNests.Models;

namespace PlantNests.Controllers
{

    public class ProductController : Controller
    {

        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;
        public ProductController(MyDbContext context, IWebHostEnvironment hostEnvironment) 
        { 
            _context = context; 
            this.hostEnvironment = hostEnvironment; 
        }
     
        public IActionResult Index(string term = "")
        {
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();
            var cat = (from emp in _context.categories
                       where term == "" || emp.categoryName.ToLower().StartsWith(term)
                       select new Category
                       {
                           categoryId = emp.categoryId,
                           categoryName = emp.categoryName,
                           description = emp.description,
                           imagepath=emp.imagepath,
                           price= emp.price,

                       }
                        ).ToList();
                        ;

            return View(cat);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var cat = _context.categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(categorymodel model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isUnique = !_context.categories.Any(p => p.categoryName == model.categoryName);
            if (!isUnique)
            {
                ModelState.AddModelError("categoryName", "categoryName name must be unique.");
                return View(model);
            }

            var category = new Category();
            category.categoryName = model.categoryName;
            category.description = model.description;
            category.price=model.price;
            category.CreateDate = DateTime.Now;
            category.Lastmodifield = DateTime.Now;
            _context.categories.Add(category);
            _context.SaveChanges();
            var uniqueFileName = $"{category.categoryId}.jpg";
            var imageDirectory = Path.Combine(hostEnvironment.WebRootPath, "Image/Category");
            var filename = "Image/Category/" + uniqueFileName;
            var fullImagePath = Path.Combine(imageDirectory, uniqueFileName);
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            using (var stream = new FileStream(fullImagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);

            }

            category.imagepath = filename;
            _context.categories.Update(category);
            _context.SaveChanges();
            TempData["note"] = "Insert Data successfully";
            TempData["isSuccess"] = true;

            return RedirectToAction("Index", "Product", new { isSuccess = true });

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            var cat = _context.categories.Where(p => p.categoryId == id).FirstOrDefault();
            ViewBag.id = id;
            return View(cat);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category model, IFormFile file)
        {
            

            var newproductUpdate = _context.categories.FirstOrDefault(p => p.categoryId == model.categoryId);

            if (newproductUpdate != null)
            {

                if (newproductUpdate.categoryName != model.categoryName)
                {
                    var isUnique = !_context.categories.Any(p => p.categoryName == model.categoryName);
                    if (!isUnique)
                    {
                        ModelState.AddModelError("categoryName", "categoryName name must be unique");
                        return View(model);
                    }
                }
                newproductUpdate.categoryName = model.categoryName;
                newproductUpdate.description = model.description;
                newproductUpdate.description = model.description;
                newproductUpdate.CreateDate = DateTime.Now;
                newproductUpdate.Lastmodifield = DateTime.Now;

                var uniqueFileName = $"{model.categoryId}_{DateTime.Now.Ticks}.jpg";
                var imageDirectory = Path.Combine(hostEnvironment.WebRootPath, "Image/Category");
                var filename = "Image/Category/" + uniqueFileName;
                var fullImagePath = Path.Combine(imageDirectory, uniqueFileName);
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                using (var stream = new FileStream(fullImagePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                }
                newproductUpdate.imagepath = filename;
                _context.Entry(newproductUpdate).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["edit"] = "update Data successfully";
                return RedirectToAction("Index", "Category");


            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            var data = _context.categories.Find(id);
            var CurrentImage = Path.Combine(hostEnvironment.WebRootPath, data.imagepath);
            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            };
            _context.categories.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        
        public IActionResult productIndex(string term)
        {
            term = string.IsNullOrEmpty(term) ? "": term.ToLower();
            var cat = (from emp in _context.products.Include(p=>p.Category)
                       where term == "" || emp.productName.ToLower().StartsWith(term)
                       select emp).ToList();


            return View(cat);
        }
        public IActionResult ProductCreate()
        {
            ViewBag.cat = _context.categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel model, IFormFile file)
        {
            ViewBag.cat = _context.categories.ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isUnique = !_context.products.Any(p => p.productName == model.productName);
            if (!isUnique)
            {
                ModelState.AddModelError("productName", "productName name must be unique");
                return View(model);
            }
            var product = new Product();
            product.productName = model.productName;
            product.description = model.description;
            product.price = model.price;
            product.categoryId = model.categoryId;
             product.Lastmodifield = DateTime.Now;
            product.CreateDate = DateTime.Now;
            _context.products.Add(product);
            _context.SaveChanges();

            var uniqueFileName = $"{product.productId}.jpg";
            var imageDirectory = Path.Combine(hostEnvironment.WebRootPath, "Image/ProductLogo");
            var filename = "Image/ProductLogo/" + uniqueFileName;
            var fullImagePath = Path.Combine(imageDirectory, uniqueFileName);
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            using (var stream = new FileStream(fullImagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            product.productimage = filename;
            _context.products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("productIndex", "Product");

        }

        [HttpGet]
        public IActionResult ProductEdit(int id)
        {
            ViewBag.cat = _context.categories.ToList();
            var product = _context.products.Where(p => p.productId == id).FirstOrDefault();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(Product model,int productId, IFormFile file)
        {
            ViewBag.cat = _context.categories.ToList();

            try
            {
                var product = _context.products.FirstOrDefault(p => p.productId == model.productId);

                if (product != null)
                {
                    product.productName = model.productName;
                    product.categoryId = model.categoryId;
                    product.description = model.description;
                    product.price = model.price;
                    product.Lastmodifield = DateTime.Now;
                    product.CreateDate = DateTime.Now;

                    var uniqueFileName = $"{model.productId}_{DateTime.Now.Ticks}.jpg";
                    var imageDirectory = Path.Combine(hostEnvironment.WebRootPath, "Image/ProductLogo");
                    var filename = "Image/ProductLogo/" + uniqueFileName;
                    var fullImagePath = Path.Combine(imageDirectory, uniqueFileName);

                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    using (var stream = new FileStream(fullImagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    product.productimage = filename;
                    _context.Entry(product).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return RedirectToAction("productIndex", "Product");
            }
            catch (Exception ex)
            {
                // Log the error or handle it accordingly
                Console.WriteLine($"Error occurred: {ex.Message}");
                // Optionally return an error view or message to the user
                return View("Error"); // Replace with an appropriate error handling approach
            }


        }
        public IActionResult OrderList(string term = "")
        {
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();
            var cat = (from emp in _context.orders.Include(p => p.Product).Include(p => p.Customer)
                       where term == "" || emp.Product.productName.ToLower().StartsWith(term)
                       select emp).ToList();

            //var order = _context.orders.Include(p=>p.Customer).Include(p=>p.Product).ToList();
            return View(cat);
        }
        [HttpPost]
        public IActionResult Orderview(int id)
        {
            var order = _context.orders.FirstOrDefault(p=>p.OrderId==id);
                order.Status = "Accepted";
            _context.orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("OrderList","Product");
        }
        public IActionResult deliver(int id)
        {
            var order = _context.orders.FirstOrDefault(p => p.OrderId == id);
            order.Status = "Deliverd";
            _context.orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("OrderList", "Product");
        }

        public IActionResult reviewlist(string term = "")
        {
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();
            var cat = (from emp in _context.ratings.Include(p => p.Product).Include(p => p.Customer)
            where term == "" || emp.Product.productName.ToLower().StartsWith(term)
                       select emp).ToList();


            return View(cat);
        }
        public IActionResult Inventory(string term = "")
        {
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();
            var cat = (from emp in _context.inventories.Include(p => p.Product).Include(p => p.Customer).Include(p=>p.Order)
                       where term == "" || emp.Product.productName.ToLower().StartsWith(term)
                       select emp).ToList();


            return View(cat);
        }
        public IActionResult SoldProduct(string term = "")
        {
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();
            var cat = (from emp in _context.soldProducts.Include(p => p.Product).Include(p=>p.Order)
                       where term == "" || emp.Product.productName.ToLower().StartsWith(term)
                       select emp).ToList();


            return View(cat);
            //var sold = _context.soldProducts.Include(p=>p.Product).Include(p=>p.Order).ToList();
            //return View(sold);
        }
        
        [HttpPost]
        public IActionResult OrderCancelled()
        {
            var user = HttpContext.Session.GetString("id");

            if (user != null)
            {
                int id = int.Parse(user);
                var order = _context.orders.Include(p=>p.Customer).FirstOrDefault(p => p.Customer.Id==id);
                order.Status = "Cancelled";
                _context.orders.Update(order);
                _context.SaveChanges();
            }
            return RedirectToAction("Home", "Client");

        }
    }
}
