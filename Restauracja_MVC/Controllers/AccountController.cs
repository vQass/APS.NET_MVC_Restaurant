using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models;
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

        public AccountController(IConfiguration config, IUserManager userManager)
        {
            _config = config;
            _userManager = userManager;
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
                // Move to class or find other solution ASAP
                // W sumie na necie tak robia ale to chyba uselessowe poradniki

                string connectionString = _config.GetConnectionString("DbConnection");
                using (var connection = new SqlConnection(connectionString))
                {

                    string qs = "INSERT INTO [dbo].[Users]([Email],[Password],[IsActive],[Role])" +
                        "VALUES (@Email,@Password,1,3)";
                    SqlCommand command = new SqlCommand(qs, connection);

                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
                    command.Parameters["@Email"].Value = obj.Email;
                    command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar);
                    command.Parameters["@Password"].Value = Hasher.GenerateHash(obj.Password);
                    try
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        // Error
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            return View();
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

                string connectionString = _config.GetConnectionString("DbConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    string qs = "SELECT [Password] FROM [dbo].[Users] WHERE [Email] = @Email";

                    SqlCommand command = new SqlCommand(qs, connection);

                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
                    command.Parameters["@Email"].Value = obj.Email;

                    try
                    {
                        command.Connection.Open();
                        SqlDataReader dr = command.ExecuteReader();
                        if (dr.Read())
                        {
                            string dbPass = dr["Password"].ToString();
                            string loginPass = Hasher.GenerateHash(obj.Password);

                            if (dbPass == loginPass)
                            {
                                try
                                {

                                    //qs = "SELECT [u.ID], [Role], [Name], [Surname], [Phone], [CityID], [Address]" +
                                    //    " FROM Users AS u INNER JOIN UsersDetails AS ud" +
                                    //    " ON [u.ID] = [ud.ID] WHERE [Email] = @Email";

                                    qs = "SELECT u.ID, r.Name AS Role, ud.Name, Surname, Phone, c.Name AS City , Address" +
                                        " FROM Users AS u" +
                                        " INNER JOIN UsersDetails AS ud" +
                                        " ON u.ID = ud.ID" +
                                        " LEFT OUTER JOIN Cities AS c" +
                                        " ON ud.CityID = c.ID" +
                                        " INNER JOIN Roles AS r " +
                                        " ON u.Role = r.ID" +
                                        " WHERE Email = @Email";


                                    command.Parameters.Clear();

                                    command.CommandText = qs;

                                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar);
                                    command.Parameters["@Email"].Value = obj.Email;

                                    dr.Close();
                                    dr = command.ExecuteReader();

                                    if (dr.Read())
                                    {
                                        //ViewBag.Exception = dr.FieldCount + dr.GetName(0) + dr.GetName(1) +  dr.GetName(2) + dr.GetName(3) + dr.GetName(4) + dr.GetName(5) + dr.GetName(6);

                                        //return View();

                                        var user = new ApplicationUser(

                                            dr["ID"].ToString(),
                                            obj.Email,
                                            dr["Role"].ToString(),
                                            dr["Name"].ToString(),
                                            dr["Surname"].ToString(),
                                            dr["Phone"].ToString(),
                                            dr["City"].ToString(),
                                            dr["Address"].ToString()
                                        );

                                        await _userManager.SignIn(this.HttpContext, user, false);

                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                                catch (Exception e)
                                {
                                    // Error
                                    ViewBag.Exception = e.Message;
                                }
                            }
                        }
                    }
                    catch(Exception e)
                    {

                    }
                }
            }
            return View();
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
    }
}
