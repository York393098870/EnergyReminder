namespace CoreLibrary.DataBase;

public partial class DataBase
{
    public class EnergyData
    {
        public int OldAmountOfEnergy { get; set; }
        public string LastUpdateTime { get; set; }
        public string GameType { get; set; }

        public string EnergyFullTime { get; set; }
    }
}