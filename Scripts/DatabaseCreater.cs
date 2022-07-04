using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;

public class DatabaseCreater : MonoBehaviour
{

    #region Var

    public string connection;
    public IDbConnection database;

    #endregion

    private void Start()
    {
        connection = "URI=file:" + Application.persistentDataPath + "/" + "CryptoMarket";
        database = new SqliteConnection(connection);

        CreateTable();
    }

    private void CreateTable ()
    {
        database.Open();

        IDbCommand query = database.CreateCommand();
        query.CommandText = "CREATE TABLE IF NOT EXISTS Crypto (crypto_id INTEGER PRIMARY KEY, full_name TEXT, short_name TEXT)";
        query.ExecuteReader();

        database.Close();

        database.Open();

        IDbCommand query2 = database.CreateCommand();
        query2.CommandText = "CREATE TABLE IF NOT EXISTS Positions (transaction_id INTEGER PRIMARY KEY, amount REAL, invested REAL, crypto_price REAL, created_at TEXT, crypto_id INTEGER, FOREIGN KEY(crypto_id) REFERENCES Crypto(crypto_id))";
        query2.ExecuteReader();

        database.Close();
    }

}
