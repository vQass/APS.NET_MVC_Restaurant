using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models.Cities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Controllers
{
    public class CitiesController : Controller
    {

        private readonly IConfiguration _config;
        private readonly string connectionString;

        public CitiesController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("DbConnection");
        }

        // GET: CityController
        public ActionResult Index()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
                TempData["Error"] = null;
            }
            return View(GetCityList());
        }

        [NonAction]
        private List<CityListItem> GetCityList()
        {
            var list = new List<CityListItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT * FROM Cities";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CityListItem()
                    {
                        ID = byte.Parse(dr["ID"].ToString()),
                        Name = dr["Name"].ToString()
                    });
                }
            }
            return list;
        }

        // GET: CityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CityCreate city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (CheckUniqueCity(city.Name))
                    {
                        AddCity(city);
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
        private bool CheckUniqueCity(string name, byte id = 0)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT Name FROM Cities WHERE Name = @Name AND ID != @ID";
                using var command = new SqlCommand(qs, connection);

                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                command.Parameters["@Name"].Value = name;
                command.Parameters.Add("@ID", System.Data.SqlDbType.TinyInt);
                command.Parameters["@ID"].Value = id;

                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();

                if(dr.HasRows)
                {
                    TempData["Error"] = "Wprowadzona nazwa jest już zajęta";
                    return false;
                }
            }
            return true;
        }


        [NonAction]
        private bool CheckIfCityInUse(byte id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT Name FROM UsersDetails WHERE CityID = @ID";
                using var command = new SqlCommand(qs, connection);

                command.Parameters.Add("@ID", System.Data.SqlDbType.TinyInt);
                command.Parameters["@ID"].Value = id;

                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                if(dr.HasRows)
                {
                    TempData["Error"] = "Miasto używane przez użytkowników";
                    return true;
                }

                dr.Close();

                command.Parameters.Clear();
                
                qs = "SELECT Name FROM OrdersDetails WHERE CityID = @ID";
                
                command.CommandText = qs;

                command.Parameters.Add("@ID", System.Data.SqlDbType.TinyInt);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr2 = command.ExecuteReader();

                if(dr2.HasRows)
                {
                    TempData["Error"] = "Miasto używane w zamówieniach";
                    return true ;
                }
            }
            return false;
        }

        [NonAction]
        private void AddCity(CityCreate city)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = "INSERT INTO [dbo].[Cities] ([Name])" +
            "VALUES (@Name)";
            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].Value = city.Name;

            command.ExecuteNonQuery();
        }

        // GET: CityController/Edit/5
        public ActionResult Edit(byte id)
        {
            CityCreate meal = GetCityCreateByID(id);
            if (meal != null)
                return View(meal);
            else return RedirectToAction(nameof(Index));
        }

        // POST: CityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(byte id, CityCreate city)
        {
            try
            {
                if (CheckUniqueCity(city.Name, id) && !CheckIfCityInUse(id))
                {
                    UpdateCity(id, city);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private void UpdateCity(byte id, CityCreate city)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = "UPDATE Cities " +
                "SET [Name] = @Name WHERE ID = @ID";
            
            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].Value = city.Name;
            command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
            command.Parameters["@ID"].Value = id;

            command.ExecuteNonQuery();
        }

        [NonAction]
        private CityCreate GetCityCreateByID(byte id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT Name FROM Cities WHERE ID = @ID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                command.Parameters.Add("@ID", System.Data.SqlDbType.TinyInt);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    return new CityCreate()
                    {
                        Name = dr["Name"].ToString()
                    };
                }
            }
            return null;
        }

        // GET: CityController/Delete/5
        public ActionResult Delete(byte id)
        {
            if(!CheckIfCityInUse(id))
            {
                DeleteCity(id);
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        private void DeleteCity(short id)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = $"DELETE FROM Cities WHERE id = @ID";

            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@ID", System.Data.SqlDbType.TinyInt);
            command.Parameters["@ID"].Value = id;

            command.ExecuteNonQuery();
        }
    }
}
