using Firebase.Auth;
using System;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductPanel : MonoBehaviour
{
    public VisualElement productPanel;
    private VisualElement productPanelBg;

    public Label productPanelTitle;
    public VisualElement nutriScoreBadge;
    public Label nutriScoreLabel;

    public Button addProductBtn;
    public IntegerField massInput;

    private ProgressBar protsProgressBar;
    private Label protsProgressBarMass;
    private Label protsProgressPercentRise;
    private VisualElement protsProgressBarAdditional;

    private ProgressBar fatsProgressBar;
    private Label fatsProgressBarMass;
    private Label fatsProgressPercentRise;
    private VisualElement fatsProgressBarAdditional;


    private ProgressBar carbsProgressBar;
    private Label carbsProgressBarMass;
    private Label carbsProgressPercentRise;
    private VisualElement carbsProgressBarAdditional;


    private ProgressBar caloriesProgressBar;
    private Label caloriesProgressBarMass;
    private Label caloriesProgressPercentRise;
    private VisualElement caloriesProgressBarAdditional;

    public products product;


    void Start()
    {
        productPanel = GetComponent<UIDocument>().rootVisualElement;
        productPanelBg = productPanel.Q<VisualElement>("ProductPanelBg");

        productPanelTitle = productPanel.Q<Label>("ProductPanelTitle");
        nutriScoreBadge = productPanel.Q<VisualElement>("ProductPanelNutriScoreBadge");
        nutriScoreLabel = productPanel.Q<Label>("ProductPanelNutriScoreLabel");
        addProductBtn = productPanel.Q<Button>("ProductPanelBtn");
        massInput = productPanel.Q<IntegerField>("ProductPanelMassInput");

        // Prots
        protsProgressBar = productPanel.Q<ProgressBar>("ProductPanelProtsProgressBar");
        protsProgressBarMass = productPanel.Q<Label>("ProtsProgressBarMass");
        protsProgressPercentRise = productPanel.Q<Label>("ProtsProgressBarPercentRise");
        protsProgressBarAdditional = productPanel.Q<VisualElement>("ProtsProgressBarAdditional");

        // Fats
        fatsProgressBar = productPanel.Q<ProgressBar>("ProductPanelFatsProgressBar");
        fatsProgressBarMass = productPanel.Q<Label>("FatsProgressBarMass");
        fatsProgressPercentRise = productPanel.Q<Label>("FatsProgressBarPercentRise");
        fatsProgressBarAdditional = productPanel.Q<VisualElement>("FatsProgressBarAdditional");

        // Carbs
        carbsProgressBar = productPanel.Q<ProgressBar>("ProductPanelCarbsProgressBar");
        carbsProgressBarMass = productPanel.Q<Label>("CarbsProgressBarMass");
        carbsProgressPercentRise = productPanel.Q<Label>("CarbsProgressBarPercentRise");
        carbsProgressBarAdditional = productPanel.Q<VisualElement>("CarbsProgressBarAdditional");

        // Calories
        caloriesProgressBar = productPanel.Q<ProgressBar>("ProductPanelCaloriesProgressBar");
        caloriesProgressBarMass = productPanel.Q<Label>("CaloriesProgressBarMass");
        caloriesProgressPercentRise = productPanel.Q<Label>("CaloriesProgressBarPercentRise");
        caloriesProgressBarAdditional = productPanel.Q<VisualElement>("CaloriesProgressBarAdditional");

        massInput.RegisterValueChangedCallback(OnMassValueChanged);
        productPanelBg.RegisterCallback<ClickEvent>(evt => Hide());
    }

    public void Show(bool withTransparentBg = false)
    {
        productPanel.style.display = DisplayStyle.Flex;
        if (withTransparentBg) productPanelBg.style.display = DisplayStyle.Flex;
    }

    public void Hide(Action callback = null)
    {
        productPanel.style.display = DisplayStyle.None;
        if (productPanelBg.style.display == DisplayStyle.Flex)
        {
            productPanelBg.style.display = DisplayStyle.None;
        }
        massInput.value = 100;
        callback?.Invoke();
    }

    private void OnMassValueChanged(ChangeEvent<int> evt)
    {
        CalculateMacrosByMass((evt.target as IntegerField).value);
    }

    public void SetProductData(products _product)
    {
        if (_product == null)
        {
            Debug.LogWarning("Product not found");
            throw new Exception("Продукт не знайдено");
        }
        product = _product;
        productPanelTitle.text = product.name.FirstCharacterToUpper();
        nutriScoreLabel.text = product.nutri_score.ToString().ToUpper();
        nutriScoreBadge.AddToClassList($"nutri-score-badge-{product.nutri_score.ToString().ToLower()}");
        
        CalculateMacrosByMass(massInput.value);
    }

    public void CalculateMacrosByMass(int mass)
    {
        int calculatedCals = (int)(product.calories * (mass / 100f));
        int calculatedProts = (int)(product.prots * (mass / 100f));
        int calculatedFats = (int)(product.fats * (mass / 100f));
        int calculatedCarbs = (int)(product.carbs * (mass / 100f));

        int caloriesPercentRise = (int)((calculatedCals / (float)User.Instance.CaloriesNeeded) * 100f);
        int protsPercentRise = (int)((calculatedProts / (float)User.Instance.ProtsNeeded) * 100f);
        int fatsPercentRise = (int)((calculatedFats / (float)User.Instance.FatsNeeded) * 100f);
        int carbsPercentRise = (int)((calculatedCarbs / (float)User.Instance.CarbsNeeded) * 100f);

        caloriesProgressBarMass.text = $"+{calculatedCals} ккал. ";
        caloriesProgressPercentRise.text = $"(+{caloriesPercentRise} %)";
        caloriesProgressBarAdditional.style.width = Length.Percent(caloriesProgressBar.value + caloriesPercentRise);

        protsProgressBarMass.text = $"+{calculatedProts} ккал. ";
        protsProgressPercentRise.text = $"(+{protsPercentRise} %)";
        protsProgressBarAdditional.style.width = Length.Percent(protsProgressBar.value + protsPercentRise);

        fatsProgressBarMass.text = $"+{calculatedFats} ккал. ";
        fatsProgressPercentRise.text = $"(+{fatsPercentRise} %)";
        fatsProgressBarAdditional.style.width = Length.Percent(fatsProgressBar.value + fatsPercentRise);

        carbsProgressBarMass.text = $"+{calculatedCarbs} ккал. ";
        carbsProgressPercentRise.text = $"(+{carbsPercentRise} %)";
        carbsProgressBarAdditional.style.width = Length.Percent(carbsProgressBar.value + carbsPercentRise);
    }
}
