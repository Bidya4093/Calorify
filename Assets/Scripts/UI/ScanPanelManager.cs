using Firebase.Auth;
using System;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ScanPanelManager : MonoBehaviour
{
    private ProductPanel productPanel;

    ProductsLoader productsLoader;
    products product;


    void Start()
    {

        productsLoader = new ProductsLoader();
        productPanel = GetComponent<ProductPanel>();
    }

    private void AddProductToDailyList(ClickEvent evt)
    {
        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();
        Todays_history todaysHistory = todaysHistoryManager.InsertRecord(product.product_id, productPanel.massInput.value, FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        ProductHistoryItem productHistoryItem = new ProductHistoryItem(todaysHistory);
        ProductHistoryList.items.Add(productHistoryItem);

        User.AddToEaten(productHistoryItem.macrosInfo);

        DataManager.LoadChartsData();

        PanelManager panelManager = GameObject.Find("MainPage").GetComponent<PanelManager>();

        panelManager.ResetDisplay();
        panelManager.ToHome();
        panelManager.homeBtn.AddToClassList("menu__item--active");

        productPanel.addProductBtn.UnregisterCallback<ClickEvent>(AddProductToDailyList);
    }

    public void LoadProductData(string vuforiaId)
    {

        productPanel.Show();
        product = productsLoader.GetByVuforiaId(vuforiaId);
        productPanel.SetProductData(product);

        productPanel.addProductBtn.RegisterCallback<ClickEvent>(AddProductToDailyList);
    }
}
