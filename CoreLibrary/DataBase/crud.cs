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

        foreach (var item in data) command.Parameters.AddWithValue("@" + item.Key, item.Value);

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

    public static EnergyData GetEnergyDataByUuid(string uuid)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = FullPath
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

    public static void UpdateEnergyToDataBase(string uuid, int? amountOfEnergy = null, string? lastUpdateTime = null,
        string? gameType = null, string? energyFullTime = null)
    {
        var connectionString = "Data Source=" + FullPath;
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        var command = connection.CreateCommand();
        var setClauses = new List<string>();

        if (AddParameterIfNotNull(command, "@AmountOfEnergy", amountOfEnergy))

            setClauses.Add("AmountOfEnergy = @AmountOfEnergy");

        if (AddParameterIfNotNull(command, "@LastUpdateTime", lastUpdateTime))

            setClauses.Add("LastUpdateTime = @LastUpdateTime");

        if (AddParameterIfNotNull(command, "@GameType", gameType))

            setClauses.Add("GameType = @GameType");

        if (AddParameterIfNotNull(command, "@EnergyFullTime", energyFullTime))

            setClauses.Add("EnergyFullTime = @EnergyFullTime");

        if (setClauses.Count == 0)
        {
            Console.WriteLine("没有提供更新的信息，操作取消。");
            return;
        }

        command.CommandText = $"""
                                 UPDATE Energy
                                 SET {string.Join(", ", setClauses)}
                                 WHERE UUID = @UUID
                               """;
        command.Parameters.AddWithValue("@UUID", uuid);

        try
        {
            command.ExecuteNonQuery();
            transaction.Commit();
            Console.WriteLine("修改成功");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("数据库写入失败，可能存在冲突。", ex);
        }
    }

    private static bool AddParameterIfNotNull(SqliteCommand command, string parameterName, object? value)
    {
        if (value == null) return false; // Parameter was not added
        command.Parameters.AddWithValue(parameterName, value);
        return true; // Parameter was added
    }
}