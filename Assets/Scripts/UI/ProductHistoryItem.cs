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

    public VisualElement productPanelRoot;
    public VisualElement historyList;
    public TodaysHistoryManager todaysHistoryManager;
    public Todays_history todaysHistory;
    public products product;
    public MacrosInfo macrosInfo;
    public int mass;

    private Button deleteBtn;
    private IntegerField massInput;
    private Button saveChangesBtn;


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
        productPanelRoot = GameObject.Find("ProductPanel").GetComponent<UIDocument>().rootVisualElement;
        deleteBtn = productPanelRoot.Q<Button>("ProductPanelDeleteBtn");
        massInput = productPanelRoot.Q<IntegerField>("ProductPanelWeightInput");
        saveChangesBtn = productPanelRoot.Q<Button>("ProductPanelBtn");
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

        Label caloriesLabel = new Label($"{macrosInfo.calories} κκΰλ");
        caloriesLabel.name = "HistoryItemCalories";
        caloriesLabel.AddToClassList(ussItemCalories);
        itemBottom.Add(caloriesLabel);

        Label weightLabel = new Label($"{todaysHistory.mass} γ");
        weightLabel.name = "HistoryItemWeight";
        weightLabel.AddToClassList(ussItemWeight);
        itemBottom.Add(weightLabel);

        VisualElement mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;
        historyList = mainRoot.Q<VisualElement>("HistoryContainer");

        if (ProductHistoryList.empty)
            style.marginBottom = 0;

        historyList.Insert(0, this);

        editBtn.RegisterCallback<ClickEvent>(OnEditBtnClick);

        VisualElement productPanelBackground = productPanelRoot.Q<VisualElement>("ProductPanelBackground");
        productPanelBackground.RegisterCallback<ClickEvent>(CloseProductPanel);
        ProductHistoryList.CheckEmptyList();
    }

    private void OnEditBtnClick(ClickEvent evt)
    {
        VisualElement productPanelRoot = GameObject.Find("ProductPanel").GetComponent<UIDocument>().rootVisualElement;
        
        productPanelRoot.style.display = DisplayStyle.Flex;

        OpenProductPanelWithData();

    }

    private void OpenProductPanelWithData()
    {
        UpdateProductPanelData();
        massInput.value = todaysHistory.mass;

        deleteBtn.RegisterCallback<ClickEvent>(DeleteItem);
        massInput.RegisterValueChangedCallback(ChangeItemData);
        saveChangesBtn.RegisterCallback<ClickEvent>(SaveChanges);

    }

    private void UpdateProductPanelData()
    {
        VisualElement nutriScoreBadge = productPanelRoot.Q<VisualElement>("HistoryItemScore");
        Label nutriScoreLabel = nutriScoreBadge.Q<Label>();
        Label title = productPanelRoot.Q<Label>("ProductPanelTitle");
        Label dateLabel = productPanelRoot.Q<Label>("ProductPanelDate");

        nutriScoreBadge.ClearClassList();
        nutriScoreBadge.AddToClassList(ussNutriScore);
        nutriScoreBadge.AddToClassList(ussNutriScore + $"-{product.nutri_score.ToLower()}");
        nutriScoreBadge.AddToClassList(ussItemScore);

        nutriScoreLabel.text = product.nutri_score.ToUpper();
        title.text = product.name;
        dateLabel.text = todaysHistory.date;

        UpdateProductMacrosData();
    }

    private void UpdateProductMacrosData()
    {
        Label caloriesLabel = productPanelRoot.Q<Label>("MacrosCaloriesValue");
        Label protsLabel = productPanelRoot.Q<Label>("MacrosProtsValue");
        Label fatsLabel = productPanelRoot.Q<Label>("MacrosFatsValue");
        Label carbsLabel = productPanelRoot.Q<Label>("MacrosCarbsValue");

        caloriesLabel.text = macrosInfo.calories.ToString();
        protsLabel.text = macrosInfo.prots.ToString();
        fatsLabel.text = macrosInfo.fats.ToString();
        carbsLabel.text = macrosInfo.carbs.ToString();
    }

    private void CloseProductPanel(ClickEvent evt)
    {
        productPanelRoot.style.display = DisplayStyle.None;

        deleteBtn.UnregisterCallback<ClickEvent>(DeleteItem);
        massInput.UnregisterValueChangedCallback(ChangeItemData);
        saveChangesBtn.UnregisterCallback<ClickEvent>(SaveChanges);
    }

    private void DeleteItem(ClickEvent evt)
    {
        RemoveFromHierarchy();
        if (historyList.Query<ProductHistoryItem>("HistoryItem").Last() != null)
            historyList.Query<ProductHistoryItem>("HistoryItem").Last().style.marginBottom = 0;
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

        Label caloriesLabel = this.Q<Label>("HistoryItemCalories");
        caloriesLabel.text = $"{macrosInfo.calories} κκΰλ";

        Label weightLabel = this.Q<Label>("HistoryItemWeight");
        weightLabel.text = $"{mass} γ";

    }
}
