namespace CoreLibrary.Tools;

public class CoreTools
{
    public string GetNewUuid()
    {
        var uuid = Guid.NewGuid().ToString();
        return uuid;
    }

    public static string GetKeyFromValue(Dictionary<string, string> dictionary, string value)
    {
        var key = dictionary.FirstOrDefault(x => x.Value == value).Key;
        return key;
    }
}