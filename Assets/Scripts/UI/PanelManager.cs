using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelManager : MonoBehaviour
{
    private TemplateContainer homePage;
    private TemplateContainer scanPage;
    private TemplateContainer advicePage;
    private TemplateContainer adviceTopicsPage;
    private VisualElement adviceTopicPage;
    private VisualElement homeContainer;
    private VisualElement scanContainer;
    private VisualElement mainBg;
    private TemplateContainer activityTemplate;
    private VisualElement bottomMenu;

    private List<VisualElement> bottomMenuBtns;

    public VisualElement homeBtn;
    private VisualElement scanBtn;
    private VisualElement adviceBtn;
    private VisualElement btnSlider;

    private Button messageBtn;
    private Button profileBtn;
    private Button closeAdviceTopicBtn;
    private Button rationBtn;
    private Button activityBtn;
    private Button waterBtn;
    private Button productsBtn;

    public GameObject scanPageObject;
    public GameObject messagePageObject;
    public GameObject profilePageObject;
    public GameObject productPanelObject;
    public GameObject waterPanelObject;

    private VisualElement messageRoot;
    private VisualElement mainRoot;
    private VisualElement profileRoot;
    private VisualElement productPanelRoot;
    private VisualElement waterPanelRoot;
    private ProgressBar waterProgressBar;



    void Start()
    {
        mainRoot = GetComponent<UIDocument>().rootVisualElement;
        messageRoot = messagePageObject.GetComponent<UIDocument>().rootVisualElement;
        profileRoot = profilePageObject.GetComponent<UIDocument>().rootVisualElement;
        productPanelRoot = productPanelObject.GetComponent<UIDocument>().rootVisualElement;
        waterPanelRoot = waterPanelObject.GetComponent<UIDocument>().rootVisualElement;

        homePage = mainRoot.Q<TemplateContainer>("HomePage");
        scanPage = mainRoot.Q<TemplateContainer>("ScanPage");
        advicePage = mainRoot.Q<TemplateContainer>("AdvicePage");
        adviceTopicsPage = advicePage.Q<TemplateContainer>("AdviceTopicsPage");
        adviceTopicPage = advicePage.Q<VisualElement>("AdviceTopicPage");
        homeContainer = homePage.Q<VisualElement>("HomeContainer");
        activityTemplate = homePage.Q<TemplateContainer>("ActivityTemplate");
        scanContainer = scanPage.Q<VisualElement>("ScanPanelContainer");
        mainBg = mainRoot.Q<VisualElement>("MainBackground");


        bottomMenu = mainRoot.Q<TemplateContainer>("BottomMenu");
        bottomMenuBtns = bottomMenu.Query<VisualElement>(className: "menu__item").ToList();

        homeBtn = bottomMenu.Q<VisualElement>("home");
        scanBtn = bottomMenu.Q<VisualElement>("scaner");
        adviceBtn = bottomMenu.Q<VisualElement>("advices");
        btnSlider = homePage.Q<VisualElement>("HomeNavBtnSlider");

        profileBtn = mainRoot.Q<Button>("ProfileBtn");
        messageBtn = mainRoot.Q<Button>("MessageBtn");
        rationBtn = homePage.Q<Button>("RationBtn");
        activityBtn = homePage.Q<Button>("ActivityBtn");
        waterBtn = homePage.Q<Button>("WaterPanelBtn");
        productsBtn = homePage.Q<Button>("ProductSearchBtn");
        closeAdviceTopicBtn = adviceTopicPage.Q<Button>("CloseBtn");


        messageRoot.style.display = DisplayStyle.None;
        profileRoot.style.display = DisplayStyle.None;
        productPanelRoot.style.display = DisplayStyle.None;
        waterPanelRoot.style.display = DisplayStyle.None;
        scanContainer.style.display = DisplayStyle.None;
        mainBg.style.display = DisplayStyle.None;

        mainRoot.AddToClassList("home-template");
        profileRoot.AddToClassList("profile-template");
        messageRoot.AddToClassList("message-template");

        homeBtn.RegisterCallback<ClickEvent>(OnHomeBtnClick);
        scanBtn.RegisterCallback<ClickEvent>(OnScanBtnClick);
        adviceBtn.RegisterCallback<ClickEvent>(OnAdviceBtnClick);
        profileBtn.clicked += OpenProfilePage;
        messageBtn.clicked += OpenMessagePage;
        rationBtn.clicked += OpenRationPanel;
        activityBtn.clicked += OpenActivityPanel;
        waterBtn.clicked += OpenWaterPanel;
        //productsBtn.clicked += OpenProductsSearchPage;
        closeAdviceTopicBtn.clicked += CloseAdviceTopicPage;

        waterProgressBar = homePage.Q<ProgressBar>("WaterProgressBar");

        AttachTopicToAdviceTopicPage();

        waterProgressBar.RegisterValueChangedCallback(ChangeWaterProgress);
        mainRoot.RegisterCallback<TransitionEndEvent>(HandleMainSlideOutEnd);
    }

    private void ChangeWaterProgress(ChangeEvent<float> evt)
    {
        VisualElement progressBarProgress = (evt.currentTarget as ProgressBar).Query(className: "unity-progress-bar__progress");
        progressBarProgress.style.width = Length.Percent(evt.newValue);
    }

    private void AttachTopicToAdviceTopicPage()
    {
        adviceTopicsPage.Query<Button>("AdviceHeadMoreBtn").ForEach(moreArticleBtn =>
        {
            moreArticleBtn.clicked += OpenAdviceTopicPage;
        });
    }

    private void OnBottomMenuClick (ClickEvent evt)
    {
        if (evt == null) return;
        VisualElement ClickedElement = evt?.currentTarget as VisualElement;
        if (ClickedElement.ClassListContains("menu__item--active"))
            return;
        
        ResetDisplay();

        ClickedElement.AddToClassList("menu__item--active");
    }

    public void ResetDisplay()
    {
        scanPageObject.SetActive(false);
        homePage.style.display = DisplayStyle.None;
        scanPage.style.display = DisplayStyle.None;
        advicePage.style.display = DisplayStyle.None;

        bottomMenuBtns.ForEach((btn) =>
        {
            btn.RemoveFromClassList("menu__item--active");
        });
    }

    public void OnHomeBtnClick(ClickEvent evt)
    {
        OnBottomMenuClick(evt);
        ToHome();
    }

    public void ToHome()
    {
        homePage.style.display = DisplayStyle.Flex;
        GetComponent<ScanPanelManager>().Hide();
    }

    private void OnScanBtnClick(ClickEvent evt)
    {
        OnBottomMenuClick(evt);
        scanPageObject.SetActive(true);
        scanPage.style.display = DisplayStyle.Flex;
    }

    private void OnAdviceBtnClick(ClickEvent evt)
    {
        OnBottomMenuClick(evt);
        advicePage.style.display = DisplayStyle.Flex;
        GetComponent<ScanPanelManager>().Hide();
    }

    private void OpenProfilePage()
    {
        profileRoot.style.display = DisplayStyle.Flex;
        mainBg.style.display = DisplayStyle.Flex;
        profileRoot.AddToClassList("profile-template--slide-in");
        mainRoot.AddToClassList("home-template--slide-out-right");
        mainBg.AddToClassList("main-bg--active");
    }

    private void HandleMainSlideOutEnd(TransitionEndEvent evt)
    {
        if (mainRoot.ClassListContains("slide-out-right") || mainRoot.ClassListContains("slide-out-left"))
        {
            mainRoot.style.display = DisplayStyle.None;
        }
    }

    private void OpenMessagePage()
    {
        messageRoot.style.display = DisplayStyle.Flex;
        mainBg.style.display = DisplayStyle.Flex;
        messageRoot.AddToClassList("message-template--slide-in");
        mainRoot.AddToClassList("home-template--slide-out-left");
        mainBg.AddToClassList("main-bg--active");
    }

    private void OpenWaterPanel()
    {
        waterPanelRoot.style.display = DisplayStyle.Flex;
    }

    private void OpenAdviceTopicPage()
    {
        adviceTopicPage.style.display = DisplayStyle.Flex;
        adviceTopicsPage.style.display = DisplayStyle.None;
    }

    private void CloseAdviceTopicPage()
    {
        adviceTopicPage.style.display = DisplayStyle.None;
        adviceTopicsPage.style.display = DisplayStyle.Flex;
    }

    private void OpenRationPanel()
    {
        homeContainer.style.visibility = Visibility.Visible;
        GetComponent<SwipeMenu>().ToRationPanel();
    }

    private void OpenActivityPanel()
    {
        activityTemplate.style.visibility = Visibility.Visible;
        GetComponent<SwipeMenu>().ToActivityPanel();
    }

}
