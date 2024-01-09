using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine.UIElements;

public class FirebaseManager : MonoBehaviour
{

    private VisualElement root;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    static public FirebaseAuth auth;    
    static public FirebaseUser firebaseUser;
    static public DatabaseReference DBreference;

    //Login variables
    [Header("Login")]
    public TextField signInEmailInput;
    public TextField signInPasswordInput;
    //public TMP_Text warningLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField heightInput;
    public TMP_InputField weightInput;
    //public TMP_InputField goalInput;
    //public TMP_InputField passwordRegisterVerifyField;
    //public TMP_Text warningRegisterText;


    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        signInEmailInput = root.Q<TextField>("SignInEmailInput");
        signInPasswordInput = root.Q<TextField>("SignInPasswordInput");

    }

    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(signInEmailInput.value, signInPasswordInput.value));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            //warningLoginText.text = message;
            Debug.LogError(message);
        }
        else
        {
            //firebaseUser is now logged in
            //Now get the result
            firebaseUser = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", firebaseUser.DisplayName, firebaseUser.Email);
            //warningLoginText.text = "";
            //SceneManager.LoadScene("MainScreenUIToolkit");

            //yield return new WaitForSeconds(2);
            //StartCoroutine(UpdateUsernameDatabase("Updated username"));
            //StartCoroutine(LoadUserData());
            StartCoroutine(SceneLoader.LoadSceneAsync("MainScreenUIToolkit"));

        }
    }

    public IEnumerator Register(string _email, string _password, string _username = "Bohdan")
    {
        Debug.Log($" {_email}, {_password}, {_username}");
        if (_username == "")
        {
            //If the username field is blank show a warning
            //warningRegisterText.text = "Missing Username";
            Debug.LogError("Missing Username");
        }
        //else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
        //{
        //    //If the password does not match show a warning
        //    warningRegisterText.text = "Password Does Not Match!";
        //}
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            Debug.Log(_email + " " + _password);
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                //warningRegisterText.text = message;
                Debug.LogError(message);
            }
            else
            {
                //firebaseUser has now been created
                //Now get the result
                firebaseUser = RegisterTask.Result.User;
                if (firebaseUser != null)
                {
                    //Create a firebaseUser profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update firebaseUser profile function passing the profile with the username
                    Task ProfileTask = firebaseUser.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        //warningRegisterText.text = "Username Set Failed!";
                        Debug.LogError("Username Set Failed!");
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        //warningRegisterText.text = "";
                        CreateUser();
                        //SceneManager.LoadScene("MainScreenUIToolkit");

                        StartCoroutine(SceneLoader.LoadSceneAsync("MainScreenUIToolkit"));
                    }
                }
            }
        }
    }


    void CreateUser()
    {
        int age = 24;
        string sex = "male";


        User user = new User(usernameRegisterField.text, emailRegisterField.text, sex, age, float.Parse(heightInput.text),
                    float.Parse(weightInput.text), (short)GoalType.KeepFit, (short)ActivityType.Regular);

        MacrosManager.Calculate(sex, (short)ActivityType.Regular, float.Parse(weightInput.text), float.Parse(heightInput.text), age);

        user.caloriesNeeded = MacrosManager.caloriesNeeded;
        user.carbsNeeded = MacrosManager.carbsNeeded;
        user.fatsNeeded = MacrosManager.fatsNeeded;
        user.protsNeeded = MacrosManager.protsNeeded;

        string json = JsonUtility.ToJson(user);
        DBreference.Child("users").Child(firebaseUser.UserId).SetRawJsonValueAsync(json);
    }

}


