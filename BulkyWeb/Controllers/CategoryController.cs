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
            if (ModelState.IsValid) // Validatie bij een object (elke prop zal moeten ingevuld zijn)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges(); // werkelijke uitvoering
                return RedirectToAction("Index", "Category"); // Indien in dezelfde controller is ActionResult genoeg
            }
            return View();
            
        }
    }
}
