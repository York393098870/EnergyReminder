using System;
using System.Diagnostics;
using System.Globalization;
using CoreLibrary;
using CoreLibrary.DataBase;
using ReactiveUI;

namespace EnergyReminder.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        StartCalculate = ReactiveCommand.Create(CalculateInformationAndShow);
        InitializeDataBase();
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

    private void InitializeDataBase()
    {
        if (DataBase.CheckDataBaseIfExisted())
        {
            Console.WriteLine("DataBase existed.");
        }
        else
        {
            DataBase.CreateDataBase();
            Console.WriteLine("DataBase created.");
        }
    }
}