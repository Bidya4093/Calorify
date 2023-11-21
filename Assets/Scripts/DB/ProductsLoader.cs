using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductsLoader : MonoBehaviour
{
    public List<products> dishes;

    void Start()
    {
        var connection = new DataService("productsdb.db");
        var products = connection.GetProducts();
        dishes = new List<products>();
        ToList(products);
    }

    private void ToList(IEnumerable<products> products)
    {
        foreach (var product in products)
        {
            dishes.Add(product);
        }
    }

}
