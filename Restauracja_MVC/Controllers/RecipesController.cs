using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models.Recipes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Restauracja_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RecipesController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string connectionString;

        public RecipesController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("DbConnection");
        }

        // GET: RecipesController
        public ActionResult Index()
        {
            return View(GetRecipiesList());
        }

        [NonAction]
        private List<RecipesListItem> GetRecipiesList()
        {
            List<RecipesListItem> list = new List<RecipesListItem>();
            List<RecipeElement> recipes = new List<RecipeElement>();
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT m.ID, m.Name, c.Name AS Category FROM Meals m " +
                    "INNER JOIN MealsCategories c ON m.Category = c.ID ORDER BY Category, Name";

                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new RecipesListItem()
                    {
                        MealID = short.Parse(dr["ID"].ToString()),
                        Meal = dr["Name"].ToString(),
                        Category = dr["Category"].ToString(),
                    });
                }

                qs = "SELECT r.MealID, i.Name FROM Recipes r " +
                    "INNER JOIN Ingredients i ON r.IngredientID = i.ID ORDER BY i.Name";

                command.CommandText = qs;

                dr.Close();

                using SqlDataReader dr2 = command.ExecuteReader();

                while (dr2.Read())
                {
                    recipes.Add(new RecipeElement()
                    {
                        MealID = short.Parse(dr2["MealID"].ToString()),
                        Ingredient = dr2["Name"].ToString()
                    });
                }

                foreach (var item in list)
                {
                    var temp = recipes.Where(x => x.MealID == item.MealID);
                    item.Ingredientlist = new List<string>();
                    foreach (var item2 in temp)
                    {
                        item.Ingredientlist.Add(item2.Ingredient);
                    }
                }
            }
            return list;
        }

        // GET: RecipesController/Edit/5
        public ActionResult Edit(short id)
        {
            RecipeEdit recipe = GetRecipeEditByID(id);
            if (recipe != null)
                return View(recipe);
            else return RedirectToAction(nameof(Index));
        }

        [NonAction]
        private RecipeEdit GetRecipeEditByID(short id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                RecipeEdit recipe = new RecipeEdit();
                recipe.IngredientsAdded = new Dictionary<short, string>();
                recipe.IngredientsNotAdded = new Dictionary<short, string>();
                string qs = "SELECT ID, Name FROM Meals WHERE ID = @ID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    recipe.MealID = short.Parse(dr["ID"].ToString());
                    recipe.MealName = dr["Name"].ToString();

                    command.Parameters.Clear();
                    qs = "SELECT r.IngredientID, i.Name FROM Recipes r " +
                        "INNER JOIN Ingredients i ON r.IngredientID = i.ID WHERE r.MealID = @ID";
                    command.CommandText = qs;

                    command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
                    command.Parameters["@ID"].Value = id;

                    dr.Close();
                    using SqlDataReader dr2 = command.ExecuteReader();

                    while (dr2.Read())
                    {
                        recipe.IngredientsAdded.Add(byte.Parse(dr2["IngredientID"].ToString()), dr2["Name"].ToString());
                    }

                    command.Parameters.Clear();
                    qs = "SELECT * FROM Ingredients";
                    command.CommandText = qs;

                    command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
                    command.Parameters["@ID"].Value = id;

                    dr2.Close();
                    using SqlDataReader dr3 = command.ExecuteReader();

                    while (dr3.Read())
                    {
                        if (!recipe.IngredientsAdded.ContainsKey(byte.Parse(dr3["ID"].ToString())))
                        {
                            recipe.IngredientsNotAdded.Add(byte.Parse(dr3["ID"].ToString()), dr3["Name"].ToString());
                        }
                    }
                    return recipe;
                }
            }
            return null;
        }

        // GET: RecipesController/RemoveIngredient/5
        public ActionResult RemoveIngredient(short IngredientID, short MealID)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "DELETE FROM Recipes WHERE MealID = @MealID AND IngredientID = @IngredientID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                command.Parameters.Add("@MealID", System.Data.SqlDbType.SmallInt);
                command.Parameters["@MealID"].Value = MealID;
                command.Parameters.Add("@IngredientID", System.Data.SqlDbType.SmallInt);
                command.Parameters["@IngredientID"].Value = IngredientID;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }

            }
            return RedirectToAction("Edit", new { id = MealID });
        }

        // GET: RecipesController/AddIngredient/5
        public ActionResult AddIngredient(short IngredientID, short MealID)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "INSERT INTO Recipes  ([MealID], [IngredientID]) VALUES(@MealID, @IngredientID)";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                command.Parameters.Add("@MealID", System.Data.SqlDbType.SmallInt);
                command.Parameters["@MealID"].Value = MealID;
                command.Parameters.Add("@IngredientID", System.Data.SqlDbType.SmallInt);
                command.Parameters["@IngredientID"].Value = IngredientID;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }

            }
            return RedirectToAction("Edit", new { id = MealID });
        }
    }
}
