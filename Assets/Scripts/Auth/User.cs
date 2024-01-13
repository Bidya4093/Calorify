using System;
using System.Dynamic;
using UnityEngine;

public enum GoalType
{
    LoseWeight,
    PutOnWeight,
    KeepFit
}

public enum ActivityType
{
    None,
    Regular,
    Often
}

public enum SexType
{
    Male,
    Female
}

public class User : MonoBehaviour
{
    // Дані користувача
    public string username;
    public string email;
    public GoalType goal = GoalType.KeepFit;
    public ActivityType activity = ActivityType.Regular;
    public float height = 0;
    public float weight = 0;
    public SexType sex = SexType.Female;
    public int age = 0;

    public int caloriesNeeded = 0;
    public int caloriesEaten = 0;
    public int carbsNeeded = 0;
    public int carbsEaten = 0;
    public int fatsNeeded = 0;
    public int fatsEaten = 0;
    public int protsNeeded = 0;
    public int protsEaten = 0;

    static public User Instance;

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public User(string _username = "", string _email = "", SexType _sex = SexType.Female, int _age = 0, float _height = 0, float _weight = 0, GoalType _goal = GoalType.KeepFit, ActivityType _activity = ActivityType.Regular)
    {
        username = _username;
        email = _email;
        sex = _sex;
        age = _age;
        height = _height;
        weight = _weight;
        goal = _goal;
        activity = _activity;
    }

    public int CarbsNeeded
    {
        get { return carbsNeeded; }
        set { carbsNeeded = value; }
    }
    public int CarbsEaten
    {
        get { return carbsEaten; }
        set { carbsEaten = value; }
    }
    public int FatsNeeded
    {
        get { return fatsNeeded; }
        set { fatsNeeded = value; }
    }
    public int FatsEaten
    {
        get { return fatsEaten; }
        set { fatsEaten = value; }
    }
    public int ProtsNeeded
    {
        get { return protsNeeded; }
        set { protsNeeded = value; }
    }
    public int ProtsEaten
    {
        get { return protsEaten; }
        set { protsEaten = value; }
    }
    public int CaloriesNeeded
    {
        get { return caloriesNeeded; }
        set { caloriesNeeded = value; }
    }
    public int CaloriesEaten
    {
        get { return caloriesEaten; }
        set { caloriesEaten = value; }
    }

    public void SetAll(string _username, string _email, GoalType _goal, ActivityType _activity, float _height, float _weight, SexType _sex, int _age)
    {
        SetUsername(_username);
        SetEmail(_email);
        SetGoal(_goal);
        SetActivity(_activity);
        SetHeight(_height);
        SetWeight(_weight);
        SetSex(_sex);
        SetAge(_age);
    }

    public void SetSex(SexType _sex)
    {
        sex = _sex;
    }

    public void SetAge(int _age)
    {
        if (_age <= 0)
        {
            throw new Exception("Age must be greater than 0!");
        }
        else if (_age > 150)
        {
            throw new Exception("Age must be less than 150!");
        }
        if (age == _age) return;

        age = _age;
    }

    public void SetUsername(string _username) {
        if (_username.Length == 0)
        {
            throw new Exception("Username empty");
        }
        username = _username;
    }

    public void SetEmail(string _email) {
        if (_email.Length == 0) { 
            throw new Exception("Email empty");
        }
        if (!_email.Contains("@gmail.com") || !(_email.Length > 10))
        {
            throw new Exception("Incorrect email");
        }
        email = _email;
    }

    public void SetPassword(string _password) {

        if (_password.Length == 0)
        {
            throw new Exception("Password empty");
        }
        if (!(_password.Length > 6))
        {
            throw new Exception("Password must be longer than 6 characters");
        }
        // додати більше перевірок(цифри, великі і маленькі букви)
        // додати шкалу оцінки паролю
    }

    public void SetGoal(GoalType _goal)
    {
        goal = _goal;
    }

    public void SetActivity(ActivityType _activity)
    {
        activity = _activity;
    }

    public void SetHeight(float _height) {
        if (_height <= 0)
        {
            throw new Exception("Height must be greater than 0!");
        } else if (_height > 300)
        {
            throw new Exception("Height must be less than 300!");
        }
        if (height == _height) return;

        height = _height;
    }

    //public void SetHeight(string _height)
    //{
    //    if (string.IsNullOrEmpty(_height))
    //    {
    //        throw new Exception("Height empty");
    //    }

    //    SetHeight(float.Parse(_height));
    //}

    public void SetWeight(float _weight) {
        if (_weight <= 0)
        {
            throw new Exception("Weight must be greater than 0!");
        }
        else if (_weight > 700)
        {
            throw new Exception("Weight must be less than 700!");
        }
        if (weight == _weight) return;

        weight = _weight;
    }

    //public void SetWeight(string _weight)
    //{
    //    if (string.IsNullOrEmpty(_weight))
    //    {
    //        throw new Exception("Weight empty");
    //    }

    //    SetWeight(float.Parse(_weight));
    //}

    public string GetUsername() { return username; }
    public string GetEmail() { return email; }
    public GoalType GetGoal() { return goal; }
    public ActivityType GetActivity() { return activity; }
    public float GetHeight() { return height; }
    public float GetWeight() { return weight; }
    public SexType GetSex() { return sex; }
    public int GetAge() { return age; }


    public void Show()
    {
        Debug.Log($"User: {username} {email}. Goal {goal} Activity {activity} " +
            $" Height {height} Weight {weight}");
    }
}
