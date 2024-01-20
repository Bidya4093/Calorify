using Firebase.Auth;
using Firebase.Database;
using MyUILibrary;
using UnityEngine;
using UnityEngine.UIElements;

public class DataManager
{
    static private DatabaseReference DBReference;
    static private FirebaseUser user;

    static private VisualElement mainRoot;
    static private VisualElement profileRoot;

    static private RadialProgress caloriesProgress;
    static private RadialProgress fatsProgress;
    static private RadialProgress proteinsProgress;
    static private RadialProgress carbsProgress;
    static private ProgressBar waterProgressBar;

    static private Label caloriesProgressPercent;
    static private Label caloriesProgressLabel;
    static private Label fatsProgressLabel;
    static private Label proteinsProgressLabel;
    static private Label carbsProgressLabel;
    static private Label waterProgressBarLabel;

    static private DropdownField settingsGoalDropdown;
    static private DropdownField settingsThemeDropdown;
    static private DropdownField settingsLanguageDropdown;
    static private DropdownField settingsMeasurementDropdown;

    static private TextField personalDataEmailInput;
    static private TextField personalDataNameInput;
    static private FloatField userParametersHeightInput;
    static private FloatField userParametersWeightInput;
    static private RadioButtonGroup userParametersSexRadioToggle;

    static private Label profileCardName;
    static private Label profileCardEmail;
    static private Label changePasswordEmailLabel;

    static public void Init()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        DBReference = FirebaseDatabase.DefaultInstance.RootReference;
        mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;
        profileRoot = GameObject.Find("ProfilePage").GetComponent<UIDocument>().rootVisualElement;

        caloriesProgress = mainRoot.Q<RadialProgress>("CaloriesRadialProgress");
        fatsProgress = mainRoot.Q<RadialProgress>("FatsRadialProgress");
        proteinsProgress = mainRoot.Q<RadialProgress>("ProteinsRadialProgress");
        carbsProgress = mainRoot.Q<RadialProgress>("CarbsRadialProgress");
        waterProgressBar = mainRoot.Q<ProgressBar>("WaterProgressBar");

        caloriesProgressPercent = mainRoot.Q<Label>("CaloriesRadialProgressPercent");
        caloriesProgressLabel = mainRoot.Q<Label>("CaloriesRadialProgressLabel");
        fatsProgressLabel = mainRoot.Q<Label>("FatsRadialProgressLabel");
        proteinsProgressLabel = mainRoot.Q<Label>("ProteinsRadialProgressLabel");
        carbsProgressLabel = mainRoot.Q<Label>("CarbsRadialProgressLabel");
        waterProgressBarLabel = mainRoot.Q<Label>("WaterProgressBarLabel");

        //settingsGoalDropdown = profileRoot.Q<DropdownField>("SettingsGoalDropdown");
        //settingsThemeDropdown = profileRoot.Q<DropdownField>("SettingsThemeDropdown");
        //settingsLanguageDropdown = profileRoot.Q<DropdownField>("SettingsLanguageDropdown");
        //settingsMeasurementDropdown = profileRoot.Q<DropdownField>("SettingsMeasurementDropdown");

        //personalDataEmailInput = profileRoot.Q<TextField>("PersonalDataEmailInput");
        //personalDataNameInput = profileRoot.Q<TextField>("PersonalDataNameInput");
        //userParametersHeightInput = profileRoot.Q<FloatField>("UserParametersHeightInput");
        //userParametersWeightInput = profileRoot.Q<FloatField>("UserParametersWeightInput");
        //userParametersSexRadioToggle = profileRoot.Q<RadioButtonGroup>("UserParametersSexRadioToggle");

        //profileCardName = profileRoot.Q<Label>("ProfileCardName");
        //profileCardEmail = profileRoot.Q<Label>("ProfileCardEmail");
        //changePasswordEmailLabel = profileRoot.Q<Label>("ChangePasswordEmailLabel");
        // Duplicate of Profile

    }

    static public void LoadChartsData()
    {
        caloriesProgressPercent.text = ((User.Instance.CaloriesEaten*100) / User.Instance.CaloriesNeeded).ToString() + "%";
        caloriesProgress.progress = ((float)User.Instance.CaloriesEaten / (float)User.Instance.CaloriesNeeded)*100f;
        fatsProgress.progress = ((float)User.Instance.FatsEaten / (float)User.Instance.FatsNeeded) *100f;
        proteinsProgress.progress = ((float)User.Instance.ProtsEaten / (float)User.Instance.ProtsNeeded) *100f;
        carbsProgress.progress = ((float)User.Instance.CarbsEaten / (float)User.Instance.CarbsNeeded) *100f;
        waterProgressBar.value = ((float)User.Instance.WaterDrunk / (float)User.Instance.WaterNeeded) * 100f;

        caloriesProgressLabel.text = User.Instance.CaloriesEaten.ToString() + " / " + User.Instance.CaloriesNeeded.ToString() + " ����.";
        fatsProgressLabel.text = User.Instance.FatsEaten.ToString() + " / " + User.Instance.FatsNeeded.ToString() + " �.";
        proteinsProgressLabel.text = User.Instance.ProtsEaten.ToString() + " / " + User.Instance.ProtsNeeded.ToString() + " �.";
        carbsProgressLabel.text = User.Instance.CarbsEaten.ToString() + " / " + User.Instance.CarbsNeeded.ToString() + " �.";
        waterProgressBarLabel.text = ((float)User.Instance.WaterDrunk/1000).ToString() + " / " + ((float)User.Instance.WaterNeeded/1000).ToString() + " �.";
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
