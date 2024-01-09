using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    //private VisualElement signUpBackBtn;
    private Button signInContinueBtn;


    private Label signInLink;
    private Label signUpLink;

    //public GameObject scanPageObject;
    //public GameObject messagePageObject;
    //public GameObject profilePageObject;
    //public GameObject productPanelObject;

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
        //signUpBackBtn = signUpPage.Q<VisualElement>("BackBtn");

        signInBackBtn.RegisterCallback<ClickEvent>(CloseSignInPage);
        //signUpBackBtn.RegisterCallback<ClickEvent>(ToPreviousSignUpPage);

        signInContinueBtn = signInPage.Q<Button>("SignInContinueBtn");
        signInContinueBtn.RegisterCallback<ClickEvent>(CompleteSignIn);

        signUpStepPages.ForEach(page =>
        {
            Button nextBtn = page.Q<Button>(className: "auth-btn");
            nextBtn.RegisterCallback<ClickEvent>(ToNextSignUpPage);

            VisualElement backBtn = page.Q<VisualElement>("BackBtn");
            backBtn.RegisterCallback<ClickEvent>(ToPreviousSignUpPage);

        });
    }

    void OpenSignInPage(ClickEvent evt)
    {
        welcomePage.style.display = DisplayStyle.None;
        signUpPage.style.display = DisplayStyle.None;
        signInPage.style.display = DisplayStyle.Flex;
    }

    void CloseSignInPage(ClickEvent evt)
    {
        welcomePage.style.display = DisplayStyle.Flex;
        signInPage.style.display = DisplayStyle.None;
    }

    void OpenSignUpPage(ClickEvent evt)
    {
        welcomePage.style.display = DisplayStyle.None;
        signInPage.style.display = DisplayStyle.None;
        signUpPage.style.display = DisplayStyle.Flex;
        currentSignUpStep = 0;
    }

    void CloseSignUpPage(ClickEvent evt)
    {
        welcomePage.style.display = DisplayStyle.Flex;
        signUpPage.style.display = DisplayStyle.None;
    }

    void ToPreviousSignUpPage(ClickEvent evt)
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

    void ToNextSignUpPage(ClickEvent evt)
    {
        
        currentSignUpStep++;
        if (currentSignUpStep >= signUpStepPages.Count)
        {
            currentSignUpStep = signUpStepPages.Count - 1;
            CompleteSignUp(evt);
            return;
        }
        UpdateSignUpPages();

    }

    void UpdateSignUpPages()
    {
        signUpStepPages.ForEach((stepPage) =>
        {
            if (signUpStepPages.IndexOf(stepPage) == currentSignUpStep)
            {
                stepPage.style.display = DisplayStyle.Flex;

            } else
            {
                stepPage.style.display = DisplayStyle.None;
            }
        });
    }

    void CompleteSignIn(ClickEvent evt)
    {
        // Перевірка даних
        Debug.Log("You have successfully logged in");
        GetComponent<FirebaseManager>().LoginButton();

    }

    void CompleteSignUp(ClickEvent evt)
    {
        // Реєстрація
        Debug.Log("You have successfully registered");
    }
}
