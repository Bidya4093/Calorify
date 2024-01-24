
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductHistoryList : MonoBehaviour
{
    static public List<ProductHistoryItem> items = new List<ProductHistoryItem> { };
    static VisualElement mainRoot;
    static VisualElement historyList;
    static VisualElement historyEmptyContainer;
    static public bool empty = true;

    void Start()
    {
        Render();
        CheckEmptyList();
    }

    public void Render()
    {
        mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;
        historyList = mainRoot.Q<VisualElement>("HistoryContainer");

        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();

        List<Todays_history> todaysHistoryItems  = todaysHistoryManager.GetCurrentUserHistory();

        foreach (Todays_history item in todaysHistoryItems)
        {
            ProductHistoryItem historyItem = new ProductHistoryItem(item);
            items.Insert(0, historyItem);
            historyList.Insert(0, historyItem);
            empty = false;
        }
    }

    static public void CheckEmptyList()
    {
        historyEmptyContainer = mainRoot.Q<VisualElement>("HistoryEmptyContainer");

        if (historyList.Query<ProductHistoryItem>("HistoryItem").ToList().Count == 0)
        {
            historyEmptyContainer.style.display = DisplayStyle.Flex;
            empty = true;
        } else
        {
            historyEmptyContainer.style.display = DisplayStyle.None;
            empty = false;
        }
    }

}
