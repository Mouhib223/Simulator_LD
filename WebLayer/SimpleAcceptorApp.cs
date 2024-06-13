using QuickFix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickFix.Fields;
using SimulatorLD.BuisnessLayer.Models;
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
        private string _host = "127.0.0.1";
        private int _port = 5019;
        public SimpleAcceptorApp()
        {
        }


        //-----------------------------------------------------------------------------
        //Here we tried to test if the order is matching with the rules is the DB Manually but it doesn't work
        /*static public bool IsMatching(QuickFix.FIX44.NewOrderSingle order)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=RulesDB;Integrated Security=True;Trust ;Server Certificate=True";
  
           Console.WriteLine("Entered!");
            // Replace with your actual connection string
            string query = "SELECT * FROM [dbo].[Rules];";
            Console.WriteLine("Entered!");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("connection DONE!");
                SqlCommand command = new SqlCommand(query, connection);
                Console.WriteLine("COMMAND DONE!");


                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DBLayer.DAOs.Rule rule = new DBLayer.DAOs.Rule
                        {
                            RuleId = reader.GetInt32(0),
                            RuleType = (RuleTypesEnum)reader.GetInt32(1),
                            Symbol = reader.GetString(2),
                            MinPrice = reader.GetFloat(3),
                            MaxPrice = reader.GetFloat(4),
                            MinQty = reader.GetFloat(5),
                            MaxQty = reader.GetFloat(6),
                            Description = reader.GetString(7),

                        };

                        Console.WriteLine($"Id: {rule.RuleId}, Symbol: {rule.Symbol},Description: {rule.Description}, RuleTypeID: {rule.RuleType}, " +
                                          $"MinPrice: {rule.MinPrice}, MaxPrice: {rule.MaxPrice}, MinQty: {rule.MinQty}, MaxQty: {rule.MaxQty}");
                        if (rule.Symbol == order.GetString(QuickFix.Fields.Tags.Symbol))
                        {
                            Console.WriteLine("Rule Matched !!");
                            Console.WriteLine("the symbol is " + rule.Symbol + "===>" + rule.Description);
                            return true;
                        }
                        else { Console.WriteLine("Not matcheed"); }
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return false;
            }
        }*/
        public class OrderProcessor
        {
            public void InsertOrderIntoDatabase(NewOrderSingle order)
            {
                string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string

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
        }
            public class DatabaseHandler
        {
            private string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string

        public void InsertFixMessage(NewOrderSingle order)
        {
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
        }}
        static public bool IsMatching(QuickFix.FIX44.NewOrderSingle order)
        {
            string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string
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

                        Console.WriteLine($"Id: {rule.RuleId}, Symbol: {rule.Symbol},Description: {rule.Description}, RuleTypeID: {rule.RuleType}, " +
                                          $"MinPrice: {rule.MinPrice}, MaxPrice: {rule.MaxPrice}, MinQty: {rule.MinQty}, MaxQty: {rule.MaxQty}");

                        if (!order.IsSetField(QuickFix.Fields.Tags.Symbol))
                        {
                            Console.WriteLine("Symbol field is not set in the order.");
                            return false;
                        }

                        // Verify if the rule's symbol matches the order's symbol
                        Console.WriteLine("The order Symbole is : "+order.GetString(QuickFix.Fields.Tags.Symbol));
                        if (rule.Symbol == order.GetString(QuickFix.Fields.Tags.Symbol))
                        {
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
                                        Console.WriteLine("Rule Matched !!");
                                        Console.WriteLine("The Quantity is " + rule.Symbol + " ===> " + rule.Description);
                                        return true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Quantity is not within the specified range.");
                                    }
                                    Console.WriteLine("Rule Matched !!");
                                    Console.WriteLine("The symbol is " + rule.Symbol + " ===> " + rule.Description);
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("Price is not within the specified range.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Price field is not set in the order.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Symbol does not match.");
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

            if (IsMatching(order)) { Console.WriteLine("This Order is Matching a Rule !"); }
            else { Console.WriteLine("No Rule Match This Order"); }
            Console.WriteLine("Fix Message Recived : ");
            if (order.Header.IsSetField(QuickFix.Fields.Tags.SenderCompID))
            {
                string senderCompID = order.Header.GetString(QuickFix.Fields.Tags.SenderCompID);
                Console.WriteLine("SenderCompID: \t" + senderCompID);
            }

            // TargetCompID
            if (order.Header.IsSetField(QuickFix.Fields.Tags.TargetCompID))
            {
                string targetCompID = order.Header.GetString(QuickFix.Fields.Tags.TargetCompID);
                Console.WriteLine("TargetCompID: \t" + targetCompID);
            }

            // MsgType
            if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgType))
            {
                string msgType = order.Header.GetString(QuickFix.Fields.Tags.MsgType);
                Console.WriteLine("MsgType: \t" + msgType);
                //if type is D parse the message and save it in the databse
                if (msgType == "D") { Console.WriteLine("D is a New Order - Single");
                    OrderProcessor processor = new OrderProcessor();
                    processor.InsertOrderIntoDatabase(order);
                }
                if (msgType == "0") { Console.WriteLine("0 is a Heartbeat"); }
                if (msgType == "5") { Console.WriteLine("5 is a Logout"); }
                if (msgType == "A") { Console.WriteLine("A is a Logon"); }
                if (msgType == "F") { Console.WriteLine("D is an Order Cancel Request"); }
                if (msgType == "8") { Console.WriteLine("D is an Execution Report"); }
            }

            // MsgSeqNum
            if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgSeqNum))
            {
                int msgSeqNum = order.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                Console.WriteLine("MsgSeqNum: \t" + msgSeqNum);
            }

            // SendingTime
            if (order.Header.IsSetField(QuickFix.Fields.Tags.SendingTime))
            {
                DateTime sendingTime = order.Header.GetDateTime(QuickFix.Fields.Tags.SendingTime);
                Console.WriteLine("SendingTime: \t" + sendingTime);
            }

            if (order.IsSetField(QuickFix.Fields.Tags.ClOrdID))
            {
                string clOrdID = order.GetString(QuickFix.Fields.Tags.ClOrdID);
                Console.WriteLine("ClOrdID: \t" + clOrdID);
            }

            // Side
            if (order.IsSetField(QuickFix.Fields.Tags.Side))
            {
                char side = order.GetChar(QuickFix.Fields.Tags.Side);
                Console.WriteLine("Side: \t" + side);
            }

            // TransactTime
            if (order.IsSetField(QuickFix.Fields.Tags.TransactTime))
            {
                DateTime transactTime = order.GetDateTime(QuickFix.Fields.Tags.TransactTime);
                Console.WriteLine("TransactTime: \t" + transactTime);
            }

            // OrderQty
            if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
            {
                decimal orderQty = order.GetDecimal(QuickFix.Fields.Tags.OrderQty);
                Console.WriteLine("OrderQty: \t" + orderQty);
            }

            // OrdType
            if (order.IsSetField(QuickFix.Fields.Tags.OrdType))
            {
                char ordType = order.GetChar(QuickFix.Fields.Tags.OrdType);
                Console.WriteLine("OrdType: \t" + ordType);
            }

            // Price
            if (order.IsSetField(QuickFix.Fields.Tags.Price))
            {
                decimal price = order.GetDecimal(QuickFix.Fields.Tags.Price);
                Console.WriteLine("Price: \t" + price);
            }

            // Symbol
            if (order.IsSetField(QuickFix.Fields.Tags.Symbol))
            {
                string symbol = order.GetString(QuickFix.Fields.Tags.Symbol);
                Console.WriteLine("Symbol: \t" + symbol);
            }
            //Quantity
            if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
            {
                string Quantity = order.GetString(QuickFix.Fields.Tags.OrderQty);
                Console.WriteLine("Quantity: \t" + Quantity);
            }

            /*Console.WriteLine("GGGGGGG");*/

            //Console.WriteLine("the symbol is " + order.Symbol + " : I will Execute Partially !");

            //Session.SendToTarget(order, sessionID);
            //ToApp(null, sessionID);
            //Console.WriteLine("Entered!");
            TreatOrder(order);
            /* if (IsMatching(order)) 
              { Console.WriteLine("This Order is Matching a Rule !"); }
                else { Console.WriteLine("No Rule Match This Order"); }
                       Console.WriteLine("Fix Message Recived : ");

                              if (order.Header.IsSetField(QuickFix.Fields.Tags.SenderCompID))
                              {
                                   string senderCompID = order.Header.GetString(QuickFix.Fields.Tags.SenderCompID);
                                    Console.WriteLine("SenderCompID: \t" + senderCompID);
                                  }

                                           // TargetCompID
                             if (order.Header.IsSetField(QuickFix.Fields.Tags.TargetCompID))
                               {
                                 string targetCompID = order.Header.GetString(QuickFix.Fields.Tags.TargetCompID);
                                 Console.WriteLine("TargetCompID: \t" + targetCompID);
                                    }

                                                          // MsgType
                                  if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgType))
                                   {
                                     string msgType = order.Header.GetString(QuickFix.Fields.Tags.MsgType);
                                     Console.WriteLine("MsgType: \t" + msgType);
                                     if (msgType == "D") { Console.WriteLine("D is a New Order - Single"); }
                                     if (msgType == "0") { Console.WriteLine("0 is a Heartbeat"); }
                                     if (msgType == "5") { Console.WriteLine("5 is a Logout"); }
                                     if (msgType == "A") { Console.WriteLine("A is a Logon"); }
                                     if (msgType == "F") { Console.WriteLine("D is an Order Cancel Request"); }
                                     if (msgType == "8") { Console.WriteLine("D is an Execution Report"); }
                                 }

                                 // MsgSeqNum
                                 if (order.Header.IsSetField(QuickFix.Fields.Tags.MsgSeqNum))
                                 {
                                     int msgSeqNum = order.Header.GetInt(QuickFix.Fields.Tags.MsgSeqNum);
                                     Console.WriteLine("MsgSeqNum: \t" + msgSeqNum);
                                 }

                                 // SendingTime
                                 if (order.Header.IsSetField(QuickFix.Fields.Tags.SendingTime))
                                 {
                                     DateTime sendingTime = order.Header.GetDateTime(QuickFix.Fields.Tags.SendingTime);
                                     Console.WriteLine("SendingTime: \t" + sendingTime);
                                 }

                                 if (order.IsSetField(QuickFix.Fields.Tags.ClOrdID))
                                 {
                                     string clOrdID = order.GetString(QuickFix.Fields.Tags.ClOrdID);
                                     Console.WriteLine("ClOrdID: \t" + clOrdID);
                                 }

                                 // Side
                                 if (order.IsSetField(QuickFix.Fields.Tags.Side))
                                 {
                                     char side = order.GetChar(QuickFix.Fields.Tags.Side);
                                     Console.WriteLine("Side: \t" + side);
                                 }

                                 // TransactTime
                                 if (order.IsSetField(QuickFix.Fields.Tags.TransactTime))
                                 {
                                     DateTime transactTime = order.GetDateTime(QuickFix.Fields.Tags.TransactTime);
                                     Console.WriteLine("TransactTime: \t" + transactTime);
                                 }

                                 // OrderQty
                                 if (order.IsSetField(QuickFix.Fields.Tags.OrderQty))
                                 {
                                     decimal orderQty = order.GetDecimal(QuickFix.Fields.Tags.OrderQty);
                                     Console.WriteLine("OrderQty: \t" + orderQty);
                                 }

                                 // OrdType
                                 if (order.IsSetField(QuickFix.Fields.Tags.OrdType))
                                 {
                                     char ordType = order.GetChar(QuickFix.Fields.Tags.OrdType);
                                     Console.WriteLine("OrdType: \t" + ordType);
                                 }

                                 // Price
                                 if (order.IsSetField(QuickFix.Fields.Tags.Price))
                                 {
                                     decimal price = order.GetDecimal(QuickFix.Fields.Tags.Price);
                                     Console.WriteLine("Price: \t" + price);
                                 }

                                 // Symbol
                                 if (order.IsSetField(QuickFix.Fields.Tags.Symbol))
                                 {
                                     string symbol = order.GetString(QuickFix.Fields.Tags.Symbol);
                                     Console.WriteLine("Symbol: \t" + symbol);
                                 }
                                 //Quantity
                                 /*if (order.IsSetField(QuickFix.Fields.Tags.Quantity))
                                 {
                                     string Quantity = order.GetString(QuickFix.Fields.Tags.Quantity);
                                     Console.WriteLine("Quantity: \t" + Quantity);
                                 }*/

            /*Console.WriteLine("GGGGGGG");*/

            //Console.WriteLine("the symbol is " + order.Symbol + " : I will Execute Partially !");

            //Session.SendToTarget(order, sessionID);
            //ToApp(null, sessionID);
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





        /*--------------------------Another Method--------------------------------
         * string insertQuery = "INSERT INTO Fixmessage (msgBody) VALUES (@storedmessage)";
        string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=RulesDB;Integrated Security=True;Trust ;Server Certificate=True";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@msgBody", storedmessage);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Message stored successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while storing the message: " + e.Message);
            }
        }*/

        /*We have tryed with an sql query but it does not work//
        using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=RulesDB;Integrated Security=True;Trust ;Server Certificate=True"))
        {

            string storedmessage = message.ToString();
            string query = "INSERT INTO FixMessages (msgBody) VALUES (storedmessage)";
            _context.Database.ExecuteSqlRaw(query, connection);
        }*/
        /* try
         {
             string _storedMessage = message.ToString();
             Console.WriteLine("This is the message received before the pars :  " + _storedMessage);
             //Console.WriteLine("IN:  " + message);



             // Parse the FIX message and store key-value pairs
             //Dictionary<string, string> parsedFields = ParseFixMessage(fixMessageString);

             FixMessage msg = new FixMessage();
             // Debug: Print parsed fields
             Console.WriteLine("Parsed Fields:");

             foreach (var pair in parsedFields)
             {
                 Test(pair.Key, pair.Value);
             }
         }
         //  foreach (var pair in parsedFields)
         /*
             if (parsedFields.TryGetValue("8", out string value))
                 msg.BeginString = value;
             if (parsedFields.TryGetValue("35", out string val))
                 // Test(pair.Key, pair.Value);
                 msg.MsgType = val;
             if (parsedFields.TryGetValue("49", out string val1))
                 msg.SenderCompID = val1;
             if (parsedFields.TryGetValue("55", out string val2))
                 msg.Symbol = val2;
             if (parsedFields.TryGetValue("44", out string val3))
                 msg.Price = val3;
             if (parsedFields.TryGetValue("38", out string val4))
                 msg.OrderQuantity = val4;
             if (parsedFields.TryGetValue("54", out string val5))
                 msg.Side = val5;
             if (parsedFields.TryGetValue("60", out string val6) && DateTime.TryParse(val6, out DateTime transactTime))
                 msg.TransactTime = transactTime;
    >>>>>>> firstcommit
             }*/

        /* foreach (var field in parsedFields)
         {
             Console.WriteLine($"{field.Key} = {field.Value}");
         }*/


        // Extract BeginString and MessageType
        /* string beginString = parsedFields.ContainsKey("8") ? parsedFields["8"] : null;
         string messageType = parsedFields.ContainsKey("35") ? parsedFields["35"] : null;*/

        /*_context.FixMessages.Add(msg);
        _context.SaveChanges();


            */



      
      



    
        private void Test(string tag, string value)
        {

            var message = new FixMessage();
            if (tag == "8")
                Console.WriteLine($"BeginString: {value}");
            if (tag == "9")
                Console.WriteLine($"Bodylength: {value}");
            if (tag == "9")
                Console.WriteLine($"MsgType: {value}");
            if (tag == "49")
                Console.WriteLine($"SenderCompID: {value}");
            if (tag == "56")
                Console.WriteLine($"TargetCompID: {value}");
            if (tag == "34")
                Console.WriteLine($"MsgSequenceNum: {value}");
            if (tag == "55")
                Console.WriteLine($"Symbol: {value}");
            if (tag == "44")
                Console.WriteLine($"Price: {value}");
            if (tag == "38")
                Console.WriteLine($"Order Quantity: {value}");
            if (tag == "54")
                Console.WriteLine($"Side: {value}");


            // message.A;


        }


      
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

       /* public Message GetStoredMessage(Message message, SessionID sessionID)
        {
            FromApp(message, sessionID);
            return _storedMessage;
        }*/

        //Here We are spliting the message 
        /* public static Dictionary<string, string> ParseFixMessage(string fixMessage)
         {
             var parsedFields = new Dictionary<string, string>();

             // Step 1: Split the message by the '|' delimiter
             string[] keyValuePairs = fixMessage.Split('|');

             Console.WriteLine("The first Split is done   !! ");

             // Step 2: Display each key-value pair before further splitting
             Console.WriteLine("Key-Value Pairs before second split:");
             foreach (var pair in keyValuePairs)
             {
                 if (!string.IsNullOrEmpty(pair))
                 {
                     Console.WriteLine(pair);
                 }
             }

             Console.WriteLine("The Second Split is done   !! ");

             // Step 3: Split each key-value pair by the '=' delimiter and add to the dictionary
             foreach (var pair in keyValuePairs)
             {
                 if (!string.IsNullOrEmpty(pair))
                 {
                     string[] keyValue = pair.Split('=');
                     if (keyValue.Length == 2)
                     {
                         parsedFields[keyValue[0]] = keyValue[1];
                     }
                 }
             }
             Console.WriteLine("The Final Output   !! ");

             // Step 4: Display parsed key-value pairs
             Console.WriteLine("\nParsed Key-Value Pairs:");
             foreach (var field in parsedFields)
             {
                 Console.WriteLine($"Tag: {field.Key}, Value: {field.Value}");
             }

             return parsedFields;
         }
     }*/



        /*public string OnMessage()
        {
            FromApp(message, sessionID);
            string stored = message.ToString();

            return stored;
        }*/


        /* public string GetStoredMessage()
         {
             FromApp)
             return ToString(OnMessage( message, SessionID sessionID));
         }
         */

    }
}



