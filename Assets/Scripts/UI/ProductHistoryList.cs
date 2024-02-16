using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;
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
        mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;
        historyList = mainRoot.Q<VisualElement>("HistoryContainer");
    }

    static public void Render()
    {
        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();

        List<Todays_history> todaysHistoryItems = todaysHistoryManager.GetCurrentUserHistory();


        foreach (Todays_history item in todaysHistoryItems)
        {
            ProductHistoryItem historyItem = new ProductHistoryItem();
            historyItem.InitializeAsync(item);
            items.Insert(0, historyItem);
            Debug.Log(historyItem);
            historyList.Insert(0, historyItem);
        }

        lastItem = historyList.Query<ProductHistoryItem>("HistoryItem").Last();
        CheckEmptyList();
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
