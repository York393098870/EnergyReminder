using Microsoft.Data.Sqlite;
using static CoreLibrary.GlobalVariables;

namespace CoreLibrary.DataBase;

public partial class DataBase
{
    private static void InsertData(string tableName, Dictionary<string, object> data)
    {
        var connectionString = "Data Source=" + FullPath;
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();
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

    public static Dictionary<string, string> GetUserBasicDataDictionary()
    {
        var connectionString = "Data Source=" + FullPath;
        var userDictionary = new Dictionary<string, string>();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        const string query = "SELECT UUID, UserName FROM Users";
        using var command = new SqliteCommand(query, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var uuid = reader.GetString(0);
            var userName = reader.GetString(1);
            userDictionary.Add(uuid, userName);
        }

        return userDictionary;
    }

    public static EnergyData? GetEnergyDataByUuid(string uuid)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = GlobalVariables.FullPath
        };

        using var connection = new SqliteConnection(connectionStringBuilder.ToString());
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT AmountOfEnergy, LastUpdateTime, GameType,EnergyFullTime FROM Energy WHERE UUID = @uuid";
        command.Parameters.AddWithValue("@uuid", uuid);

        using var reader = command.ExecuteReader();

        if (!reader.Read()) throw new Exception("未找到匹配的数据");

        var amountOfEnergy = reader.GetInt32(0);
        var lastUpdateTime = reader.GetString(1);
        var gameType = reader.GetString(2);
        var energyFullTime = reader.GetString(3);

        return new EnergyData
        {
            OldAmountOfEnergy = amountOfEnergy,
            LastUpdateTime = lastUpdateTime,
            GameType = gameType,
            EnergyFullTime = energyFullTime
        };
    }

    public static void UpdateEnergy(string uuid, int? amountOfEnergy = null, string? lastUpdateTime = null,
        string? gameType = null,
        string? energyFullTime = null)
    {
        Console.WriteLine("尝试写入");

        var connectionString = "Data Source=" + FullPath;

        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        using var transaction = connection.BeginTransaction();
        var command = connection.CreateCommand();

        command.CommandText = """
                              
                                      UPDATE Energy
                                      SET
                                        AmountOfEnergy = COALESCE(@AmountOfEnergy, AmountOfEnergy),
                                        LastUpdateTime = COALESCE(@LastUpdateTime, LastUpdateTime),
                                        GameType = COALESCE(@GameType, GameType),
                                        EnergyFullTime = COALESCE(@EnergyFullTime, EnergyFullTime)
                                      WHERE UUID = @UUID
                                    
                              """;

        command.Parameters.AddWithValue("@UUID", uuid);
        command.Parameters.AddWithValue("@AmountOfEnergy", amountOfEnergy);
        command.Parameters.AddWithValue("@LastUpdateTime", lastUpdateTime);
        command.Parameters.AddWithValue("@GameType", gameType);
        command.Parameters.AddWithValue("@EnergyFullTime", energyFullTime);

        try
        {
            command.ExecuteNonQuery();
            transaction.Commit();
            Console.WriteLine("修改成功");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine("数据库写入失败,可能存在冲突。");
            Console.WriteLine("异常消息:" + ex.Message);
            Console.WriteLine("堆栈跟踪:" + ex.StackTrace);
        }
    }
}