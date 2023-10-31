using Microsoft.Data.Sqlite;

namespace CoreLibrary.DataBase;

public partial class DataBase
{
    private static void InsertData(string connectionString, string tableName, Dictionary<string, object> data)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var command = connection.CreateCommand();

                var columns = string.Join(", ", data.Keys);
                var parameters = string.Join(", ", data.Keys.Select(k => "@" + k));

                command.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                foreach (var item in data)
                {
                    command.Parameters.AddWithValue("@" + item.Key, item.Value);
                }

                command.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}