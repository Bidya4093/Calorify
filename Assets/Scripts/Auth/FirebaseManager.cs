using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine.UIElements;
using System;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    

    private VisualElement root;

    //Firebase variables
    public bool enableAutoLogin;
    public DependencyStatus dependencyStatus;
    static public FirebaseAuth auth;    
    static public FirebaseUser firebaseUser;
    static public DatabaseReference DBreference;

    //Login variables
    public TextField signInEmailInput;
    public TextField signInPasswordInput;
    static public Label errorLabel;

    //Register variables
    public TextField signUpEmailInput;
    public TextField signUpPasswordInput;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        signInEmailInput = root.Q<TextField>("SignInEmailInput");
        signInPasswordInput = root.Q<TextField>("SignInPasswordInput");
        signUpEmailInput = root.Q<TextField>("SignUpEmailInput");
        signUpPasswordInput = root.Q<TextField>("SignUpPasswordInput");
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        Task<DependencyStatus> DependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => DependencyTask.IsCompleted);

        dependencyStatus = DependencyTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            //If they are avalible Initialize Firebase
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
            if (enableAutoLogin)
                StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;

        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        AuthStateChanged(this, null);
    }

    IEnumerator CheckForAutoLogin()
    {
        if (firebaseUser != null)
        {
            Task ReloadUserTask = firebaseUser.ReloadAsync();

            yield return new WaitUntil(() => ReloadUserTask.IsCompleted);
            yield return AutoLogin();
        }
        else
        {

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != Scenes.Auth.ToString())
            {
                yield return StartCoroutine(SceneLoader.LoadSceneAsync(Scenes.Auth));
                GetComponent<AuthPanelManager>().OpenSignInPage(null);
            }
        }
    }

    void HandleDatabaseValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        else if (args.Snapshot.Value == null || args.Snapshot == null)
        {
            //No data exists yet
            Debug.Log("No data exists yet");

        }
        else
        {
            // Do something with the data in args.Snapshot
            Debug.Log("HandleDatabaseValueChanged");
            DataSnapshot snapshot = args.Snapshot;
            User.SetUserDataWithSnapshot(snapshot);
        }
    }

    void SetUserDataWithFirebase()
    {
        DBreference.Child("users").Child(firebaseUser.UserId).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                User.SetUserDataWithSnapshot(snapshot);
                User.Instance.Show();
                DataManager.Init();
                DataManager.LoadChartsData();
                DataManager.LoadProfileData();
                DataManager.LoadSettingsData();
            }
        });
    }

    private IEnumerator AutoLogin()
    {
        if (firebaseUser != null)
        {

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != Scenes.Main.ToString())
            {
                yield return StartCoroutine(SceneLoader.LoadSceneAsync(Scenes.Main));
            }
            SetUserDataWithFirebase();

        }
        else
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != Scenes.Auth.ToString())
            {
                yield return StartCoroutine(SceneLoader.LoadSceneAsync(Scenes.Auth));
            }
            GetComponent<AuthPanelManager>().OpenSignInPage(null);
        }
    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != firebaseUser)
        {
            bool signedIn = firebaseUser != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && firebaseUser != null)
            {
                Debug.Log("Signed out " + firebaseUser.Email);
            }
            firebaseUser = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + firebaseUser.Email);
                FirebaseDatabase.DefaultInstance.GetReference("users/"+firebaseUser.UserId).ValueChanged += HandleDatabaseValueChanged;
            }
        }
    }

    public void LoginButton()
    {
        StartCoroutine(Login(signInEmailInput.value, signInPasswordInput.value));
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(signUpEmailInput.value, signUpPasswordInput.value, User.Instance.GetUsername()));
    }

    static public void UpdateEmail(string email)
    {
        if (firebaseUser != null)
        {
            firebaseUser.UpdateEmailAsync(email).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User email updated successfully.");
            });
        }
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

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
            Debug.LogError(message);
            errorLabel.text = message;
        }
        else
        {
            //firebaseUser is now logged in
            //Now get the result
            firebaseUser = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", firebaseUser.DisplayName, firebaseUser.Email);
            errorLabel.text = "";

            StartCoroutine(SceneLoader.LoadSceneAsync(Scenes.Main));
            SetUserDataWithFirebase();

        }
    }

    static public IEnumerator UpdateProfile(string _username, Uri path)
    {
        if (_username == "")
        {
            Debug.LogError("Missing Username");
        } else 
        {
            if (firebaseUser != null)
            {
                //Create a firebaseUser profile and set the username
                UserProfile profile = new UserProfile { DisplayName = _username, PhotoUrl = path };

                //Call the Firebase auth update firebaseUser profile function passing the profile with the username
                Task ProfileTask = firebaseUser.UpdateUserProfileAsync(profile);

                yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                if (ProfileTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                    FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    Debug.LogError("Username Set Failed!");
                }
                else
                {
                    // Username is now set;
                    User.Instance.SetUsername(firebaseUser.DisplayName);
                }
            }
        }
    }

    public IEnumerator Register(string _email, string _password, string _username)
    {
            //Call the Firebase auth signin function passing the email and password=
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
                errorLabel.text = message;
            }
            else
            {
            //firebaseUser has now been created
            //Now get the result
            UpdateProfile(_username, null);
            firebaseUser = RegisterTask.Result.User;
            errorLabel.text = "";
            yield return GetComponent<Auth>().CreateUser();
            DBreference.Child("users").Child(firebaseUser.UserId).ValueChanged += HandleDatabaseValueChanged;
            yield return StartCoroutine(SceneLoader.LoadSceneAsync(Scenes.Main));
            SetUserDataWithFirebase();
        }
    }

    public async Task RegisterCheckError(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password=
        await auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWith((RegisterTask) =>
        {
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
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;

                }
                throw new Exception(message);
                
            }
            else
            {
                if (RegisterTask.Result.User != null)
                {
                    RegisterTask.Result.User.DeleteAsync().ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.LogError("DeleteAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                            return;
                        }

                        Debug.Log("User deleted successfully.");
                    });
                }
            }
        });
    }

    static public IEnumerator UpdateUserDatabaseData()
    {
        string json = JsonUtility.ToJson(User.Instance);
        Debug.Log("User JSON: " + json);

        Task userDataTask = DBreference.Child("users").Child(firebaseUser.UserId).SetRawJsonValueAsync(json);
        yield return new WaitUntil(predicate: () => userDataTask.IsCompleted);
        if (userDataTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {userDataTask.Exception}");
        } else
        {
            // user data has now been updated
        }
    }

    static public IEnumerator UpdateUserValue(string nameValue, object value)
    {
        if (value is Enum)
            value = (int)value;

        Task userValueTask = DBreference.Child("users").Child(firebaseUser.UserId).Child(nameValue).SetValueAsync(value);

        yield return new WaitUntil(predicate: () =>  userValueTask.IsCompleted);

        if (userValueTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {userValueTask.Exception}");
        }
        else
        {
            // user value has now been updated
            //onCallback.Invoke();
        }
    }

}


