using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Restauracja_MVC.Models.Orders;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {

        private readonly IConfiguration _config;
        private readonly string connectionString;

        public OrdersController(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("DbConnection");
        }

        // GET: OrdersController
        public ActionResult Index()
        {
            return View(GetOrderList());
        }

        [NonAction]
        private List<OrderListItem> GetOrderList()
        {
            var list = new List<OrderListItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                string qs = "SELECT od.OrderID, u.Email, od.OrderDate FROM OrdersDetails od " +
                    "INNER JOIN Users u ON od.UserID = u.ID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                using SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new OrderListItem()
                    {
                        OrderID = long.Parse(dr["OrderID"].ToString()),
                        Email = dr["Email"].ToString(),
                        OrderDate = dr["OrderDate"].ToString()
                    });
                }
            }
            return list;
        }

        // GET: OrdersController/Details/5
        public ActionResult Details(long id)
        {
            OrderDetails order = GetOrderDetailsByID(id);
            if (order != null)
                return View(order);
            else return RedirectToAction(nameof(Index));
        }

        [NonAction]
        private OrderDetails GetOrderDetailsByID(long id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                OrderDetails order = new OrderDetails();
                string qs = "SELECT od.OrderID, u.Email, c.Name AS City, od.Name, od.Surname, od.Address, " +
                    "od.Phone, od.TotalPrice, od.OrderDate FROM OrdersDetails od " +
                    "INNER JOIN Cities c ON od.CityID = c.ID " +
                    "INNER JOIN Users u ON u.ID = od.UserID " +
                    "WHERE od.OrderID = @ID";
                using var command = new SqlCommand(qs, connection);
                command.Connection.Open();

                command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt);
                command.Parameters["@ID"].Value = id;

                using SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    order.OrderID = long.Parse(dr["OrderID"].ToString());
                    order.Email = dr["Email"].ToString();
                    order.Name = dr["Name"].ToString();
                    order.Surname = dr["Surname"].ToString();
                    order.City = dr["City"].ToString();
                    order.Address = dr["Address"].ToString();
                    order.Phone = dr["Phone"].ToString();
                    order.TotalPrice = dr["TotalPrice"].ToString();
                    order.OrderDate = dr["OrderDate"].ToString();
                    order.OrderList = new Dictionary<string, byte>();
                    command.Parameters.Clear();
                    qs = "SELECT m.Name AS MealName, Amount FROM Orders o " +
                        "INNER JOIN Meals m ON o.MealID = m.ID WHERE o.ID = @ID";
                    command.CommandText = qs;

                    command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt);
                    command.Parameters["@ID"].Value = id;

                    dr.Close();
                    using SqlDataReader dr2 = command.ExecuteReader();

                    while (dr2.Read())
                    {
                        order.OrderList.Add(dr2["MealName"].ToString(), byte.Parse(dr2["Amount"].ToString()));
                    }
                    return order;
                }
            }
            return null;
        }

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            OrderDetails order = GetOrderDetailsByID(id);
            if (order != null)
                return View(order);
            else return RedirectToAction(nameof(Index));
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrderDetails order)
        {
            try
            {
                DeleteOrder(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return View();
        }

        [NonAction]
        private void DeleteOrder(long id)
        {
            using var connection = new SqlConnection(connectionString);
            string qs = $"DELETE FROM [dbo].[OrdersDetails] WHERE OrderID = @ID";

            using var command = new SqlCommand(qs, connection);
            command.Connection.Open();

            command.Parameters.Add("@ID", System.Data.SqlDbType.SmallInt);
            command.Parameters["@ID"].Value = id;

            command.ExecuteNonQuery();
        }

    }
}
