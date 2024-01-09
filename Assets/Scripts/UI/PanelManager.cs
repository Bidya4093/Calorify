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
    private TemplateContainer activityTemplate;
    private VisualElement bottomMenu;

    private List<VisualElement> bottomMenuBtns;

    private VisualElement homeBtn;
    private VisualElement scanBtn;
    private VisualElement adviceBtn;

    private Button messageBtn;
    private Button profileBtn;
    private Button closeAdviceTopicBtn;
    private Button rationBtn;
    private Button activityBtn;

    public GameObject scanPageObject;
    public GameObject messagePageObject;
    public GameObject profilePageObject;
    public GameObject productPanelObject;

    private VisualElement messageRoot;
    private VisualElement mainRoot;
    private VisualElement profileRoot;
    private VisualElement productPanelRoot;



    void Start()
    {
        mainRoot = GetComponent<UIDocument>().rootVisualElement;
        messageRoot = messagePageObject.GetComponent<UIDocument>().rootVisualElement;
        profileRoot = profilePageObject.GetComponent<UIDocument>().rootVisualElement;
        productPanelRoot = productPanelObject.GetComponent<UIDocument>().rootVisualElement;


        messageRoot.style.display = DisplayStyle.None;
        profileRoot.style.display = DisplayStyle.None;
        productPanelRoot.style.display = DisplayStyle.None;


        homePage = mainRoot.Q<TemplateContainer>("HomePage");
        scanPage = mainRoot.Q<TemplateContainer>("ScanPage");
        advicePage = mainRoot.Q<TemplateContainer>("AdvicePage");
        adviceTopicsPage = advicePage.Q<TemplateContainer>("AdviceTopicsPage");
        adviceTopicPage = advicePage.Q<VisualElement>("AdviceTopicPage");
        homeContainer = homePage.Q<VisualElement>("HomeContainer");
        activityTemplate = homePage.Q<TemplateContainer>("ActivityTemplate");


        bottomMenu = mainRoot.Q<TemplateContainer>("BottomMenu");
        bottomMenuBtns = bottomMenu.Query<VisualElement>(className: "menu__item").ToList();

        homeBtn = bottomMenu.Q<VisualElement>("home");
        scanBtn = bottomMenu.Q<VisualElement>("scaner");
        adviceBtn = bottomMenu.Q<VisualElement>("advices");
        profileBtn = mainRoot.Q<Button>("ProfileBtn");
        messageBtn = mainRoot.Q<Button>("MessageBtn");
        rationBtn = homePage.Q<Button>("RationBtn");
        activityBtn = homePage.Q<Button>("ActivityBtn");
        closeAdviceTopicBtn = adviceTopicPage.Q<Button>("CloseBtn");


        homeBtn.RegisterCallback<ClickEvent>(OnHomeBtnClick);
        scanBtn.RegisterCallback<ClickEvent>(OnScanBtnClick);
        adviceBtn.RegisterCallback<ClickEvent>(OnAdviceBtnClick);
        profileBtn.clicked += OpenProfilePage;
        messageBtn.clicked += OpenMessagePage;
        rationBtn.clicked += OpenRationPanel;
        activityBtn.clicked += OpenActivityPanel;
        closeAdviceTopicBtn.clicked += CloseAdviceTopicPage;


        AttachProductPanelToProduct();

        AttachTopicToAdviceTopicPage();
    }

    private void AttachProductPanelToProduct()
    {
        homePage.Query<Button>("HistoryItemEditBtn").ForEach(editProductBtn =>
        {
            editProductBtn.clicked += OpenProductPanel;
        });
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
        VisualElement ClickedElement = evt.currentTarget as VisualElement;

        scanPageObject.SetActive(false);
        homePage.style.display = DisplayStyle.None;
        scanPage.style.display = DisplayStyle.None;
        advicePage.style.display = DisplayStyle.None;

        bottomMenuBtns.ForEach((btn) =>
        {
            btn.RemoveFromClassList("menu__item--active");
        });
        ClickedElement.AddToClassList("menu__item--active");
    }
    private void OnHomeBtnClick(ClickEvent evt)
    {
        OnBottomMenuClick(evt);
        homePage.style.display = DisplayStyle.Flex;
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
    }

    private void OpenProfilePage()
    {
        mainRoot.style.display = DisplayStyle.None;
        profileRoot.style.display = DisplayStyle.Flex;
    }

    private void OpenMessagePage()
    {
        mainRoot.style.display = DisplayStyle.None;
        messageRoot.style.display = DisplayStyle.Flex;
    }

    private void OpenProductPanel()
    {
        productPanelRoot.style.display = DisplayStyle.Flex;
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
        rationBtn.AddToClassList("active");
        activityBtn.RemoveFromClassList("active");
        homeContainer.style.display = DisplayStyle.Flex;
        activityTemplate.style.display = DisplayStyle.None;
    }

    private void OpenActivityPanel()
    {
        rationBtn.RemoveFromClassList("active");
        activityBtn.AddToClassList("active");
        homeContainer.style.display = DisplayStyle.None;
        activityTemplate.style.display = DisplayStyle.Flex;
    }

}
