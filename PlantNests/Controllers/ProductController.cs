using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PlantNests.Migrations;
using PlantNests.Models;
using System.Net.Mail;
using System.Net;
using NuGet.Versioning;

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
                           imagepath = emp.imagepath,
                           price = emp.price,

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
            category.price = model.price;
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
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();
            var cat = (from emp in _context.products.Include(p => p.Category)
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

            Inventory inventory = new Inventory();
            {
                inventory.productId = product.productId;
                inventory.Quantity = 4;
                inventory.CreateDate = DateTime.Now;
                inventory.Lastmodifield = DateTime.Now;
            }
            _context.inventories.Add(inventory);
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
        public async Task<IActionResult> ProductEdit(Product model, int productId, IFormFile file)
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

                    var uniqueFileName = $"{model.productId}.jpg";
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

                var inventory = _context.inventories.Where(p => p.productId == model.productId).FirstOrDefault();
                inventory.productId = model.productId;
                inventory.Quantity = 2;
                inventory.CreateDate = DateTime.Now;
                inventory.Lastmodifield = DateTime.Now;
                _context.inventories.Update(inventory);
                _context.SaveChanges();
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
                       where term == "" || emp.ordernumber.ToLower().StartsWith(term)
                       select emp).ToList();

            //var order = _context.orders.Include(p=>p.Customer).Include(p=>p.Product).ToList();
            return View(cat);
        }
        [HttpPost]
        public IActionResult Orderview(int id)
        {

            var order = _context.orders.FirstOrDefault(p => p.OrderId == id);
            order.Status = "Accepted";
            _context.orders.Update(order);
            _context.SaveChanges();
            OrderSendAcceptEmail(id);
            return RedirectToAction("OrderList", "Product");
        }
        public IActionResult deliver(int id)
        {
            var order = _context.orders.FirstOrDefault(p => p.OrderId == id);
            order.Status = "Deliverd";
            _context.orders.Update(order);
            _context.SaveChanges();
            UpdateInventoryFromOrders();
            OrderSendDeliverdEmail(id);
            return RedirectToAction("OrderList", "Product");
        }
        [HttpPost]
        public IActionResult OrderCancelled(int id)
        {
            var Payment = _context.payments.FirstOrDefault(p => p.OrderId == id);
            var order = _context.orders.FirstOrDefault(p => p.OrderId == id);
            order.Status = "Cancelled";  
            _context.orders.Update(order);
            _context.SaveChanges();
            Payment.PaymentStatus = "Refund";
            _context.payments.Update(Payment);
            _context.SaveChanges();
            OrderCancelledEmail(id);
            OrderCancelldUpdateInventory(id);
            return RedirectToAction("Home", "Client");

        }
        //ordercancelldAdmin
        public void OrderCancelledEmail(int id)
        {
            ViewBag.cart = _context.orders.Where(p => p.OrderId == id).ToList();

            var custs = HttpContext.Session.GetString("id");
            var user = int.Parse(custs);
            var cust = _context.customers.FirstOrDefault(p => p.Id == user);

            string emailBody = $"Dear {cust.Name},\n\n <br/>";
            emailBody += $"{cust.Email},\n\n <br/>";
            emailBody += $"{cust.address},\n\n <br/>";
            emailBody += $"{cust.Number},\n\n <br/>";

            emailBody += "I your order cancelled Order:\n\n <br/>";

            foreach (var item in ViewBag.cart)
            {
                emailBody += $"OderNumber:{item.ordernumber}:\n\n <br/>";
                emailBody += $"Quantity:{item.quantity},Total:{item.totalamount},Status{item.Status},\n\n <br/>";
            }

            emailBody += "\n\nRegards,\nYour Company";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("email@gmail.com");
                mail.To.Add(cust.Email);
                mail.Subject = "Order Cancelled";
                mail.Body = emailBody;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential credential = new NetworkCredential(cust.Email, "slgdcoyxolrrusmt");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Port = 587;
                    smtp.Send(mail);
                }

            }

        }

        //ordercancelledsendemailAdminfrontsearahahaiid
        public void OrderCancelledEmailAdmin(int orderid)
        {
            ViewBag.cart = _context.orders.Where(p => p.OrderId == orderid).ToList();

            var custs = HttpContext.Session.GetString("id");
            var user = int.Parse(custs);
            var cust = _context.customers.FirstOrDefault(p => p.Id == user);

            string emailBody = $"Dear {cust.Name},\n\n <br/>";
            emailBody += $"{cust.Email},\n\n <br/>";
            emailBody += $"{cust.address},\n\n <br/>";
            emailBody += $"{cust.Number},\n\n <br/>";

            emailBody += "I Want to cancelled Order:\n\n <br/>";

            foreach (var item in ViewBag.cart)
            {
                emailBody += $"OderNumber:{item.ordernumber}:\n\n <br/>";
                emailBody += $"Quantity:{item.quantity},Total:{item.totalamount},Status{item.Status},\n\n <br/>";
            }

            emailBody += "\n\nRegards,\nYour Company";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("email@gmail.com");
                mail.To.Add(cust.Email);
                mail.Subject = "Order Cancelled from Admin";
                mail.Body = emailBody;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential credential = new NetworkCredential(cust.Email, "slgdcoyxolrrusmt");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Port = 587;
                    smtp.Send(mail);
                }
               RedirectToAction("Home", "Client");
            }
            
        }
        public void UpdateInventoryFromOrders()
        {
            var orders = _context.orders.ToList();
            foreach (var order in orders)
            {
                var inventory = _context.inventories.FirstOrDefault(p => p.productId == order.productId);
                if (inventory != null && inventory.Quantity >= order.quantity)
                {
                    inventory.Quantity -= order.quantity;

                    if (inventory.Quantity < 0)
                    {
                        inventory.Quantity = 0;
                    }

                    _context.SaveChanges();
                }
            }
        }
        public void OrderCancelldUpdateInventory(int id)
        {
            var order = _context.orders.Where(p => p.OrderId == id).ToList();
            //var orders = _context.orders.ToList();
            foreach (var orders in order)
            {
                var inventory = _context.inventories.FirstOrDefault(p => p.productId == orders.productId);
                if (inventory != null && inventory.Quantity <= orders.quantity)
                {
                    inventory.Quantity += orders.quantity;

                    //if (inventory.Quantity > 1)
                    //{
                    //    inventory.Quantity = 1;
                    //}

                    _context.SaveChanges();
                }
            }
        }
        public void OrderSendAcceptEmail(int id)
        {
            ViewBag.cart = _context.orders.Where(p => p.OrderId == id).ToList();

            var custs = HttpContext.Session.GetString("id");
            var user = int.Parse(custs);
            var cust = _context.customers.FirstOrDefault(p => p.Id == user);

            string emailBody = $"Dear {cust.Name},\n\n";
            emailBody += $"{cust.Email},\n\n";
            emailBody += $"{cust.address},\n\n";
            emailBody += $"{cust.Number},\n\n";

            emailBody += "Below are the details of your order:\n\n";

            foreach (var item in ViewBag.cart)
            {
                emailBody += $"Quantity:{item.quantity},Total:{item.totalamount},Status:{item.Status},\n\n";
            }

            emailBody += "\n\nRegards,\nYour Company";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("email@gmail.com");
                mail.To.Add(cust.Email);
                mail.Subject = "Order Accepted";
                mail.Body = emailBody;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential credential = new NetworkCredential(cust.Email, "slgdcoyxolrrusmt");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Port = 587;
                    smtp.Send(mail);
                }

            }

        }
        public void OrderSendDeliverdEmail(int id)
        {
            ViewBag.cart = _context.orders.Where(p => p.OrderId == id).ToList();

            var custs = HttpContext.Session.GetString("id");
            var user = int.Parse(custs);
            var cust = _context.customers.FirstOrDefault(p => p.Id == user);

            string emailBody = $"Dear {cust.Name},\n\n <br/>";
            emailBody += $"{cust.Email},\n\n <br/>";
            emailBody += $"{cust.address},\n\n <br/>";
            emailBody += $"{cust.Number},\n\n <br/>";

            emailBody += "Below are the details of your order:\n\n <br/>";

            foreach (var item in ViewBag.cart)
            {
                emailBody += $"Quantity:{item.quantity},Total:{item.totalamount},Status{item.Status},\n\n <br/>";
            }

            emailBody += "\n\nRegards,\nYour Company";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("email@gmail.com");
                mail.To.Add(cust.Email);
                mail.Subject = "Order Deliverd";
                mail.Body = emailBody;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential credential = new NetworkCredential(cust.Email, "slgdcoyxolrrusmt");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Port = 587;
                    smtp.Send(mail);
                }

            }

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
            var cat = (from emp in _context.inventories.Include(p => p.Product)
                       where term == "" || emp.Product.productName.ToLower().StartsWith(term)
                       select emp).ToList();


            return View(cat);
        }

    }

}
