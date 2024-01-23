using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class ScanPanelManager : MonoBehaviour
{
    private VisualElement mainRoot;
    private VisualElement scanPanel;

    private Label scanPanelTitle;
    private VisualElement nutriScoreBadge;

    private Button addProductBtn;
    private IntegerField massInput;

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
    ProductsLoader productsLoader;


    void Start()
    {
        mainRoot = GetComponent<UIDocument>().rootVisualElement;
        scanPanel = mainRoot.Q<TemplateContainer>("ScanPage");

        scanPanelTitle = scanPanel.Q<Label>("ScanPanelTitle");
        nutriScoreBadge = scanPanel.Q<VisualElement>("ScanPanelNutriScoreBadge");
        addProductBtn = scanPanel.Q<Button>("ScanPanelBtn");
        massInput = scanPanel.Q<IntegerField>("ScanPanelMassInput");

        // Prots
        protsProgressBar = scanPanel.Q<ProgressBar>("ProtsProgressBar");
        protsProgressBarMass = scanPanel.Q<Label>("ProtsProgressBarMass");
        protsProgressPercentRise = scanPanel.Q<Label>("ProtsProgressPercentRise");
        protsProgressBarAdditional = scanPanel.Q<VisualElement>("ProtsProgressBarAdditional");

        // Fats
        fatsProgressBar = scanPanel.Q<ProgressBar>("FatsProgressBar");
        fatsProgressBarMass = scanPanel.Q<Label>("FatsProgressBarMass");
        fatsProgressPercentRise = scanPanel.Q<Label>("FatsProgressPercentRise");
        fatsProgressBarAdditional = scanPanel.Q<VisualElement>("FatsProgressBarAdditional");

        // Carbs
        carbsProgressBar = scanPanel.Q<ProgressBar>("CarbsProgressBar");
        carbsProgressBarMass = scanPanel.Q<Label>("CarbsProgressBarMass");
        carbsProgressPercentRise = scanPanel.Q<Label>("CarbsProgressPercentRise");
        carbsProgressBarAdditional = scanPanel.Q<VisualElement>("CarbsProgressBarAdditional");

        // Calories
        caloriesProgressBar = scanPanel.Q<ProgressBar>("CaloriesProgressBar");
        caloriesProgressBarMass = scanPanel.Q<Label>("CaloriesProgressBarMass");
        caloriesProgressPercentRise = scanPanel.Q<Label>("CaloriesProgressPercentRise");
        caloriesProgressBarAdditional = scanPanel.Q<VisualElement>("CaloriesProgressBarAdditiona");

        productsLoader = new ProductsLoader();

        Debug.Log(productsLoader.GetById(1));
        Debug.Log(productsLoader.GetById(1).ToString());
    }
}
