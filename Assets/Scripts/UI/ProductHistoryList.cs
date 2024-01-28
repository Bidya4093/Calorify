
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ProductHistoryList : MonoBehaviour
{
    static public List<ProductHistoryItem> items = new List<ProductHistoryItem> { };
    static VisualElement mainRoot;
    static public VisualElement historyList;
    static VisualElement historyEmptyContainer;
    static public VisualElement lastItem;
    static public bool empty = true;

    void Start()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null) return;
        Render();
        CheckEmptyList();
    }

    public void Render()
    {
        mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;
        historyList = mainRoot.Q<VisualElement>("HistoryContainer");

        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();

        //for (int i = 0; i < 10; i++)
        //{
        //    Todays_history todaysHistory = todaysHistoryManager.InsertRecord(7, 100, FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        //    ProductHistoryItem productHistoryItem = new ProductHistoryItem(todaysHistory);
        //}

        List<Todays_history> todaysHistoryItems = todaysHistoryManager.GetCurrentUserHistory();


        foreach (Todays_history item in todaysHistoryItems)
        {
            ProductHistoryItem historyItem = new ProductHistoryItem(item);
            items.Insert(0, historyItem);
            historyList.Insert(0, historyItem);
        }
        lastItem = historyList.Query<ProductHistoryItem>("HistoryItem").Last();

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
