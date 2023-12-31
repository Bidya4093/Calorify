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



    void Start()
    {
        profileRoot = GetComponent<UIDocument>().rootVisualElement;
        mainRoot = GameObject.Find("MainPage").GetComponent<UIDocument>().rootVisualElement;

        profileEditBtn = profileRoot.Q<Button>("ProfileCardEditBtn");
        backBtn = profileRoot.Q<Button>("BackBtn");
        closeProfilePage = profileRoot.Q<Button>("CloseBtn");
        closeChangePasswordPage = profileRoot.Q<TemplateContainer>("ChangePasswordTemplate").Q<Button>("CloseBtn");

        changePasswordOption = profileRoot.Q<VisualElement>("SettingsChangePassword");


        profileEditTemplate = profileRoot.Q<TemplateContainer>("ProfileEditTemplate");
        profileTemplate = profileRoot.Q<TemplateContainer>("ProfileTemplate");
        changePasswordTemplate = profileRoot.Q<TemplateContainer>("ChangePasswordTemplate");


        profileEditBtn.clicked += OpenProfileEditPage;
        closeProfilePage.clicked += CloseProfilePage;
        backBtn.clicked += CloseProfileEditPage;
        changePasswordOption.RegisterCallback<ClickEvent>(OpenChangePasswordPage);
        closeChangePasswordPage.clicked += CloseChangePasswordPage;

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
}
