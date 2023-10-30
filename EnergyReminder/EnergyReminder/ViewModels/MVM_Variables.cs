using System.Reactive;
using ReactiveUI;

namespace EnergyReminder.ViewModels;

public partial class MainViewModel
{
    private int _accountIndex = 0;
    private string _amountOfEnergy = "0";
    private string _restOfTime = "0小时0分钟";
    private string _timeOfFullEnergy = "今日XX时XX分";
    private string _newAmountOfEnergy = "";

    public ReactiveCommand<Unit, Unit> StartCalculate { get; }
}