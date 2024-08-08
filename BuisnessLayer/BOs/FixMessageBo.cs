using Microsoft.Data.SqlClient;
using SimulatorLD.DBLayer.DAOs;
using SimulatorLD.DBLayer.Repository;

namespace SimulatorLD.BuisnessLayer.BOs
{
    public class FixMessageBo
    {

        private FixMessageRepo _msg;

        public FixMessageBo()
        {
            _msg = new DBLayer.Repository.FixMessageRepo();
        }

        public List<Fixmessage> GetAllMessages()
        {

            return _msg.GetAllMessages();
        }

        public void AddMessage(Fixmessage msg)
        {
            _msg.AddMessage(msg);


        }
        public void InsertFixMessage(QuickFix.FIX44.Message order)
        {
            //private string connectionString = "server=TN1PFE-008\\SQLEXPRESS; database=RulesDB;Integrated Security=True; TrustServerCertificate=True"; // Replace with your actual connection string

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
}
