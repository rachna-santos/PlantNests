using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using PlantNests.Migrations;
using PlantNests.Models;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace PlantNests.Controllers
{
    public class ClientController : Controller
    {
        private readonly MyDbContext _context;
        public ClientController(MyDbContext context)
        {
            _context = context;
        }
       public IActionResult Category(int[] category, int min, int max, string term="", string sort = "")
        {
            var user = HttpContext.Session.GetString("id");

            if (user != null)
            {
                int id = int.Parse(user);
                var users = _context.customers.FirstOrDefault(p => p.Id == id);
                var Name = users.Email;
                ViewBag.UserName = Name;
            }

            List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
            int cartItemCount = cartItems.Sum(item => item.qty);
            ViewBag.count = cartItemCount;
            //pricerangeslider
            var lowprice = _context.products.Min(p=>p.price);
            ViewBag.prce = lowprice;
            var highprice = _context.products.Max(p => p.price);
            ViewBag.prces = highprice;
            //groupcategory

            var countcategory = _context.products.GroupBy(p => p.categoryId).Select(g => new { catId = g.Key, Count = g.Count() })
             .Join(_context.categories,
             pv => pv.catId,
             p=>p.categoryId,
             (pv,p) => new { catId =pv.catId,catName=p.categoryName, Count = pv.Count}).ToList();

            ViewBag.groupcategory = countcategory;


            var totalcat = _context.categories.Count();
            ViewBag.total = totalcat;

            var products = _context.products.AsQueryable();
      

            // Apply sorting based on the selected option
            switch (sort)
            {
                case "new":
                    products = products.OrderByDescending(p => p.productName);
                    break;
                case "az":
                    products = products.OrderBy(p => p.productName);
                    break;
                case "za":
                    products = products.OrderByDescending(p => p.productName);
                    break;
                case "lowhigh":
                    products = products.OrderBy(p => p.price);
                    break;
                case "highlow":
                    products = products.OrderByDescending(p => p.price);
                    break;
                default:
                    break;
            }
            var selectprice = _context.products.Where(p => p.price >= min && p.price <= max).ToList();

            var product = _context.products.ToList();
            var productQuantities = new Dictionary<int, int>();

            foreach (var item in product)
            {
                var quantity = _context.inventories
                    .Where(p => p.productId == item.productId)
                    .Sum(p => p.Quantity);

                productQuantities[item.productId] = quantity;
            }

            ViewBag.ProductQuantities = productQuantities;
            return View(product);

            }
       [HttpGet]
       public async Task<IActionResult> CategoryFilter(int[] categories, int min, int max, string term = "",string sort="")
            {
                var user = HttpContext.Session.GetString("id");

                 if (user != null)
                 {
                    int id = int.Parse(user);
                    var users = _context.customers.FirstOrDefault(p => p.Id == id);
                    var Name = users.Email;
                    ViewBag.UserName = Name;
                 }

                List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
                int cartItemCount = cartItems.Sum(item => item.qty);
                ViewBag.count = cartItemCount;

                var lowprice = _context.products.Min(p => p.price);
                ViewBag.prce = lowprice;
                var highprice = _context.products.Max(p => p.price);
                ViewBag.prces = highprice;

                //groupcategory
                var countcategory = _context.products.GroupBy(p => p.categoryId).Select(g => new { catId = g.Key, Count = g.Count() })
                 .Join(_context.categories,
                 pv => pv.catId,
                 p => p.categoryId,
                 (pv, p) => new { catId = pv.catId, catName = p.categoryName, Count = pv.Count }).ToList();
                ViewBag.groupcategory = countcategory;

                //sortdata
                var products = _context.products.AsQueryable();

                if (categories.Length > 0)
                {
                    products = products.Where(p => categories.Contains(p.categoryId));
                }

                // Apply sorting based on the selected option
                switch (sort)
                {
                    case "new":
                        products = products.OrderByDescending(p => p.productName);
                        break;
                    case "az":
                        products = products.OrderBy(p => p.productName);
                        break;
                    case "za":
                        products = products.OrderByDescending(p => p.productName);
                        break;
                    case "lowhigh":
                        products = products.OrderBy(p => p.price);
                        break;
                    case "highlow":
                        products = products.OrderByDescending(p => p.price);
                        break;
                    default:
                        break;
                }
                var product = await _context.products.Where(p => categories.Contains(p.categoryId)).ToListAsync();

                //setquantity
              
                var productQuantities = new Dictionary<int, int>();

                foreach (var item in product)
                {
                    var quantity = _context.inventories
                        .Where(p => p.productId == item.productId)
                        .Sum(p => p.Quantity);

                    productQuantities[item.productId] = quantity;
                }

                ViewBag.ProductQuantities = productQuantities;
                return View(product);

            }
       [HttpGet]
       public async Task<IActionResult> AssendingVlaue(int[] categories, int min, int max, string term = "", string sort = "")
                {
                    var user = HttpContext.Session.GetString("id");

                    if (user != null)
                    {
                        int id = int.Parse(user);
                        var users = _context.customers.FirstOrDefault(p => p.Id == id);
                        var Name = users.Email;
                        ViewBag.UserName = Name;
                    }

                    List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
                    int cartItemCount = cartItems.Sum(item => item.qty);
                     ViewBag.count = cartItemCount;
                    //pricerangeslider
                        var lowprice = _context.products.Min(p => p.price);
                        ViewBag.prce = lowprice;
                        var highprice = _context.products.Max(p => p.price);
                        ViewBag.prces = highprice;

                //groupcategory
                var countcategory = _context.products.GroupBy(p => p.categoryId).Select(g => new { catId = g.Key, Count = g.Count() })
                     .Join(_context.categories,
                     pv => pv.catId,
                     p => p.categoryId,
                     (pv, p) => new { catId = pv.catId, catName = p.categoryName, Count = pv.Count }).ToList();
                    ViewBag.groupcategory = countcategory;

                //sortdata
                    var products = _context.products.AsQueryable();

                    switch (sort)
                    {
                        case "new":
                            products = products.OrderByDescending(p => p.productName);
                            break;
                        case "az":
                            products = products.OrderBy(p => p.productName);
                            break;
                        case "za":
                            products = products.OrderByDescending(p => p.productName);
                            break;
                        case "lowhigh":
                            products = products.OrderBy(p => p.price);
                            break;
                        case "highlow":
                            products = products.OrderByDescending(p => p.price);
                            break;
                        default:
                            break;
                    }
                    var productList = await products.ToListAsync();
                    ViewBag.product = productList;
                    var selectprice = _context.products.Where(p => p.price >= min && p.price <= max).ToList();

                    var productQuantities = new Dictionary<int, int>();

                    foreach (var item in productList)
                    {
                        var quantity = _context.inventories
                            .Where(p => p.productId == item.productId)
                            .Sum(p => p.Quantity);

                        productQuantities[item.productId] = quantity;
                    }

                    ViewBag.ProductQuantities = productQuantities;
                    return View();
                }
       [HttpGet]
       public async Task<IActionResult> pricefilter(int[] category, int min, int max, string term = "", string sort = "")
       {
                var user = HttpContext.Session.GetString("id");

                if (user != null)
                {
                    int id = int.Parse(user);
                    var users = _context.customers.FirstOrDefault(p => p.Id == id);
                    var Name = users.Email;
                    ViewBag.UserName = Name;
                }

                List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
                int cartItemCount = cartItems.Sum(item => item.qty);
                ViewBag.count = cartItemCount;
                //pricerangeslider
                var lowprice = _context.products.Min(p => p.price);
                ViewBag.prce = lowprice;
                var highprice = _context.products.Max(p => p.price);
                ViewBag.prces = highprice;

                //groupcategory
                 var countcategory = _context.products.GroupBy(p => p.categoryId).Select(g => new { catId = g.Key, Count = g.Count() })
                 .Join(_context.categories,
                 pv => pv.catId,
                 p => p.categoryId,
                 (pv, p) => new { catId = pv.catId, catName = p.categoryName, Count = pv.Count }).ToList();
                ViewBag.groupcategory = countcategory;

            //sortdata
            var products = _context.products.AsQueryable();

            switch (sort)
            {
                case "new":
                    products = products.OrderByDescending(p => p.productName);
                    break;
                case "az":
                    products = products.OrderBy(p => p.productName);
                    break;
                case "za":
                    products = products.OrderByDescending(p => p.productName);
                    break;
                case "lowhigh":
                    products = products.OrderBy(p => p.price);
                    break;
                case "highlow":
                    products = products.OrderByDescending(p => p.price);
                    break;
                default:
                    break;
            }
                var productList = await products.ToListAsync();
                ViewBag.product = productList;
                var selectprice=_context.products.Where(p=>p.price>=min && p.price<=max).ToList();
                return View(selectprice);
        }
        [HttpGet]
        public IActionResult Home(string term="")
        {
            // Retrieve the user session ID
            var userIdString = HttpContext.Session.GetString("id");

            if (!string.IsNullOrEmpty(userIdString))
            {
                if (int.TryParse(userIdString, out int userId))
                {
                    var user = _context.customers.FirstOrDefault(p => p.Id == userId);
                    if (user != null)
                    {
                        ViewBag.UserName = user.Email;
                    }
                }
            }


            // Retrieve the shopping cart from the session
            List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
            int cartItemCount = cartItems.Sum(item => item.qty);
            ViewBag.count = cartItemCount;

            // Normalize the search term
            term= string.IsNullOrEmpty(term)?"":term.Trim().ToLower();

            // Query products based on the search term
            var products = _context.products
                                   .Include(p => p.Category)
                                   .Where(p => string.IsNullOrEmpty(term) || p.productName.ToLower().StartsWith(term))
                                   .ToList();
            var productQuantities = new Dictionary<int, int>();

            foreach (var product in products)
            {
                var quantity = _context.inventories
                    .Where(p => p.productId == product.productId)
                    .Sum(p => p.Quantity);

                productQuantities[product.productId] = quantity;
            }

            ViewBag.ProductQuantities = productQuantities;

            ViewBag.ordershow = _context.orders.ToList();

            return View(products);
        

        }
         [HttpPost]
        public IActionResult Home(int productid,int price, int qty,string image)
        {
            var value = HttpContext.Session.GetString("id");
            if (value == null)
            {
                return RedirectToAction("custLogin", "Client");
            }

            if (value != null)
            {
                int ids = int.Parse(value);
                var users = _context.customers.FirstOrDefault(p => p.Id == ids);
                var Name = users.Email;
                ViewBag.UserName = Name;
            }

            List<ShoppingCart> cart = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
            int cartItemCount = cart.Sum(item => item.qty);
            ViewBag.count = cartItemCount;
            var user= HttpContext.Session.GetString("id");
            
            var id = int.Parse(user);

            Product p = _context.products.Where(p => p.productId == productid).FirstOrDefault();

            ShoppingCart c =new ShoppingCart();
            c.productId=productid;
            p.productimage = image;
            c.qty = qty;
            c.price = price;
            c.bill = c.price * c.qty;
            c.image = image;
            c.CreateDate = DateTime.Now;
            c.Lastmodifield = DateTime.Now;
            c.Id = id;
            List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
           
            cartItems.Add(c);
            HttpContext.Session.SetObject("Cart", cartItems);
            HttpContext.Session.SetString("Price", price.ToString() ?? "No price available");
            return RedirectToAction("OrderPlace","Client");
        }
        [HttpGet]
        public IActionResult OrderPlace()
        {
            var payment = _context.paymenttypes.ToList();
            ViewBag.payment = payment;

            var user = HttpContext.Session.GetString("id");

            if (user != null)
            {
                int id = int.Parse(user);
                var users = _context.customers.FirstOrDefault(p => p.Id == id);
                var Name = users.Email;
                ViewBag.UserName = Name;

                decimal totalBill = 0;
                if (HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") != null)
                {
                    List<ShoppingCart> cartItem = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart");
                    // Calculate total bill
                    foreach (var item in cartItem)
                    {
                        totalBill += item.bill;

                    }
                    ViewBag.bill = totalBill;
                }

                List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
                int cartItemCount = cartItems.Sum(item => item.qty);
                ViewBag.count = cartItemCount;
                var customer = _context.customers.Where(p => p.Id == id).FirstOrDefault();
                var CustomerViewModel = new CustomerViewModel
                {
                    Id = customer.Id,
                    Email=customer.Email,
                    Name = customer.Name,
                    Password = customer.Password,
                    address = customer.address,
                    Number=customer.Number,

                };
                return View(CustomerViewModel);
            }
            return View();
        }
        [HttpPost]
        public IActionResult OrderPlace(int PaymentTypeId)
        {
            var payment = _context.paymenttypes.ToList();
            ViewBag.payment = payment;

            var user = HttpContext.Session.GetString("id");

             if (user != null)
            {
                int id = int.Parse(user);
                var users = _context.customers.FirstOrDefault(p => p.Id == id);
                var Name = users.Email;
                ViewBag.UserName = Name;

                decimal totalBill = 0;
                if (HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") != null)
                {
                    List<ShoppingCart> cartItem = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart");
                    // Calculate total bill
                    foreach (var item in cartItem)
                    {
                        totalBill += item.bill;

                    }
                    ViewBag.bill = totalBill;
                }

                List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
                int cartItemCount = cartItems.Sum(item => item.qty);
                ViewBag.count = cartItemCount;
                var customer = _context.customers.Where(p => p.Id == id).FirstOrDefault();
                if (cartItems != null)
                {
                    foreach (var item in cartItems)
                    {
                        _context.shoppingCarts.Add(item);
                        _context.SaveChanges();
                        var order = new Order();
                        order.quantity = item.qty;
                        order.subtotal = item.bill;
                        order.productId= item.productId;
                        order.totalamount = item.bill;
                        order.ordernumber = Guid.NewGuid().ToString();
                        order.Id = item.Id;
                        order.Status = "Pending";
                        order.CreateDate = DateTime.Now;
                        order.Lastmodifield = DateTime.Now;
                        _context.orders.Add(order);
                        _context.SaveChanges();
                        Payment pay = new Payment();
                        pay.Id= item.Id;
                        pay.OrderId = order.OrderId;
                        pay.PaymentStatus = "paid";
                        pay.paymenttypeId = PaymentTypeId;
                        pay.TotalAmount = order.totalamount;
                        pay.CreateDate=DateTime.Now;
                        pay.Lastmodifield = DateTime.Now;
                        _context.payments.Add(pay);
                        _context.SaveChanges();
                         OrderSendConfirmEmail(customer, cartItems);
                        return RedirectToAction("Home","Client");
                    }
                }
                return View();
            }
            return View();
        }
        //orderconfirmemailsend
        public void OrderSendConfirmEmail(Customer cust,List<ShoppingCart> cart)
        {
            string emailBody = $"Dear {cust.Name},\n\n";
            emailBody += $"{cust.Email},\n\n";
            emailBody += $"{cust.address},\n\n";
            emailBody += $"{cust.Number},\n\n";


            foreach (var item in cart)
            {
                emailBody += $"Product:{item.productId},Quantity:{item.qty},Total:{item.bill}\n\n";
            }

            emailBody += "\n\nRegards,\nYour Company";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("email@gmail.com");
                mail.To.Add(cust.Email);
                mail.Subject = "Order Confirmation";
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
        public IActionResult Customer()
        {
            var user = HttpContext.Session.GetString("id");
           
             if (user != null)
            {
                int id = int.Parse(user);
                var users = _context.customers.FirstOrDefault(p => p.Id == id);
                var Name = users.Email;
                ViewBag.UserName = Name;
            }
            decimal totalBill = 0;
            if (HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") != null)
            {
                List<ShoppingCart> cartItem = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart");
                // Calculate total bill
                foreach (var item in cartItem)
                {
                    totalBill += item.bill;

                }
                ViewBag.bill = totalBill;
            }

            List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
            int cartItemCount = cartItems.Sum(item => item.qty);
            ViewBag.count = cartItemCount;
            return View();
        }
        [HttpPost]
        public IActionResult Customer(string name,string email,string password,string phone,string address)
        {
            
            decimal totalBill = 0;
            if (HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") != null)
            {
                List<ShoppingCart> cartItem = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart");
                // Calculate total bill
                foreach (var item in cartItem)
                {
                    totalBill += item.bill;

                }
                ViewBag.bill = totalBill;
            }


            List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
            int cartItemCount = cartItems.Sum(item => item.qty);
            ViewBag.count = cartItemCount;

            Customer user = new Customer();
            user.Number = phone;
            user.Name = name;
            user.Email = email;
            user.address = address;
            user.Password = password;
            _context.customers.Add(user);
            _context.SaveChanges();
             HttpContext.Session.SetString("id",user.Id.ToString());
            return RedirectToAction("Home", "Client");
        }
        [HttpGet]
        public IActionResult CheckOut()
        {
            var user = HttpContext.Session.GetString("id");

            ; if (user != null)
            {
                int id = int.Parse(user);
                var users = _context.customers.FirstOrDefault(p => p.Id == id);
                var Name = users.Email;
                ViewBag.UserName = Name;
            }

            decimal totalBill = 0;
            if (HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") != null)
            {
                List<ShoppingCart> cartItem = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart");
                // Calculate total bill
                foreach (var item in cartItem)
                {
                    totalBill += item.bill;

                }
                ViewBag.bill= totalBill;
            }
            
            List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
            int cartItemCount = cartItems.Sum(item => item.qty);
            ViewBag.count = cartItemCount;
            
            return View(cartItems);

        }
        public IActionResult custLogin()
        {
            var user = HttpContext.Session.GetString("id");
             if (user != null)
            {
                int id = int.Parse(user);

                var users = _context.customers.FirstOrDefault(p => p.Id == id);
                var Name = users.Email;
                ViewBag.UserName = Name;
            }

            List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
            int cartItemCount = cartItems.Sum(item => item.qty);
            ViewBag.count = cartItemCount;
            return View();  
        }
        [HttpPost]
        public IActionResult custLogin(Customer model)
        {
            var user = HttpContext.Session.GetString("id");
            
             if (user != null)
            {
                int id = int.Parse(user);
                var users = _context.customers.FirstOrDefault(p => p.Id == id);
                var Name = users.Email;
                ViewBag.UserName = Name;
            }

            List<ShoppingCart> cartItems = HttpContext.Session.GetObject<List<ShoppingCart>>("Cart") ?? new List<ShoppingCart>();
            int cartItemCount = cartItems.Sum(item => item.qty);
            ViewBag.count = cartItemCount;
            var cust = _context.customers.FirstOrDefault(p => p.Email == model.Email);
            if (cust != null)
            {
                HttpContext.Session.SetString("id", cust.Id.ToString());
                return RedirectToAction("Home","Client");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();  
        }
        [HttpPost]
        public IActionResult Contact(string message)
        {


            var value = HttpContext.Session.GetString("id");
         if(value==null)
            {
                return RedirectToAction("custLogin", "Client");

            }

            if (value != null)
            {
                int ids = int.Parse(value);
                var users = _context.customers.FirstOrDefault(p => p.Id == ids);               
                var Name = users.Email;
                ViewBag.UserName = Name;

                var contact = _context.customers.FirstOrDefault(p => p.Id == ids);

                Contact c = new Contact();
                {
                    c.Message = message;
                    c.Id = ids;

                };
                _context.contacts.Add(c);
                _context.SaveChanges();
                return RedirectToAction("Home", "Client");
            }

            return View();
        }
        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("id");
            return RedirectToAction("custLogin", "Client");
        }
     
        [HttpPost]
        public IActionResult Rating(int productid,int rating)
        {
            var user = HttpContext.Session.GetString("id");

            if (user != null)
            {
                int id = int.Parse(user);
                var review = new Rating();
                review.Id = id;
                review.review = rating;
                review.productId = productid;
                review.CreateDate = DateTime.Now;
                review.Lastmodifield = DateTime.Now;
                _context.ratings.Add(review);
                _context.SaveChanges();

            }
            return RedirectToAction("Home", "Client");

        }

    }
}
