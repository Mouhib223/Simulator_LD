using Microsoft.Data.SqlClient;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using SimulatorLD.DBLayer.DAOs;
using SimulatorLD.DBLayer.Repository;
using SimulatorLD.WebLayer;

namespace SimulatorLD.BuisnessLayer.BOs
{
    public class OrderBo
    {
        private OrderRepo _order;
        public SimpleAcceptorApp test;

        
        
        


        public OrderBo()
        {
            _order = new DBLayer.Repository.OrderRepo();
        }
        public List<Order> GetAllOrders()
        {

            return _order.GetAllOrders();
        }

        public void AddOrder(Order order)
        {
            _order.AddOrder(order);
           


        }
        public void InsertOrderIntoDatabase(NewOrderSingle order)
        {
            //string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

            // Get connection string from appsettings.json
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            // Extracting order details from NewOrderSingle
            var newOrder = new DBLayer.DAOs.Order
            {
                BeginString = order.Header.GetString(Tags.BeginString), // assuming BeginString tag
                SenderCompId = order.Header.GetString(Tags.SenderCompID), // assuming SenderCompID tag
                ClientCompId = order.Header.GetString(Tags.TargetCompID), // assuming TargetCompID tag for ClientCompID
                Symbol = order.Symbol.getValue(),
                Price = order.Price.getValue().ToString(),
                OrderQuantity = order.OrderQty.getValue().ToString(),
                Side = order.Side.getValue().ToString(),
                TransactTime = order.TransactTime.getValue(),
                RuleId = 1 // Assign appropriate RuleId, here it is hardcoded for example purposes
            };


            string query = "INSERT INTO Orders (BeginString, SenderCompID, ClientCompID, Symbol, Price, OrderQuantity, Side, TransactTime, RuleId) " +
                           "VALUES (@BeginString, @SenderCompID, @ClientCompID, @Symbol, @Price, @OrderQuantity, @Side, @TransactTime, @RuleId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@BeginString", newOrder.BeginString);
                command.Parameters.AddWithValue("@SenderCompID", newOrder.SenderCompId);
                command.Parameters.AddWithValue("@ClientCompID", newOrder.ClientCompId);
                command.Parameters.AddWithValue("@Symbol", newOrder.Symbol);
                command.Parameters.AddWithValue("@Price", newOrder.Price);
                command.Parameters.AddWithValue("@OrderQuantity", newOrder.OrderQuantity);
                command.Parameters.AddWithValue("@Side", newOrder.Side);
                command.Parameters.AddWithValue("@TransactTime", newOrder.TransactTime);
                command.Parameters.AddWithValue("@RuleId", newOrder.RuleId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Order inserted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        public void InsertOrderIntoDatabaseCancel(OrderCancelRequest order)
        {
            //string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

            // Get connection string from appsettings.json
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            // Extracting order details from NewOrderSingle
            var newOrder = new DBLayer.DAOs.Order
            {
                BeginString = order.Header.GetString(Tags.BeginString), // assuming BeginString tag
                SenderCompId = order.Header.GetString(Tags.SenderCompID), // assuming SenderCompID tag
                ClientCompId = order.Header.GetString(Tags.TargetCompID), // assuming TargetCompID tag for ClientCompID
                Symbol = order.Symbol.getValue(),
                Price = "0",
                OrderQuantity = order.OrderQty.getValue().ToString(),
                Side = order.Side.getValue().ToString(),
                TransactTime = order.TransactTime.getValue(),
                RuleId = 1 // Assign appropriate RuleId, here it is hardcoded for example purposes
            };


            string query = "INSERT INTO Orders (BeginString, SenderCompID, ClientCompID, Symbol, Price, OrderQuantity, Side, TransactTime, RuleId) " +
                           "VALUES (@BeginString, @SenderCompID, @ClientCompID, @Symbol, @Price, @OrderQuantity, @Side, @TransactTime, @RuleId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@BeginString", newOrder.BeginString);
                command.Parameters.AddWithValue("@SenderCompID", newOrder.SenderCompId);
                command.Parameters.AddWithValue("@ClientCompID", newOrder.ClientCompId);
                command.Parameters.AddWithValue("@Symbol", newOrder.Symbol);
                command.Parameters.AddWithValue("@Price", newOrder.Price);
                command.Parameters.AddWithValue("@OrderQuantity", newOrder.OrderQuantity);
                command.Parameters.AddWithValue("@Side", newOrder.Side);
                command.Parameters.AddWithValue("@TransactTime", newOrder.TransactTime);
                command.Parameters.AddWithValue("@RuleId", newOrder.RuleId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Order inserted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        public void InsertOrderIntoDatabaseReplace(OrderCancelReplaceRequest order)
        {
            //string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

            // Get connection string from appsettings.json
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            // Extracting order details from NewOrderSingle
            var newOrder = new DBLayer.DAOs.Order
            {
                BeginString = order.Header.GetString(Tags.BeginString), // assuming BeginString tag
                SenderCompId = order.Header.GetString(Tags.SenderCompID), // assuming SenderCompID tag
                ClientCompId = order.Header.GetString(Tags.TargetCompID), // assuming TargetCompID tag for ClientCompID
                Symbol = order.Symbol.getValue(),
                Price = "0",
                OrderQuantity = order.OrderQty.getValue().ToString(),
                Side = order.Side.getValue().ToString(),
                TransactTime = order.TransactTime.getValue(),
                RuleId = 1 // Assign appropriate RuleId, here it is hardcoded for example purposes
            };


            string query = "INSERT INTO Orders (BeginString, SenderCompID, ClientCompID, Symbol, Price, OrderQuantity, Side, TransactTime, RuleId) " +
                           "VALUES (@BeginString, @SenderCompID, @ClientCompID, @Symbol, @Price, @OrderQuantity, @Side, @TransactTime, @RuleId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@BeginString", newOrder.BeginString);
                command.Parameters.AddWithValue("@SenderCompID", newOrder.SenderCompId);
                command.Parameters.AddWithValue("@ClientCompID", newOrder.ClientCompId);
                command.Parameters.AddWithValue("@Symbol", newOrder.Symbol);
                command.Parameters.AddWithValue("@Price", newOrder.Price);
                command.Parameters.AddWithValue("@OrderQuantity", newOrder.OrderQuantity);
                command.Parameters.AddWithValue("@Side", newOrder.Side);
                command.Parameters.AddWithValue("@TransactTime", newOrder.TransactTime);
                command.Parameters.AddWithValue("@RuleId", newOrder.RuleId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Order inserted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        static public bool IsMatching(QuickFix.FIX44.NewOrderSingle order)
        {
            //string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string
            // Build configuration
            //Working without the direct connectionstring
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            // Get connection string from appsettings.json
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            string query = "SELECT * FROM Rules;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DBLayer.DAOs.Rule rule = new DBLayer.DAOs.Rule
                        {
                            RuleId = reader.GetInt32(0),
                            RuleType = (RuleTypesEnum)Enum.Parse(typeof(RuleTypesEnum), reader.GetString(1)),// Parse the string to Enum
                            Symbol = reader.GetString(2),
                            MinPrice = reader.GetFloat(3),
                            MaxPrice = reader.GetFloat(4),
                            MinQty = reader.GetFloat(5),
                            MaxQty = reader.GetFloat(6),
                            Description = reader.GetString(7)
                        };


                        if (!order.IsSetField(QuickFix.Fields.Tags.Symbol))
                        {
                            Console.WriteLine("Symbol field is not set in the order.");
                            return false;
                        }

                        // Verify if the rule's symbol matches the order's symbol
                        //Console.WriteLine("The order Symbole is : "+order.GetString(QuickFix.Fields.Tags.Symbol));

                        //Console.WriteLine("This Order is Matching a Rule !"); 
                        if (rule.Symbol == order.GetString(QuickFix.Fields.Tags.Symbol))
                        {
                            Console.WriteLine("===============================================================");
                            // Check if the Price field is present in the order
                            if (order.IsSetField(QuickFix.Fields.Tags.Price))
                            {
                                decimal price = order.GetDecimal(QuickFix.Fields.Tags.Price);
                                decimal Quantity = order.GetDecimal(QuickFix.Fields.Tags.OrderQty);
                                // Verify if the price is within the interval of rule.MinPrice and rule.MaxPrice
                                if (price >= (decimal)rule.MinPrice && price <= (decimal)rule.MaxPrice)
                                {
                                    if (Quantity >= (decimal)rule.MinQty && Quantity <= (decimal)rule.MaxQty)
                                    {
                                        Console.WriteLine("Rule Matched : ");
                                        Console.WriteLine($"Id: {rule.RuleId}, Symbol: {rule.Symbol},Description: {rule.Description}, RuleTypeID: {rule.RuleType}, " +
                                          $"MinPrice: {rule.MinPrice}, MaxPrice: {rule.MaxPrice}, MinQty: {rule.MinQty}, MaxQty: {rule.MaxQty}");

                                        /*Console.WriteLine("Rule Matched !!");*/
                                        /*Console.WriteLine($"Id: {rule.RuleId}, Symbol: {rule.Symbol},Description: {rule.Description}, RuleTypeID: {rule.RuleType}, " +
                                          $"MinPrice: {rule.MinPrice}, MaxPrice: {rule.MaxPrice}, MinQty: {rule.MinQty}, MaxQty: {rule.MaxQty}");
*/
                                        //Console.WriteLine("The Quantity is " + rule.Symbol + " ===> " + rule.Description);
                                        return true;
                                    }
                                    else
                                    {
                                        //Console.WriteLine("Quantity is not within the specified range.");
                                    }
                                    /*Console.WriteLine("Rule Matched !!");
                                    Console.WriteLine("The symbol is " + rule.Symbol + " ===> " + rule.Description);*/
                                    return true;
                                }
                                else
                                {
                                    //Console.WriteLine("Price is not within the specified range.");
                                }
                            }
                            else
                            {
                                //Console.WriteLine("Price field is not set in the order.");
                            }
                        }
                        else
                        {
                            // Console.WriteLine("Symbol does not match.");
                        }



                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }
        }
        static public bool IsMatchingCancel(QuickFix.FIX44.OrderCancelRequest order)
        {
            //string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string
            // Build configuration
            //Working without the direct connectionstring
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            // Get connection string from appsettings.json
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            string query = "SELECT * FROM Rules;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DBLayer.DAOs.Rule rule = new DBLayer.DAOs.Rule
                        {
                            RuleId = reader.GetInt32(0),
                            RuleType = (RuleTypesEnum)Enum.Parse(typeof(RuleTypesEnum), reader.GetString(1)),// Parse the string to Enum
                            Symbol = reader.GetString(2),
                            MinPrice = reader.GetFloat(3),
                            MaxPrice = reader.GetFloat(4),
                            MinQty = reader.GetFloat(5),
                            MaxQty = reader.GetFloat(6),
                            Description = reader.GetString(7)
                        };


                        if (!order.IsSetField(QuickFix.Fields.Tags.Symbol))
                        {
                            Console.WriteLine("Symbol field is not set in the order.");
                            return false;
                        }

                        // Verify if the rule's symbol matches the order's symbol
                        //Console.WriteLine("The order Symbole is : "+order.GetString(QuickFix.Fields.Tags.Symbol));

                        //Console.WriteLine("This Order is Matching a Rule !"); 
                        if (rule.Symbol == order.GetString(QuickFix.Fields.Tags.Symbol))
                        {
                            Console.WriteLine("===============================================================");
                            // Check if the Price field is present in the order

                            //decimal price = order.GetDecimal(QuickFix.Fields.Tags.Price);
                            decimal Quantity = order.GetDecimal(QuickFix.Fields.Tags.OrderQty);
                            // Verify if the price is within the interval of rule.MinPrice and rule.MaxPrice

                            if (Quantity >= (decimal)rule.MinQty && Quantity <= (decimal)rule.MaxQty)
                            {
                                Console.WriteLine("Rule Matched : ");
                                Console.WriteLine($"Id: {rule.RuleId}, Symbol: {rule.Symbol},Description: {rule.Description}, RuleTypeID: {rule.RuleType}, " +
                                  $"MinQty: {rule.MinQty}, MaxQty: {rule.MaxQty}");

                                /*Console.WriteLine("Rule Matched !!");*/
                                /*Console.WriteLine($"Id: {rule.RuleId}, Symbol: {rule.Symbol},Description: {rule.Description}, RuleTypeID: {rule.RuleType}, " +
                                  $"MinPrice: {rule.MinPrice}, MaxPrice: {rule.MaxPrice}, MinQty: {rule.MinQty}, MaxQty: {rule.MaxQty}");
*/
                                //Console.WriteLine("The Quantity is " + rule.Symbol + " ===> " + rule.Description);
                                return true;
                            }
                            else
                            {
                                //Console.WriteLine("Quantity is not within the specified range.");
                            }
                            /*Console.WriteLine("Rule Matched !!");
                            Console.WriteLine("The symbol is " + rule.Symbol + " ===> " + rule.Description);*/
                            return true;


                        }
                        else
                        {
                            // Console.WriteLine("Symbol does not match.");
                        }



                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }
        }
        static public bool IsMatchingCancelReplace(QuickFix.FIX44.OrderCancelReplaceRequest order)
        {
            //string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string
            // Build configuration
            //Working without the direct connectionstring
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            // Get connection string from appsettings.json
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            string query = "SELECT * FROM Rules;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DBLayer.DAOs.Rule rule = new DBLayer.DAOs.Rule
                        {
                            RuleId = reader.GetInt32(0),
                            RuleType = (RuleTypesEnum)Enum.Parse(typeof(RuleTypesEnum), reader.GetString(1)),// Parse the string to Enum
                            Symbol = reader.GetString(2),
                            MinPrice = reader.GetFloat(3),
                            MaxPrice = reader.GetFloat(4),
                            MinQty = reader.GetFloat(5),
                            MaxQty = reader.GetFloat(6),
                            Description = reader.GetString(7)
                        };


                        if (!order.IsSetField(QuickFix.Fields.Tags.Symbol))
                        {
                            Console.WriteLine("Symbol field is not set in the order.");
                            return false;
                        }

                        // Verify if the rule's symbol matches the order's symbol
                        //Console.WriteLine("The order Symbole is : "+order.GetString(QuickFix.Fields.Tags.Symbol));

                        //Console.WriteLine("This Order is Matching a Rule !"); 
                        if (rule.Symbol == order.GetString(QuickFix.Fields.Tags.Symbol))
                        {
                            Console.WriteLine("===============================================================");
                            // Check if the Price field is present in the order

                            //decimal price = order.GetDecimal(QuickFix.Fields.Tags.Price);
                            decimal Quantity = order.GetDecimal(QuickFix.Fields.Tags.OrderQty);
                            // Verify if the price is within the interval of rule.MinPrice and rule.MaxPrice

                            if (Quantity >= (decimal)rule.MinQty && Quantity <= (decimal)rule.MaxQty)
                            {
                                Console.WriteLine("Rule Matched : ");
                                Console.WriteLine($"Id: {rule.RuleId}, Symbol: {rule.Symbol},Description: {rule.Description}, RuleTypeID: {rule.RuleType}, " +
                                  $"MinQty: {rule.MinQty}, MaxQty: {rule.MaxQty}");

                                /*Console.WriteLine("Rule Matched !!");*/
                                /*Console.WriteLine($"Id: {rule.RuleId}, Symbol: {rule.Symbol},Description: {rule.Description}, RuleTypeID: {rule.RuleType}, " +
                                  $"MinPrice: {rule.MinPrice}, MaxPrice: {rule.MaxPrice}, MinQty: {rule.MinQty}, MaxQty: {rule.MaxQty}");
*/
                                //Console.WriteLine("The Quantity is " + rule.Symbol + " ===> " + rule.Description);
                                return true;
                            }
                            else
                            {
                                //Console.WriteLine("Quantity is not within the specified range.");
                            }
                            /*Console.WriteLine("Rule Matched !!");
                            Console.WriteLine("The symbol is " + rule.Symbol + " ===> " + rule.Description);*/
                            return true;


                        }
                        else
                        {
                            // Console.WriteLine("Symbol does not match.");
                        }



                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }
        }
    }
}

