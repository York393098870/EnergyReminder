﻿namespace CoreLibrary;

public class GlobalVariables
{
    private static string FilePath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string FileName => "EnergyReminder_DataBase.db";
    public static string FullPath { get; } = Path.Combine(FilePath, FileName);
}