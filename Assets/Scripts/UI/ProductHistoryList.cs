
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductHistoryList : MonoBehaviour
{
    static List<ProductHistoryItem> productHistoryItem;
    void Start()
    {
        Render();
    }

    public void Render()
    {
        VisualElement mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;
        VisualElement historyList = mainRoot.Q<VisualElement>("HistoryContainer");

        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();


        List<Todays_history> todaysHistoryItems  = todaysHistoryManager.GetHistory();

        foreach (Todays_history item in todaysHistoryItems)
        {
            ProductHistoryItem historyItem = new ProductHistoryItem(item);
            productHistoryItem.Add(historyItem);
            historyList.Insert(0, historyItem);


            Debug.Log(item);
        }
    }

}
