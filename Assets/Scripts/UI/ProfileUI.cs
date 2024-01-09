using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using Firebase;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ProfileUI : MonoBehaviour
{

    public TMP_InputField profileNameInput;
    public TMP_InputField profileHeightInput;
    public TMP_InputField profileWeightInput;
    public Toggle[] profileGoalToggles;
    public ToggleGroup profileGoalToggleGroup;
    public FirebaseUser User;
    private DatabaseReference DBreference;

    private void Start()
    {
        User = FirebaseManager.firebaseUser;
        DBreference = FirebaseManager.DBreference;
        //StartCoroutine(LoginTest("tester4@gmail.com", "testpassword"));
        StartCoroutine(LoadUserData());
    }

    public IEnumerator LoadUserData()
    {
        Task<DataSnapshot> DBTask = FirebaseManager.DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        Debug.Log(DBTask.Result.GetRawJsonValue());

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            profileNameInput.text = "None";
            profileHeightInput.text = "0";
            profileWeightInput.text = "0";
            profileGoalToggles[(int)GoalType.KeepFit].isOn = true;
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            
            profileNameInput.text = snapshot.Child("username").Value.ToString();
            profileHeightInput.text = snapshot.Child("height").Value.ToString();
            profileWeightInput.text = snapshot.Child("weight").Value.ToString();
            profileGoalToggles[Convert.ToInt32(snapshot.Child("goal").Value)].isOn = true;
        }
    }

    public IEnumerator LoginTest(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = FirebaseManager.auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
        }
        else
        {
            //firebaseUser is now logged in
            //Now get the result
            User = LoginTask.Result.User;

            StartCoroutine(LoadUserData());

        }
    }

    public void OnEndEditUsername()
    {
        StartCoroutine(UpdateUsernameAuth());
        StartCoroutine(UpdateUsernameDatabase());
    }
    public void OnEndEditHeight()
    {
        StartCoroutine(UpdateHeight());
    }
    public void OnEndEditWeight()
    {
        StartCoroutine(UpdateWeight());
    }
    public void OnEndEditGoal()
    {
        StartCoroutine(UpdateGoal());
    }


    private IEnumerator UpdateUsernameAuth()
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = profileNameInput.text };

        //Call the Firebase auth update user profile function passing the profile with the username
        Task ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }
    }

    private IEnumerator UpdateUsernameDatabase()
    {
        //Set the currently logged in user username in the database
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(profileNameInput.text);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }

    public IEnumerator UpdateHeight()
    {
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("height").SetValueAsync(profileHeightInput.text);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Height are now updated
        }
    }

    public IEnumerator UpdateWeight()
    {
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("weight").SetValueAsync(profileWeightInput.text);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Height are now updated
        }
    }

    public IEnumerator UpdateGoal()
    {
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("goal").SetValueAsync(profileWeightInput.text);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Height are now updated
        }
    }
}