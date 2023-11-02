namespace CoreLibrary.Tools;

public class NewCalculator
{
    public static (int RestOfEnergy, int EnergyNow) CalculateNewEnergy(string energyFullTIme)
    {
        var dateTimeNow = DateTime.Now;
        var energyFullTime = DateTime.Parse(energyFullTIme);
        var restOfMinutes = (energyFullTime - dateTimeNow).TotalMinutes;
        var restOfEnergy = (int)(restOfMinutes / 6);
        var energyNow = 239 - restOfEnergy;
        return (RestOfEnergy: restOfEnergy, EnergyNow: energyNow);
    }

    public static string CalculateTimeForEnergyFill(int energyNow)
    {
        var energyDifference = 239 - energyNow;
        var result = ConvertMinutesToHours(energyDifference * 6);
        return result.TimePeriod;
    }

    private static (int Hour, int Minutes, string TimePeriod) ConvertMinutesToHours(int minutes)
    {
        var resultOfHour = minutes / 60;
        var resultOfMinutes = minutes % 60;
        return (Hour: resultOfHour, Minutes: resultOfMinutes, TimePeriod: $"{resultOfHour}小时{resultOfMinutes}分钟");
    }
}