using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models.Zamowienia;
using Restauracja_MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Restauracja_MVC.Controllers
{
    public class OrdersSelected : Controller
    {
        private readonly IConfiguration _config;
        private readonly string connectionString;


        public OrdersSelected(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("DbConnection");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(zamowienieViewModel vm)
        {
            if (!ModelState.IsValid || vm.listaZamowien == null || !vm.listaZamowien.Exists(x => x.Amountx != 0))
            {
                TempData["FailedForm"] = "Formularz zostal blednie wypelniony";
                return RedirectToAction("Index", "Zamowienia");
            }
            else
            {
                List<Zamowienie> zamowienieFinalne = new List<Zamowienie>();
                foreach (var item in vm.listaZamowien)
                {

                    if (item.Amountx != 0)
                    {
                        zamowienieFinalne.Add(item);
                    }
                }

                //////////////////////////////////////////////////////////////////////
                /////////////////Pobieranie max ID ordersDetails//////////////////////
                //////////////////////////////////////////////////////////////////////

                long MaxOrderID = 0;
                using (var connectionMaxID = new SqlConnection(connectionString))
                {
                    string qsMaxID = "SELECT MAX(OrderID) AS OrderID FROM OrdersDetails";

                    using var commandMaxID = new SqlCommand(qsMaxID, connectionMaxID);
                    commandMaxID.Connection.Open();

                    using SqlDataReader drMaxID = commandMaxID.ExecuteReader();

                    if (drMaxID.Read())
                    {
                        long.TryParse(drMaxID["OrderID"].ToString(), out MaxOrderID);
                    }


                    connectionMaxID.Close();
                }


                //////////////////////////////////////////////////////////////////////
                /////////////Wysłanie zamówienia do tabeli OrdersDetails//////////////
                //////////////////////////////////////////////////////////////////////
                using (var connectionSetOrderDetails = new SqlConnection(connectionString))
                {
                    float wartoscZamowienia = 0;
                    string qsSetOrderDetails = "INSERT INTO OrdersDetails (" + "\"" + "OrderID" + "\"" + ",\"" + "UserID" + "\"," + "\"" + "CityID" + "\"," + "\"" + "Name" + "\"," + "\"" + "Surname" + "\"," + "\"" + "Address" + "\"," + "\"" + "Phone" + "\"," + "\"" + "TotalPrice" + "\"" + ") VALUES ";

                    for (int i = 0; i < zamowienieFinalne.Count(); i++)
                    {
                        wartoscZamowienia += zamowienieFinalne[i].Amountx * zamowienieFinalne[i].Pricex;
                    }
                    wartoscZamowienia.ToString("0.00");
                    String CurrentUser = User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
                    qsSetOrderDetails += "(" + (MaxOrderID + 1) + "," + CurrentUser + ", @CityIDx, @NameOfUser, @SurnameOfUser, @Addressx, @Phonex," + wartoscZamowienia + ");";

                    using var commandSetOrderDetails = new SqlCommand(qsSetOrderDetails, connectionSetOrderDetails);
                    commandSetOrderDetails.Connection.Open();

                    commandSetOrderDetails.Parameters.Add("@CityIDx", System.Data.SqlDbType.Int);
                    commandSetOrderDetails.Parameters["@CityIDx"].Value = vm.CityIDx;

                    commandSetOrderDetails.Parameters.Add("@NameOfUser", System.Data.SqlDbType.VarChar);
                    commandSetOrderDetails.Parameters["@NameOfUser"].Value = vm.NameOfUser;

                    commandSetOrderDetails.Parameters.Add("@SurnameOfUser", System.Data.SqlDbType.VarChar);
                    commandSetOrderDetails.Parameters["@SurnameOfUser"].Value = vm.SurnameOfUser;

                    commandSetOrderDetails.Parameters.Add("@Addressx", System.Data.SqlDbType.VarChar);
                    commandSetOrderDetails.Parameters["@Addressx"].Value = vm.Addressx;

                    commandSetOrderDetails.Parameters.Add("@Phonex", System.Data.SqlDbType.VarChar);
                    commandSetOrderDetails.Parameters["@Phonex"].Value = vm.Phonex;

                    using SqlDataReader dr3 = commandSetOrderDetails.ExecuteReader();

                    connectionSetOrderDetails.Close();
                }

                //////////////////////////////////////////////////////////////////////
                /////////////////////Dodanie do tabeli Orders/////////////////////////
                //////////////////////////////////////////////////////////////////////

                using (var connectionSetOrders = new SqlConnection(connectionString))
                {
                    string qsSetOrders = "INSERT INTO Orders VALUES ";
                    foreach (var item in zamowienieFinalne)
                    {
                        if ((zamowienieFinalne.Count == 1) || item == zamowienieFinalne.Last())
                        {
                            qsSetOrders += " (" + (MaxOrderID + 1) + "," + item.IDx + "," + item.Amountx + ");";
                        }
                        else
                        {
                            qsSetOrders += " (" + (MaxOrderID + 1) + "," + item.IDx + "," + item.Amountx + "),";
                        }
                    }
                    using var commandSetOrders = new SqlCommand(qsSetOrders, connectionSetOrders);
                    commandSetOrders.Connection.Open();

                    using SqlDataReader drSetOrders = commandSetOrders.ExecuteReader();

                    connectionSetOrders.Close();
                }

                ViewBag.Success = true;
                return View("~/Views/Home/Index.cshtml");
            }
        }
    }
}
