using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductHistoryItem : VisualElement
{

    public new class UxmlFactory : UxmlFactory<ProductHistoryItem> { }

    public products product;
    public MacrosInfo macrosInfo;
    public int mass;

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

    public ProductHistoryItem(Todays_history todaysHistory)
    {
        AddToClassList(ussItem);
        name = "HistoryItem";
        macrosInfo = MacrosManager.CalculateMacrosByMass(todaysHistory.mass, todaysHistory.product_id);
        product = new ProductsLoader().GetById(todaysHistory.product_id);
        mass = todaysHistory.mass;

        Shadow imageShadow = new Shadow();
        imageShadow.name = "HistoryItemImageShadow";
        imageShadow.AddToClassList(ussImageShadow);
        hierarchy.Add(imageShadow);
        Debug.Log(todaysHistory);
        Debug.Log(macrosInfo);
        Debug.Log(product);
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
        VisualElement historyList = mainRoot.Q<VisualElement>("HistoryContainer");

        if (historyList.childCount == 0)
            style.marginBottom = 0;

        historyList.Insert(0, this);
    }
}
