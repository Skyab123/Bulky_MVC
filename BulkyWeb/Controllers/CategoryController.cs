using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db) 
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Displayorder cannot exactly match the Name!");
            }

            if (ModelState.IsValid) // Validatie bij een object (elke prop zal moeten ingevuld zijn)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges(); // werkelijke uitvoering
                return RedirectToAction("Index", "Category"); // Indien in dezelfde controller is ActionResult genoeg
            }
            return View();
            
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }

            Category? categoryFromDb = _db.Categories.FirstOrDefault(cat => cat.Id == id); // .Find() enkel op PK

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //[HttpPost]
        //public IActionResult Edit(Category obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Categories.Update(obj);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
    }
}
