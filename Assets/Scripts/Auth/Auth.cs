using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Auth : MonoBehaviour
{
    // Поля вводу даних


    private VisualElement authRoot;
    private VisualElement signUpPage;
    private VisualElement signInPage;

    public TextField signInEmailInput;
    public TextField signInPasswordInput;
    public TextField signUpEmailInput;
    public TextField signUpPasswordInput;
    public TextField nameInput;
    public FloatField heightInput;
    public FloatField weightInput;
    public IntegerField ageInput;
    public RadioButtonGroup goalRadio;
    public RadioButtonGroup activityRadio;
    public RadioButtonGroup sexRadio;

    public Button verificationContinueBtn;

    public delegate Task NextPageCallback();

    void Start()
    {
        authRoot = GetComponent<UIDocument>().rootVisualElement;
        signUpPage = authRoot.Q<VisualElement>("SignUp");
        signInPage = authRoot.Q<VisualElement>("SignIn");

        Button emailContinueBtn = signUpPage.Q<Button>("EmailContinueBtn");
        Button goalContinueBtn = signUpPage.Q<Button>("GoalContinueBtn");
        Button activityContinueBtn = signUpPage.Q<Button>("ActivityContinueBtn");
        Button nameContinueBtn = signUpPage.Q<Button>("NameContinueBtn");
        Button heightContinueBtn = signUpPage.Q<Button>("HeightContinueBtn");
        Button weightContinueBtn = signUpPage.Q<Button>("WeightContinueBtn");
        Button ageContinueBtn = signUpPage.Q<Button>("AgeContinueBtn");
        Button sexContinueBtn = signUpPage.Q<Button>("SexContinueBtn");
        Button completeAuthBtn = signUpPage.Q<Button>("CompleteAuthBtn");
        //verificationContinueBtn = signUpPage.Q<Button>("VerificationContinueBtn");

        // Trigger validation on continue btn click
        RegisterValidation(emailContinueBtn, ValidateRegisterData);
        //verificationContinueBtn.RegisterCallback<ClickEvent>(ToNextSignUpPageBase);
        RegisterValidation(goalContinueBtn, ValidateGoalRadio);
        RegisterValidation(activityContinueBtn, ValidateActivityRadio);
        RegisterValidation(nameContinueBtn, ValidateNameInput);
        RegisterValidation(heightContinueBtn, ValidateHeightInput);
        RegisterValidation(weightContinueBtn, ValidateWeightInput);
        RegisterValidation(ageContinueBtn, ValidateAgeInput);
        RegisterValidation(sexContinueBtn, ValidateSexRadio);
        completeAuthBtn.RegisterCallback<ClickEvent>(ToNextSignUpPageBase);

        signUpEmailInput = signUpPage.Q<TextField>("SignUpEmailInput");
        signUpPasswordInput = signUpPage.Q<TextField>("SignUpPasswordInput");
        signInEmailInput = signInPage.Q<TextField>("SignInEmailInput");
        signInPasswordInput = signInPage.Q<TextField>("SignInPasswordInput");

        goalRadio = signUpPage.Q<RadioButtonGroup>("GoalRadioGroup");
        activityRadio = signUpPage.Q<RadioButtonGroup>("ActivityRadioGroup");
        nameInput = signUpPage.Q<TextField>("NameInput");
        heightInput = signUpPage.Q<FloatField>("HeightInput");
        weightInput = signUpPage.Q<FloatField>("WeightInput");
        ageInput = signUpPage.Q<IntegerField>("AgeInput");
        sexRadio = signUpPage.Q<RadioButtonGroup>("SexRadioGroup");

        //verificationContinueBtn.SetEnabled(false);
        nameInput.value = "Bohdan";
        heightInput.value = 180f;
        weightInput.value = 70f;
        ageInput.value = 40;

    }

    private void RegisterValidation(Button btn, NextPageCallback ValidateCallback)
    {
        btn.RegisterCallback<ClickEvent>(async (ClickEvent evt) =>
        {
            await GetComponent<AuthPanelManager>().ToNextSignUpPageWithCallback(ValidateCallback, evt);
        });
    }

    private void ToNextSignUpPageBase(ClickEvent evt)
    {
        GetComponent<AuthPanelManager>().ToNextSignUpPageWithCallback(null, evt);
    }

    Task ValidateGoalRadio()
    {
        try
        {
            return Task.Run(() =>
            {
                GoalType goal = (GoalType)goalRadio.value;
                User.Instance.SetGoal(goal);
            });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    Task ValidateActivityRadio()
    {
        try
        {
            return Task.Run(() =>
            {
                ActivityType activity = (ActivityType)activityRadio.value;
                User.Instance.SetActivity(activity);
            });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    Task ValidateSexRadio()
    {
        try
        {
            return Task.Run(() =>
            {
                SexType sex = (SexType)sexRadio.value;
                User.Instance.SetSex(sex);
            });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    Task ValidateNameInput()
    {
        try
        {
            return Task.Run(() =>
            {
                User.Instance.SetUsername(nameInput.value);
            });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    Task ValidateHeightInput()
    {
        try
        {
            return Task.Run(() =>
            {
                User.Instance.SetHeight(heightInput.value);
            });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    Task ValidateWeightInput()
    {
        try
        {
            return Task.Run(() =>
            {
                User.Instance.SetWeight(weightInput.value);
            });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    Task ValidateAgeInput()
    {
        try
        {
            return Task.Run(() =>
            {
                User.Instance.SetAge(ageInput.value);
            });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    async Task ValidateRegisterData()
    {
        try
        {
            User.Instance.SetEmail(signUpEmailInput.value);
            await GetComponent<FirebaseManager>().RegisterCheckError(signUpEmailInput.value, signUpPasswordInput.value);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public void ClearSignInFields()
    {
        signInEmailInput.value = "";
        signInPasswordInput.value = "";
    }

    public void ClearSignUpFields()
    {
        signUpEmailInput.value = "";
        signUpPasswordInput.value = "";
        nameInput.value = "";
        heightInput.value = 0f;
        weightInput.value = 0f;
        ageInput.value = 0;
    }

    public void CompleteSignIn(ClickEvent evt)
    {
        FirebaseManager.errorLabel = signInPage.Q<Label>("ErrorLabel");
        GetComponent<FirebaseManager>().LoginButton();
    }

    public void CompleteSignUp(ClickEvent evt)
    {
        GetComponent<FirebaseManager>().RegisterButton();
    }

    public IEnumerator CreateUser()
    {
        MacrosManager.CalculateUserNeeds();

        User.Instance.WaterNeeded = 2400;
        User.Instance.SetLanguage(Language.Ukrainian);
        User.Instance.SetTheme(Theme.Light);
        User.Instance.SetMeasurementUnits(MeasurementUnits.Metric);

        yield return StartCoroutine(FirebaseManager.UpdateUserDatabaseData());
    }
}