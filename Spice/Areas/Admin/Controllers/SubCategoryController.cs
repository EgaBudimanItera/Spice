using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModel;


namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        [TempData]
        public string StatusMessage { get; set; }
        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.SubCategory.Include(s=>s.Category).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = new SubCategory(),
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
            };
            return View(model);
        }

        //POST -Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id==model.SubCategory.CategoryId);
                if (doesSubCategoryExists.Count() > 0)
                {
                    //error
                    StatusMessage = "Error : Sub Category exist under " + doesSubCategoryExists.First().Category.Name + " Category. Please use another name";

                }
                else
                {
                    _db.SubCategory.Add(model.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                

                
            }
            SubCategoryAndCategoryViewModel ModelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
                StatusMessage=StatusMessage,
            };
            return View(ModelVM);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List <SubCategory> subCategories= new List<SubCategory>();
            subCategories =await (from SubCategory in _db.SubCategory
                             where SubCategory.CategoryId == id
                             select SubCategory).ToListAsync();
            return Json(new SelectList(subCategories,"Id","Name"));
            //return Json(new { a="asu"});
        }
        //GET -Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(n => n.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
            };
            return View(model);
        }

        //POST -Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);
                if (doesSubCategoryExists.Count() > 0)
                {
                    //error
                    StatusMessage = "Error : Sub Category exist under " + doesSubCategoryExists.First().Category.Name + " Category. Please use another name";

                }
                else
                {
                    var subCatromDb = await _db.SubCategory.FindAsync(model.SubCategory.Id);
                    subCatromDb.Name = model.SubCategory.Name;//update field tertentu

                   
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }



            }
            SubCategoryAndCategoryViewModel ModelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
                StatusMessage = StatusMessage,
            };
            return View(ModelVM);
        }


        //get -Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
           // var sc= await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound(); 
            }
            return View(subCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var subcategory = await _db.SubCategory.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            _db.SubCategory.Remove(subcategory);//remove value dari list kategory
            await _db.SaveChangesAsync();//simpen kedatabase
            return RedirectToAction(nameof(Index));
        }


        //get -Delete
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);//include ini untuk join subcategory dengan category tp harus ada virtualnya di model
            // var sc= await _db.SubCategory.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);//await harus ditemani dgn async
            if (subCategory == null)
            {
                return NotFound();
            }
            return View(subCategory);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsConfirmed(int? id)
        {
            return RedirectToAction("Edit", new { id = id });
        }
    }
}