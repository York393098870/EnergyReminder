namespace CoreLibrary;

public class TimeCalculator
{
    private int RestOfEnergy { get; set; }

    public TimeCalculator(int restOfEnergy)
    {
        RestOfEnergy = restOfEnergy;
    }

    public DateTime TimeCalculate()
    {
        var dateTimeNow = DateTime.Now;
        var newDateTime = dateTimeNow.AddMinutes(CalculateRestOfMinutes());
        return newDateTime;
    }

    public int CalculateRestOfMinutes()
    {
        var restOfMinutes = 6 * RestOfEnergy;
        return restOfMinutes;
    }
    
    

    public (int Hour, int Minutes, string TimePeriod) ConvertMinutesToHours(int minutes)
    {
        var resultOfHour = minutes / 60;
        var resultOfMinutes = minutes % 60;
        return (Hour: resultOfHour, Minutes: resultOfMinutes, TimePeriod: $"{resultOfHour}小时{resultOfMinutes}分钟");
    }
}