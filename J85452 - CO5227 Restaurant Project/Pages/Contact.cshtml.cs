using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    public class ContactModel : PageModel
    {
        // Creates an instance of the customer class so that the form in Contact.CSHTML can be captured
        private AppDbContext _db;
        [BindProperty]
        public CustomerClass Customer { get; set; }
        [BindProperty]
        public string message { get; set; }
        public ContactModel(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Prevents invalid inputs from being entered
            // The forms are automatically set up to prevent invalid inputs (e.g. by inserting letters in a phone number)
            if (!ModelState.IsValid)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("CustomerContactFieldError", "Invalid data for customer contact form");
                // End of adapted code
                return Page();
            }

            try
            {
                // Automatically sets the CustomerID based on the number of rows saved in the database
                Customer.CustomerID = count();
                // Saves the contents of the input boxes from the view into the customer table
                _db.Customer.Add(Customer);
                await _db.SaveChangesAsync();

                // Send email with the contents of the input boxes
                SmtpClient smptClient = new SmtpClient("smtp.gmail.com", 587);
                smptClient.Credentials = new System.Net.NetworkCredential("ncco5227@gmail.com", "");
                smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smptClient.EnableSsl = true;
                MailMessage mailMessage = new MailMessage();

                // For the purpose of this functionality, a new Gmail has been created. As the password is dispalyed in cleartext (potentially a security risk), it was not appropriate to use any existing email accounts
                // Password removed
                // Code adapted from StackOverflow (Karuppiah, 2013) and Jason Watmore's Blog (2020)
                mailMessage.From = new MailAddress("ncco5227@gmail.com", "StoneHouse Restaurant (CO5227)");
                mailMessage.To.Add(new MailAddress("2003943@chester.ac.uk"));
                mailMessage.CC.Add(new MailAddress(Customer.CustomerEmail));
                mailMessage.Subject = "StoneHouse Restaurant - New Customer Contact";
                mailMessage.Body = "Hello,\n" +
                    "A customer has contacted StoneHouse Restaurant with the following infomration:\n"
                    + "Name: " + Customer.CustomerName + "\n"
                    + "Street Name: " + Customer.CustomerStreetName + "\n"
                    + "Parish: " + Customer.CustomerParish + "\n"
                    + "City: " + Customer.CustomerCity + "\n"
                    + "County: " + Customer.CustomerCounty + "\n"
                    + "PostCode: " + Customer.CustomerPostCode + "\n"
                    + "Phone: " + Customer.CustomerPhone + "\n"
                    + "Email: " + Customer.CustomerEmail + "\n"
                    + "Message: " + message + "\n"
                    + "Kind regards, \n"
                    + "StoneHouse Restaurant Team\n"
                    + "\n"
                    + "Please note: This email is sent as part of an assignment at University of Chester (for the module CO5227) from an account created specifically for this purpose";
                smptClient.Send(mailMessage);
                // End of adapted code
            }
            catch (Exception ex)
            {
                // Registers an error that is displayed in the CSHTML file
                // Adapted from StackOverflow (Isma, 2017)
                ModelState.AddModelError("CustomerContactFieldError", "Invalid data for customer contact form");
                // End of adapted code
                return Page();
            }
            return Page();
        }

        // Method to find the number of rows in the customer table and returns that number increased by 1
        // Created with guidance from StackOverflow (Tripathi, 2013)
        public int count()
        {
            CustomerClass[] customers = _db.Customer.FromSqlRaw("SELECT * FROM Customer").ToArray();
            int count = 0;
            for (int i = 0; i < customers.Length; i++)
            {
                count++;
            }
            if (count != 0)
            {
                CustomerClass lastCustomer = customers[count - 1];
                int lastID = lastCustomer.CustomerID;
                return lastID + 1;
            } else
            {
                return 1;
            }
        }
        // End of adapted code

        public void OnGet()
        {
        }
    }
}