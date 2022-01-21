using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models.Meals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Controllers
{
    public class MealsController : Controller
    {

        private readonly IConfiguration _config;
        private readonly string connectionString;


        public MealsController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("DbConnection");
        }


        // GET: MealsController
        public ActionResult Index()
        {
            return View(GetMealsList());
        }

        [NonAction]
        private List<MealListItem> GetMealsList()
        {
            var list = new List<MealListItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT m.ID, m.Name, m.Price, mc.Name AS Category FROM Meals AS m " +
                    "INNER JOIN MealsCategories mc ON m.Category = mc.ID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new MealListItem()
                    {
                        ID = Int16.Parse(dr["ID"].ToString()),
                        Category = dr["Category"].ToString(),
                        Name = dr["Name"].ToString(),
                        Price = float.Parse(dr["Price"].ToString())
                    });
                }
            }
            return list;
        }


        // GET: MealsController/Details/5
        public ActionResult Details(Int16 id)
        {
            MealDetails meal = GetMealDetailsByID(id);
            if (meal != null)
                return View(meal);
            else return RedirectToAction(nameof(Index));
        }

        [NonAction]
        private MealDetails GetMealDetailsByID(Int16 id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT m.ID, m.Name, m.Price, m.Description, mc.Name AS Category FROM Meals AS m " +
                    "INNER JOIN MealsCategories mc ON m.Category = mc.ID " +
                    "WHERE m.ID = @ID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    return new MealDetails()
                    {
                        ID = Int16.Parse(dr["ID"].ToString()),
                        Category = dr["Category"].ToString(),
                        Description = dr["Description"].ToString(),
                        Name = dr["Name"].ToString(),
                        Price = float.Parse(dr["Price"].ToString())
                    };
                }
            }
            return null;
        }

        // GET: MealsController/Create
        public ActionResult Create()
        {
            MealCreate meal = new MealCreate();
            meal.MealsCategories = GetMealsCategoriesList();
            return View(meal) ;
        }

        // POST: MealsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MealCreate meal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AddMeal(meal);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return View();
        }

        [NonAction]
        private void AddMeal(MealCreate meal)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = "INSERT INTO [dbo].[Meals] ([Name], [Price], [Description], [Category])" +
            "VALUES (@Name, @Price, @Description, @category)";
            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].Value = meal.Name;
            command.Parameters.Add("@Price", System.Data.SqlDbType.Float);
            command.Parameters["@Price"].Value = meal.Price;
            command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
            command.Parameters["@Description"].Value = meal.Description;
            command.Parameters.Add("@Category", System.Data.SqlDbType.TinyInt);
            command.Parameters["@Category"].Value = meal.Category;

            command.ExecuteNonQuery();
        }


        // GET: MealsController/Edit/5
        public ActionResult Edit(Int16 id)
        {
            MealCreate meal = GetMealCreateByID(id);
            meal.MealsCategories = GetMealsCategoriesList();
            if (meal != null)
                return View(meal);
            else return RedirectToAction(nameof(Index));

        }

        [NonAction]
        private MealCreate GetMealCreateByID(Int16 id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT m.ID, m.Name, m.Price, m.Description, m.Category FROM Meals AS m " +
                    "INNER JOIN MealsCategories mc ON m.Category = mc.ID " +
                    "WHERE m.ID = @ID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    return new MealCreate()
                    {
                        Category = Byte.Parse(dr["Category"].ToString()),
                        Description = dr["Description"].ToString(),
                        Name = dr["Name"].ToString(),
                        Price = float.Parse(dr["Price"].ToString())
                    };
                }
            }
            return null;
        }

        private List<SelectListItem> GetMealsCategoriesList()
        {
            var list = new List<SelectListItem>();

            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT ID, Name FROM MealsCategories";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SelectListItem { Value = dr["ID"].ToString(), Text = dr["Name"].ToString() });
                }
            }
            return list;
        }

        // POST: MealsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Int16 id, MealCreate meal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UpdateMeal(id, meal);
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
        public void UpdateMeal(Int16 id, MealCreate meal)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = "UPDATE [dbo].[Meals] " +
                "SET [Name] = @Name, [Price] = @Price, [Description] = @Description, [Category] = @Category " +
                $"WHERE [ID] = @ID";
            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].Value = meal.Name;
            command.Parameters.Add("@Price", System.Data.SqlDbType.Float);
            command.Parameters["@Price"].Value = meal.Price;
            command.Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
            command.Parameters["@Description"].Value = meal.Description;
            command.Parameters.Add("@Category", System.Data.SqlDbType.TinyInt);
            command.Parameters["@Category"].Value = meal.Category;
            command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
            command.Parameters["@ID"].Value = id;

            command.ExecuteNonQuery();
        }


        // GET: MealsController/Delete/5
        public ActionResult Delete(Int16 id)
        {
            MealDetails meal =  GetMealDetailsByID(id);
            if (meal != null)
                return View(meal);
            else return RedirectToAction(nameof(Index));

        }

        // POST: MealsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Int16 id, MealDetails meal)
        {
            try
            {
                DeleteMeal(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return View();
        }

        [NonAction]
        private void DeleteMeal(Int16 id)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = $"DELETE FROM [dbo].[Meals] WHERE id = @ID";

            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
            command.Parameters["@ID"].Value = id;

            command.ExecuteNonQuery();
        }
    }
}
