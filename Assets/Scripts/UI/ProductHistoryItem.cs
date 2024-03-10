using Firebase.Auth;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ProductHistoryItem : ProductItem
{
    public new class UxmlFactory : UxmlFactory<ProductHistoryItem, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public TodaysHistoryManager todaysHistoryManager;
    public Todays_history todaysHistory;

    public Button editBtn;

    private readonly string ussItemEditBtn = "history-item__edit-btn";
    private readonly string ussBtnIcon = "btn-icon";

    public ProductHistoryItem() { }

    public async Task InitializeAsync(Todays_history _todaysHistory)
    {
        todaysHistory = _todaysHistory;
        todaysHistoryManager = new TodaysHistoryManager();
        mass = todaysHistory.mass;


        product = await new ProductsLoader().GetById(todaysHistory.product_id);
        macrosInfo = MacrosManager.CalculateMacrosByMass(mass, product);
        Init(product.name, mass, macrosInfo.calories, product.nutri_score);

        if (ProductHistoryList.empty)
            style.marginBottom = 0;

        ProductHistoryList.historyList.Insert(0, this);

        ProductHistoryList.CheckEmptyList();
    }

    public override void Init(string _name, int mass, int calories, string nutri_score)
    {

        base.Init(_name, mass, calories, nutri_score);

        editBtn = new Button();
        editBtn.name = "HistoryItemEditBtn";
        editBtn.AddToClassList(ussItemEditBtn);
        editBtn.AddToClassList(ussBtnIcon);
        topContainer.Add(editBtn);

        editBtn.RegisterCallback<ClickEvent>(OnEditBtnClick);
    }

    private void OnEditBtnClick(ClickEvent evt)
    {
        HistoryProductPanel.productPanelRoot.style.display = DisplayStyle.Flex;
        OpenProductPanelWithData();
    }

    private void OpenProductPanelWithData()
    {
        UpdateProductPanelData();
        HistoryProductPanel.massInput.value = todaysHistory.mass;

        HistoryProductPanel.deleteBtn.RegisterCallback<ClickEvent>(DeleteItem);
        HistoryProductPanel.massInput.RegisterValueChangedCallback(ChangeItemData);
        HistoryProductPanel.saveChangesBtn.RegisterCallback<ClickEvent>(SaveChanges);
        HistoryProductPanel.productPanelBackground.RegisterCallback<ClickEvent>(CloseProductPanel);
    }

    private void UpdateProductPanelData()
    {
        HistoryProductPanel.nutriScoreBadge.ClearClassList();
        HistoryProductPanel.nutriScoreBadge.AddToClassList(ussNutriScore);
        HistoryProductPanel.nutriScoreBadge.AddToClassList(ussNutriScore + $"-{product.nutri_score.ToLower()}");
        HistoryProductPanel.nutriScoreBadge.AddToClassList(ussItemScore);

        HistoryProductPanel.nutriScoreLabel.text = product.nutri_score.ToUpper();
        HistoryProductPanel.title.text = product.name;
        HistoryProductPanel.dateLabel.text = todaysHistory.date;

        UpdateProductMacrosData();
    }

    private void UpdateProductMacrosData()
    {
        HistoryProductPanel.caloriesLabel.text = macrosInfo.calories.ToString();
        HistoryProductPanel.protsLabel.text = macrosInfo.prots.ToString();
        HistoryProductPanel.fatsLabel.text = macrosInfo.fats.ToString();
        HistoryProductPanel.carbsLabel.text = macrosInfo.carbs.ToString();
    }

    private void CloseProductPanel(ClickEvent evt)
    {
        HistoryProductPanel.productPanelRoot.style.display = DisplayStyle.None;

        HistoryProductPanel.deleteBtn.UnregisterCallback<ClickEvent>(DeleteItem);
        HistoryProductPanel.massInput.UnregisterValueChangedCallback(ChangeItemData);
        HistoryProductPanel.saveChangesBtn.UnregisterCallback<ClickEvent>(SaveChanges);
        HistoryProductPanel.productPanelBackground.UnregisterCallback<ClickEvent>(CloseProductPanel);
    }

    private void DeleteItem(ClickEvent evt)
    {
        RemoveFromHierarchy();

        if (ProductHistoryList.historyList.Query<ProductHistoryItem>("HistoryItem").Last() != null)
            ProductHistoryList.historyList.Query<ProductHistoryItem>("HistoryItem").Last().style.marginBottom = 0;

        ProductHistoryList.CheckEmptyList();
        todaysHistoryManager.DeleteRecord(todaysHistory.id);
        User.SubtractFromEatem(macrosInfo);
        ProductHistoryList.items.Remove(this);
        DataManager.LoadChartsData();
        CloseProductPanel(evt);
    }

    private void ChangeItemData(ChangeEvent<int> evt)
    {
        mass = (evt.target as IntegerField).value;
        User.SubtractFromEatem(macrosInfo);
        macrosInfo = MacrosManager.CalculateMacrosByMass(mass, product);
        User.AddToEaten(macrosInfo);
        UpdateProductMacrosData();
    }

    private void SaveChanges(ClickEvent evt)
    {
        todaysHistoryManager.UpdateMass(todaysHistory.id, mass);
        todaysHistory.mass = mass;
        UpdateCaloriesAndMass();
        DataManager.LoadChartsData();
        CloseProductPanel(evt);
    }

    private void UpdateCaloriesAndMass()
    {
        caloriesLabel.text = $"{macrosInfo.calories} ����";
        weightLabel.text = $"{mass} �";
    }
}
