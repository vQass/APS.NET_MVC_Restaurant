using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models;
using Restauracja_MVC.Models.Meals;
using Restauracja_MVC.Models.Zamowienia;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Controllers
{
    public class ZamowieniaController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string connectionString;


        public ZamowieniaController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("DbConnection");
        }
        public IActionResult Index()
        {
            List<Skladniki> numeryId = new List<Skladniki>();
            var list = new List<Skladniki>();
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT ID, Name FROM Ingredients";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Skladniki()
                    {
                        IDsk = Int16.Parse(dr["ID"].ToString()),
                        Namesk = dr["Name"].ToString(),
                    });
                }
            }
            return View(list);
        }

        [HttpPost]
        public IActionResult FilteredMeals(List<Skladniki> list)
        {
            List<Zamowienie> listOfFilteredMeals = new List<Zamowienie>();
            List<string> tab = new List<string>();
            string qs = "";
            foreach (var item in list)
            {
                if (item.isCheckedsk == true)
                {
                    tab.Add(" i.ID = " + item.IDsk.ToString()); // Lista w której są ID składników zaznaczonych przez usera
                }
            }

            using (var connection = new SqlConnection(connectionString))
            {
                if (tab.Count > 0)
                {
                    var last = tab.Last();
                    qs = "SELECT m.ID, m.Name, m.Price, m.Description, m.Category FROM Meals m " +
                                "WHERE NOT EXISTS " +
                                "(SELECT i.ID FROM Ingredients " +
                                "INNER JOIN Recipes r ON m.ID = r.MealID " +
                                "INNER JOIN Ingredients i ON r.IngredientID = i.ID " +
                                "WHERE ";
                    foreach (string skladnik in tab)
                    {
                        if (skladnik == last)
                        {
                            qs += skladnik + ")";
                        }
                        else
                        {
                            qs += skladnik + " OR ";
                        }
                    }
                }
                else
                {
                    qs = "SELECT m.ID, m.Name, m.Price, m.Description, m.Category FROM Meals m";
                }
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    listOfFilteredMeals.Add(new Zamowienie()
                    {
                        IDx = Int16.Parse(dr["ID"].ToString()),
                        NameOfMeal = dr["Name"].ToString(),
                        Pricex = float.Parse(dr["Price"].ToString()),
                        Descriptionx = dr["Description"].ToString(),
                        Categoryx = dr["Category"].ToString()
                    });
                }
            }
            return View(listOfFilteredMeals);
        }

    }
}
