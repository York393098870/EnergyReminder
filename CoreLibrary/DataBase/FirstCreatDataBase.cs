﻿using Microsoft.Data.Sqlite;

namespace CoreLibrary.DataBase;

public partial class DataBase
{
    private static void CreateDataBase()
    {
        //创建数据库
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = GlobalVariables.FullPath
        };

        using var connection = new SqliteConnection(connectionStringBuilder.ToString());
        connection.Open();

        using var transaction = connection.BeginTransaction();
        using (var command = connection.CreateCommand())
        {
            //Create table "User"
            command.CommandText = """
                                  
                                                                         CREATE TABLE IF NOT EXISTS Users (
                                                                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                             UserName TEXT NOT NULL,
                                                                             UUID TEXT NOT NULL UNIQUE
                                                                         );
                                                                         
                                  """;
            command.ExecuteNonQuery();

            //Create table "Energy"
            command.CommandText = """
                                  
                                                                         CREATE TABLE IF NOT EXISTS Energy (
                                                                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                             UUID TEXT NOT NULL,
                                                                             AmountOfEnergy INTEGER NOT NULL,
                                                                             LastUpdateTime TEXT NOT NULL,GameType TEXT NOT NULL,EnergyFullTime TEXT NOT NULL,
                                                                             FOREIGN KEY(UUID) REFERENCES Users(UUID)
                                                                         );
                                                                         
                                  """;
            command.ExecuteNonQuery();
        }

        transaction.Commit();
    }
}