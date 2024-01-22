using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Printer2 : MonoBehaviour
{
    private void Start()
    {
        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();
        ProductsLoader productsLoader = new ProductsLoader();

        todaysHistoryManager.InsertRecord(3, 45f, FirebaseAuth.DefaultInstance.CurrentUser.UserId);

        foreach(Todays_history record in todaysHistoryManager.GetHistory())
        {
            products dish = productsLoader.GetById(record.product_id);
            Debug.Log($"Назва: {dish.name}; Маса: {record.mass}; Калорії: {record.mass * dish.calories / 100}");
        }
    }
}
