using Firebase.Auth;
using UnityEngine;
using UnityEngine.UIElements;

public class ScanPanelManager : MonoBehaviour
{
    private VisualElement mainRoot;
    private VisualElement scanPanel;
    private VisualElement scanPanelContainer;


    private Label scanPanelTitle;
    private VisualElement nutriScoreBadge;
    private Label nutriScoreLabel;

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
    products product;


    void Start()
    {
        mainRoot = GetComponent<UIDocument>().rootVisualElement;
        scanPanel = mainRoot.Q<TemplateContainer>("ScanPage");

        scanPanelContainer = mainRoot.Q<VisualElement>("ScanPanelContainer");

        scanPanelTitle = scanPanel.Q<Label>("ScanPanelTitle");
        nutriScoreBadge = scanPanel.Q<VisualElement>("ScanPanelNutriScoreBadge");
        nutriScoreLabel = scanPanel.Q<Label>("ScanPanelNutriScoreLabel");
        addProductBtn = scanPanel.Q<Button>("ScanPanelBtn");
        massInput = scanPanel.Q<IntegerField>("ScanPanelMassInput");

        // Prots
        protsProgressBar = scanPanel.Q<ProgressBar>("ScanPanelProtsProgressBar");
        protsProgressBarMass = scanPanel.Q<Label>("ProtsProgressBarMass");
        protsProgressPercentRise = scanPanel.Q<Label>("ProtsProgressBarPercentRise");
        protsProgressBarAdditional = scanPanel.Q<VisualElement>("ProtsProgressBarAdditional");

        // Fats
        fatsProgressBar = scanPanel.Q<ProgressBar>("ScanPanelFatsProgressBar");
        fatsProgressBarMass = scanPanel.Q<Label>("FatsProgressBarMass");
        fatsProgressPercentRise = scanPanel.Q<Label>("FatsProgressBarPercentRise");
        fatsProgressBarAdditional = scanPanel.Q<VisualElement>("FatsProgressBarAdditional");

        // Carbs
        carbsProgressBar = scanPanel.Q<ProgressBar>("ScanPanelCarbsProgressBar");
        carbsProgressBarMass = scanPanel.Q<Label>("CarbsProgressBarMass");
        carbsProgressPercentRise = scanPanel.Q<Label>("CarbsProgressBarPercentRise");
        carbsProgressBarAdditional = scanPanel.Q<VisualElement>("CarbsProgressBarAdditional");

        // Calories
        caloriesProgressBar = scanPanel.Q<ProgressBar>("ScanPanelCaloriesProgressBar");
        caloriesProgressBarMass = scanPanel.Q<Label>("CaloriesProgressBarMass");
        caloriesProgressPercentRise = scanPanel.Q<Label>("CaloriesProgressBarPercentRise");
        caloriesProgressBarAdditional = scanPanel.Q<VisualElement>("CaloriesProgressBarAdditional");

        productsLoader = new ProductsLoader();
        massInput.RegisterValueChangedCallback(OnMassValueChanged);
        addProductBtn.RegisterCallback<ClickEvent>(AddProductToDailyList);

    }

    private void AddProductToDailyList(ClickEvent evt)
    {
        TodaysHistoryManager todaysHistoryManager = new TodaysHistoryManager();
        Todays_history todaysHistory = todaysHistoryManager.InsertRecord(product.product_id, massInput.value, FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        ProductHistoryItem productHistoryItem = new ProductHistoryItem(todaysHistory);
        ProductHistoryList.items.Add(productHistoryItem);

        User.AddToEaten(productHistoryItem.macrosInfo);

        DataManager.LoadChartsData();

        Hide();
        GetComponent<PanelManager>().OnHomeBtnClick(evt);
    }

    public void Show()
    {
        scanPanelContainer.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        scanPanelContainer.style.display = DisplayStyle.None;
    }

    private void OnMassValueChanged(ChangeEvent<int> evt)
    {
        CalculateMacrosByMass((evt.target as IntegerField).value);
    }

    public void LoadProductData(int productId)
    {
        Show();
        product = productsLoader.GetById(productId);

        scanPanelTitle.text = product.name;
        nutriScoreLabel.text = product.nutri_score.ToUpper();
        nutriScoreBadge.AddToClassList($"nutri-score-badge-{product.nutri_score.ToLower()}");

        CalculateMacrosByMass(massInput.value);
    }

    private void CalculateMacrosByMass(int mass)
    {
        int calculatedCals = (int)(product.calories * (mass / 100f));
        int calculatedProts = (int)(product.prots * (mass / 100f));
        int calculatedFats = (int)(product.fats * (mass / 100f));
        int calculatedCarbs = (int)(product.carbs * (mass / 100f));

        int caloriesPercentRise = (int)((calculatedCals / (float)User.Instance.CaloriesNeeded) * 100f);
        int protsPercentRise = (int)((calculatedProts / (float)User.Instance.ProtsNeeded) * 100f);
        int fatsPercentRise = (int)((calculatedFats / (float)User.Instance.FatsNeeded) * 100f);
        int carbsPercentRise = (int)((calculatedCarbs / (float)User.Instance.CarbsNeeded) * 100f);

        caloriesProgressBarMass.text = $"+{calculatedCals} êêàë. ";
        caloriesProgressPercentRise.text = $"(+{caloriesPercentRise} %)";
        caloriesProgressBarAdditional.style.width = Length.Percent(caloriesProgressBar.value + caloriesPercentRise);

        protsProgressBarMass.text = $"+{calculatedProts} êêàë. ";
        protsProgressPercentRise.text = $"(+{protsPercentRise} %)";
        protsProgressBarAdditional.style.width = Length.Percent(protsProgressBar.value + protsPercentRise);

        fatsProgressBarMass.text = $"+{calculatedFats} êêàë. ";
        fatsProgressPercentRise.text = $"(+{fatsPercentRise} %)";
        fatsProgressBarAdditional.style.width = Length.Percent(fatsProgressBar.value + fatsPercentRise);

        carbsProgressBarMass.text = $"+{calculatedCarbs} êêàë. ";
        carbsProgressPercentRise.text = $"(+{carbsPercentRise} %)";
        carbsProgressBarAdditional.style.width = Length.Percent(carbsProgressBar.value + carbsPercentRise);
    }
}
