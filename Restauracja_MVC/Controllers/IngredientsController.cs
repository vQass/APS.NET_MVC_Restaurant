using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models.Ingredients;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Restauracja_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IngredientsController : Controller
    {

        private readonly IConfiguration _config;
        private readonly string connectionString;


        public IngredientsController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("DbConnection");
        }



        // GET: IngredientsController
        public ActionResult Index()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
                TempData["Error"] = null;
            }
            return View(GetIngredientsList());
        }

        [NonAction]
        private List<IngredientListItem> GetIngredientsList()
        {
            var list = new List<IngredientListItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT * FROM [Ingredients]";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new IngredientListItem()
                    {
                        ID = Int16.Parse(dr["ID"].ToString()),
                        Name = dr["Name"].ToString(),
                    });
                }
            }
            return list;
        }

        [NonAction]
        private IngredientListItem GetMealListItemByID(short id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT * FROM [dbo].[Ingredients] WHERE ID = @ID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                command.Parameters.Add("@ID", System.Data.SqlDbType.VarChar);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    return new IngredientListItem()
                    {
                        ID = Int16.Parse(dr["ID"].ToString()),
                        Name = dr["Name"].ToString(),
                    };
                }
            }
            return null;
        }

        // GET: IngredientsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IngredientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ingredient ingredient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(CheckUniqueIngredient(ingredient.Name))
                    {
                        AddIngredient(ingredient);
                    }
                    else
                    {
                        TempData["Error"] = "Podana nazwa występuje w bazie danych";
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return View();
        }

        [NonAction]
        private bool CheckUniqueIngredient(string name)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT Name FROM Ingredients WHERE Name = @Name";
                using var command = new SqlCommand(qs, connection);

                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                command.Parameters["@Name"].Value = name;

                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                return !dr.HasRows;
            }
            return false;
        }


        [NonAction]
        private void AddIngredient(Ingredient ingredient)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = "INSERT INTO [dbo].[Ingredients] ([Name])" +
            "VALUES (@Name)";
            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].Value = ingredient.Name;

            command.ExecuteNonQuery();
        }

        // GET: IngredientsController/Edit/5
        public ActionResult Edit(short id)
        {
            IngredientListItem ingredient = GetMealListItemByID(id);
            if (ingredient != null)
                return View(ingredient);
            else return RedirectToAction("Index");
        }

        // POST: IngredientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(short id, Ingredient ingredient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(CheckUniqueIngredient(ingredient.Name))
                    {
                        UpdateIngredient(id, ingredient);
                    }
                    else
                    {
                        TempData["Error"] = "Podana nazwa występuje w bazie danych";
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return View();
        }

        [NonAction]
        public void UpdateIngredient(short id, Ingredient ingredient)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = "UPDATE [dbo].[Ingredients] " +
                "SET [Name] = @Name " +
                $"WHERE [ID] = @ID";
            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].Value = ingredient.Name;
            command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
            command.Parameters["@ID"].Value = id;

            command.ExecuteNonQuery();
        }

        // GET: IngredientsController/Delete/5
        public ActionResult Delete(short id)
        {
            IngredientListItem ingredient = GetMealListItemByID(id);
            if (ingredient != null)
                return View(ingredient);
            else return RedirectToAction("Index");
        }

        // POST: IngredientsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(short id, Ingredient ingredient)
        {
            try
            {
                DeleteIngredient(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return View();
        }

        [NonAction]
        private void DeleteIngredient(short id)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = $"DELETE FROM [dbo].[Ingredients] WHERE id = @ID";

            using var command = new SqlCommand(qs, connection);

            command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
            command.Parameters["@ID"].Value = id;

            command.Connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
