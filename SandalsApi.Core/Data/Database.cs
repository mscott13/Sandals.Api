using SandalsApi.Core.Models;
using SandalsApi.Core.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SandalsApi.Core.Data
{
    public class Database
    {
        private static string connectionString = "Data Source=DESKTOP-CPPH6N8;Initial Catalog=Sandals;Integrated Security=True";

        public static User GetUser(string username, bool hashVisible)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select userId, users.createdDate, username, firstName, lastName, firmName, phone, email, address, country, hash, userType.title as userType, isVerified from users " +
                                "inner join userType on userType.userTypeId = users.userType where username = @username";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            User user = new User
                            {
                                userId = Convert.ToInt32(reader["userId"]),
                                createdDate = Convert.ToDateTime(reader["createdDate"]),
                                username = reader["username"].ToString(),
                                firstName = reader["firstName"].ToString(),
                                lastName = reader["lastName"].ToString(),
                                firmName = reader["firmName"].ToString(),
                                phone = reader["phone"].ToString(),
                                email = reader["email"].ToString(),
                                address = reader["address"].ToString(),
                                country = reader["country"].ToString(),
                                hash = reader["hash"].ToString(),
                                userType = reader["userType"].ToString(),
                                isVerified = Convert.ToBoolean(reader["isVerified"])
                            };

                            if (!hashVisible)
                            {
                                user.hash = null;
                            }
                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static User GetUserViaUserId(int userId, bool hashVisible)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select userId, users.createdDate, username, firstName, lastName, firmName, phone, email, address, country, hash, userType.title as userType, isVerified from users " +
                                "inner join userType on userType.userTypeId = users.userType where users.userId = @userId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            User user = new User
                            {
                                userId = Convert.ToInt32(reader["userId"]),
                                createdDate = Convert.ToDateTime(reader["createdDate"]),
                                username = reader["username"].ToString(),
                                firstName = reader["firstName"].ToString(),
                                lastName = reader["lastName"].ToString(),
                                firmName = reader["firmName"].ToString(),
                                phone = reader["phone"].ToString(),
                                email = reader["email"].ToString(),
                                address = reader["address"].ToString(),
                                country = reader["country"].ToString(),
                                hash = reader["hash"].ToString(),
                                userType = reader["userType"].ToString(),
                                isVerified = Convert.ToBoolean(reader["isVerified"])
                            };
                            if (!hashVisible)
                            {
                                user.hash = null;
                            }
                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static User GetUserByEmail(string email, bool hashVisible)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select userId, users.createdDate, username, firstName, lastName, firmName, phone, email, address, country, hash, userType.title as userType, isVerified from users " +
                                "inner join userType on userType.userTypeId = users.userType where email = @email";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            User user = new User
                            {
                                userId = Convert.ToInt32(reader["userId"]),
                                createdDate = Convert.ToDateTime(reader["createdDate"]),
                                username = reader["username"].ToString(),
                                firstName = reader["firstName"].ToString(),
                                lastName = reader["lastName"].ToString(),
                                firmName = reader["firmName"].ToString(),
                                phone = reader["phone"].ToString(),
                                email = reader["email"].ToString(),
                                address = reader["address"].ToString(),
                                country = reader["country"].ToString(),
                                hash = reader["hash"].ToString(),
                                userType = reader["userType"].ToString(),
                                isVerified = Convert.ToBoolean(reader["isVerified"])
                            };
                            if (!hashVisible)
                            {
                                user.hash = null;
                            }
                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static List<User> GetUsers(bool hashVisible)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select userId, users.createdDate, username, firstName, lastName, firmName, phone, email, address, country, hash, userType.title as userType, isVerified from users " +
                                "inner join userType on userType.userTypeId = users.userType order by users.createdDate desc";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<User> users = new List<User>();
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    userId = Convert.ToInt32(reader["userId"]),
                                    createdDate = Convert.ToDateTime(reader["createdDate"]),
                                    username = reader["username"].ToString(),
                                    firstName = reader["firstName"].ToString(),
                                    lastName = reader["lastName"].ToString(),
                                    firmName = reader["firmName"].ToString(),
                                    phone = reader["phone"].ToString(),
                                    email = reader["email"].ToString(),
                                    address = reader["address"].ToString(),
                                    country = reader["country"].ToString(),
                                    hash = reader["hash"].ToString(),
                                    userType = reader["userType"].ToString(),
                                    isVerified = Convert.ToBoolean(reader["isVerified"])
                                };
                                users.Add(user);
                                if (!hashVisible)
                                {
                                    user.hash = null;
                                }
                            }
                            return users;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static bool CreateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "insert into users values (getdate(), @username, @firstName, @lastName, @firmName, @phone, @email, @address, @country, @hash, @userType, @isVerified, @recoveryState)";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@username", user.username);
                    command.Parameters.AddWithValue("@firstName", user.firstName);
                    command.Parameters.AddWithValue("@lastName", user.lastName);
                    command.Parameters.AddWithValue("@firmName", user.firmName);
                    command.Parameters.AddWithValue("@phone", user.phone);
                    command.Parameters.AddWithValue("@email", user.email);
                    command.Parameters.AddWithValue("@address", user.address);
                    command.Parameters.AddWithValue("@country", user.country);
                    command.Parameters.AddWithValue("@hash", user.hash);
                    command.Parameters.AddWithValue("@userType", Convert.ToInt32(user.userType));
                    command.Parameters.AddWithValue("@isVerified", false);
                    command.Parameters.AddWithValue("@recoveryState", false);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public static bool UpdatePassword(string username, string hash)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Update users set hash=@hash where username=@username";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@hash", hash);
                    command.Parameters.AddWithValue("@username", username);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public static bool CheckEmailExist(string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from users where email = @email";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@email", email.Trim());
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
        }

        public static bool CheckUsernameExist(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from users where username = @username";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@username", username.Trim());
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
        }

        public static bool SetAccountRecoveryState(int userId, bool state)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Update users set recoveryState = @recoveryState where userId = @userId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@recoveryState", state);
                    command.Parameters.AddWithValue("@userId", userId);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public static void SetRecoveryCode(int userId, string code)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Insert into accountRecovery values (@userId, @recoveryCode, getdate(), @expiryDate)";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@recoveryCode", code);
                    command.Parameters.AddWithValue("@expiryDate", DateTime.Now.AddHours(3));
                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetRecoveryCode(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select top 1 * from accountRecovery where userId=@userId order by expiryDate desc";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            if (DateTime.Compare(Convert.ToDateTime(reader["expiryDate"]), DateTime.Now) >= 0)
                            {
                                return "expired";
                            }
                            return reader["recoveryCode"].ToString();
                        }
                        return null;
                    }
                }
            }
        }

        public static List<Dictionary<string, string>> GetUploadedImagesForOrder(string orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from orderMainImageReferences where orderMainId=@orderMainId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@orderMainId", orderId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Dictionary<string, string>> images = new List<Dictionary<string, string>>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                images.Add(new Dictionary<string, string>() {
                                    { "image", reader["image"].ToString() },
                                    { "mediaType", reader["mediaType"].ToString() },
                                    { "extension", reader["extension"].ToString() }
                                });
                            }
                        }
                        return images;
                    }
                }
            }
        }

        public static List<Product> GetProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from products order by createdDate desc";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Product> products = new List<Product>();
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    productId = Convert.ToInt32(reader["productId"]),
                                    createdDate = Convert.ToDateTime(reader["createdDate"]),
                                    description = reader["description"].ToString(),
                                    image = reader["image"].ToString(),
                                    retailPrice = Convert.ToDecimal(reader["retailPrice"]),
                                    mediaType = reader["mediaType"].ToString(),
                                    extension = reader["extension"].ToString()
                                };
                                products.Add(product);
                            }
                            return products;
                        }
                        return null;
                    }
                }
            }
        }

        public static bool CreateProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Insert into products values (@createdDate, @description, @retailPrice, @image, @mediaType, @extension)";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@createdDate", DateTime.Now);
                    command.Parameters.AddWithValue("@description", product.description);
                    command.Parameters.AddWithValue("@retailPrice", product.retailPrice);
                    command.Parameters.AddWithValue("@image", product.image);
                    command.Parameters.AddWithValue("@mediaType", product.mediaType);
                    command.Parameters.AddWithValue("@extension", product.extension);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public static List<OrderMain> GetOrders(int orderCount = 0)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from orderMain order by createdDate desc";

                if (orderCount != 0 && orderCount > 0)
                {
                    cmdStr = "Select Top " + orderCount + " * from orderMain order by createdDate desc";
                }

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<OrderMain> orders = new List<OrderMain>();
                            while (reader.Read())
                            {
                                OrderMain order = new OrderMain
                                {
                                    orderMainId = Convert.ToInt32(reader["orderMainId"]),
                                    userId = Convert.ToInt32(reader["userId"]),
                                    createdDate = Convert.ToDateTime(reader["createdDate"]),
                                    shortDescription = reader["shortDescription"].ToString(),
                                    total = Convert.ToDecimal(reader["total"]),
                                    orderDetails = GetOrderDetails(Convert.ToInt32(reader["orderMainId"])),
                                    uploadedImages = GetOrderUploads(Convert.ToInt32(reader["orderMainId"])),
                                    orderStatus = reader["orderStatus"].ToString(),
                                    user = GetUserViaUserId(Convert.ToInt32(reader["userId"]), false)
                                };
                                orders.Add(order);
                            }
                            return orders;
                        }
                        return null;
                    }
                }
            }
        }

        public static List<OrderMain> GetOrdersForUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from orderMain where userId=@userId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<OrderMain> orders = new List<OrderMain>();
                            while (reader.Read())
                            {
                                OrderMain order = new OrderMain
                                {
                                    orderMainId = Convert.ToInt32(reader["orderMainId"]),
                                    userId = Convert.ToInt32(reader["userId"]),
                                    createdDate = Convert.ToDateTime(reader["createdDate"]),
                                    shortDescription = reader["shortDescription"].ToString(),
                                    total = Convert.ToDecimal(reader["total"]),
                                    orderDetails = GetOrderDetails(Convert.ToInt32(reader["orderMainId"])),
                                    uploadedImages = GetOrderUploads(Convert.ToInt32(reader["orderMainId"])),
                                    orderStatus = reader["orderStatus"].ToString(),
                                    user = GetUserViaUserId(Convert.ToInt32(reader["userId"]), false)
                                };
                                orders.Add(order);
                            }
                            return orders;
                        }
                        return null;
                    }
                }
            }
        }

        public static OrderMain GetOrder(int orderMainId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from orderMain where orderMainId=@orderMainId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@orderMainId", orderMainId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<OrderMain> orders = new List<OrderMain>();
                            reader.Read();

                            OrderMain order = new OrderMain
                            {
                                orderMainId = Convert.ToInt32(reader["orderMainId"]),
                                userId = Convert.ToInt32(reader["userId"]),
                                createdDate = Convert.ToDateTime(reader["createdDate"]),
                                shortDescription = reader["shortDescription"].ToString(),
                                total = Convert.ToDecimal(reader["total"]),
                                orderDetails = GetOrderDetails(orderMainId),
                                uploadedImages = GetOrderUploads(orderMainId),
                                orderStatus = reader["orderStatus"].ToString()
                            };
                            return order;
                        }
                        return null;
                    }
                }
            }
        }

        public static List<OrderDetail> GetOrderDetails(int orderMainId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select orderMainId, products.productId, orderDetail.retailPrice, products.title, quantity, orderDetail.image from orderDetail inner join products on products.productId = orderDetail.productId where orderMainId = @orderMainId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@orderMainId", orderMainId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<OrderDetail> orderDetails = new List<OrderDetail>();
                            while (reader.Read())
                            {
                                OrderDetail orderDetail = new OrderDetail
                                {
                                    orderMainId = Convert.ToInt32(reader["orderMainId"]),
                                    productId = Convert.ToInt32(reader["productId"]),
                                    productDescription = reader["title"].ToString(),
                                    retailPrice = Convert.ToDecimal(reader["retailPrice"]),
                                    quantity = Convert.ToInt32(reader["quantity"]),
                                    image = reader["image"].ToString()
                                };
                                orderDetails.Add(orderDetail);
                            }
                            return orderDetails;
                        }
                        return null;
                    }
                }
            }
        }

        public static List<Dictionary<string, string>> GetOrderUploads(int orderMainId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from orderMainImageReferences where orderMainId=@orderMainId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@orderMainId", orderMainId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Dictionary<string, string>> images = new List<Dictionary<string, string>>();
                            while (reader.Read())
                            {
                                var values = new Dictionary<string, string>();
                                values.Add("image", reader["image"].ToString());
                                values.Add("mediaType", reader["mediaType"].ToString());
                                values.Add("extension", reader["extension"].ToString());
                                images.Add(values);
                            }
                            return images;
                        }
                        return null;
                    }
                }
            }
        }

        public static bool CreateOrder(OrderMain orderMain)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    string cmdStr1 = "Insert into orderMain OUTPUT Inserted.orderMainId values (@userId, @createdDate, @shortDescription, @total, @orderStatus)";
                    using (SqlCommand command1 = new SqlCommand(cmdStr1, connection, transaction))
                    {
                        command1.Parameters.AddWithValue("@userId", orderMain.userId);
                        command1.Parameters.AddWithValue("@createdDate", DateTime.Now);
                        command1.Parameters.AddWithValue("@shortDescription", orderMain.GetShortDescription());
                        command1.Parameters.AddWithValue("@total", orderMain.GetTotal());
                        command1.Parameters.AddWithValue("@orderStatus", "PROCESSING");
                        int orderMainId = (int)command1.ExecuteScalar();

                        if (orderMain.orderDetails != null)
                        {
                            string cmdStr2 = "insert into orderDetail values (@orderMainId, @productId, @retailPrice, @quantity, @image)";
                            for (int i = 0; i < orderMain.orderDetails.Count; i++)
                            {
                                using (SqlCommand command2 = new SqlCommand(cmdStr2, connection, transaction))
                                {
                                    command2.Parameters.AddWithValue("@orderMainId", orderMainId);
                                    command2.Parameters.AddWithValue("@productId", orderMain.orderDetails[i].productId);
                                    command2.Parameters.AddWithValue("@retailPrice", orderMain.orderDetails[i].retailPrice);
                                    command2.Parameters.AddWithValue("@quantity", orderMain.orderDetails[i].quantity);
                                    command2.Parameters.AddWithValue("@image", GetImageForProduct(orderMain.orderDetails[i].productId));
                                    command2.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }

                        if (orderMain.uploadedImages != null)
                        {
                            string cmdStr3 = "insert into orderMainImageReferences values(@orderMainId, @image, @mediaType, @extension)";
                            for (int i = 0; i < orderMain.uploadedImages.Count; i++)
                            {
                                using (SqlCommand command3 = new SqlCommand(cmdStr3, connection, transaction))
                                {
                                    command3.Parameters.AddWithValue("@orderMainId", orderMainId);
                                    command3.Parameters.AddWithValue("@image", orderMain.uploadedImages[i]["image"]);
                                    command3.Parameters.AddWithValue("@mediaType", orderMain.uploadedImages[i]["mediaType"]);
                                    command3.Parameters.AddWithValue("@extension", orderMain.uploadedImages[i]["extension"]);
                                    command3.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw (e);
                }
            }
        }

        public static string GetImageForProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select image from products where productId=@productId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return reader["image"].ToString();
                        }
                        return null;
                    }
                }
            }
        }

        public static Product GetProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select * from products where productId=@productId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return new Product
                            {
                                productId = Convert.ToInt32(reader["productId"]),
                                createdDate = Convert.ToDateTime(reader["createdDate"]),
                                description = reader["title"].ToString(),
                                image = reader["image"].ToString(),
                                retailPrice = Convert.ToDecimal(reader["retailPrice"]),
                                mediaType = reader["mediaType"].ToString(),
                                extension = reader["extension"].ToString()
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public static Product GetProductViaImage(string image)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select Top 1 * from products where image=@image";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@image", image);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return new Product
                            {
                                productId = Convert.ToInt32(reader["productId"]),
                                createdDate = Convert.ToDateTime(reader["createdDate"]),
                                description = reader["title"].ToString(),
                                image = reader["image"].ToString(),
                                retailPrice = Convert.ToDecimal(reader["retailPrice"]),
                                mediaType = reader["mediaType"].ToString(),
                                extension = reader["extension"].ToString()
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public static void Log(string message)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Insert into activityLog values (@date, @message)";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.Parameters.AddWithValue("@message", message);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<LogItem> GetLogs(int years)
        {
            List<LogItem> logItems = new List<LogItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "select * from activityLog where datediff(year, date, getdate()) <= @years order by date desc";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@years", years);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                logItems.Add(new LogItem
                                {
                                    date = Convert.ToDateTime(reader["date"]),
                                    dateFormatted = Convert.ToDateTime(reader["date"]).ToShortDateString(),
                                    message = reader["message"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            PrepareActivityLog(logItems, years);
            return logItems;
        }

        public static Dictionary<string, string> GetUploadedImage(string image)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Select Top 1 * from orderMainImageReferences where image=@image";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@image", image);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Dictionary<string, string> values = new Dictionary<string, string>();
                            values.Add("image", reader["image"].ToString());
                            values.Add("mediaType", reader["mediaType"].ToString());
                            values.Add("extension", reader["extension"].ToString());
                            return values;
                        }
                        return null;
                    }
                }
            }
        }

        public static bool SetOrderStatus(int orderMainId, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "Update orderMain set orderStatus=@orderStatus where orderMainId=@orderMainId";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@orderStatus", status);
                    command.Parameters.AddWithValue("@orderMainId", orderMainId);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        public static Dashboard GetDashboard()
        {
            Dashboard dashboard = new Dashboard();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr1 = "select ISNULL((select sum(total) from orderMain where month(createdDate) = month(getdate()) and year(createdDate) = year(getdate()) and orderStatus = 'PROCESSED'), 0 )  as thisMonthTotal, ISNULL((select sum(total) from orderMain where month(createdDate) = month(dateadd(month, -1, getdate())) and year(createdDate) = year((dateadd(month, -1, getdate()))) and orderStatus = 'PROCESSED'), 0) as prevMonthTotal;";

                using (SqlCommand command1 = new SqlCommand(cmdStr1, connection))
                {
                    using (SqlDataReader reader1 = command1.ExecuteReader())
                    {
                        reader1.Read();
                        dashboard.thisMonthTotal = Convert.ToDecimal(reader1["thisMonthTotal"]);
                        dashboard.prevMonthTotal = Convert.ToDecimal(reader1["prevMonthTotal"]);
                        dashboard.difference = dashboard.thisMonthTotal - dashboard.prevMonthTotal;
                    }
                }
                dashboard.monthlyTotals = GetMonthlyTotals(DateTime.Now);

                string cmdStr2 = "Select (select count(*) from users) as registeredUsers , (select count(*) from orderMain where orderStatus = @OPEN) as openOrders, (select count(*) from orderMain where orderStatus = @PROCESSING) as processing,  (select count(*) from orderMain where orderStatus = @PROCESSED) as processed";
                using (SqlCommand command2 = new SqlCommand(cmdStr2, connection))
                {
                    command2.Parameters.AddWithValue("@OPEN", Constants.ORDER_STATUS_OPEN);
                    command2.Parameters.AddWithValue("@PROCESSING", Constants.ORDER_STATUS_PROCESSING);
                    command2.Parameters.AddWithValue("@PROCESSED", Constants.ORDER_STATUS_PROCESSED);
                    Dictionary<string, int> generalStatistics = new Dictionary<string, int>();

                    using (SqlDataReader reader = command2.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            generalStatistics.Add("reg_users", Convert.ToInt32(reader["registeredUsers"]));
                            generalStatistics.Add("open_orders", Convert.ToInt32(reader["openOrders"]));
                            generalStatistics.Add("processing", Convert.ToInt32(reader["processing"]));
                            generalStatistics.Add("processed", Convert.ToInt32(reader["processed"]));
                            dashboard.generalStatistics = generalStatistics;
                        }
                    }
                }
                dashboard.userActivity = GetUserActivity();
                dashboard.orders = GetOrders(5);
            }
            return dashboard;
        }

        public static Dictionary<string, List<LogItem>> PrepareActivityLog(List<LogItem> logItems, int years)
        {
            Dictionary<string, List<LogItem>> result = new Dictionary<string, List<LogItem>>();
            int startYear = DateTime.Now.Year;

            for (int h = 0; h <= years; h++)
            {
                result.Add(startYear.ToString(), new List<LogItem>());
                startYear -= 1;
            }

            if (logItems != null)
            {
                for (int i = 0; i < logItems.Count; i++)
                {
                    string key = logItems[i].date.Year.ToString();

                    if (result.ContainsKey(key))
                    {
                        result[key].Add(logItems[i]);
                    }
                }
            }
            return result;
        }

        public static UserActivity GetUserActivity()
        {
            UserActivity userActivity = new UserActivity();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr1 = "select CONVERT(VARCHAR(2),date,108) as hour, count(userId) as userCount from userActivity where CAST(date as date) = cast(@date as date) GROUP BY CONVERT(VARCHAR(2),date,108) order by CONVERT(VARCHAR(2),date,108) asc";

                using (SqlCommand command1 = new SqlCommand(cmdStr1, connection))
                {
                    command1.Parameters.AddWithValue("@date", DateTime.Now);
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {
                        Dictionary<string, int> hours = new Dictionary<string, int>();
                        hours.Add("00", 0);
                        hours.Add("01", 0);
                        hours.Add("02", 0);
                        hours.Add("03", 0);
                        hours.Add("04", 0);
                        hours.Add("05", 0);
                        hours.Add("06", 0);
                        hours.Add("07", 0);
                        hours.Add("08", 0);
                        hours.Add("09", 0);
                        hours.Add("10", 0);
                        hours.Add("11", 0);
                        hours.Add("12", 0);
                        hours.Add("13", 0);
                        hours.Add("14", 0);
                        hours.Add("15", 0);
                        hours.Add("16", 0);
                        hours.Add("17", 0);
                        hours.Add("18", 0);
                        hours.Add("19", 0);
                        hours.Add("20", 0);
                        hours.Add("21", 0);
                        hours.Add("22", 0);
                        hours.Add("23", 0);

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (hours.ContainsKey(reader["hour"].ToString()))
                                {
                                    hours[reader["hour"].ToString()] = Convert.ToInt32(reader["userCount"]);
                                }
                            }
                        }

                        int max = 0;
                        foreach (var hour in hours)
                        {
                            if (hour.Value > max)
                            {
                                max = hour.Value;
                            }
                        }

                        max += 520;
                        hours.Add("max", max);
                        userActivity.userActivityCounters = hours;
                    }
                }

                string cmdStr2 = "select count(*) as activeUsers from userActivity where  datediff(minute, date, getdate()) <= @activityThreshold";
                using (SqlCommand command2 = new SqlCommand(cmdStr2, connection))
                {
                    command2.Parameters.AddWithValue("@activityThreshold", 5);
                    using (SqlDataReader reader2 = command2.ExecuteReader())
                    {
                        if (reader2.HasRows)
                        {
                            reader2.Read();
                            userActivity.currentlyActiveUsers = Convert.ToInt32(reader2["activeUsers"]);
                        }
                    }
                }
            }

            userActivity.logEvents = PrepareActivityLog(GetLogs(5), 5);
            return userActivity;
        }

        public static List<Dictionary<string, decimal>> GetMonthlyTotals(DateTime date)
        {
            List<Dictionary<string, decimal>> monthlyTotals = new List<Dictionary<string, decimal>>();
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Jan", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Feb", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Mar", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Apr", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "May", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Jun", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Jul", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Aug", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Sep", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Oct", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Nov", 0 } });
            monthlyTotals.Add(new Dictionary<string, decimal>() { { "Dec", 0 } });

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmdStr = "select Month(createdDate) as Month, sum(total) as total from orderMain where orderStatus = 'PROCESSED' and YEAR(createdDate) = YEAR(@date) group by Month(createdDate) order by Month asc";

                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@date", date);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int currentMonth = Convert.ToInt32(reader["Month"]) - 1;
                                string key = monthlyTotals[currentMonth].ElementAt(0).Key;
                                monthlyTotals[currentMonth][key] = Convert.ToDecimal(reader["total"]);
                            }
                        }
                    }

                }
            }
            return monthlyTotals;
        }
    }
}
