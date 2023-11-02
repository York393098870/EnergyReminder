using System.Globalization;
using System.Transactions;

namespace CoreLibrary.DataBase;

public partial class DataBase
{
    public static void FirstUse()
    {
        using var transaction = new TransactionScope();

        CreateDataBase();
        InitializeInsert();
        transaction.Complete();
    }

    private static void InitializeInsert()
    {
        InsertData(tableName: "Users", data: new Dictionary<string, object>
        {
            { "UUID", "000001" },
            { "Username", "默认用户" }
        });
        InsertData(tableName: "Energy", data: new Dictionary<string, object>
        {
            { "UUID", "000001" }, { "AmountOfEnergy", 1 },
            { "LastUpdateTime", DateTime.Now.ToString(CultureInfo.CurrentCulture) }, { "GameType", "崩坏：星穹铁道" },
            {
                "EnergyFullTime",
                DateTime.Now.AddHours(24).ToString(CultureInfo.CurrentCulture)
            }
        });
    }

    public static void TestMethod(bool enable = false)
    {
        if (!enable) return;
        InsertData(tableName: "Users", data: new Dictionary<string, object>
        {
            { "UUID", "000002" },
            { "Username", "测试" }
        });
        InsertData(tableName: "Energy", data: new Dictionary<string, object>
        {
            { "UUID", "000002" }, { "AmountOfEnergy", 2 },
            { "LastUpdateTime", DateTime.Now.ToString(CultureInfo.CurrentCulture) }, { "GameType", "崩坏：星穹铁道" },
            {
                "EnergyFullTime",
                DateTime.Now.AddHours(24).ToString(CultureInfo.CurrentCulture)
            }
        });
    }
}