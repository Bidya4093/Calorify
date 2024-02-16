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

    public Button verificationContinueBtn;
    public VisualElement verificationContainer;

    static public bool verificationEmail = false;
    static public float timeRemaining = 20f;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        signInEmailInput = root.Q<TextField>("SignInEmailInput");
        signInPasswordInput = root.Q<TextField>("SignInPasswordInput");
        signUpEmailInput = root.Q<TextField>("SignUpEmailInput");
        signUpPasswordInput = root.Q<TextField>("SignUpPasswordInput");
        verificationContinueBtn = root.Q<Button>("VerificationContinueBtn");
        verificationContainer = root.Q<VisualElement>("Verification");
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
                ProductHistoryList.Render();
                //Debug.Log(new ProductsLoader().GetByVuforiaIdAsync("28a212786884484e86e14250e23eeb1c"));

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
                Debug.Log("Signed out " + firebaseUser.Email + ", Id: " + firebaseUser.UserId);
            }
            firebaseUser = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + firebaseUser.Email + ", Id: " + firebaseUser.UserId);
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

            string message = "Вхід неможливо виконати!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Введіть електронну пошту!";
                    break;
                case AuthError.MissingPassword:
                    message = "Введіть пароль!";
                    break;
                case AuthError.WrongPassword:
                    message = "Неправильний пароль!";
                    break;
                case AuthError.InvalidEmail:
                    message = "Неправильна електронна пошта!";
                    break;
                case AuthError.UserNotFound:
                    message = "Такого користувача не існує!";
                    break;
            }
            Debug.LogWarning(message);
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

    static public async Task ReauthenticateAsync(string password, string email = null)
    {
        if (String.IsNullOrEmpty(email)) {
            email = firebaseUser?.Email;
        }
        // Get auth credentials from the user for re-authentication. The example below shows
        // email and password credentials but there are multiple possible providers,
        // such as GoogleAuthProvider or FacebookAuthProvider.
        Credential credential = EmailAuthProvider.GetCredential(email, password);

        if (firebaseUser != null)
        {
            await firebaseUser.ReauthenticateAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("ReauthenticateAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("ReauthenticateAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User reauthenticated successfully.");
            });
        }
    }

    static public bool IsPasswordsSame(string password, string repeatPassword)
    {
        return password.Equals(repeatPassword);
    }

    static public async Task UpdatePassword(string currentPassword, string newPassword)
    {

        await ReauthenticateAsync(currentPassword);
        if (firebaseUser != null)
        {
            await firebaseUser.UpdatePasswordAsync(newPassword).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdatePasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdatePasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Password updated successfully.");
            });
        }
    }

    static public async Task SendVerificationEmail(FirebaseUser user = null)
    {
        if (user == null) user = firebaseUser;
        if (user != null)
        {
            await user.SendEmailVerificationAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendEmailVerificationAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                    return;
                }
                verificationEmail = true;
                timeRemaining = 20f;
                Debug.Log("Email sent successfully.");
            });
        }
    }
    public IEnumerator Register(string _email, string _password, string _username)
    {
        //Call the Firebase auth signin function passing the email and password=

        Task<AuthResult> RegisterTask =  Task.Run(() => auth.CreateUserWithEmailAndPasswordAsync(_email, _password));

        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Реєстрацію неможливо виконати!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Введіть електронну пошту!";
                    break;
                case AuthError.MissingPassword:
                    message = "Введіть пароль!";
                    break;
                case AuthError.WeakPassword:
                    message = "Слабкий пароль!";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Електронна пошта вже використовується!";
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
        await auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWith(async (RegisterTask) =>
        {
            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                //RegisterTask.Exception.
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
                firebaseUser = RegisterTask.Result.User;
                if (firebaseUser != null)
                {
                    //await SendVerificationEmail(firebaseUser);
                    //verificationContainer.style.display = DisplayStyle.Flex;
                    //verificationContinueBtn.style.display = DisplayStyle.Flex;

                    //await CheckEmailVerification();
                    await DeleteUserAsync(RegisterTask.Result.User);
                    //OnApplicationQuit();
                }
            }
        });
    }

    static public async Task DeleteUserAsync(FirebaseUser user = null)
    {
        if (user == null) user = firebaseUser;
        await user.DeleteAsync().ContinueWith(task =>
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

    static public IEnumerator UpdateUserValue(string nameValue, object value, Action onCallback = null)
    {
        if (value is Enum)
            value = (int)value;

        Task userValueTask = DBreference.Child("users").Child(firebaseUser.UserId).Child(nameValue).SetValueAsync(value);

        yield return new WaitUntil(predicate: () =>  userValueTask.IsCompleted);

        if (userValueTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update value task with {userValueTask.Exception}");
        }
        else
        {
            // user value has now been updated
            onCallback?.Invoke();
        }
        
    }

    static public IEnumerator GetUserValue(string nameValue, Action<object> onCallback = null)
    {
        Task<DataSnapshot> userValueTask = DBreference.Child("users").Child(firebaseUser.UserId).Child(nameValue).GetValueAsync();

        yield return new WaitUntil(predicate: () => userValueTask.IsCompleted);

        if (userValueTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to get value task with {userValueTask.Exception}");
        }
        else
        {
            // user value has now been updated
            DataSnapshot snapshot = userValueTask.Result;


            onCallback?.Invoke(snapshot.Value);
        }
    }

    private async Task CheckEmailVerification()
    {
        // ЧОМУ ТИ НЕ ПРАЦЮЄШ?????????????????????????????????????????
        Debug.Log("Verification: " + firebaseUser.IsEmailVerified);

        if (!firebaseUser.IsEmailVerified)
        {
            await firebaseUser.ReloadAsync();
            await CheckEmailVerification();

        }
        else
        {
            Debug.Log("Stop 1");

            //verificationContinueBtn.SetEnabled(true);// <-- Воно не працює
            //verificationContinueBtn.style.display = DisplayStyle.Flex; // <-- Це не працює, але в іншому варіанті 
            //GetComponent<AuthPanelManager>().ToNextSignUpPage(); // <-- І воно теж
            Debug.Log("Stop 2"); // <-- А це навіть не виводиться
        }

    }
}


