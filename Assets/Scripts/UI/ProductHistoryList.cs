
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductHistoryList : MonoBehaviour
{
    List<ProductHistoryItem> productHistoryItems = new List<ProductHistoryItem> { };
    void Start()
    {
        Render();
    }

    public void Render()
    {
        VisualElement mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;
        VisualElement historyList = mainRoot.Q<VisualElement>("HistoryContainer");

        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();

        List<Todays_history> todaysHistoryItems  = todaysHistoryManager.GetCurrentUserHistory();

        foreach (Todays_history item in todaysHistoryItems)
        {
            ProductHistoryItem historyItem = new ProductHistoryItem(item);
            productHistoryItems.Insert(0, historyItem);
            historyList.Insert(0, historyItem);
        }
    }



}
