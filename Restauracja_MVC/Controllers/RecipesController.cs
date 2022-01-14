using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restauracja_MVC.Models.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Controllers
{
    public class RecipesController : Controller
    {
            List<RecipesListItem> list = new List<RecipesListItem>();
        // GET: RecipesController
        public ActionResult Index()
        {
            list.Add(new RecipesListItem {
                MealID = 2,
                IngredientIDlist = new List<int>() {
                1, 2, 3, 4
                }
            });

            return View(list);
        }

        // GET: RecipesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RecipesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecipesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: RecipesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RecipesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: RecipesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RecipesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
