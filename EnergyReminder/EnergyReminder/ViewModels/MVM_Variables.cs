using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace EnergyReminder.ViewModels;

public partial class MainViewModel
{
    private int _accountIndex;
    private string _amountOfEnergyNow = "0";
    private ObservableCollection<string> _comboBoxItems = new();
    private string _gameType = "无";
    private string _newAmountOfEnergy = "";
    private string _restOfTime = "0小时0分钟";
    private string _selectedItem = "";
    private string _timeOfFullEnergy = "今日XX时XX分";
    private string _uuidShowed = "尚未选择账号";

    public ReactiveCommand<Unit, Unit> StartCalculate { get; }
}