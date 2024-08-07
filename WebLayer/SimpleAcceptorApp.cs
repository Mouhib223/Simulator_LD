using QuickFix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickFix.Fields;

using SimulatorLD.DBLayer.Repository;
using SimulatorLD.BuisnessLayer.BOs;
using SimulatorLD.DBLayer.DAOs;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using QuickFix.FIX44;
using System.Drawing;
using System.Net.Sockets;
using Microsoft.Extensions.Hosting;

namespace SimulatorLD.WebLayer
{

    /// <summary>
    /// Just a simple server that will let you connect to it and ignore any
    /// application-level messages you send to it.
    /// Note that this app is *NOT* a message cracker.
    /// </summary>
    public class SimpleAcceptorApp : QuickFix.MessageCracker, QuickFix.IApplication
    {
        static readonly decimal DEFAULT_MARKET_PRICE = 10;

        int orderID = 0;
        int execID = 0;

        private string GenOrderID() { return (++orderID).ToString(); }
        private string GenExecID() { return (++execID).ToString(); }
        

        /* private readonly RulesManagementDbContext _context;

         public SimpleAcceptorApp(RulesManagementDbContext context)
         {
             _context = context;
         }*/
        //public Message StoredMessage {  get; set; }
        #region QuickFix.Application Methods
        private FixMessageRepo _msg;
        public RuleRepo _rules;

        // private readonly RulesManagementDbContext _context;

        /* public SimpleAcceptorApp(FixMessageRepo msg)
         {
             _msg = msg;
         }*/
        
        public SimpleAcceptorApp()
        {
        }
        public class OrderProcessor
        {
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


        }
            public class DatabaseHandler
        {
            //private string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string

            
            public void InsertFixMessage(QuickFix.FIX44.Message order)
        {
                var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

                // Get connection string from appsettings.json
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                string fixMessageString = order.ToString(); // Convert the FIX message to a string representation
            string query = "INSERT INTO FIXMessages (msgBody) VALUES (@MsgBody)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MsgBody", fixMessageString);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
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
        public void TreatOrder(QuickFix.FIX44.NewOrderSingle order)
        {

            //Console.WriteLine("Entered!");

            var rules = _rules.GetAllRules();
            //Console.WriteLine("Entered!");

            foreach (DBLayer.DAOs.Rule rule in rules)
            {

                Console.WriteLine("Iam in process order");
                /*if (rule.IsMatching(order))
                {
                    rule.ProcessOrder(order);
                }*/
            }
        }


        public void OnMessage(QuickFix.FIX44.NewOrderSingle order, SessionID sessionID)
        {
            

            DatabaseHandler dbHandler = new DatabaseHandler();
            dbHandler.InsertFixMessage(order);

            /*if (IsMatching(order)) { Console.WriteLine("This Order is Matching a Rule !"); }
            else { Console.WriteLine("No Rule Match This Order"); }
            Console.WriteLine("Fix Message Recived : ");*/
            void PrintHeader(string title)
            {
                Console.WriteLine(new string('=', 60));
                Console.WriteLine($"{title}".PadLeft(40));
                Console.WriteLine(new string('=', 60));
            }

            void PrintField(string fieldName, string fieldValue)
            {
                Console.WriteLine($"{fieldName,-25}: {fieldValue}");
            }

            PrintHeader("Fix Message Received");

            // SenderCompID
            if (order.Header.IsSetField(QuickFix.Fields.Tags.SenderCompID))
            {
                string senderCompID = order.Header.GetString(QuickFix.Fields.Tags.SenderCompID);
                PrintField("SenderCompID", senderCompID);
            }

            // TargetCompID
            if (order.Header.IsSetField(QuickFix.Fields.Tags.TargetCompID))
            {
                string targetCompID = order.Header.GetString(QuickFix.Fields.Tags.TargetCompID);
                PrintField("TargetCompID", targetCompID);
            }

            // MsgType
            if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgType))
            {
                string msgType = order.Header.GetString(QuickFix.Fields.Tags.MsgType);
                PrintField("MsgType", msgType);

                switch (msgType)
                {
                    case "D":
                        PrintField("Message Type", "New Order - Single");
                        OrderProcessor processor = new OrderProcessor();
                        processor.InsertOrderIntoDatabase(order);
                        break;
                    case "0":
                        PrintField("Message Type", "Heartbeat");
                        break;
                    case "5":
                        PrintField("Message Type", "Logout");
                        break;
                    case "A":
                        PrintField("Message Type", "Logon");
                        break;
                    case "F":
                        PrintField("Message Type", "Order Cancel Request");
                        break;
                    case "8":
                        PrintField("Message Type", "Execution Report");
                        break;
                    default:
                        PrintField("Message Type", "Unknown");
                        break;
                }
            }

            // MsgSeqNum
            if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgSeqNum))
            {
                int msgSeqNum = order.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                PrintField("MsgSeqNum", msgSeqNum.ToString());
            }

            // SendingTime
            if (order.Header.IsSetField(QuickFix.Fields.Tags.SendingTime))
            {
                DateTime sendingTime = order.Header.GetDateTime(QuickFix.Fields.Tags.SendingTime);
                PrintField("SendingTime", sendingTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            // ClOrdID
            if (order.IsSetField(QuickFix.Fields.Tags.ClOrdID))
            {
                string clOrdID1 = order.GetString(QuickFix.Fields.Tags.ClOrdID);
                PrintField("ClOrdID", clOrdID1);
            }

            // Side
            if (order.IsSetField(QuickFix.Fields.Tags.Side))
            {
                char side1 = order.GetChar(QuickFix.Fields.Tags.Side);
                PrintField("Side", side1.ToString());
            }

            // TransactTime
            if (order.IsSetField(QuickFix.Fields.Tags.TransactTime))
            {
                DateTime transactTime = order.GetDateTime(QuickFix.Fields.Tags.TransactTime);
                PrintField("TransactTime", transactTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            // OrderQty
            if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
            {
                decimal orderQty1 = order.GetDecimal(QuickFix.Fields.Tags.OrderQty);
                PrintField("OrderQty", orderQty1.ToString());
            }

            // OrdType
            if (order.IsSetField(QuickFix.Fields.Tags.OrdType))
            {
                char ordType1 = order.GetChar(QuickFix.Fields.Tags.OrdType);
                PrintField("OrdType", ordType1.ToString());
            }

            // Price
            if (order.IsSetField(QuickFix.Fields.Tags.Price))
            {
                decimal price1 = order.GetDecimal(QuickFix.Fields.Tags.Price);
                PrintField("Price", price1.ToString("0.00"));
            }

            // Symbol
            if (order.IsSetField(QuickFix.Fields.Tags.Symbol))
            {
                string symbol1 = order.GetString(QuickFix.Fields.Tags.Symbol);
                PrintField("Symbol", symbol1);
            }

            // Quantity
            if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
            {
                string quantity = order.GetString(QuickFix.Fields.Tags.OrderQty);
                PrintField("Quantity", quantity);
            }
            
            if (IsMatching(order)) { Console.WriteLine(); }
            else { Console.WriteLine("No Rule Match this Order !"); }
            

            //SENDING AN EXECUTION REPORT WORK 
            Symbol symbol = order.Symbol;
            Side side = order.Side;
            OrdType ordType = order.OrdType;
            OrderQty orderQty = order.OrderQty;
            Price price = new Price(DEFAULT_MARKET_PRICE);
            ClOrdID clOrdID = order.ClOrdID;
           
            switch (ordType.getValue())
            {
                case OrdType.LIMIT:
                    price = order.Price;
                    if (price.Obj == 0)
                        throw new IncorrectTagValue(price.Tag);
                    break;
                case OrdType.MARKET: break;
                default: throw new IncorrectTagValue(ordType.Tag);
            }

            QuickFix.FIX44.ExecutionReport exReport = new QuickFix.FIX44.ExecutionReport(
                new OrderID(GenOrderID()),
                new ExecID(GenExecID()),
                new ExecType(ExecType.FILL),
                new OrdStatus(OrdStatus.FILLED),
                symbol, //shouldn't be here?
                side,
                new LeavesQty(0),
                new CumQty(orderQty.getValue()),
                new AvgPx(price.getValue()));

            exReport.Set(clOrdID);
            exReport.Set(symbol);
            exReport.Set(orderQty);
            exReport.Set(new LastQty(orderQty.getValue()));
            exReport.Set(new LastPx(price.getValue()));
            exReport.Text = new Text("Execute All !");

            if (order.IsSetAccount())
                exReport.SetField(order.Account);

            try
            {
                Session.SendToTarget(exReport, sessionID);
            }
            catch (SessionNotFound ex)
            {
                Console.WriteLine("==session not found exception!==");
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //??
            TreatOrder(order);
            
        }
        //OrderCancelRequest Report
        public void OnMessage(QuickFix.FIX44.OrderCancelRequest order, SessionID s)
        {
            DatabaseHandler dbHandler = new DatabaseHandler();
            dbHandler.InsertFixMessage(order);

            void PrintHeader(string title)
            {
                Console.WriteLine(new string('=', 60));
                Console.WriteLine($"{title}".PadLeft(40));
                Console.WriteLine(new string('=', 60));
            }

            void PrintField(string fieldName, string fieldValue)
            {
                Console.WriteLine($"{fieldName,-25}: {fieldValue}");
            }

            PrintHeader("Fix Message Received");
            // SenderCompID
            if (order.Header.IsSetField(QuickFix.Fields.Tags.SenderCompID))
            {
                string senderCompID = order.Header.GetString(QuickFix.Fields.Tags.SenderCompID);
                PrintField("SenderCompID", senderCompID);
            }

            // TargetCompID
            if (order.Header.IsSetField(QuickFix.Fields.Tags.TargetCompID))
            {
                string targetCompID = order.Header.GetString(QuickFix.Fields.Tags.TargetCompID);
                PrintField("TargetCompID", targetCompID);
            }

            // MsgType
            if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgType))
            {
                string msgType = order.Header.GetString(QuickFix.Fields.Tags.MsgType);
                PrintField("MsgType", msgType);

                switch (msgType)
                {
                    case "D":
                        PrintField("Message Type", "New Order - Single");
                       
                        break;
                    case "0":
                        PrintField("Message Type", "Heartbeat");
                        break;
                    case "5":
                        PrintField("Message Type", "Logout");
                        break;
                    case "A":
                        PrintField("Message Type", "Logon");
                        break;
                    case "F":
                        PrintField("Message Type", "Order Cancel Request");
                        OrderProcessor processor = new OrderProcessor();
                        processor.InsertOrderIntoDatabaseCancel(order);
                        break;
                    case "8":
                        PrintField("Message Type", "Execution Report");
                        break;
                    default:
                        PrintField("Message Type", "Unknown");
                        break;
                }
            }

            // MsgSeqNum
            if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgSeqNum))
            {
                int msgSeqNum = order.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                PrintField("MsgSeqNum", msgSeqNum.ToString());
            }

            // SendingTime
            if (order.Header.IsSetField(QuickFix.Fields.Tags.SendingTime))
            {
                DateTime sendingTime = order.Header.GetDateTime(QuickFix.Fields.Tags.SendingTime);
                PrintField("SendingTime", sendingTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            // ClOrdID
            if (order.IsSetField(QuickFix.Fields.Tags.ClOrdID))
            {
                string clOrdID1 = order.GetString(QuickFix.Fields.Tags.ClOrdID);
                PrintField("ClOrdID", clOrdID1);
            }

            // Side
            if (order.IsSetField(QuickFix.Fields.Tags.Side))
            {
                char side1 = order.GetChar(QuickFix.Fields.Tags.Side);
                PrintField("Side", side1.ToString());
            }

            // TransactTime
            if (order.IsSetField(QuickFix.Fields.Tags.TransactTime))
            {
                DateTime transactTime = order.GetDateTime(QuickFix.Fields.Tags.TransactTime);
                PrintField("TransactTime", transactTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            // OrderQty
            if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
            {
                decimal orderQty1 = order.GetDecimal(QuickFix.Fields.Tags.OrderQty);
                PrintField("OrderQty", orderQty1.ToString());
            }

            // OrdType
            if (order.IsSetField(QuickFix.Fields.Tags.OrdType))
            {
                char ordType1 = order.GetChar(QuickFix.Fields.Tags.OrdType);
                PrintField("OrdType", ordType1.ToString());
            }

            // Price
            if (order.IsSetField(QuickFix.Fields.Tags.Price))
            {
                decimal price1 = order.GetDecimal(QuickFix.Fields.Tags.Price);
                PrintField("Price", price1.ToString("0.00"));
            }

            // Symbol
            if (order.IsSetField(QuickFix.Fields.Tags.Symbol))
            {
                string symbol1 = order.GetString(QuickFix.Fields.Tags.Symbol);
                PrintField("Symbol", symbol1);
            }

            // Quantity
            if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
            {
                string quantity = order.GetString(QuickFix.Fields.Tags.OrderQty);
                PrintField("Quantity", quantity);
            }

            if (IsMatchingCancel(order)) { Console.WriteLine(); }
            else { Console.WriteLine("No Rule Match This Order"); }
            string orderid = (order.IsSetOrderID()) ? order.OrderID.Obj : "unknown orderID";
            QuickFix.FIX44.OrderCancelReject ocj = new QuickFix.FIX44.OrderCancelReject(
                new OrderID(orderid), order.ClOrdID, order.OrigClOrdID, new OrdStatus(OrdStatus.REJECTED), new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REQUEST));
            ocj.CxlRejReason = new CxlRejReason(CxlRejReason.OTHER);
            ocj.Text = new Text("This is an OrderCancelRequest");

            try
            {
                Session.SendToTarget(ocj, s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //OrderCancelReplaceRequest Report 
        public void OnMessage(QuickFix.FIX44.OrderCancelReplaceRequest order, SessionID s)
        {
            DatabaseHandler dbHandler = new DatabaseHandler();
            dbHandler.InsertFixMessage(order);

            void PrintHeader(string title)
            {
                Console.WriteLine(new string('=', 60));
                Console.WriteLine($"{title}".PadLeft(40));
                Console.WriteLine(new string('=', 60));
            }

            void PrintField(string fieldName, string fieldValue)
            {
                Console.WriteLine($"{fieldName,-25}: {fieldValue}");
            }

            PrintHeader("Fix Message Received");
            // SenderCompID
            if (order.Header.IsSetField(QuickFix.Fields.Tags.SenderCompID))
            {
                string senderCompID = order.Header.GetString(QuickFix.Fields.Tags.SenderCompID);
                PrintField("SenderCompID", senderCompID);
            }

            // TargetCompID
            if (order.Header.IsSetField(QuickFix.Fields.Tags.TargetCompID))
            {
                string targetCompID = order.Header.GetString(QuickFix.Fields.Tags.TargetCompID);
                PrintField("TargetCompID", targetCompID);
            }

            // MsgType
            if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgType))
            {
                string msgType = order.Header.GetString(QuickFix.Fields.Tags.MsgType);
                PrintField("MsgType", msgType);
                OrderProcessor processor = new OrderProcessor();
                processor.InsertOrderIntoDatabaseReplace(order);
                switch (msgType)
                {
                    case "D":
                        PrintField("Message Type", "New Order - Single");
                        
                        break;
                    case "0":
                        PrintField("Message Type", "Heartbeat");
                        break;
                    case "5":
                        PrintField("Message Type", "Logout");
                        break;
                    case "A":
                        PrintField("Message Type", "Logon");
                        break;
                    case "F":
                        PrintField("Message Type", "Order Cancel Request");
                        break;
                    case "8":
                        PrintField("Message Type", "Execution Report");
                        break;
                    default:
                        PrintField("Message Type", "Unknown");
                        break;
                }
            }

            // MsgSeqNum
            if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgSeqNum))
            {
                int msgSeqNum = order.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                PrintField("MsgSeqNum", msgSeqNum.ToString());
            }

            // SendingTime
            if (order.Header.IsSetField(QuickFix.Fields.Tags.SendingTime))
            {
                DateTime sendingTime = order.Header.GetDateTime(QuickFix.Fields.Tags.SendingTime);
                PrintField("SendingTime", sendingTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            // ClOrdID
            if (order.IsSetField(QuickFix.Fields.Tags.ClOrdID))
            {
                string clOrdID1 = order.GetString(QuickFix.Fields.Tags.ClOrdID);
                PrintField("ClOrdID", clOrdID1);
            }

            // Side
            if (order.IsSetField(QuickFix.Fields.Tags.Side))
            {
                char side1 = order.GetChar(QuickFix.Fields.Tags.Side);
                PrintField("Side", side1.ToString());
            }

            // TransactTime
            if (order.IsSetField(QuickFix.Fields.Tags.TransactTime))
            {
                DateTime transactTime = order.GetDateTime(QuickFix.Fields.Tags.TransactTime);
                PrintField("TransactTime", transactTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            // OrderQty
            if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
            {
                decimal orderQty1 = order.GetDecimal(QuickFix.Fields.Tags.OrderQty);
                PrintField("OrderQty", orderQty1.ToString());
            }

            // OrdType
            if (order.IsSetField(QuickFix.Fields.Tags.OrdType))
            {
                char ordType1 = order.GetChar(QuickFix.Fields.Tags.OrdType);
                PrintField("OrdType", ordType1.ToString());
            }

            // Price
            if (order.IsSetField(QuickFix.Fields.Tags.Price))
            {
                decimal price1 = order.GetDecimal(QuickFix.Fields.Tags.Price);
                PrintField("Price", price1.ToString("0.00"));
            }

            // Symbol
            if (order.IsSetField(QuickFix.Fields.Tags.Symbol))
            {
                string symbol1 = order.GetString(QuickFix.Fields.Tags.Symbol);
                PrintField("Symbol", symbol1);
            }

            // Quantity
            if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
            {
                string quantity = order.GetString(QuickFix.Fields.Tags.OrderQty);
                PrintField("Quantity", quantity);
            }

            if (IsMatchingCancelReplace(order)) { Console.WriteLine("Cancel Rule Matched !"); }
            else { Console.WriteLine("No Rule Match This Order"); }
            string orderid = (order.IsSetOrderID()) ? order.OrderID.Obj : "unknown orderID";
            QuickFix.FIX44.OrderCancelReject ocj = new QuickFix.FIX44.OrderCancelReject(
                new OrderID(orderid), order.ClOrdID, order.OrigClOrdID, new OrdStatus(OrdStatus.REJECTED), new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REQUEST));
            ocj.CxlRejReason = new CxlRejReason(CxlRejReason.OTHER);
            ocj.Text = new Text("This is an OrderCancelReplaceRequest");

            try
            {
                Session.SendToTarget(ocj, s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void FromApp(QuickFix.Message message, SessionID sessionID)
        {
            /*string storedmessage = message.ToString();
            Console.WriteLine(storedmessage);

            Fixmessage fixmessage = new Fixmessage()

            { MsgBody = storedmessage };
            Console.WriteLine("this is the msg body to add in the DB" + fixmessage.MsgBody);
            */
            /* if (_msg != null)
             {
                 _msg.AddMessage(fixmessage);
                 //AddMessage(fixmessage);
                 Console.WriteLine("Added to the db");
             }
             else
             {
                 Console.WriteLine("_msg is null. Cannot add message to the database.");
             }*/
            Crack(message, sessionID);

        }


        #endregion

       // string fixMessage = "8=FIX.4.4|9=65|35=D|49=YOUR_SENDER_COMP_ID|56=TARGET_COMP_ID|34=2|52=20230615-19:30:00.000|11=12345|21=1|55=IBM|54=1|60=20230615-19:30:00.000|10=128|";

        public void ToApp(QuickFix.Message message, SessionID sessionID)
        {/*
              //SendToTarget(exReport);

            //Console.WriteLine("Sending message to app: " + message.ToString());

            // Example: Setting a field
            if (message.Header.GetString(Tags.MsgType) == MsgType.ORDER_SINGLE)
            {
                message.SetField(new StringField(58, "You are Good Nounou"));
            }
            Console.WriteLine("Sending message to app: " + message.ToString());
            


            //Console.WriteLine("OUT: " + message);
        }

        public void SendExecution(ExecutionReport report)
        {
            ExecutionReport exReport = new QuickFix.FIX44.ExecutionReport(
                new OrderID("12345"), // Required
                new ExecID("67890"),   // Required
                new ExecType(ExecType.FILL), // Required
                new OrdStatus(OrdStatus.FILLED),
                new Symbol("IBM"),
                new Side(Side.BUY),
                new LeavesQty(1),
                new CumQty(10),
                new AvgPx(15));
            Session.SendToTarget(exReport);
          */
        }
        public void FromAdmin(QuickFix.Message message, SessionID sessionID)
        {
            Console.WriteLine("IN:  " + message);
        }

        public void ToAdmin(QuickFix.Message message, SessionID sessionID)
        {
            Console.WriteLine("OUT:  " + message);
        }

        public void OnCreate(SessionID sessionID) { }
        public void OnLogout(SessionID sessionID) { }
        public void OnLogon(SessionID sessionID) { }

    }
}



