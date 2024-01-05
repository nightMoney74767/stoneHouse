using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.CS;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    // Controller for checkout
    public class CheckoutModel : PageModel
    {
        public AppDbContext _db;
        private readonly UserManager<AppUserClass> _userManager;
        private readonly SignInManager<AppUserClass> _signInManager;
        public IList<CheckoutItemClass> Items { get; private set; }
        public OrderHistoryClass Order = new OrderHistoryClass();
        public bool purchaseConfirmed;

        public decimal Total = 0;
        public long AmountPayable = 0;

        public CheckoutModel(AppDbContext db, UserManager<AppUserClass> userManager, SignInManager<AppUserClass> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            try
            {
                if (_signInManager.IsSignedIn(User))
                {
                    var user = await _userManager.GetUserAsync(User);
                    CheckoutCustomerClass customer = await _db.CheckoutCustomer.FindAsync(user.Email);
                    Items = _db.CheckoutItem.FromSqlRaw("SELECT Menu.ItemID, Menu.ItemPrice, Menu.ItemDescription, BasketItem.BasketID, BasketItem.Quantity FROM Menu INNER JOIN BasketItem ON Menu.ItemID = BasketItem.ItemID WHERE BasketID = {0}", customer.BasketID).ToList();
                }

                if (_signInManager.IsSignedIn(User))
                {
                    foreach (var item in Items)
                    {
                        // Calculate total price and display it in the view
                        Total += (item.Quantity * item.ItemPrice);
                    }
                    AmountPayable = (long)(Total * 100);
                }
            } catch (Exception ex)
            {
                ModelState.AddModelError("CheckoutError", "Unable to get basket");
            }
           
        }

        // Increase item quantity by 1
        public async Task<IActionResult> OnPostIncrementAsync(int itemID, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomerClass customer = await _db.CheckoutCustomer.FindAsync(user.Email);
            BasketItemClass basketItem = await _db.BasketItem.FindAsync(itemID, customer.BasketID);
            basketItem.Quantity = basketItem.Quantity + 1;
            _db.Attach(basketItem).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToPage();
        }

        // Decrease item quantity by 1; removes the item if the quantity becomes 0
        public async Task<IActionResult> OnPostDecrementAsync(int itemID, int basketID)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomerClass customer = await _db.CheckoutCustomer.FindAsync(user.Email);
            BasketItemClass basketItem = await _db.BasketItem.FindAsync(itemID, customer.BasketID);
            basketItem.Quantity = basketItem.Quantity - 1;
            if (basketItem.Quantity == 0)
            {
                _db.BasketItem.Remove(basketItem);
            } else
            {
                _db.Attach(basketItem).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
            return RedirectToPage();
        }

        // Process and log user's purchase
        public async Task Process()
        {
            var user = await _userManager.GetUserAsync(User);

            var currentOrder = _db.OrderHistory.FromSqlRaw("SELECT * FROM OrderHistory").OrderByDescending(b => b.OrderID).FirstOrDefault();
            if (currentOrder == null)
            {
                Order.OrderID = 1;
            }
            else
            {
                Order.OrderID = currentOrder.OrderID + 1;
            }

            // New customer
            CustomerClass newCustomer = new CustomerClass();
            int numOfCustomers = _db.Customer.FromSqlRaw("SELECT * FROM Customer").Sum(b => b.CustomerID);
            int newCustomerID = numOfCustomers + 1;
            newCustomer.CustomerID = newCustomerID;
            newCustomer.CustomerEmail = user.Email;
            newCustomer.CustomerStreetName = "";
            newCustomer.CustomerParish = "";
            newCustomer.CustomerCity = "";
            newCustomer.CustomerCounty = "";
            newCustomer.CustomerPhone = "";
            newCustomer.CustomerName = user.UserName;
            _db.Customer.Add(newCustomer);

            CheckoutCustomerClass checkoutCustomerClass = await _db.CheckoutCustomer.FindAsync(user.Email);
            var basketItems =
                _db.BasketItem
                .FromSqlRaw("SELECT * From BasketItem " +
                "WHERE BasketID = {0}", checkoutCustomerClass.BasketID)
                .ToList();

            Order.CustomerID = newCustomer.CustomerID;
            Order.RestaurantID = 1;

            // Find restaurant name
            RestaurantClass restaurant = await _db.Restaurant.FindAsync(Order.RestaurantID);
            string restaurantName = restaurant.RestaurantName;

            foreach (var item in basketItems)
            {
                Data.OrderItemClass oi = new Data.OrderItemClass()
                {
                    OrderID = Order.OrderID,
                    ItemID = item.ItemID,
                    Quantity = item.Quantity,
                    Subtotal = 0
                };


                // Get price
                MenuClass menuItem = await _db.Menu.FindAsync(item.ItemID);
                decimal menuItemPrice = menuItem.ItemPrice;

                // Reduce the number of items available attribute of selected menu items by the quantity; Allows stock control
                menuItem.NumOfItemsAvailable -= oi.Quantity;
                
                // Allocate subtotal cost to order item
                oi.Subtotal = menuItemPrice * oi.Quantity;

                _db.OrderItem.Add(oi);

                // Remove basket item
                _db.BasketItem.Remove(item);
            }

            // Add total price to order history record
            CheckoutCustomerClass customer = await _db.CheckoutCustomer.FindAsync(user.Email);
            Items = _db.CheckoutItem.FromSqlRaw("SELECT Menu.ItemID, Menu.ItemPrice, Menu.ItemDescription, BasketItem.BasketID, BasketItem.Quantity FROM Menu INNER JOIN BasketItem ON Menu.ItemID = BasketItem.ItemID WHERE BasketID = {0}", customer.BasketID).ToList();
            foreach (var item in Items)
            {
                // Calculate total price and display it in the view
                Order.FinalPrice += (item.Quantity * item.ItemPrice);
            }

            // Add today's date to order history
            DateTime date = DateTime.Today;
            string dateString = date.ToString();
            Order.DateOfSale = dateString;

            _db.OrderHistory.Add(Order);

            await _db.SaveChangesAsync();

            // Confirm purchase
            ModelState.AddModelError("PurchaseConfirmed", "User purchase has been confirmed");

            // Send email with the contents of the input boxes
            SmtpClient smptClient = new SmtpClient("smtp.gmail.com", 587);
            smptClient.Credentials = new System.Net.NetworkCredential("ncco5227@gmail.com", "@16wqg10");
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.EnableSsl = true;
            MailMessage mailMessage = new MailMessage();

            // For the purpose of this functionality, a new Gmail has been created. As the password is dispalyed in cleartext (potentially a security risk), it was not appropriate to use any existing email accounts
            // Code adapted from StackOverflow (Karuppiah, 2013) and Jason Watmore's Blog (2020)
            mailMessage.From = new MailAddress("ncco5227@gmail.com", "StoneHouse Restaurant (CO5227)");
            mailMessage.To.Add(new MailAddress(user.Email));
            mailMessage.CC.Add(new MailAddress("2003943@chester.ac.uk"));
            mailMessage.Subject = "StoneHouse Restaurant - Purchase confirmed";
            mailMessage.Body = "Hello,\n" +
                "Thank you for ordering at StoneHouse Restaurant. Please find a log of your order below. This message has been sent to you for your records.\n"
                + "Order ID: " + Order.OrderID + "\n"
                + "Customer ID: " + Order.CustomerID + "\n"
                + "Restaurant Name: " + restaurantName + "\n"
                + "Total Price: " + Order.FinalPrice.ToString("C") + "\n"
                + "Date of order: " + Order.DateOfSale + "\n"
                + "Items Ordered: " + "\n";

            // Get items within order item table based on the order id
            var orderItems = _db.OrderItem.FromSqlRaw("SELECT * FROM OrderItem WHERE OrderID = {0}", Order.OrderID);

            foreach (var item in orderItems)
            {
                mailMessage.Body += "\tItem ID: " + item.ItemID + "\n"
                    + "\t\tQuantity: " + item.Quantity + "\n"
                    + "\t\tSubtotal: " + item.Subtotal.ToString("C") + "\n";
            }

            mailMessage.Body += "\nKind regards, \n"
                + "StoneHouse Restaurant Team\n"
                + "\n"
                + "Please note: This email is sent as part of an assignment at University of Chester (for the module CO5227) from an account created specifically for this purpose"; ;
            smptClient.Send(mailMessage);
            // End of adapted code
        }

        // Process Stripe payment
        public IActionResult OnPostCharge(string stripeEmail, string stripeToken, long amount)
        {
            int numOfCustomers = _db.Customer.FromSqlRaw("SELECT * FROM Customer").Sum(b => b.CustomerID);
            int newCustomerID = numOfCustomers + 1;

            var customers = new CustomerService();
            var charges = new ChargeService();

            var stripeCustomer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = amount,
                Description = "Pay with Card",
                Currency = "gbp",
                Customer = stripeCustomer.Id
            });

            // Process purchase in the process method above
            Process().Wait();
            return RedirectToPage("/PurchaseConfirmed");
        }
    }
}