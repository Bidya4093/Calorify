using Firebase.Auth;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ProductItem : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ProductItem, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public products product;
    public MacrosInfo macrosInfo;
    public int mass = 100;

    private Shadow imageShadow;
    public VisualElement image;
    public VisualElement nutriScoreBadge;
    public Label nutriScoreLabel;
    public VisualElement container;
    public VisualElement topContainer;
    public Label title;
    public VisualElement bottomContainer;

    public Label caloriesLabel;
    public Label weightLabel;

    public readonly string ussItem = "history-item";
    public readonly string ussImageShadow = "history-item__image-shadow";
    public readonly string ussImage = "history-item__image";
    public readonly string ussNutriScore = "nutri-score-badge";
    public readonly string ussItemScore = "history-item__score";
    public readonly string ussItemContainer = "history-item__container";
    public readonly string ussItemTop = "history-item__top";
    public readonly string ussItemTitle = "history-item__title";
    public readonly string ussItemBottom = "history-item__bottom";
    public readonly string ussItemCalories = "history-item__calories";
    public readonly string ussItemWeight = "history-item__weight";

    public ProductItem()
    {
        //Init("Гречана каша", mass, 300, NutriScore.A.ToString());
    }

    public ProductItem(products _product)
    {
        product = _product;
        macrosInfo = MacrosManager.CalculateMacrosByMass(mass, product.product_id);

        Init(product.name, mass, macrosInfo.calories, product.nutri_score);
    }

    public virtual void Init(string _name, int mass, int calories, string nutri_score)
    {
        AddToClassList(ussItem);
        name = "HistoryItem";

        imageShadow = new Shadow();
        imageShadow.name = "HistoryItemImageShadow";
        imageShadow.AddToClassList(ussImageShadow);
        hierarchy.Add(imageShadow);

        image = new VisualElement();
        image.name = "HistoryItemImage";
        image.AddToClassList(ussImage);
        imageShadow.Add(image);

        nutriScoreBadge = new VisualElement();
        nutriScoreBadge.name = "HistoryItemScore";
        nutriScoreBadge.AddToClassList(ussNutriScore);
        nutriScoreBadge.AddToClassList(ussNutriScore + $"-{nutri_score.ToString().ToLower()}");
        nutriScoreBadge.AddToClassList(ussItemScore);
        image.Add(nutriScoreBadge);

        nutriScoreLabel = new Label(nutri_score.ToString().ToUpper());
        nutriScoreBadge.Add(nutriScoreLabel);

        // Container
        container = new VisualElement();
        container.name = "HistoryItemContainer";
        container.AddToClassList(ussItemContainer);
        hierarchy.Add(container);

        // Top
        topContainer = new VisualElement();
        topContainer.name = "HistoryItemTop";
        topContainer.AddToClassList(ussItemTop);
        container.Add(topContainer);

        title = new Label(_name);
        title.name = "HistoryItemTitle";
        title.AddToClassList(ussItemTitle);
        topContainer.Add(title);

        // Bottom
        bottomContainer = new VisualElement();
        bottomContainer.name = "HistoryItemBottom";
        bottomContainer.AddToClassList(ussItemBottom);
        container.Add(bottomContainer);

        caloriesLabel = new Label($"{calories} ккал");
        caloriesLabel.name = "HistoryItemCalories";
        caloriesLabel.AddToClassList(ussItemCalories);
        bottomContainer.Add(caloriesLabel);

        weightLabel = new Label($"{mass} г");
        weightLabel.name = "HistoryItemWeight";
        weightLabel.AddToClassList(ussItemWeight);
        bottomContainer.Add(weightLabel);

    }
}
