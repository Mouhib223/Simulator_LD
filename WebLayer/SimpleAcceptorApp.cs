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
        /*public class OrderProcessor
        {
            


        }*/
            /*public class DatabaseHandler
        {
            
        }*/
        
        /*public void TreatOrder(QuickFix.FIX44.NewOrderSingle order)
        {

            //Console.WriteLine("Entered!");

            var rules = _rules.GetAllRules();
            //Console.WriteLine("Entered!");

            foreach (DBLayer.DAOs.Rule rule in rules)
            {

                Console.WriteLine("Iam in process order");
                *//*if (rule.IsMatching(order))
                {
                    rule.ProcessOrder(order);
                }*//*
            }
        }*/


        public void OnMessage(QuickFix.FIX44.NewOrderSingle order, SessionID sessionID)
        {


            FixMessageBo dbHandler = new FixMessageBo();
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
                        OrderBo processor = new OrderBo();
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
            
            if (OrderBo.IsMatching(order)) { Console.WriteLine(); }
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
            //TreatOrder(order);
            
        }
        //OrderCancelRequest Report
        public void OnMessage(QuickFix.FIX44.OrderCancelRequest order, SessionID s)
        {
            FixMessageBo dbHandler = new FixMessageBo();
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
                        OrderBo processor = new OrderBo();
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

            if (OrderBo.IsMatchingCancel(order)) { Console.WriteLine(); }
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
            FixMessageBo dbHandler = new FixMessageBo();
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
                OrderBo processor = new OrderBo();
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

            if (OrderBo.IsMatchingCancelReplace(order)) { Console.WriteLine("Cancel Rule Matched !"); }
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



