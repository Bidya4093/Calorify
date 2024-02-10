using Firebase.Auth;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManualAdd : MonoBehaviour
{
    // Start is called before the first frame update
    private VisualElement mainRoot;
    private TemplateContainer homePage;
    private TemplateContainer manualAddProductPage;
    private TemplateContainer manualAddActivityPage;
    private VisualElement productScrollContainer;
    private VisualElement activityScrollContainer;
    private VisualElement productRecentRequestsContainer;
    private VisualElement activityRecentRequestsContainer;
    private VisualElement addProductEmptyContainer;
    private VisualElement addActivityEmptyContainer;
    private VisualElement addProductRecentRequests;
    private VisualElement addActivityRecentRequests;
    private Button addProductCloseBtn;
    private Button addActivityCloseBtn;
    private TextField addProductSearchInput;
    private TextField addActivitySearchInput;

    private List<products> recentProductsRequest = new List<products>();
    private List<Activity> recentActivityRequest = new List<Activity>();

    private ProductPanel productPanel;
    //private ActivityPanel activityPanel;

    ProductsLoader productsLoader;
    ActivitiesManager activitiesManager;
    products product;
    Activity activity;

    void Start()
    {
        mainRoot = GetComponent<UIDocument>().rootVisualElement;
        homePage = mainRoot.Q<TemplateContainer>("HomePage");
        manualAddProductPage = mainRoot.Q<TemplateContainer>("ManualAddProduct");
        manualAddActivityPage = mainRoot.Q<TemplateContainer>("ManualAddActivity");

        productScrollContainer = mainRoot.Q<VisualElement>("AddProductScrollContainer");
        activityScrollContainer = mainRoot.Q<VisualElement>("AddActivityScrollContainer");

        addProductEmptyContainer = mainRoot.Q<VisualElement>("AddProductEmptyContainer");
        addActivityEmptyContainer = mainRoot.Q<VisualElement>("AddActivityEmptyContainer");

        productRecentRequestsContainer = manualAddProductPage.Q<VisualElement>("AddProductRecentRequestsContainer");
        activityRecentRequestsContainer = manualAddActivityPage.Q<VisualElement>("AddActivityRecentRequestsContainer");

        addProductRecentRequests = manualAddProductPage.Q<VisualElement>("AddProductRecentRequests");
        addActivityRecentRequests = manualAddActivityPage.Q<VisualElement>("AddActivityRecentRequests");

        addProductCloseBtn = manualAddProductPage.Q<Button>("AddProductCloseBtn");
        addActivityCloseBtn = manualAddActivityPage.Q<Button>("AddActivityCloseBtn");

        addProductSearchInput = manualAddProductPage.Q<TextField>("AddProductSearchInput");
        addActivitySearchInput = manualAddActivityPage.Q<TextField>("AddActivitySearchInput");

        productPanel = GameObject.Find("ProductPanel").GetComponent<ProductPanel>();
        //activityPanel = GameObject.Find("ActivityPanel").GetComponent<ActivityPanel>();

        productsLoader = new ProductsLoader();
        activitiesManager = new ActivitiesManager();

        addProductCloseBtn.clicked += CloseManualAddProductPage;
        addActivityCloseBtn.clicked += CloseManualAddActivityPage;

        addProductSearchInput.RegisterValueChangedCallback(SearchProduct);
        addActivitySearchInput.RegisterValueChangedCallback(SearchActivity);

        //CheckEmptyActivity();
        CheckEmptyProduct();
    }

    private void CheckEmptyProduct()
    {
        if (recentProductsRequest.Count > 0)
        {
            addProductEmptyContainer.style.display = DisplayStyle.None;
        } else
        {
            addProductEmptyContainer.style.display = DisplayStyle.Flex;
        }
    }

    //private void CheckEmptyActivity()
    //{
    //    throw new NotImplementedException();
    //}

    private void AddProductToDailyList(ClickEvent evt)
    {
        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();
        Todays_history todaysHistory = todaysHistoryManager.InsertRecord(product.product_id, productPanel.massInput.value, FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Debug.Log(todaysHistory);

        ProductHistoryItem productHistoryItem = new ProductHistoryItem(todaysHistory);
        ProductHistoryList.items.Add(productHistoryItem);

        User.AddToEaten(productHistoryItem.macrosInfo);

        DataManager.LoadChartsData();

        CloseManualAddProductPage();
        productPanel.Hide();
        recentProductsRequest.Add(product);
        BalanceRecentRequests(recentProductsRequest);

        addProductRecentRequests.Clear();
        foreach (products recentProduct in recentProductsRequest)
        {
            ManualAddProductItem productItem = new ManualAddProductItem(recentProduct);
            addProductRecentRequests.Insert(0, productItem);
        }

        addProductEmptyContainer.style.display = DisplayStyle.None;
        productRecentRequestsContainer.style.display = DisplayStyle.Flex;
        addProductSearchInput.value = "";
        productPanel.addProductBtn.UnregisterCallback<ClickEvent>(AddProductToDailyList);
    }

    private void AddActivityToDailyList(ClickEvent evt)
    {
        //TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();
        //Todays_history todaysHistory = todaysHistoryManager.InsertRecord(product.product_id, productPanel.massInput.value, FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        //ProductHistoryItem productHistoryItem = new ProductHistoryItem(todaysHistory);
        //ProductHistoryList.items.Add(productHistoryItem);

        //User.AddToEaten(productHistoryItem.macrosInfo);

        //DataManager.LoadChartsData();

        //CloseManualAddActivityPage();
        //activityPanel.Hide();

        ////recentActivityRequest.Add(activity);
        ////BalanceRecentRequests(recentActivityRequest); 

        //addProductRecentRequests.Clear();
        //foreach (Activity recentActivity in recentActivityRequest)
        //{
        //    ManualAddActivityItem activityItem = new ManualAddActivityItem(recentActivity);
        //    addActivityRecentRequests.Insert(0, activityItem);
        //}

        //productRecentRequestsContainer.style.display = DisplayStyle.Flex;
        //addActivitySearchInput.value = "";
        //activityPanel.addActivityBtn.UnregisterCallback<ClickEvent>(AddActivityToDailyList);
    }

    private void BalanceRecentRequests<T> (List<T> recentRequests)
    {
        if (recentRequests.Count > 5)
        {
            recentRequests.RemoveAt(0);
        }
    }

    public void LoadProductData(int productId)
    {
        productPanel.Show(true);
        product = productsLoader.GetById(productId);
        productPanel.SetProductData(product);
        productPanel.addProductBtn.RegisterCallback<ClickEvent>(AddProductToDailyList);
    }

    public void LoadActivityData(int activityId)
    {
        //activityPanel.Show(true);
        //activity = activitiesManager.GetById(activityId);
        //activityPanel.SetActivityData(activity);
        //activityPanel.addProductBtn.RegisterCallback<ClickEvent>(AddActivityToDailyList);
    }


    void CloseManualAddProductPage()
    {
        homePage.style.display = DisplayStyle.Flex;
        manualAddProductPage.style.display = DisplayStyle.None;
        addProductSearchInput.value = "";
    }

    void CloseManualAddActivityPage()
    {
        homePage.style.display = DisplayStyle.Flex;
        manualAddActivityPage.style.display = DisplayStyle.None;
        addActivitySearchInput.value = "";
    }

    void SearchProduct(ChangeEvent<string> evt)
    {
        productScrollContainer.Clear();

        if (String.IsNullOrEmpty(evt.newValue))
        {
            productRecentRequestsContainer.style.display = DisplayStyle.Flex;
        }
        else { 
            productRecentRequestsContainer.style.display = DisplayStyle.None;
            foreach (products item in productsLoader.IncludeSubstring(evt.newValue))
            {
                ManualAddProductItem productItem = new ManualAddProductItem(item);
                productScrollContainer.Add(productItem);
            }
        }
    }

    void SearchActivity(ChangeEvent<string> evt)
    {
        //activityScrollContainer.Clear();

        //if (String.IsNullOrEmpty(evt.newValue))
        //{
        //    activityRecentRequestsContainer.style.display = DisplayStyle.Flex;
        //}
        //else
        //{
        //    activityRecentRequestsContainer.style.display = DisplayStyle.None;
        //    foreach (Activity item in activitiesManager.IncludeSubstring(evt.newValue))
        //    {
        //        ManualAddActivityItem productItem = new ManualAddActivityItem(item);
        //        activityScrollContainer.Add(productItem);
        //    }
        //}
    }
}
