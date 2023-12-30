using Game.UI;
using NUnit.Framework.Internal.Filters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelManager : MonoBehaviour
{
    private VisualElement homePage;
    private VisualElement scanPage;
    private VisualElement advicePage;
    private VisualElement bottomMenu;
    private List<VisualElement> bottomMenuBtns;

    private VisualElement homeBtn;
    private VisualElement scanBtn;
    private VisualElement adviceBtn;

    private Button messageBtn;
    private Button backBtn;
    private Button profileBtn;



    public GameObject scanPageObject;
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        homePage = root.Q<TemplateContainer>("HomePage");
        scanPage = root.Q<TemplateContainer>("ScanPage");
        advicePage = root.Q<TemplateContainer>("AdvicePage");
        bottomMenu = root.Q<TemplateContainer>("BottomMenu");

        bottomMenuBtns = bottomMenu.Query<VisualElement>(className: "menu__item").ToList();

        homeBtn = bottomMenu.Q<VisualElement>("home");
        scanBtn = bottomMenu.Q<VisualElement>("scaner");
        adviceBtn = bottomMenu.Q<VisualElement>("advices");
        
        homeBtn.RegisterCallback<ClickEvent>(OnHomeBtnClick);
        scanBtn.RegisterCallback<ClickEvent>(OnScanBtnClick);
        adviceBtn.RegisterCallback<ClickEvent>(OnAdviceBtnClick);

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
}
