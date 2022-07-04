using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Net;

public class ActivePositions : MonoBehaviour
{

    #region Var

    public string connection;
    public IDbConnection database;

    private IDataReader reader;
    private IDataReader reader2;

    public TMP_Dropdown cryptoId;

    public TextMeshProUGUI test;

    private double currentPrice;

    private double sAmount = 0;
    private double sInvested = 0;
    private double averageBuyPrice;

    #endregion


    private void Start()
    {
        connection = "URI=file:" + Application.persistentDataPath + "/" + "CryptoMarket";
        database = new SqliteConnection(connection);

        cryptoId.options.Clear();

        database.Open();

        IDbCommand query = database.CreateCommand();
        query.CommandText = "SELECT short_name FROM Crypto";
        reader = query.ExecuteReader();
        while (reader.Read())
        {
            cryptoId.options.Add(new TMP_Dropdown.OptionData() { text = reader[0].ToString() });
        }

        database.Close();

    }


    public void FindCyptoPrice()
    {

        database.Open();

        IDbCommand query = database.CreateCommand();
        query.CommandText = "SELECT full_name FROM Crypto WHERE crypto_id ='" + CryptoId() + "'";
        reader = query.ExecuteReader();

        string web = new WebClient().DownloadString("https://www.binance.com/en/price/" + reader[0].ToString() + "");

        string pomStart = "The live price of " + cryptoId.options[cryptoId.value].text + " is $ ";
        string pomEnd = " per";

        if (web.Contains(pomStart))
        {
            int pom = web.IndexOf(pomStart, 0) + pomStart.Length;
            int pom2 = web.IndexOf(pomEnd, pom);
            web = web.Substring(pom, pom2 - pom);

            currentPrice = double.Parse(web);
        }

        database.Close();

        Debug.Log(currentPrice);
    }

    public void AverageBuyPrice()
    {
        database.Open();

        IDbCommand query = database.CreateCommand();
        query.CommandText = "SELECT amount FROM Positions WHERE crypto_id = '" + CryptoId() + "'";
        reader = query.ExecuteReader();

        while (reader.Read())
        {
            sAmount += double.Parse(reader[0].ToString());
        }

        IDbCommand query2 = database.CreateCommand();
        query2.CommandText = "SELECT invested FROM Positions WHERE crypto_id = '" + CryptoId() + "'";
        reader = query2.ExecuteReader();

        while (reader.Read())
        {
            sInvested += double.Parse(reader[0].ToString());
        }

        averageBuyPrice = sAmount / sInvested;

    }

    public int CryptoId()
    {
        return cryptoId.value + 1;
    }
}
