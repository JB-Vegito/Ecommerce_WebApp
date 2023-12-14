using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Data;
// var builder = WebApplication.CreateBuilder(args);

namespace ecommerce_web.Models{
    public static class DataConnection{
        public static void UserRegistration(Users user){
            string? username = user.username;
            string? password = user.password;
            using(SqlConnection connection = new SqlConnection()){
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                try{
                connection.Open();
                string sql = "INSERT INTO users Values('"+username+"','"+password+"')";
                SqlCommand command = new SqlCommand(sql,connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Record Inserted");
                connection.Close();}
                catch(Exception exception){
                    Console.WriteLine("Record not inserted !!!" + exception);
                }          
                // finally{
                //     connection.Close();
                // }          
            }
        }

        public static int UserLogin(Users user){
            string? username = user.username;
            string? password = user.password;
            int error_status=1;

            using(SqlConnection connection = new SqlConnection()){
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                try{
                    connection.Open();
                    string sql = "SELECT * FROM users";
                    SqlCommand command = new SqlCommand(sql,connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read()){
                        if(reader.GetString(0)==username && reader.GetString(1)==password){
                            error_status=0;
                        }
                    }
                    connection.Close();
                }
                catch(SqlException){
                    Console.WriteLine("Sql exception occurred !!");
                }
                return error_status;
            }
        }

        public static void AdminCategoryUpdate(Category category){
            // int status=0;
            if(category==null){
                Console.WriteLine("The category object is empty !!");
            }
            else{
                using(SqlConnection connection = new SqlConnection()){
                    connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                    try{
                        connection.Open();
                        string sql = "Update Categories SET CategoryName='" + category.CategoryName +"' WHERE CategoryId ='" + category.CategoryId +"'";
                        SqlCommand command = new SqlCommand(sql,connection);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Record Updated");
                        // status=1;
                    }
                    catch(SqlException sqlexception){
                        Console.WriteLine("Exception occurred. Record Not Updated !!");
                        Console.WriteLine(sqlexception);
                    }
                    finally{
                        connection.Close();
                    }
                }
            }  
            // return status;
        }
        public static void AdminProductUpdate(Inventory product){
            if(product==null){
                Console.WriteLine("The product object is empty !!");
            }
            else{
                using(SqlConnection connection = new SqlConnection()){
                    connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                    try{
                        connection.Open();
                        string sql = "UPDATE Inventories SET Name='"+product.Name+"',FullName='"+product.FullName+"',Description='"+product.Description+"',Price="+product.Price.ToString()+",Quantity="+product.Quantity.ToString()+",CategoryId='"+product.CategoryId+"' WHERE Id="+product.Id;
                        SqlCommand command = new SqlCommand(sql,connection);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Record updated");
                    }
                    catch(SqlException sqlexception){
                        Console.WriteLine("Exception occurred. Record not updated !!");
                        Console.WriteLine(sqlexception);
                    }
                    finally{
                        connection.Close();
                    }
                }
            }
        }
        public static int getCartPrice(string? username){
            int totalPrice=0;
            using(SqlConnection connection = new SqlConnection()){
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                try{
                    connection.Open();
                    string sql = "SELECT Carts.Item_Quantity,Inventories.Price FROM Carts JOIN Inventories ON Carts.Product_Id=Inventories.Id AND Username='"+username+"'";
                    SqlCommand command = new SqlCommand(sql,connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read()){
                        totalPrice+= reader.GetInt32(0) * reader.GetInt32(1);
                    }                    
                }
                catch(SqlException){
                    Console.WriteLine("Sql exception occurred !!");
                }    
                finally{
                    connection.Close();
                }            
            }
            return totalPrice;
        }
        public static void updateQuantityUp(int id){
            // var builder = WebApplication.CreateBuilder();
            Console.WriteLine(id);
            int quantity=0;
            using(SqlConnection connection = new SqlConnection()){
                // connection.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");                
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                try{
                    connection.Open();
                    string sql1 = "SELECT Item_Quantity FROM Carts WHERE Cart_Id="+Convert.ToString(id);
                    SqlCommand command1 = new SqlCommand(sql1,connection);
                    SqlDataReader reader1 = command1.ExecuteReader();                    
                    while(reader1.Read()){
                        quantity = reader1.GetInt32(0);
                    }
                    if(quantity>-1){quantity+=1;}                    
                    Console.WriteLine(quantity);
                    connection.Close();
                    connection.Open();
                    string sql2 = "UPDATE Carts SET Item_Quantity="+Convert.ToString(quantity)+"WHERE Cart_Id="+Convert.ToString(id);                    
                    SqlCommand command2 = new SqlCommand(sql2,connection);
                    command2.ExecuteNonQuery();
                    Console.WriteLine("Record Updated");
                }
                catch(SqlException e){
                    Console.WriteLine("Exception occurred. Record not updated !!!");
                    Console.WriteLine(e);
                }
                finally{
                    connection.Close();
                }
            }
        }
        public static void updateQuantityDown(int id){
            // var builder = WebApplication.CreateBuilder();
            Console.WriteLine(id);
            int quantity=0;
            using(SqlConnection connection = new SqlConnection()){
                // connection.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");                
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                try{
                    connection.Open();
                    string sql1 = "SELECT Item_Quantity FROM Carts WHERE Cart_Id="+Convert.ToString(id);
                    SqlCommand command1 = new SqlCommand(sql1,connection);
                    SqlDataReader reader1 = command1.ExecuteReader();                    
                    while(reader1.Read()){
                        quantity = reader1.GetInt32(0);
                    }
                    if(quantity!=0){quantity-=1;}                    
                    Console.WriteLine(quantity);
                    connection.Close();
                    connection.Open();
                    string sql2 = "UPDATE Carts SET Item_Quantity="+Convert.ToString(quantity)+"WHERE Cart_Id="+Convert.ToString(id);                    
                    SqlCommand command2 = new SqlCommand(sql2,connection);
                    command2.ExecuteNonQuery();
                    Console.WriteLine("Record Updated");
                }
                catch(SqlException e){
                    Console.WriteLine("Exception occurred. Record not updated !!!");
                    Console.WriteLine(e);
                }
                finally{
                    connection.Close();
                }
            }
        }       
        public static void ClearCart(string? username){
            
            using(SqlConnection connection = new SqlConnection()){
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                try{
                    connection.Open();
                    string sql = "DELETE FROM Carts WHERE Username='"+username+"'";
                    SqlCommand command = new SqlCommand(sql,connection);
                    command.ExecuteNonQuery();                   
                }
                catch(SqlException){
                    Console.WriteLine("Sql exception occurred !!");
                }    
                finally{
                    connection.Close();
                }            
            }           
        } 
        public static void updateOrder(OrderDetails details){
            
            using(SqlConnection connection = new SqlConnection()){
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                try{
                    connection.Open();
                    string sql = "UPDATE Shipping SET Shipping='" + details.Shipping + "' WHERE OrderId = '" + details.OrderId + "'";
                    SqlCommand command = new SqlCommand(sql,connection);
                    command.ExecuteNonQuery();                   
                }
                catch(SqlException){
                    Console.WriteLine("Sql exception occurred !!");
                }    
                finally{
                    connection.Close();
                }            
            }           
        }      
        public static DataSet UserOrder(string username){
            DataSet order = new DataSet();
            using(SqlConnection connection = new SqlConnection()){
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                string sqlStatement = "select DISTINCT Orders.OrderId, OrderDate, Address, State, City, PinCode, FinalAmount, Shipping FROM Shipping JOIN Orders ON Orders.Username = '"+username+"' and Orders.OrderId = Shipping.OrderId;";
                SqlCommand command = new SqlCommand(sqlStatement,connection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(order);
            }
            return(order);
        }  
        public static int getUserCount(){
            int count=0;
            using(SqlConnection connection = new SqlConnection()){
                connection.ConnectionString = "Data Source = localhost; Initial Catalog = database; Integrated Security = SSPI";
                try{
                    connection.Open();                    
                    string statement = "SELECT COUNT(*) FROM users";
                    SqlCommand command = new SqlCommand(statement,connection);
                    var data = command.ExecuteScalar();
                    if(data!=null){
                        count = int.Parse(data.ToString());
                    }
                }
                catch(SqlException sqlexception){
                    Console.WriteLine("Exception occurred !!!");
                }
                finally{
                    connection.Close();
                }
            }
            Console.WriteLine(count);
            return count;
        }
    }
}