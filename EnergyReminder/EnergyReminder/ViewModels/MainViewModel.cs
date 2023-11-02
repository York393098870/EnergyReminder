using System;
using System.Collections.ObjectModel;
using System.Globalization;
using CoreLibrary;
using CoreLibrary.DataBase;
using CoreLibrary.Tools;
using ReactiveUI;

namespace EnergyReminder.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        InitializeDataBase(); //初始化数据库

        Test(); //Todo: Remove this line.This is Only For Test.

        StartCalculate = ReactiveCommand.Create(CalculateInformationAndShow);

        var userBasicDataDictionary = DataBase.GetUserBasicDataDictionary();
        ComboBoxItems = new ObservableCollection<string>();


        foreach (var value in userBasicDataDictionary.Values)
        {
            ComboBoxItems.Add(value);
        }

        SelectedItem = ComboBoxItems[0];

        UuidShowed = CoreTools.GetKeyFromValue(DataBase.GetUserBasicDataDictionary(), SelectedItem);


        TimeOfFullEnergy = DataBase.GetEnergyDataByUuid(UuidShowed).EnergyFullTime;
    }

    private void CalculateInformationAndShow()
    {
        AmountOfEnergy = NewAmountOfEnergy;
        var restOfEnergy = 239 - int.Parse(AmountOfEnergy);
        var timeCalculator = new TimeCalculator(restOfEnergy);
        var resultOfNewDueTime = timeCalculator.TimeCalculate();
        var restOfTime = timeCalculator.ConvertMinutesToHours(timeCalculator.CalculateRestOfMinutes()).TimePeriod;
        RestOfTime = restOfTime;
        TimeOfFullEnergy = resultOfNewDueTime.ToString(CultureInfo.CurrentCulture);
    }

    private void ChangedItem(int option)
    {
        //当选择的账号发生变化时，更新显示的数据
        var newSelectedItem = ComboBoxItems[option];
        UuidShowed = CoreTools.GetKeyFromValue(DataBase.GetUserBasicDataDictionary(), newSelectedItem);
    }

    private void InitializeDataBase(bool update = false)
        //初始化
    {
        if (DataBase.CheckDataBaseIfExisted() && !update)
        {
            //读取数据库当中的数据
            Console.WriteLine("数据库已存在，将读取已有的数据库数据。");
        }
        else
        {
            DataBase.FirstUse();
            Console.WriteLine("数据库不存在，将创建新的数据库。");
        }
    }

    private void Test()
    {
        DataBase.TestMethod();
    }
}