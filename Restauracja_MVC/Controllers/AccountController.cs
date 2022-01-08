﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models;
using Restauracja_MVC.Models.AccountViewModel;
using Restauracja_MVC.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserManager _userManager;
        private readonly string connectionString;

        public AccountController(IConfiguration config, IUserManager userManager)
        {
            _config = config;
            _userManager = userManager;
            connectionString = _config.GetConnectionString("DbConnection");
        }

        public IActionResult Index()
        {
            return View("Register");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterViewModel obj)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var command = PrepareRegisterCommand(connection, obj);
                    try
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = e.Message;
                        return View();
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Probably should move it somewhere, no idea where yet
        private SqlCommand PrepareRegisterCommand(SqlConnection connection, UserRegisterViewModel obj)
        {
            string qs = "INSERT INTO [dbo].[Users]([Email],[Password],[IsActive],[Role])" +
                "VALUES (@Email,@Password,1,3)";
            SqlCommand command = new SqlCommand(qs, connection);

            command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
            command.Parameters["@Email"].Value = obj.Email;
            command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar);
            command.Parameters["@Password"].Value = Hasher.GenerateHash(obj.Password);
            return command;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel obj)
        {
            if (ModelState.IsValid)
            {

                using (var connection = new SqlConnection(connectionString))
                {
                    /*using(would it work if 2nd command had other name???)*/
                    var command = PreparePasswordCompareCommand(connection, obj);
                    try
                    {
                        command.Connection.Open();
                        SqlDataReader dr = command.ExecuteReader();
                        if (dr.Read())
                        {
                            // Password compare
                            if (dr["Password"].ToString() == Hasher.GenerateHash(obj.Password))
                            {
                                try
                                {
                                    command = PrepareLoginCommand(connection, obj.Email);

                                    dr.Close();
                                    dr = command.ExecuteReader();

                                    if (await SignInAsync(dr, obj.Email))
                                        return RedirectToAction("Index", "Home");

                                }
                                catch (Exception e)
                                {
                                    ViewBag.Exception = e.Message;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = e.Message;
                    }
                }
            }
            return View();
        }

        private SqlCommand PreparePasswordCompareCommand(SqlConnection connection, UserLoginViewModel obj)
        {
            string qs = "SELECT [Password] FROM [dbo].[Users] WHERE [Email] = @Email";

            SqlCommand command = new SqlCommand(qs, connection);

            command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
            command.Parameters["@Email"].Value = obj.Email;

            return command;
        }

        private SqlCommand PrepareLoginCommand(SqlConnection connection, string email)
        {
            string qs = "SELECT u.ID, r.Name AS Role, ud.Name, Surname, Phone, c.Name AS City , Address" +
                                    " FROM Users AS u" +
                                    " INNER JOIN UsersDetails AS ud" +
                                    " ON u.ID = ud.ID" +
                                    " LEFT OUTER JOIN Cities AS c" +
                                    " ON ud.CityID = c.ID" +
                                    " INNER JOIN Roles AS r " +
                                    " ON u.Role = r.ID" +
                                    " WHERE Email = @Email";

            SqlCommand command = new SqlCommand(qs, connection);

            command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
            command.Parameters["@Email"].Value = email;

            return command;
        }

        public async Task<bool> SignInAsync(SqlDataReader dr, string email)
        {
            if (dr.Read())
            {
                var user = new ApplicationUser(

                    dr["ID"].ToString(),
                    email,
                    dr["Role"].ToString(),
                    dr["Name"].ToString(),
                    dr["Surname"].ToString(),
                    dr["Phone"].ToString(),
                    dr["City"].ToString(),
                    dr["Address"].ToString()
                );

                await _userManager.SignIn(this.HttpContext, user, false);
                return true;
            }
            return false;
        }


        public async Task<IActionResult> LogoutAsync()
        {
            await _userManager.SignOut(this.HttpContext);
            return RedirectPermanent("~/Home/Index");
        }


        [Authorize]
        public IActionResult Profile()
        {
            return View(this.User.Claims.ToDictionary(x => x.Type, x => x.Value));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit()
        {

            var user = new UserEditViewModel();

            user.Name = User.Claims.Where(x => x.Type.EndsWith("name")).FirstOrDefault().Value;
            user.Surname = User.Claims.Where(x => x.Type.EndsWith("surname")).FirstOrDefault().Value;
            user.City = User.Claims.Where(x => x.Type.EndsWith("locality")).FirstOrDefault().Value;
            user.Address = User.Claims.Where(x => x.Type.EndsWith("streetaddress")).FirstOrDefault().Value;
            user.Phone = User.Claims.Where(x => x.Type.EndsWith("mobilephone")).FirstOrDefault().Value;

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel user)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string id = User.Claims.Where(x => x.Type.EndsWith("nameidentifier")).FirstOrDefault().Value;

                    var command = PrepareEditCommand(connection, id, user);

                    try
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();

                        string email = User.Claims.Where(x => x.Type.EndsWith("emailaddress")).FirstOrDefault().Value;

                        await _userManager.SignOut(this.HttpContext);
                        
                        command = PrepareLoginCommand(connection, email);

                        SqlDataReader dr = command.ExecuteReader();

                        if (await SignInAsync(dr, email))
                            return RedirectToAction("Profile");
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = e.Message;
                    }
                }
            }
            return View(user);
        }

        private SqlCommand PrepareEditCommand(SqlConnection connection, string id, UserEditViewModel user)
        {
            string qs = $"UPDATE UsersDetails SET Name = @Name, Surname = @Surname, Phone = @Phone, CityID = @CityID, Address = @Address WHERE ID = {id}";

            SqlCommand command = new SqlCommand(qs, connection);

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
            command.Parameters["@Name"].IsNullable = true;
            command.Parameters["@Name"].Value = (object)user.Name ?? DBNull.Value;

            command.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar);
            command.Parameters["@Surname"].IsNullable = true;
            command.Parameters["@Surname"].Value = (object)user.Surname ?? DBNull.Value;

            command.Parameters.Add("@Phone", System.Data.SqlDbType.VarChar);
            command.Parameters["@Phone"].IsNullable = true;
            command.Parameters["@Phone"].Value = (object)user.Phone ?? DBNull.Value;

            command.Parameters.Add("@CityID", System.Data.SqlDbType.SmallInt);
            command.Parameters["@CityID"].IsNullable = true;
            command.Parameters["@CityID"].Value = (object)user.City ?? DBNull.Value;

            command.Parameters.Add("@Address", System.Data.SqlDbType.VarChar);
            command.Parameters["@Address"].IsNullable = true;
            command.Parameters["@Address"].Value = (object)user.Address ?? DBNull.Value;

            return command;
        }

    }


}
