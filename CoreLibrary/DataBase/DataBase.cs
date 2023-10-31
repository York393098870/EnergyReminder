namespace CoreLibrary.DataBase;

public partial class DataBase
{
    public static bool CheckDataBaseIfExisted()
    {
        return File.Exists(GlobalVariables.FullPath);
    }
}