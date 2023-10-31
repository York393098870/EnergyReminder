using System;
using System.Collections.ObjectModel;
using System.Globalization;
using CoreLibrary;
using CoreLibrary.DataBase;
using ReactiveUI;

namespace EnergyReminder.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        Initialize(); //初始化
        StartCalculate = ReactiveCommand.Create(CalculateInformationAndShow);

        ComboBoxItems = new ObservableCollection<string>
        {
            "默认账号1",
            "默认账号2"
        };
    }

    private void CalculateInformationAndShow()
    {
        AmountOfEnergy = NewAmountOfEnergy;
        var restOfEnergy = 240 - int.Parse(AmountOfEnergy);
        var timeCalculator = new TimeCalculator(restOfEnergy);
        var resultOfNewDueTime = timeCalculator.TimeCalculate();
        var restOfTime = timeCalculator.ConvertMinutesToHours(timeCalculator.CalculateRestOfMinutes()).TimePeriod;
        RestOfTime = restOfTime;
        TimeOfFullEnergy = resultOfNewDueTime.ToString(CultureInfo.CurrentCulture);
    }

    private void ChangedItem(int option)
    {
        switch (option)
        {
            case 1:
                AmountOfEnergy = "0";
                RestOfTime = "0小时0分钟";
                break;
            default:
                return;
        }
    }

    private void Initialize(bool update = false)
        //初始化
    {
        if (DataBase.CheckDataBaseIfExisted() && !update)
        {
            //读取数据库当中的数据
            Console.WriteLine("DataBase existed.");
        }
        else
        {
            DataBase.FirstUse();
            Console.WriteLine("DataBase created.");
        }
    }
}