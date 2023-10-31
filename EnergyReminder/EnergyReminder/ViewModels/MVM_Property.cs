﻿using System.Collections.ObjectModel;
using ReactiveUI;

namespace EnergyReminder.ViewModels;

public partial class MainViewModel
{
    public int AccountIndex
    {
        get => _accountIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _accountIndex, value);
            ChangedItem(value);
        }
    }

    public string AmountOfEnergy
    {
        get => _amountOfEnergy;
        set => this.RaiseAndSetIfChanged(ref _amountOfEnergy, value);
    }

    public string RestOfTime
    {
        get => _restOfTime;
        set => this.RaiseAndSetIfChanged(ref _restOfTime, value);
    }

    public string TimeOfFullEnergy
    {
        get => _timeOfFullEnergy;
        set => this.RaiseAndSetIfChanged(ref _timeOfFullEnergy, value);
    }

    public string NewAmountOfEnergy
    {
        get => _newAmountOfEnergy;
        set => this.RaiseAndSetIfChanged(ref _newAmountOfEnergy, value);
    }

    public ObservableCollection<string> ComboBoxItems
    {
        get => _comboBoxItems;
        set => this.RaiseAndSetIfChanged(ref _comboBoxItems, value);
    }

    public string GameType
    {
        get => _gameType;
        set => this.RaiseAndSetIfChanged(ref _gameType, value);
    }

    public string AccountID
    {
        get => _accoutID;
        set => this.RaiseAndSetIfChanged(ref _accoutID, value);
    }
}