using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ADO_Example.Models;

namespace ADO_Example.DAL
{
    public class ProductDAL
    {

        string conString = ConfigurationManager.ConnectionStrings["adoConnectionstring"].ToString();

        //Get ALL Products
        public List<Product> GetAllProducts()
        {
            List<Product> productsList = new List<Product>(); 
            
            using(SqlConnection connection = new SqlConnection(conString))
            {
               
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProduct= new DataTable();
                connection.Open();
                sqlDA.Fill(dtProduct);
                connection.Close();

                foreach (DataRow dr in dtProduct.Rows)
                {
                    productsList.Add(new Product
                    {
                        ProductID =Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Quanty"]),
                        Remarks = dr["Remarks"].ToString()

                    }) ;
                    
                }

            }

            return productsList;
        }

        //Insert Products
        public bool InsertProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertProduct",connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quanty", product.Qty);
                command.Parameters.AddWithValue("@Remarks",product.Remarks);

                connection.Open();
               id= command.ExecuteNonQuery();
                connection.Close();

            }
            if(id>0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        //get product by productid
        public List<Product> GetProductById(int ProductId)
        {
            List<Product> productsList = new List<Product>();

            using (SqlConnection connection = new SqlConnection(conString))
            {

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductById";
                command.Parameters.AddWithValue("@Productid", ProductId);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProduct = new DataTable();
                connection.Open();
                sqlDA.Fill(dtProduct);
                connection.Close();

                foreach (DataRow dr in dtProduct.Rows)
                {
                    productsList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Quanty"]),
                        Remarks = dr["Remarks"].ToString()

                    });

                }

            }

            return productsList;
        }

        //UPdate product details

        public bool UpdateProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProduct", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@productid", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Quanty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();

            }
            if (id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //delete product
        public string DeleteProduct(int productid)
        {
            string result = "";
            using(SqlConnection connection = new SqlConnection(conString)) { 
            SqlCommand command = new SqlCommand("sp_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Productid", productid);
                command.Parameters.Add("@Return", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@Return"].Value.ToString();
                connection.Close();
            }
            return result;  
        }
    }

}