using Firebase.Auth;
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
}
