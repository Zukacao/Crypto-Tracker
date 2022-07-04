using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;
using TMPro;


public class CoinAdding : MonoBehaviour
{

    #region Var

    public string connection;
    public IDbConnection database;

    private IDataReader reader;

    public TMP_InputField coinName;
    public TMP_InputField coin;

    #endregion

    private void Start()
    {
        connection = "URI=file:" + Application.persistentDataPath + "/" + "CryptoMarket";
        database = new SqliteConnection(connection);
    }

    public void Add()
    {
        if (Checker() == true)
        {
            database.Open();

            IDbCommand query = database.CreateCommand();
            query.CommandText = "INSERT INTO Crypto (full_name, short_name) VALUES ('" + coinName.GetComponent<TMP_InputField>().text.ToLower() + "', '" + coin.GetComponent<TMP_InputField>().text.ToUpper() + "')";
            query.ExecuteReader();

            database.Close();
        }
        else
        {
            BadInput();
        }
    }

    public bool Checker()
    {
        bool check = false;

        if(coin.GetComponent<TMP_InputField>().text.Length > 1)
        {
            database.Open();

            IDbCommand query = database.CreateCommand();
            query.CommandText = "SELECT short_name FROM Crypto";
            reader = query.ExecuteReader();
            
            if(reader[0].ToString().Equals(""))
            {
                check = true;
            }
            
            while (reader.Read())
            {
                if (reader[0].ToString() != coin.GetComponent<TMP_InputField>().text.ToUpper())
                {
                    check = true;
                }
            }

            database.Close();
        }

        return check;
        
    }

    private void BadInput()
    {

    }
}
