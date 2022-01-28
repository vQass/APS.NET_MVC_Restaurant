using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models;
using Restauracja_MVC.Models.AccountViewModel;
using Restauracja_MVC.Providers;
using System;
using System.Collections.Generic;
using System.Data;
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
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = e.Message;
                    }
                }
            }
            return View();
        }

        // Probably should move it somewhere, no idea where yet
        [NonAction]
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

                                command = PrepareLoginCommand(connection, obj.Email);

                                dr.Close();
                                dr = command.ExecuteReader();

                                if (await SignInAsync(dr, obj.Email))
                                    return RedirectToAction("Index", "Home");


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

        [NonAction]
        private SqlCommand PreparePasswordCompareCommand(SqlConnection connection, UserLoginViewModel obj)
        {
            string qs = "SELECT [Password] FROM [dbo].[Users] WHERE [Email] = @Email";

            SqlCommand command = new SqlCommand(qs, connection);

            command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
            command.Parameters["@Email"].Value = obj.Email;

            return command;
        }

        [NonAction]
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

        [NonAction]
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

            var EditVM = new UserEditCitiesViewModel();

            EditVM.User = new UserEditViewModel();

            EditVM.User.Name = User.Claims.Where(x => x.Type.EndsWith("name")).FirstOrDefault().Value;
            EditVM.User.Surname = User.Claims.Where(x => x.Type.EndsWith("surname")).FirstOrDefault().Value;
            EditVM.User.City = User.Claims.Where(x => x.Type.EndsWith("locality")).FirstOrDefault().Value;
            EditVM.User.Address = User.Claims.Where(x => x.Type.EndsWith("streetaddress")).FirstOrDefault().Value;
            EditVM.User.Phone = User.Claims.Where(x => x.Type.EndsWith("mobilephone")).FirstOrDefault().Value;

            using (var connection = new SqlConnection(connectionString))
            {

                var command = PrepareCityListCommand(connection);


                try
                {
                    command.Connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    EditVM.Cities = GetCitySelectList(dr);

                    // In Claims we store name on city, in select we need id of that city
                    if (EditVM.User.City != null && EditVM.User.City != "")
                    {
                        EditVM.User.City = EditVM.Cities.Where(x => x.Text == EditVM.User.City).First().Value;
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }
            }

            return View(EditVM);
        }
        [NonAction]
        private SqlCommand PrepareCityListCommand(SqlConnection connection)
        {
            string qs = $"Select ID, Name FROM Cities";

            SqlCommand command = new SqlCommand(qs, connection);

            return command;
        }

        [NonAction]
        private List<SelectListItem> GetCitySelectList(SqlDataReader dr)
        {
            var cities = new List<SelectListItem>();
            cities.Add(new SelectListItem { Value = "", Text = "Brak" });

            while (dr.Read())
            {
                cities.Add(new SelectListItem { Value = dr["ID"].ToString(), Text = dr["Name"].ToString() });
            }
            return cities;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditCitiesViewModel EditVM)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(connectionString);

                long id = long.Parse(User.Claims.Where(x => x.Type.EndsWith("nameidentifier")).FirstOrDefault().Value);

                var command = PrepareEditCommand(connection, id, EditVM.User);

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
            return View(EditVM);
        }

        [NonAction]
        private SqlCommand PrepareEditCommand(SqlConnection connection, long id, UserEditViewModel user)
        {
            string qs = $"UPDATE UsersDetails SET Name = @Name, Surname = @Surname, Phone = @Phone, CityID = @CityID, Address = @Address WHERE ID = @ID";

            SqlCommand command = new SqlCommand(qs, connection);

            command.Parameters.Add("@ID", SqlDbType.BigInt);
            command.Parameters["@ID"].Value = id;

            command.Parameters.Add("@Name", SqlDbType.VarChar);
            command.Parameters["@Name"].IsNullable = true;
            command.Parameters["@Name"].Value = (object)user.Name ?? DBNull.Value;

            command.Parameters.Add("@Surname", SqlDbType.VarChar);
            command.Parameters["@Surname"].IsNullable = true;
            command.Parameters["@Surname"].Value = (object)user.Surname ?? DBNull.Value;

            command.Parameters.Add("@Phone", SqlDbType.VarChar);
            command.Parameters["@Phone"].IsNullable = true;
            command.Parameters["@Phone"].Value = (object)user.Phone ?? DBNull.Value;

            command.Parameters.Add("@CityID", SqlDbType.TinyInt);
            command.Parameters["@CityID"].IsNullable = true;
            command.Parameters["@CityID"].Value = (object)user.City ?? DBNull.Value;

            command.Parameters.Add("@Address", SqlDbType.VarChar);
            command.Parameters["@Address"].IsNullable = true;
            command.Parameters["@Address"].Value = (object)user.Address ?? DBNull.Value;

            return command;
        }
    }
}
