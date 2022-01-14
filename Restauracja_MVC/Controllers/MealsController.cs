using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restauracja_MVC.Models.Meals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Controllers
{
    public class MealsController : Controller
    {
        // GET: MealsController
        static List<MealListItem> list = new List<MealListItem>();

        public ActionResult Index()
        {
            list.Add(new MealListItem() { ID = 1, Category = 1, Description = "Super, bardzo dobre", Name = "Bigos", PictureName = null, Price = 12.62f });
            list.Add(new MealListItem() { ID = 2, Category = 1, Description = "Fujka", Name = "Tatar", PictureName = null, Price = 2.0f });
            list.Add(new MealListItem() { ID = 3, Category = 1, Description = "Mieszany sosik, mięso drobiowe, coś pięknego", Name = "Kebsik", PictureName = null, Price = 15.42f });

            return View(list);
        }

        // GET: MealsController/Details/5
        public ActionResult Details(int id)
        {
            
            return View(list[id]);
        }

        // GET: MealsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MealsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Meal meal)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MealsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(list[id]);
        }

        // POST: MealsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Meal meal)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MealsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(list[id]);
        }

        // POST: MealsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Meal meal)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
