using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ProductHistoryItem : VisualElement
{

    public new class UxmlFactory : UxmlFactory<ProductHistoryItem> { }

    public TodaysHistoryManager todaysHistoryManager;
    public Todays_history todaysHistory;
    public products product;
    public MacrosInfo macrosInfo;
    public int mass;

    public Label caloriesLabel;
    public Label weightLabel;

    private readonly string ussItem = "history-item";
    private readonly string ussImageShadow = "history-item__image-shadow";
    private readonly string ussImage = "history-item__image";
    private readonly string ussNutriScore = "nutri-score-badge";
    private readonly string ussItemScore = "history-item__score";
    private readonly string ussItemContainer = "history-item__container";
    private readonly string ussItemTop = "history-item__top";
    private readonly string ussItemTitle = "history-item__title";
    private readonly string ussItemEditBtn = "history-item__edit-btn";
    private readonly string ussItemBottom = "history-item__bottom";
    private readonly string ussItemCalories = "history-item__calories";
    private readonly string ussItemWeight = "history-item__weight";
    private readonly string ussBtnIcon = "btn-icon";

    public ProductHistoryItem() : this(null) { }

    public ProductHistoryItem(Todays_history _todaysHistory)
    {
        todaysHistoryManager = new TodaysHistoryManager();

        todaysHistory = _todaysHistory;
        AddToClassList(ussItem);
        name = "HistoryItem";
        macrosInfo = MacrosManager.CalculateMacrosByMass(todaysHistory.mass, todaysHistory.product_id);
        product = new ProductsLoader().GetById(todaysHistory.product_id);
        mass = todaysHistory.mass;
        
        Shadow imageShadow = new Shadow();
        imageShadow.name = "HistoryItemImageShadow";
        imageShadow.AddToClassList(ussImageShadow);
        hierarchy.Add(imageShadow);

        VisualElement image = new VisualElement();
        image.name = "HistoryItemImage";
        image.AddToClassList(ussImage);
        imageShadow.Add(image);

        VisualElement nutriScoreBadge = new VisualElement();
        nutriScoreBadge.name = "HistoryItemScore";
        nutriScoreBadge.AddToClassList(ussNutriScore);
        nutriScoreBadge.AddToClassList(ussNutriScore +$"-{product.nutri_score.ToLower()}");
        nutriScoreBadge.AddToClassList(ussItemScore);
        image.Add(nutriScoreBadge);

        Label nutriScoreLabel = new Label(product.nutri_score.ToUpper());
        nutriScoreBadge.Add(nutriScoreLabel);

        // Container
        VisualElement itemContainer = new VisualElement();
        itemContainer.name = "HistoryItemContainer";
        itemContainer.AddToClassList(ussItemContainer);
        hierarchy.Add(itemContainer);

        // Top
        VisualElement itemTop = new VisualElement();
        itemTop.name = "HistoryItemTop";
        itemTop.AddToClassList(ussItemTop);
        itemContainer.Add(itemTop);

        Label title = new Label(product.name);
        title.name = "HistoryItemTitle";
        title.AddToClassList(ussItemTitle);
        itemTop.Add(title);

        Button editBtn = new Button();
        editBtn.name = "HistoryItemEditBtn";
        editBtn.AddToClassList(ussItemEditBtn);
        editBtn.AddToClassList(ussBtnIcon);
        itemTop.Add(editBtn);

        // Bottom
        VisualElement itemBottom = new VisualElement();
        itemBottom.name = "HistoryItemBottom";
        itemBottom.AddToClassList(ussItemBottom);
        itemContainer.Add(itemBottom);

        caloriesLabel = new Label($"{macrosInfo.calories} κκΰλ");
        caloriesLabel.name = "HistoryItemCalories";
        caloriesLabel.AddToClassList(ussItemCalories);
        itemBottom.Add(caloriesLabel);

        weightLabel = new Label($"{todaysHistory.mass} γ");
        weightLabel.name = "HistoryItemWeight";
        weightLabel.AddToClassList(ussItemWeight);
        itemBottom.Add(weightLabel);


        // Inserting
        if (ProductHistoryList.empty)
            style.marginBottom = 0;

        ProductHistoryList.historyList.Insert(0, this);

        editBtn.RegisterCallback<ClickEvent>(OnEditBtnClick);

        ProductHistoryList.CheckEmptyList();
    }

    private void OnEditBtnClick(ClickEvent evt)
    {   
        ProductPanel.productPanelRoot.style.display = DisplayStyle.Flex;
        OpenProductPanelWithData();
    }

    private void OpenProductPanelWithData()
    {
        UpdateProductPanelData();
        ProductPanel.massInput.value = todaysHistory.mass;

        ProductPanel.deleteBtn.RegisterCallback<ClickEvent>(DeleteItem);
        ProductPanel.massInput.RegisterValueChangedCallback(ChangeItemData);
        ProductPanel.saveChangesBtn.RegisterCallback<ClickEvent>(SaveChanges);
        ProductPanel.productPanelBackground.RegisterCallback<ClickEvent>(CloseProductPanel);
    }

    private void UpdateProductPanelData()
    {
        ProductPanel.nutriScoreBadge.ClearClassList();
        ProductPanel.nutriScoreBadge.AddToClassList(ussNutriScore);
        ProductPanel.nutriScoreBadge.AddToClassList(ussNutriScore + $"-{product.nutri_score.ToLower()}");
        ProductPanel.nutriScoreBadge.AddToClassList(ussItemScore);

        ProductPanel.nutriScoreLabel.text = product.nutri_score.ToUpper();
        ProductPanel.title.text = product.name;
        ProductPanel.dateLabel.text = todaysHistory.date;

        UpdateProductMacrosData();
    }

    private void UpdateProductMacrosData()
    {
        ProductPanel.caloriesLabel.text = macrosInfo.calories.ToString();
        ProductPanel.protsLabel.text = macrosInfo.prots.ToString();
        ProductPanel.fatsLabel.text = macrosInfo.fats.ToString();
        ProductPanel.carbsLabel.text = macrosInfo.carbs.ToString();
    }

    private void CloseProductPanel(ClickEvent evt)
    {
        ProductPanel.productPanelRoot.style.display = DisplayStyle.None;

        ProductPanel.deleteBtn.UnregisterCallback<ClickEvent>(DeleteItem);
        ProductPanel.massInput.UnregisterValueChangedCallback(ChangeItemData);
        ProductPanel.saveChangesBtn.UnregisterCallback<ClickEvent>(SaveChanges);
        ProductPanel.productPanelBackground.UnregisterCallback<ClickEvent>(CloseProductPanel);
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
        macrosInfo = MacrosManager.CalculateMacrosByMass(mass, todaysHistory.product_id);
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
        caloriesLabel.text = $"{macrosInfo.calories} κκΰλ";
        weightLabel.text = $"{mass} γ";
    }
}
