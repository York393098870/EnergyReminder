using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Transactions;
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


        StartCalculate = ReactiveCommand.Create(CalculateInformationAndShow);

        var userBasicDataDictionary = DataBase.GetUserBasicDataDictionary();
        ComboBoxItems = new ObservableCollection<string>();


        foreach (var value in userBasicDataDictionary.Values) ComboBoxItems.Add(value);

        SelectedItem = ComboBoxItems[0];

        UuidShowed = CoreTools.GetKeyFromValue(DataBase.GetUserBasicDataDictionary(), SelectedItem);

        UpdateUiData(UuidShowed);
    }

    private void CalculateInformationAndShow()
    {
        using var transaction = new TransactionScope();

        AmountOfEnergyNow = (int.Parse(NewAmountOfEnergy) - 1).ToString();
        var restOfEnergy = 240 - int.Parse(AmountOfEnergyNow);
        var timeCalculator = new OldTimeCalculator(restOfEnergy);
        var resultOfNewDueTime = timeCalculator.TimeCalculate();
        var restOfTime = timeCalculator.ConvertMinutesToHours(timeCalculator.CalculateRestOfMinutes()).TimePeriod;
        RestOfTime = restOfTime;
        TimeOfFullEnergy = resultOfNewDueTime.ToString(CultureInfo.CurrentCulture);
        DataBase.UpdateEnergyToDataBase(UuidShowed, int.Parse(AmountOfEnergyNow),
            DateTime.Now.ToString(CultureInfo.CurrentCulture), energyFullTime: TimeOfFullEnergy);

        transaction.Complete();
    }

    private void ChangedItem(int option)
    {
        //当选择的账号发生变化时，更新显示的数据
        var newSelectedItem = ComboBoxItems[option];
        UuidShowed = CoreTools.GetKeyFromValue(DataBase.GetUserBasicDataDictionary(), newSelectedItem);
        UpdateUiData(UuidShowed);
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


    private void UpdateUiData(string uuid)
    {
        var energyBasicData = DataBase.GetEnergyDataByUuid(uuid);
        TimeOfFullEnergy = energyBasicData.EnergyFullTime;
        AmountOfEnergyNow = NewCalculator.CalculateNewEnergy(TimeOfFullEnergy).EnergyNow.ToString();
        RestOfTime =
            NewCalculator.CalculateTimeForEnergyFill(NewCalculator.CalculateNewEnergy(TimeOfFullEnergy).EnergyNow);
        GameType = energyBasicData.GameType;
    }
}