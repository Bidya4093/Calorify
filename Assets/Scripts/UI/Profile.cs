using Firebase.Auth;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.Networking;

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
    static public IntegerField userParametersAgeInput;
    static public RadioButtonGroup userParametersSexRadioToggle;

    static public Label profileCardName;
    static public Label profileCardEmail;
    static public Label changePasswordEmailLabel;

    static public VisualElement profileCardImage;
    static public VisualElement profileEditImage;
    private Button changeImageBtn;

    static public string pathToManProfilePlaceholder = "Images/man-profile-placeholder";
    static public string pathToWomanProfilePlaceholder = "Images/woman-profile-placeholder";

    static public List<string> languageChoices = new List<string> { Language.Ukrainian.ToString(), Language.English.ToString() };
    static public List<string> themeModeChoices = new List<string> { Theme.Light.ToString(), Theme.Dark.ToString() };
    static public List<string> goalChoices = new List<string> { Goal.LoseWeight.ToString(), Goal.PutOnWeight.ToString(), Goal.KeepFit.ToString() };
    static public List<string> measurementUnitChoices = new List<string> { MeasurementUnits.Metric.ToString(), MeasurementUnits.US.ToString() };


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
        userParametersAgeInput = profileRoot.Q<IntegerField>("UserParametersAgeInput");
        userParametersSexRadioToggle = profileRoot.Q<RadioButtonGroup>("UserParametersSexRadioToggle");

        profileCardName = profileRoot.Q<Label>("ProfileCardName");
        profileCardEmail = profileRoot.Q<Label>("ProfileCardEmail");
        changePasswordEmailLabel = profileRoot.Q<Label>("ChangePasswordEmailLabel");

        profileCardImage = profileRoot.Q<VisualElement>("ProfileCardImage");
        profileEditImage = profileRoot.Q<VisualElement>("ProfileEditImage");
        changeImageBtn = profileRoot.Q<Button>("ChangeImageBtn");

        changeImageBtn.clicked += LoadProfileImageFromGallery;

        settingsGoalDropdown.RegisterValueChangedCallback(OnGoalDropdownValueChanged);
        settingsThemeDropdown.RegisterValueChangedCallback(OnThemeDropdownValueChanged);
        settingsLanguageDropdown.RegisterValueChangedCallback(OnLanguageDropdownValueChanged);
        settingsMeasurementDropdown.RegisterValueChangedCallback(OnMeasurementDropdownValueChanged);

        personalDataEmailInput.RegisterValueChangedCallback(OnEmailInputValueChanged);
        personalDataNameInput.RegisterValueChangedCallback(OnNameInputValueChange);
        userParametersHeightInput.RegisterValueChangedCallback(OnHeightInputValueChanged);
        userParametersWeightInput.RegisterValueChangedCallback(OnWeightInputValueChanged);
        userParametersAgeInput.RegisterValueChangedCallback(OnAgeInputValueChanged);
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

        try
        {
            StartCoroutine(FirebaseManager.UpdateUserValue("goal", User.Instance.GetGoal()));
            MacrosManager.CalculateUserNeeds();
            DataManager.LoadChartsData();
            StartCoroutine(FirebaseManager.UpdateUserDatabaseData());

        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }

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
            //FirebaseManager.UpdateEmail(User.Instance.GetEmail());
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

    private void OnAgeInputValueChanged(ChangeEvent<int> evt)
    {
        try
        {
            User.Instance.SetAge((evt.target as IntegerField).value);
            StartCoroutine(FirebaseManager.UpdateUserValue("age", User.Instance.GetAge()));
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
            SetPlaceholderImageBySex();
            StartCoroutine(FirebaseManager.UpdateUserDatabaseData());
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    static public void SetPlaceholderImageBySex()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser.PhotoUrl != null)
            return;

        if (User.Instance.GetSex() == SexType.Male)
        {
            profileCardImage.style.backgroundImage = new StyleBackground(Resources.Load<VectorImage>(pathToManProfilePlaceholder));
            profileEditImage.style.backgroundImage = new StyleBackground(Resources.Load<VectorImage>(pathToManProfilePlaceholder));
        }
        else if (User.Instance.GetSex() == SexType.Female)
        {
            profileCardImage.style.backgroundImage = new StyleBackground(Resources.Load<VectorImage>(pathToWomanProfilePlaceholder));
            profileEditImage.style.backgroundImage = new StyleBackground(Resources.Load<VectorImage>(pathToWomanProfilePlaceholder));
        }

    }

    private void LoadProfileImageFromGallery()
    {
        if (NativeGallery.IsMediaPickerBusy())
            return;

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                SetProfileImage(path);
                StartCoroutine(FirebaseManager.UpdateProfile(User.Instance.GetUsername(), new Uri(path)));
            }
        });

        Debug.Log("Permission result: " + permission);
    }

    static public void SetProfileImage(string path = null)
    {
        if (path == null || path == "")
            path = FirebaseAuth.DefaultInstance.CurrentUser.PhotoUrl.OriginalString;

        // Create Texture from selected image
        Texture2D texture = NativeGallery.LoadImageAtPath(path);
        if (texture == null)
        {
            Debug.Log("Couldn't load texture from " + path);
            return;
        }
        profileCardImage.style.backgroundImage = texture;
        profileEditImage.style.backgroundImage = texture;
    }

    private async void RequestPermissionAsynchronously(NativeGallery.PermissionType permissionType, NativeGallery.MediaType mediaTypes)
    {
        NativeGallery.Permission permission = await NativeGallery.RequestPermissionAsync(permissionType, mediaTypes);
        Debug.Log("Permission result: " + permission);
    }

}
