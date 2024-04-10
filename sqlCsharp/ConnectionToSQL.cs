using Microsoft.Data.SqlClient;

namespace sqlCsharp
{
    public class ConnectionToSQL : IConnectionToSQL 
    {
        private SqlConnection connection;
        public ConnectionToSQL(string connectionString)
        {
            ConnectionString = connectionString;
            connection = new SqlConnection(ConnectionString);
        }

        public void Connection()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))

                try
                {
                    Open();
                    Console.WriteLine("Connected successfully");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                finally { conn.Close(); }
        }

        public void Open()
        {
            connection.Open();
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }
        public string ConnectionString { get; }
    }
}
