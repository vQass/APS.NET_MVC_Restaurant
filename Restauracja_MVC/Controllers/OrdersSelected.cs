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
        public IActionResult Index(List<Zamowienie> list)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Zamowienia");
            }
            else
            {
                List<Zamowienie> zamowienieFinalne = new List<Zamowienie>();
                foreach (var item in list)
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
                    qsSetOrderDetails += "(" + (MaxOrderID + 1) + "," + CurrentUser + "," + list[0].CityIDx + "," + "\'" + list[0].NameOfUser + "\'" + "," + "\'" + list[0].SurnameOfUser + "\'," + "\'" + list[0].Addressx + "\'," + "\'" + list[0].Phonex + "\'," + wartoscZamowienia + ");";

                    using var commandSetOrderDetails = new SqlCommand(qsSetOrderDetails, connectionSetOrderDetails);
                    commandSetOrderDetails.Connection.Open();

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
                        if ((zamowienieFinalne.Count() == 1))
                        {
                            qsSetOrders += " (" + (MaxOrderID + 1) + "," + item.IDx + "," + item.Amountx + ");";
                        }
                        else if (item == zamowienieFinalne.Last())
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

                return View("~/Views/Home/Index.cshtml");
            }
        }
    }
}
