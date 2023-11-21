using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllDishesManualLoader : MonoBehaviour
{
    public TextMeshProUGUI DebugText;
    void Start()
    {
        var connection = new DataService("productsdb.db");
        var products = connection.GetProducts();

        ToConsole(products);
    }

    private void ToConsole(IEnumerable<products> products)
    {
        foreach (var product in products)
        {
            ToConsole(product.ToString());
        }
    }
    private void ToConsole(string msg)
    {
        DebugText.text += System.Environment.NewLine + msg;
        Debug.Log(msg);
    }

}
