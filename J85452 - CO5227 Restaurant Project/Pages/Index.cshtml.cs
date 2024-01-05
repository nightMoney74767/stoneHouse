using J85452___CO5227_Restaurant_Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace J85452___CO5227_Restaurant_Project.Pages
{
    public class IndexModel : PageModel
    {
        // Set up the menu table so it can be displayed (summary of menu items)
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<MenuClass> Menu { get; set; }
        public void OnGet()
        {
            Menu = (IList<MenuClass>)_db.Menu.FromSqlRaw("SELECT * FROM Menu").ToList();
        }

        
    }
}
