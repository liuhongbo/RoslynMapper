using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RoslynMapper;
using RoslynMapper.Data;

namespace RoslynMapper.Samples
{
    /// <summary>
    /// use sampe database http://northwinddatabase.codeplex.com/
    /// </summary>
    public class DataReader : ISample
    {
        public string Name
        {
            get
            {
                return "Data Reader";
            }
        }

        public class Order
        {
            public int OrderID;
            public string CustomerID;
            public DateTime OrderDate;
            public Decimal Freight;
        }

        public void Run()
        {
            var mapper = RoslynMapper.MapEngine.DefaultInstance;

            var sql = @"select Orders.OrderID,Orders.CustomerID,OrderDate,Freight,ShipName, Employees.Photo as EmployeePhoto, Employees.Notes as EmployeeNotes, [Order Details].UnitPrice, Quantity,Discount,UnitsInStock,Discontinued from Orders inner join Customers on Orders.CustomerID=Customers.CustomerID left join Shippers on Orders.ShipVia = Shippers.ShipperID left join Employees on Orders.EmployeeID = Employees.EmployeeID left join [Order Details]  on Orders.OrderID=[Order Details].OrderID left join Products on [Order Details].ProductID = Products.ProductID";
            using (SqlConnection connection = new SqlConnection(@"Server=(localdb)\v11.0;Integrated Security=true;Initial Catalog=Northwind;"))
            {
                SqlCommand command =
                new SqlCommand(sql, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                reader.SetMapper<Order>(mapper);
                mapper.Build();
                while (reader.Read())
                {
                    var order = reader.Get<Order>();
                   
                    Console.WriteLine("OrderID:{0} CustomerID:{1} OrderDate:{2} Freight:{3}\r\n",order.OrderID,order.CustomerID,order.OrderDate,order.Freight );
                }
                reader.Close();
            }
        }
    }
}
