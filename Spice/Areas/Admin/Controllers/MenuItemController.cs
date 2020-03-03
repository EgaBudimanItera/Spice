using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModel;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }//cara kedua untuk bind create, tapi definisikan dl di controllernya


        public MenuItemController(ApplicationDbContext db,IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                Category = _db.Category,
                MenuItem=new MenuItem(),
            };

        }
        public async Task<IActionResult> Index()
        {
            var menuitem = await _db.MenuItem.Include(m=>m.Category).Include(m=>m.SubCategory).ToListAsync();
            return View(menuitem);
        }

        //GET-Create
        public IActionResult Create()
        {
            return View(MenuItemVM);
        }
    }
}