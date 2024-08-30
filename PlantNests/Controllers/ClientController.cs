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
        public IActionResult OrderPlace(Order model)
        {
            var user = HttpContext.Session.GetString("id");

            ; if (user != null)
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
                        ViewBag.order = "order is successfully";
                        UpdateInventoryFromOrders();
                        return RedirectToAction("Home","Client");
                    }
                }
                return View();
            }
            return View();
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
        public void UpdateInventoryFromOrders()
        {
            // Get all orders
            var orders = _context.orders.ToList();

            foreach (var order in orders)
            {
                // Get the product from the inventory
                var inventoryItem = _context.inventories.FirstOrDefault(i => i.productId == order.productId);

                if (inventoryItem != null)
                {
                    // Update the inventory by reducing the quantity
                    inventoryItem.Inqty -= order.quantity;

                    // Ensure the quantity doesn't go below zero
                    if (inventoryItem.Inqty < 0)
                    {
                        inventoryItem.Inqty = 0;
                    }

                    // Update the last modified date
                    inventoryItem.Lastmodifield = DateTime.Now;

                    // Log the sold record
                    var soldRecord = new SoldProduct
                    {
                        productId = order.productId,
                        OrderId = order.OrderId,
                        quantitysold = order.quantity,
                        solddate = DateTime.Now,
                        TotalAmount = order.totalamount
                    };
                    _context.soldProducts.Add(soldRecord);
                }
                else
                {
                    // If the product doesn't exist in the inventory, add it with the initial quantity minus the order quantity
                    var newInventoryItem = new Inventory
                    {
                        productId = order.productId,
                        Id = order.Id,
                        OrderId = order.OrderId,
                        totalqty = 3, // Assuming you start with 3 units for each product
                        Inqty = 3 - order.quantity, // Initial quantity minus the order quantity
                        totalamount = order.totalamount,
                        CreateDate = DateTime.Now,
                        Lastmodifield = DateTime.Now
                    };

                    if (newInventoryItem.Inqty < 0)
                    {
                        newInventoryItem.Inqty = 0;
                    }

                    _context.inventories.Add(newInventoryItem);

                    // Log the sold record
                    var soldRecord = new SoldProduct
                    {
                        productId = order.productId,
                        OrderId = order.OrderId,
                        quantitysold = order.quantity,
                        solddate = DateTime.Now,
                        TotalAmount = order.totalamount
                    };
                    _context.soldProducts.Add(soldRecord);
                }
            }

            // Save all changes to the database
            _context.SaveChanges();
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
