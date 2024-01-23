using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Printer2 : MonoBehaviour
{
    public TMP_Text output;
    private void Start()
    {
        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();
        ProductsLoader productsLoader = new ProductsLoader();
        foreach (Todays_history record in todaysHistoryManager.GetHistory())
        {
            products dish = productsLoader.GetById(record.product_id);
            Debug.Log($"Назва: {dish.name}; Маса: {record.mass}; Калорії: {record.mass * dish.calories / 100}");
        }
    }
}
