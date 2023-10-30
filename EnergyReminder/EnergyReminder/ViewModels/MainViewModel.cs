using System.Globalization;
using CoreLibrary;
using ReactiveUI;

namespace EnergyReminder.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        StartCalculate = ReactiveCommand.Create(CalculateInformationAndShow);
    }

    public void CalculateInformationAndShow()
    {
        AmountOfEnergy = NewAmountOfEnergy;
        var restOfEnergy = 240 - int.Parse(AmountOfEnergy);
        var timeCalculator = new TimeCalculator(restOfEnergy);
        var resultOfNewDueTime = timeCalculator.TimeCalculate();
        var restOfTime = timeCalculator.ConvertMinutesToHours(timeCalculator.CalculateRestOfMinutes()).TimePeriod;
        RestOfTime = restOfTime;
        TimeOfFullEnergy = resultOfNewDueTime.ToString(CultureInfo.CurrentCulture);
    }
}