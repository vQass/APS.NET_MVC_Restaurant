using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models;
using Restauracja_MVC.Models.Zamowienia;
using Restauracja_MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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
            var newModelOfOrders = new zamowienieViewModel();
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
                qs = "SELECT Name, Surname, Phone, Address, CityID FROM UsersDetails WHERE ID = @ID";
                command.CommandText = qs;

                long id = long.Parse(User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt);
                command.Parameters["@ID"].Value = id;


                dr.Close();

                using SqlDataReader dr2 = command.ExecuteReader();


                if (dr2.Read())
                {
                    newModelOfOrders.NameOfUser = dr2["Name"].ToString();
                    newModelOfOrders.SurnameOfUser = dr2["Surname"].ToString();
                    newModelOfOrders.Phonex = dr2["Phone"].ToString();
                    newModelOfOrders.Addressx = dr2["Address"].ToString();
                    short tempCity = 1;
                    short.TryParse(dr2["CityID"].ToString(), out tempCity);
                    newModelOfOrders.CityIDx = tempCity;
                }
            }

            newModelOfOrders.CityList = GetCitySelectList();

            newModelOfOrders.listaZamowien = listOfFilteredMeals;
            return View(newModelOfOrders);
        }

        [NonAction]
        private List<SelectListItem> GetCitySelectList()
        {

            using var connection = new SqlConnection(connectionString);
            string qs = "SELECT * FROM Cities";
            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            var cities = new List<SelectListItem>();

            using SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                cities.Add(new SelectListItem { Value = dr["ID"].ToString(), Text = dr["Name"].ToString() });
            }
            return cities;
        }
    }
}
