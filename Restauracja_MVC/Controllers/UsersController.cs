using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models.Users;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Restauracja_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {

        private readonly IConfiguration _config;
        private readonly string connectionString;

        public UsersController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("DbConnection");
        }

        // GET: UsersController
        public ActionResult Index()
        {
            if(TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
                TempData["Error"] = null;
            }
            return View(GetUsersList());
        }

        [NonAction]
        private List<UserListItem> GetUsersList()
        {
            var list = new List<UserListItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT ID, Email FROM Users WHERE IsActive = 1";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new UserListItem()
                    {
                        ID = short.Parse(dr["ID"].ToString()),
                        Email = dr["Email"].ToString()
                    });
                }
            }
            return list;
        }

        // GET: UsersController/Details/5
        public ActionResult Details(long id)
        {
            UserDetails user = GetUserDetailsByID(id);
            if (user != null)
            {
                if (user.City != "")
                {
                    user.City = GetCityName(user.City);
                }
                return View(user);
            }
            else return RedirectToAction(nameof(Index));
        }


        [NonAction]
        private UserDetails GetUserDetailsByID(long id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT u.ID, u.Email, ud.Name, ud.Surname, r.Name AS Role, ud.Phone, ud.CityID AS City, ud.Address, ud.CreateDate, ud.UpdateDate FROM Users u " +
                    "INNER JOIN UsersDetails ud ON u.ID = ud.ID " +
                    "INNER JOIN Roles r ON u.Role = r.ID " +
                    "WHERE u.ID = @ID AND IsActive = 1";

                using var command = new SqlCommand(qs, connection);

                command.Connection.Open();

                command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    return new UserDetails()
                    {
                        ID = long.Parse(dr["ID"].ToString()),
                        Email = dr["Email"].ToString(),
                        Name = dr["Name"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        Role = dr["Role"].ToString(),
                        Phone = dr["Phone"].ToString(),
                        City = dr["City"].ToString(),
                        Address = dr["Address"].ToString(),
                        CreateDate = dr["CreateDate"].ToString(),
                        UpdateDate = dr["UpdateDate"].ToString(),
                    };
                }
            }
            return null;
        }


        [NonAction]
        private string GetCityName(string city)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = $"SELECT Name FROM Cities WHERE ID = {city}";

                using var command = new SqlCommand(qs, connection);

                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    return dr["Name"].ToString();
                }
            }
            return null;
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(long id)
        {
            UserEdit user = GetUserEditByID(id);
            if (user != null)
            {
                user.Cities = GetCitySelectList();
                user.Roles = GetRoleSelectList();
                return View(user);
            }
            else return RedirectToAction(nameof(Index));
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserEdit user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (CheckUniqueEmail(user.Email))
                    {
                        UpdateUser(id, user);
                    }
                    else
                    {
                        TempData["Error"] = "Wprowadzony email jest już w bazie danych";
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
        private bool CheckUniqueEmail(string email)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT Name FROM Cities WHERE Name = @Name";
                using var command = new SqlCommand(qs, connection);

                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                command.Parameters["@Name"].Value = email;

                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                return !dr.HasRows;
            }
            return false;
        }


        [NonAction]
        private UserEdit GetUserEditByID(long id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT u.ID, u.Email, ud.Name, ud.Surname, u.Role, ud.Phone, ud.CityID AS City, ud.Address FROM Users u " +
                    "INNER JOIN UsersDetails ud ON u.ID = ud.ID WHERE u.ID = @ID AND IsActive = 1";

                using var command = new SqlCommand(qs, connection);

                command.Connection.Open();

                command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    byte temp;
                    return new UserEdit()
                    {
                        ID = long.Parse(dr["ID"].ToString()),
                        Email = dr["Email"].ToString(),
                        Name = dr["Name"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        Role = byte.Parse(dr["Role"].ToString()),
                        Phone = dr["Phone"].ToString(),
                        City = byte.TryParse(dr["City"].ToString(), out temp) ? temp : null,
                        Address = dr["Address"].ToString()
                    };
                }
            }
            return null;
        }

        [NonAction]
        private List<SelectListItem> GetCitySelectList()
        {

            using var connection = new SqlConnection(connectionString);
            string qs = "SELECT * FROM Cities";
            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            var cities = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Brak" }
                };

            using SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                cities.Add(new SelectListItem { Value = dr["ID"].ToString(), Text = dr["Name"].ToString() });
            }
            return cities;
        }

        [NonAction]
        private List<SelectListItem> GetRoleSelectList()
        {

            using var connection = new SqlConnection(connectionString);
            string qs = "SELECT * FROM Roles";
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


        [NonAction]
        public void UpdateUser(long id, UserEdit user)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = $"UPDATE Users SET Email = @Email, Role = @Role WHERE ID = @ID AND IsActive = 1";

            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();
            command.Transaction = connection.BeginTransaction();

            command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt);
            command.Parameters["@ID"].Value = id;

            command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
            command.Parameters["@Email"].Value = user.Email;

            command.Parameters.Add("@Role", System.Data.SqlDbType.TinyInt);
            command.Parameters["@Role"].Value = (object)user.Role;

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }

            qs = $"UPDATE UsersDetails SET Name = @Name, Surname = @Surname, Phone = @Phone, CityID = @CityID, Address = @Address WHERE ID = @ID";

            command.Parameters.Clear();
            command.CommandText = qs;

            command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt);
            command.Parameters["@ID"].Value = id;

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].IsNullable = true;
            command.Parameters["@Name"].Value = (object)user.Name ?? DBNull.Value;

            command.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar);
            command.Parameters["@Surname"].IsNullable = true;
            command.Parameters["@Surname"].Value = (object)user.Surname ?? DBNull.Value;

            command.Parameters.Add("@Phone", System.Data.SqlDbType.VarChar);
            command.Parameters["@Phone"].IsNullable = true;
            command.Parameters["@Phone"].Value = (object)user.Phone ?? DBNull.Value;

            command.Parameters.Add("@CityID", System.Data.SqlDbType.TinyInt);
            command.Parameters["@CityID"].IsNullable = true;
            command.Parameters["@CityID"].Value = (object)user.City ?? DBNull.Value;

            command.Parameters.Add("@Address", System.Data.SqlDbType.VarChar);
            command.Parameters["@Address"].IsNullable = true;
            command.Parameters["@Address"].Value = (object)user.Address ?? DBNull.Value;

            try
            {
                command.ExecuteNonQuery();
                command.Transaction.Commit();
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                command.Transaction.Rollback();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(long id)
        {
            if (id == long.Parse(User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value))
                return RedirectToAction(nameof(Index));
            UserDetails user = GetUserDetailsByID(id);
            if (user != null)
                return View(user);
            else return RedirectToAction(nameof(Index));
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, UserDetails user)
        {
            try
            {
                DeleteUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return View();
        }

        [NonAction]
        private void DeleteUser(long id)
        {
            using var connection = new SqlConnection(connectionString);

            string qs = $"UPDATE Users SET IsActive = 0 WHERE ID = @ID";

            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt);
            command.Parameters["@ID"].Value = id;

            command.ExecuteNonQuery();
        }
    }
}
