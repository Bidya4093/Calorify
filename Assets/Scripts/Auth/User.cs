using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

enum GoalType
{
    LoseWeight,
    PutOnWeight,
    KeepFit
}

enum ActivityType
{
    None,
    Regular,
    Often
}

public class User : MonoBehaviour
{
    // Дані користувача
    static private string username;
    static private string email;
    static private string password;
    static private short goal = (short)GoalType.KeepFit;
    static private short activity = (short)ActivityType.Regular;
    static private float height = 0;
    static private float weight = 0;

    static private int caloriesNeeded = 0;
    static private int caloriesEaten = 0;
    static private int carbsNeeded = 0;
    static private int carbsEaten = 0;
    static private int fatsNeeded = 0;
    static private int fatsEaten = 0;
    static private int protsNeeded = 0;
    static private int protsEaten = 0;

    static public int CarbsNeeded
    {
        get { return carbsNeeded; }
        set { carbsNeeded = value; }
    }
    static public int CarbsEaten
    {
        get { return carbsEaten; }
        set { carbsEaten = value; }
    }
    static public int FatsNeeded
    {
        get { return fatsNeeded; }
        set { fatsNeeded = value; }
    }
    static public int FatsEaten
    {
        get { return fatsEaten; }
        set { fatsEaten = value; }
    }
    static public int ProtsNeeded
    {
        get { return protsNeeded; }
        set { protsNeeded = value; }
    }
    static public int ProtsEaten
    {
        get { return protsEaten; }
        set { protsEaten = value; }
    }
    static public int CaloriesNeeded
    {
        get { return caloriesNeeded; }
        set { caloriesNeeded = value; }
    }
    static public int CaloriesEaten
    {
        get { return caloriesEaten; }
        set { caloriesEaten = value; }
    }

    static public void SetAll(string _username, string _email, string _password, short _goal, short _activity, float _height, float _weight)
    {
        SetUsername(_username);
        SetEmail(_email);
        SetPassword(_password);
        SetGoal(_goal);
        SetActivity(_activity);
        SetHeight(_height);
        SetWeight(_weight);
    }

    static public void SetUsername(string _username) {
        if (_username.Length == 0)
        {
            throw new Exception("Username field empty");
        }
        username = _username;
    }

    static public void SetEmail(string _email) {
        if (_email.Length == 0) { 
            throw new Exception("Email field empty");
        }
        if (!_email.Contains("@gmail.com") || !(_email.Length > 10))
        {
            throw new Exception("Incorrect email");
        }
        email = _email;
    }

    static public void SetPassword(string _password) {

        if (_password.Length == 0)
        {
            throw new Exception("Password field empty");
        }
        if (!(_password.Length > 6))
        {
            throw new Exception("Password must be longer than 6 characters");
        }
        password = _password;
        // додати більше перевірок(цифри, великі і маленькі букви)
        // додати шкалу оцінки паролю
    }

    static public void SetGoal(short _goal) {
        if (_goal < 0 && _goal >= Enum.GetNames(typeof(GoalType)).Length)
        {
            throw new Exception("GoalType doesn't exist");
        }
        goal = _goal;
    }

    static public void SetActivity(short _activity) {
        if (_activity < 0 && _activity >= Enum.GetNames(typeof(ActivityType)).Length)
        {
            throw new Exception("ActivityType doesn't exist");
        }
        activity = _activity;
    }

    static public void SetHeight(float _height) {
        if (_height <= 0)
        {
            throw new Exception("Height must be greater than 0!");
        } else if (_height > 300)
        {
            throw new Exception("Height must be less than 300!");
        }
        height = _height;
    }

    static public void SetHeight(string _height)
    {
        if (string.IsNullOrEmpty(_height))
        {
            throw new Exception("Height field empty");
        }

        SetHeight(float.Parse(_height));
    }

    static public void SetWeight(float _weight) {
        if (weight == _weight) return;
        if (_weight <= 0)
        {
            throw new Exception("Weight must be greater than 0!");
        }
        else if (_weight > 700)
        {
            throw new Exception("Weight must be less than 700!");
        }
        weight = _weight;
    }

    static public void SetWeight(string _weight)
    {
        if (string.IsNullOrEmpty(_weight))
        {
            throw new Exception("Weight field empty");
        }

        SetWeight(float.Parse(_weight));
    }

    static public string GetUsername() { return username; }
    static public string GetEmail() { return email; }
    static public string GetPassword() { return password; }
    static public short GetGoal() { return goal; }
    static public short GetActivity() { return activity; }
    static public float GetHeight() { return height; }
    static public float GetWeight() { return weight; }
}
