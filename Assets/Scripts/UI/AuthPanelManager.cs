using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static Auth;

public class AuthPanelManager : MonoBehaviour
{

    private VisualElement authRoot;

    private VisualElement welcomePage;
    private VisualElement signInPage;
    private VisualElement signUpPage;

    private List<VisualElement> signUpStepPages;
    private int currentSignUpStep = -1;

    private Button welcomeSignInBtn;
    private Button welcomeSignUpBtn;
    private VisualElement signInBackBtn;
    private Button signInContinueBtn;

    private Label signInLink;
    private Label signUpLink;

    private VisualElement signInShowPasswordBtn;
    private VisualElement signUpShowPasswordBtn;



    void Start()
    {
        authRoot = GetComponent<UIDocument>().rootVisualElement;

        welcomePage = authRoot.Q<VisualElement>("Welcome");
        signInPage = authRoot.Q<VisualElement>("SignIn");
        signUpPage = authRoot.Q<VisualElement>("SignUp");

        welcomeSignInBtn = welcomePage.Q<Button>("WelcomeSignInBtn");
        welcomeSignUpBtn = welcomePage.Q<Button>("WelcomeSignUpBtn");

        welcomeSignInBtn.RegisterCallback<ClickEvent>(OpenSignInPage);
        welcomeSignUpBtn.RegisterCallback<ClickEvent>(OpenSignUpPage);
        signUpStepPages = signUpPage.Children().ToList();


        signInLink = signInPage.Q<Label>("SignInLink");
        signUpLink = signUpPage.Q<Label>("SignUpLink");

        signInLink.RegisterCallback<ClickEvent>(OpenSignUpPage);
        signUpLink.RegisterCallback<ClickEvent>(OpenSignInPage);


        signInBackBtn = signInPage.Q<VisualElement>("BackBtn");
        signInBackBtn.RegisterCallback<ClickEvent>(CloseSignInPage);

        signInContinueBtn = signInPage.Q<Button>("SignInContinueBtn");
        signInContinueBtn.RegisterCallback<ClickEvent>(GetComponent<Auth>().CompleteSignIn);

        signUpStepPages.ForEach(page =>
        {
            VisualElement backBtn = page.Q<VisualElement>("BackBtn");
            backBtn.RegisterCallback<ClickEvent>(ToPreviousSignUpPage);
        });

        signInShowPasswordBtn = signInPage.Q<VisualElement>("SignInShowPasswordBtn");
        signUpShowPasswordBtn = signUpPage.Q<VisualElement>("SignUpShowPasswordBtn");
        signInShowPasswordBtn.RegisterCallback<ClickEvent>(ToggleShowPassword);
        signUpShowPasswordBtn.RegisterCallback<ClickEvent>(ToggleShowPassword);
    }

    public void OpenSignInPage(ClickEvent evt)
    {
        welcomePage.style.display = DisplayStyle.None;
        signUpPage.style.display = DisplayStyle.None;
        signInPage.style.display = DisplayStyle.Flex;
        FirebaseManager.errorLabel = signInPage.Q<Label>("ErrorLabel");

    }

    public void CloseSignInPage(ClickEvent evt)
    {
        welcomePage.style.display = DisplayStyle.Flex;
        signInPage.style.display = DisplayStyle.None;
    }

    public void OpenSignUpPage(ClickEvent evt)
    {
        welcomePage.style.display = DisplayStyle.None;
        signInPage.style.display = DisplayStyle.None;
        signUpPage.style.display = DisplayStyle.Flex;
        FirebaseManager.errorLabel = signUpPage.Q<Label>("ErrorLabel");
        currentSignUpStep = 0;
    }

    public void CloseSignUpPage(ClickEvent evt)
    {
        welcomePage.style.display = DisplayStyle.Flex;
        signUpPage.style.display = DisplayStyle.None;
    }

    public void ToPreviousSignUpPage(ClickEvent evt)
    {
        currentSignUpStep--;
        if (currentSignUpStep < 0)
        {
            currentSignUpStep = -1;
            CloseSignUpPage(evt);
            return;
        }
        UpdateSignUpPages();
    }


    public VisualElement UpdateSignUpPages()
    {
        VisualElement currentPage = null;
        signUpStepPages.ForEach((stepPage) =>
        {
            int pageIndex = signUpStepPages.IndexOf(stepPage);
            if (pageIndex == currentSignUpStep)
            {
                stepPage.style.display = DisplayStyle.Flex;
                currentPage = stepPage;
                if (stepPage.Q<Label>("ErrorLabel") != null)
                    FirebaseManager.errorLabel = stepPage.Q<Label>("ErrorLabel");
            }
            else
            {
                stepPage.style.display = DisplayStyle.None;
            }
        });
        FirebaseManager.errorLabel.text = "";
        return currentPage;
    }

    public void ToggleShowPassword(ClickEvent evt)
    {
        TextField passwordInput = (TextField)(evt.target as VisualElement).parent;
        passwordInput.isPasswordField = !passwordInput.isPasswordField;
    }

    public async Task ToNextSignUpPage(NextPageCallback nextPageCallback, ClickEvent evt)
    {
        try
        {
            if (nextPageCallback != null)
            {
                await nextPageCallback();
            }

            currentSignUpStep++;
            if (currentSignUpStep >= signUpStepPages.Count)
            {
                currentSignUpStep = signUpStepPages.Count - 1;
                GetComponent<Auth>().CompleteSignUp(evt);
                return;
            }
            UpdateSignUpPages();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            FirebaseManager.errorLabel.text = ex.Message;
        }
    }
}
