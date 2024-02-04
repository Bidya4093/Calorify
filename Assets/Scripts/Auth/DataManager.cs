using Firebase.Auth;
using Firebase.Database;
using MyUILibrary;
using UnityEngine;
using UnityEngine.UIElements;

public class DataManager
{
    static private FirebaseUser user;

    static private VisualElement mainRoot;
    static private VisualElement productPanel;

    static private RadialProgress caloriesProgress;
    static private RadialProgress fatsProgress;
    static private RadialProgress proteinsProgress;
    static private RadialProgress carbsProgress;
    static private ProgressBar waterProgressBar;

    static private ProgressBar protsProgressBar;
    static private ProgressBar fatsProgressBar;
    static private ProgressBar carbsProgressBar;
    static private ProgressBar caloriesProgressBar;

    static private Label caloriesProgressPercent;
    static private Label caloriesProgressLabel;
    static private Label fatsProgressLabel;
    static private Label proteinsProgressLabel;
    static private Label carbsProgressLabel;
    static private Label waterProgressBarLabel;

    static public void Init()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;
        productPanel = GameObject.Find("ProductPanel").GetComponent<UIDocument>().rootVisualElement;

        caloriesProgress = mainRoot.Q<RadialProgress>("CaloriesRadialProgress");
        fatsProgress = mainRoot.Q<RadialProgress>("FatsRadialProgress");
        proteinsProgress = mainRoot.Q<RadialProgress>("ProteinsRadialProgress");
        carbsProgress = mainRoot.Q<RadialProgress>("CarbsRadialProgress");
        waterProgressBar = mainRoot.Q<ProgressBar>("WaterProgressBar");

        caloriesProgressBar = productPanel.Q<ProgressBar>("ProductPanelCaloriesProgressBar");
        carbsProgressBar = productPanel.Q<ProgressBar>("ProductPanelCarbsProgressBar");
        protsProgressBar = productPanel.Q<ProgressBar>("ProductPanelProtsProgressBar");
        fatsProgressBar = productPanel.Q<ProgressBar>("ProductPanelFatsProgressBar");

        caloriesProgressPercent = mainRoot.Q<Label>("CaloriesRadialProgressPercent");
        caloriesProgressLabel = mainRoot.Q<Label>("CaloriesRadialProgressLabel");
        fatsProgressLabel = mainRoot.Q<Label>("FatsRadialProgressLabel");
        proteinsProgressLabel = mainRoot.Q<Label>("ProteinsRadialProgressLabel");
        carbsProgressLabel = mainRoot.Q<Label>("CarbsRadialProgressLabel");
        waterProgressBarLabel = mainRoot.Q<Label>("WaterProgressBarLabel");
    }

    static public void LoadChartsData()
    {
        caloriesProgressPercent.text = ((User.Instance.CaloriesEaten*100) / User.Instance.CaloriesNeeded).ToString() + "%";
        caloriesProgress.progress = ((float)User.Instance.CaloriesEaten / (float)User.Instance.CaloriesNeeded)*100f;
        fatsProgress.progress = ((float)User.Instance.FatsEaten / (float)User.Instance.FatsNeeded) *100f;
        proteinsProgress.progress = ((float)User.Instance.ProtsEaten / (float)User.Instance.ProtsNeeded) *100f;
        carbsProgress.progress = ((float)User.Instance.CarbsEaten / (float)User.Instance.CarbsNeeded) *100f;

        caloriesProgressBar.value = ((float)User.Instance.CaloriesEaten / (float)User.Instance.CaloriesNeeded) * 100f;
        fatsProgressBar.value = ((float)User.Instance.FatsEaten / (float)User.Instance.FatsNeeded) * 100f;
        protsProgressBar.value = ((float)User.Instance.ProtsEaten / (float)User.Instance.ProtsNeeded) * 100f;
        carbsProgressBar.value = ((float)User.Instance.CarbsEaten / (float)User.Instance.CarbsNeeded) * 100f;

        caloriesProgressLabel.text = User.Instance.CaloriesEaten.ToString() + " / " + User.Instance.CaloriesNeeded.ToString() + " κκΰλ.";
        fatsProgressLabel.text = User.Instance.FatsEaten.ToString() + " / " + User.Instance.FatsNeeded.ToString() + " γ.";
        proteinsProgressLabel.text = User.Instance.ProtsEaten.ToString() + " / " + User.Instance.ProtsNeeded.ToString() + " γ.";
        carbsProgressLabel.text = User.Instance.CarbsEaten.ToString() + " / " + User.Instance.CarbsNeeded.ToString() + " γ.";
        RenderWaterData();
        GameObject.Find("WaterPanel").GetComponent<WaterPanel>().LoadWaterData();
    }

    //static public void

    static public void RenderWaterData()
    {

        waterProgressBar.value = ((float)User.Instance.WaterDrunk / (float)User.Instance.WaterNeeded) * 100f;
        waterProgressBarLabel.text = ((float)User.Instance.WaterDrunk / 1000).ToString() + " / " + ((float)User.Instance.WaterNeeded / 1000).ToString() + " λ.";
    }

    static public void LoadProfileData()
    {
        Profile.profileCardName.text = User.Instance.GetUsername();
        Profile.profileCardEmail.text = User.Instance.GetEmail();
        Profile.changePasswordEmailLabel.text = User.Instance.GetEmail();
        Profile.personalDataNameInput.SetValueWithoutNotify(User.Instance.GetUsername());
        Profile.personalDataEmailInput.SetValueWithoutNotify(User.Instance.GetEmail());
        Profile.userParametersHeightInput.SetValueWithoutNotify(User.Instance.GetHeight());
        Profile.userParametersWeightInput.SetValueWithoutNotify(User.Instance.GetWeight());
        Profile.userParametersSexRadioToggle.SetValueWithoutNotify((int)User.Instance.GetSex());

        if (user.PhotoUrl != null)
        {
            Profile.SetProfileImage();
            return;
        }
        Profile.SetPlaceholderImageBySex();
        
    }

    static public void LoadSettingsData()
    {
        Profile.settingsGoalDropdown.choices = Profile.goalChoices;
        Profile.settingsGoalDropdown.SetValueWithoutNotify(Profile.goalChoices[(int)User.Instance.GetGoal()]);
        Profile.settingsThemeDropdown.choices = Profile.themeModeChoices;
        Profile.settingsThemeDropdown.SetValueWithoutNotify(User.Instance.GetTheme());
        Profile.settingsLanguageDropdown.choices = Profile.languageChoices;
        Profile.settingsLanguageDropdown.SetValueWithoutNotify(User.Instance.GetLanguage());
        Profile.settingsMeasurementDropdown.choices = Profile.measurementUnitChoices;
        Profile.settingsMeasurementDropdown.SetValueWithoutNotify(User.Instance.GetMeasurementUnits());
    }
}
