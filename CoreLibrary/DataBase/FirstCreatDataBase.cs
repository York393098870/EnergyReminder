namespace CoreLibrary.DataBase;

using Microsoft.Data.Sqlite;

public partial class DataBase
{
    private static void CreateDataBase()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = GlobalVariables.FullPath
        };

        using var connection = new SqliteConnection(connectionStringBuilder.ToString());
        connection.Open();

        using var transaction = connection.BeginTransaction();
        using (var command = connection.CreateCommand())
        {
            //Create table "User"
            command.CommandText = """
                                  
                                                                         CREATE TABLE IF NOT EXISTS Users (
                                                                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                             UserName TEXT NOT NULL,
                                                                             UUID TEXT NOT NULL UNIQUE
                                                                         );
                                                                         
                                  """;
            command.ExecuteNonQuery();

            //Create table "Energy"
            command.CommandText = """
                                  
                                                                         CREATE TABLE IF NOT EXISTS Energy (
                                                                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                             UUID TEXT NOT NULL,
                                                                             AmountOfEnergy INTEGER NOT NULL,
                                                                             LastUpdateTime TEXT NOT NULL,
                                                                             FOREIGN KEY(UUID) REFERENCES Users(UUID)
                                                                         );
                                                                         
                                  """;
            command.ExecuteNonQuery();
        }

        transaction.Commit();
    }
    
    
}