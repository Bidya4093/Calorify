using Firebase.Auth;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Profile : MonoBehaviour
{
    private VisualElement mainRoot;
    private VisualElement profileRoot;

    private TemplateContainer profileEditTemplate;
    private TemplateContainer profileTemplate;
    private TemplateContainer changePasswordTemplate;

    private Button profileEditBtn;
    private Button closeProfilePage;
    private Button backBtn;
    private VisualElement changePasswordOption;
    private Button closeChangePasswordPage;
    private Button signOutBtn;

    static public DropdownField settingsGoalDropdown;
    static public DropdownField settingsThemeDropdown;
    static public DropdownField settingsLanguageDropdown;
    static public DropdownField settingsMeasurementDropdown;

    static public TextField personalDataEmailInput;
    static public TextField personalDataNameInput;
    static public FloatField userParametersHeightInput;
    static public FloatField userParametersWeightInput;
    static public RadioButtonGroup userParametersSexRadioToggle;

    static public Label profileCardName;
    static public Label profileCardEmail;
    static public Label changePasswordEmailLabel;

    public static List<string> languageChoices = new List<string> { Language.Ukrainian.ToString(), Language.English.ToString() };
    public static List<string> themeModeChoices = new List<string> { Theme.Light.ToString(), Theme.Dark.ToString() };
    public static List<string> goalChoices = new List<string> { Goal.LoseWeight.ToString(), Goal.PutOnWeight.ToString(), Goal.KeepFit.ToString() };
    public static List<string> measurementUnitChoices = new List<string> { MeasurementUnits.Metric.ToString(), MeasurementUnits.US.ToString() };


    void Start()
    {
        profileRoot = GetComponent<UIDocument>().rootVisualElement;
        mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;

        profileTemplate = profileRoot.Q<TemplateContainer>("ProfileTemplate");
        profileEditTemplate = profileRoot.Q<TemplateContainer>("ProfileEditTemplate");
        changePasswordTemplate = profileRoot.Q<TemplateContainer>("ChangePasswordTemplate");

        profileEditBtn = profileTemplate.Q<Button>("ProfileCardEditBtn");
        closeProfilePage = profileTemplate.Q<Button>("CloseBtn");
        backBtn = profileEditTemplate.Q<Button>("BackBtn");
        changePasswordOption = profileTemplate.Q<VisualElement>("SettingsChangePassword");
        closeChangePasswordPage = changePasswordTemplate.Q<Button>("CloseBtn");
        signOutBtn = profileEditTemplate.Q<Button>("SignOutBtn");

        profileEditBtn.clicked += OpenProfileEditPage;
        closeProfilePage.clicked += CloseProfilePage;
        backBtn.clicked += CloseProfileEditPage;
        changePasswordOption.RegisterCallback<ClickEvent>(OpenChangePasswordPage);
        closeChangePasswordPage.clicked += CloseChangePasswordPage;
        signOutBtn.clicked += SignOut;

        settingsGoalDropdown = profileRoot.Q<DropdownField>("SettingsGoalDropdown");
        settingsThemeDropdown = profileRoot.Q<DropdownField>("SettingsThemeDropdown");
        settingsLanguageDropdown = profileRoot.Q<DropdownField>("SettingsLanguageDropdown");
        settingsMeasurementDropdown = profileRoot.Q<DropdownField>("SettingsMeasurementDropdown");

        personalDataEmailInput = profileRoot.Q<TextField>("PersonalDataEmailInput");
        personalDataNameInput = profileRoot.Q<TextField>("PersonalDataNameInput");
        userParametersHeightInput = profileRoot.Q<FloatField>("UserParametersHeightInput");
        userParametersWeightInput = profileRoot.Q<FloatField>("UserParametersWeightInput");
        userParametersSexRadioToggle = profileRoot.Q<RadioButtonGroup>("UserParametersSexRadioToggle");

        profileCardName = profileRoot.Q<Label>("ProfileCardName");
        profileCardEmail = profileRoot.Q<Label>("ProfileCardEmail");
        changePasswordEmailLabel = profileRoot.Q<Label>("ChangePasswordEmailLabel");

        settingsGoalDropdown.RegisterValueChangedCallback(OnGoalDropdownValueChanged);
        settingsThemeDropdown.RegisterValueChangedCallback(OnThemeDropdownValueChanged);
        settingsLanguageDropdown.RegisterValueChangedCallback(OnLanguageDropdownValueChanged);
        settingsMeasurementDropdown.RegisterValueChangedCallback(OnMeasurementDropdownValueChanged);

        personalDataEmailInput.RegisterValueChangedCallback(OnEmailInputValueChanged);
        personalDataNameInput.RegisterValueChangedCallback(OnNameInputValueChange);
        userParametersHeightInput.RegisterValueChangedCallback(OnHeightInputValueChanged);
        userParametersWeightInput.RegisterValueChangedCallback(OnWeightInputValueChanged);
        userParametersSexRadioToggle.RegisterValueChangedCallback(OnSexRadioToggleValueChanged);

    }

    private void OpenProfileEditPage()
    {
        profileEditTemplate.style.display = DisplayStyle.Flex;
        profileTemplate.style.display = DisplayStyle.None;
    }

    private void CloseProfileEditPage()
    {
        profileEditTemplate.style.display = DisplayStyle.None;
        profileTemplate.style.display = DisplayStyle.Flex;
    }

    private void CloseProfilePage()
    {
        profileRoot.style.display = DisplayStyle.None;
        mainRoot.style.display = DisplayStyle.Flex;
    }

    private void OpenChangePasswordPage(ClickEvent evt)
    {
        changePasswordTemplate.style.display = DisplayStyle.Flex;
        profileTemplate.style.display = DisplayStyle.None;
    }

    private void CloseChangePasswordPage()
    {
        changePasswordTemplate.style.display = DisplayStyle.None;
        profileTemplate.style.display = DisplayStyle.Flex;
    }

    private void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        StartCoroutine(SceneLoader.LoadSceneAsync(Scenes.Auth));
    }

    private void OnGoalDropdownValueChanged(ChangeEvent<string> evt)
    {
        int choiceIndex = (evt.currentTarget as DropdownField).index;
        User.Instance.SetGoal((GoalType)choiceIndex);
        StartCoroutine(FirebaseManager.UpdateUserValue("goal", User.Instance.GetGoal()));

    }
    private void OnThemeDropdownValueChanged(ChangeEvent<string> evt)
    {
        int choiceIndex = (evt.currentTarget as DropdownField).index;
        User.Instance.SetTheme(Theme.Values[choiceIndex]);
        StartCoroutine(FirebaseManager.UpdateUserValue("theme", User.Instance.GetTheme()));
    }
    private void OnLanguageDropdownValueChanged(ChangeEvent<string> evt)
    {
        int choiceIndex = (evt.currentTarget as DropdownField).index;
        User.Instance.SetLanguage(Language.Values[choiceIndex]);
        StartCoroutine(FirebaseManager.UpdateUserValue("language", User.Instance.GetLanguage()));
    }
    private void OnMeasurementDropdownValueChanged(ChangeEvent<string> evt)
    {
        int choiceIndex = (evt.currentTarget as DropdownField).index;
        User.Instance.SetMeasurementUnits(MeasurementUnits.Values[choiceIndex]);
        StartCoroutine(FirebaseManager.UpdateUserValue("measurementUnits", User.Instance.GetMeasurementUnits()));
    }

    private void OnEmailInputValueChanged(ChangeEvent<string> evt)
    {
        try
        {
            User.Instance.SetEmail((evt.target as TextField).value);
            StartCoroutine(FirebaseManager.UpdateUserValue("email", User.Instance.GetEmail()));
            profileCardEmail.text = User.Instance.GetEmail();
            changePasswordEmailLabel.text = User.Instance.GetEmail();
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
        // Надсилати лист для підтвердження пошти
    }

    private void OnNameInputValueChange(ChangeEvent<string> evt)
    {
        try
        {
            User.Instance.SetUsername((evt.target as TextField).value);
            StartCoroutine(FirebaseManager.UpdateUserValue("username", User.Instance.GetUsername()));
            profileCardName.text = User.Instance.GetUsername();

        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    private void OnHeightInputValueChanged(ChangeEvent<float> evt)
    {
        try
        {
            User.Instance.SetHeight((evt.target as FloatField).value);
            StartCoroutine(FirebaseManager.UpdateUserValue("height", User.Instance.GetHeight()));
            MacrosManager.CalculateUserNeeds();
            DataManager.LoadChartsData();
            StartCoroutine(FirebaseManager.UpdateUserDatabaseData());

        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }
    private void OnWeightInputValueChanged(ChangeEvent<float> evt)
    {
        try
        {
            User.Instance.SetWeight((evt.target as FloatField).value);
            StartCoroutine(FirebaseManager.UpdateUserValue("weight", User.Instance.GetWeight()));
            MacrosManager.CalculateUserNeeds();
            DataManager.LoadChartsData();
            StartCoroutine(FirebaseManager.UpdateUserDatabaseData());

        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }
    private void OnSexRadioToggleValueChanged(ChangeEvent<int> evt)
    {
        try
        {
            User.Instance.SetSex((SexType)(evt.target as RadioButtonGroup).value);
            StartCoroutine(FirebaseManager.UpdateUserValue("sex", User.Instance.GetSex()));
            MacrosManager.CalculateUserNeeds();
            DataManager.LoadChartsData();
            StartCoroutine(FirebaseManager.UpdateUserDatabaseData());

        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

}
