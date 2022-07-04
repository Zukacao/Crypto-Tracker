using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class TransactionInput : MonoBehaviour
{

    #region Var

    public string connection;
    public IDbConnection database;

    private IDataReader reader;

    public TMP_InputField amount;
    public TMP_InputField invested;

    public TMP_Dropdown cryptoId;

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

    public void Input()
    {
        database.Open();

        IDbCommand query = database.CreateCommand();
        query.CommandText = "INSERT INTO Positions (amount, invested, crypto_price, created_at, crypto_id) VALUES ('" + AmountInput() + "', '" + Invested() + "', '" + CryptoPriceInput() + "', '" + SystemTime() + "', '" + CryptoId() + "')";
        query.ExecuteReader();

        database.Close();
    }

    #region Input Values

    private float AmountInput()
    {
        return float.Parse(amount.GetComponent<TMP_InputField>().text);
    }

    private float Invested()
    {
        return float.Parse(invested.GetComponent<TMP_InputField>().text);
    }

    private float CryptoPriceInput()
    {

        return float.Parse(invested.GetComponent<TMP_InputField>().text) / float.Parse(amount.GetComponent<TMP_InputField>().text);
    }
    private string SystemTime()
    {
        return System.DateTime.Now.ToString("dd/MM/yyyy");
    }

    public int CryptoId()
    {
        return cryptoId.value + 1;
    }

    #endregion

    public void Checker()
    {
        //error za los unos sredi nekako
    }
}
