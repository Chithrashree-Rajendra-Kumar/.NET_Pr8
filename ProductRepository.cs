using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Database.NET.Models
{
    // MODEL (data access): pure ADO.NET using the connected model
    // SqlConnection + SqlCommand + SqlDataReader + SqlParameter
    public class ProductRepository
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString;

        // READ: all products
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT ProductId, ProductName, Category, Price, Quantity " +
                "FROM Product ORDER BY ProductId", con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        products.Add(Map(reader));
                }
            }
            return products;
        }

        // READ: one product (returns null if not found)
        public Product GetById(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT ProductId, ProductName, Category, Price, Quantity " +
                "FROM Product WHERE ProductId = @ProductId", con))
            {
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? Map(reader) : null;
                }
            }
        }

        // CREATE
        public void Add(Product p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO Product (ProductName, Category, Price, Quantity) " +
                "VALUES (@ProductName, @Category, @Price, @Quantity)", con))
            {
                AddParameters(cmd, p);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // UPDATE (returns false if the product does not exist)
        public bool Update(Product p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Product SET ProductName = @ProductName, Category = @Category, " +
                "Price = @Price, Quantity = @Quantity WHERE ProductId = @ProductId", con))
            {
                AddParameters(cmd, p);
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = p.ProductId;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // DELETE (returns false if the product does not exist)
        public bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM Product WHERE ProductId = @ProductId", con))
            {
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ---------- helpers ----------
        private static Product Map(SqlDataReader r)
        {
            return new Product
            {
                ProductId   = r.GetInt32(r.GetOrdinal("ProductId")),
                ProductName = r.GetString(r.GetOrdinal("ProductName")),
                Category    = r.GetString(r.GetOrdinal("Category")),
                Price       = r.GetDecimal(r.GetOrdinal("Price")),
                Quantity    = r.GetInt32(r.GetOrdinal("Quantity"))
            };
        }

        private static void AddParameters(SqlCommand cmd, Product p)
        {
            cmd.Parameters.Add("@ProductName", SqlDbType.VarChar, 100).Value = p.ProductName.Trim();
            cmd.Parameters.Add("@Category", SqlDbType.VarChar, 100).Value = p.Category.Trim();

            SqlParameter price = cmd.Parameters.Add("@Price", SqlDbType.Decimal);
            price.Precision = 10;
            price.Scale = 2;
            price.Value = p.Price;

            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = p.Quantity;
        }
    }
}
