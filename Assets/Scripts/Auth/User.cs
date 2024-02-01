using Firebase.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

public class StringEnum
{
    protected StringEnum(string value) { Value = value;  }
    public string Value { get; private set; }
    public override string ToString()
    {
        return Value;
    }

}

public class Language : StringEnum
{
    public static List<Language> Values = new List<Language>();
    private Language(string value) : base(value) { Values.Add(this); }
    public static Language Ukrainian { get { return new Language("Українська"); } }
    public static Language English { get { return new Language("Англійська"); } }
}

public class Theme : StringEnum
{
    public static List<Theme> Values = new List<Theme>();
    private Theme(string value) : base(value) { Values.Add(this); }
    public static Theme Light { get { return new Theme("Світла"); } }
    public static Theme Dark { get { return new Theme("Темна"); } }
}

public class MeasurementUnits : StringEnum
{
    public static List<MeasurementUnits> Values = new List<MeasurementUnits>(); 
    private MeasurementUnits(string value) : base(value) { Values.Add(this); }
    public static MeasurementUnits Metric { get { return new MeasurementUnits("Метри, кілограми"); } }
    public static MeasurementUnits US { get { return new MeasurementUnits("Фути, фунти"); } }
}

public class Goal : StringEnum
{
    public static List<Goal> Values = new List<Goal>();
    private Goal(string value) : base(value) { Values.Add(this); }
    public static Goal KeepFit { get { return new Goal("Підтримка"); } }
    public static Goal LoseWeight { get { return new Goal("Схуднення"); } }
    public static Goal PutOnWeight { get { return new Goal("Набір ваги"); } }
}


public class User : MonoBehaviour
{
    // Дані користувача
    [HideInInspector] public string username, email;
    [HideInInspector] public float height = 0, weight = 0;
    [HideInInspector] public int age = 0;

    [HideInInspector] public GoalType goal = GoalType.KeepFit;
    [HideInInspector] public ActivityType activity = ActivityType.Regular;
    [HideInInspector] public SexType sex = SexType.Female;
    [HideInInspector] public string language = Language.Ukrainian.ToString();
    [HideInInspector] public string theme = Theme.Light.ToString();
    [HideInInspector] public string measurementUnits = MeasurementUnits.Metric.ToString();

    [HideInInspector] public int caloriesNeeded = 0, caloriesEaten = 0, carbsNeeded = 0, carbsEaten = 0;
    [HideInInspector] public int fatsNeeded = 0, fatsEaten = 0, protsNeeded = 0, protsEaten = 0, waterNeeded = 0, waterDrunk = 0;
    
    static public User Instance;

    void Start()
    {
        CreateInstance();
    }

    public void CreateInstance()
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
        language = Language.Ukrainian.ToString();
        theme = Theme.Light.ToString();
        measurementUnits = MeasurementUnits.Metric.ToString();
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

    public int WaterNeeded
    {
        get { return waterNeeded; }
        set { waterNeeded = value; }
    }

    public int WaterDrunk
    {
        get { return waterDrunk; }
        set { waterDrunk = value; }
    }

    public void SetAll(string _username, string _email, GoalType _goal, ActivityType _activity, float _height, float _weight, SexType _sex, int _age, Language _language, Theme _theme, MeasurementUnits _measurementUnits)
    {
        SetUsername(_username);
        SetEmail(_email);
        SetGoal(_goal);
        SetActivity(_activity);
        SetLanguage(_language);
        SetTheme(_theme);
        SetMeasurementUnits(_measurementUnits);
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
    public void SetLanguage(Language _language)
    {
        language = _language.ToString();
    }

    private void SetLanguage(string _language)
    {
        language = _language;
    }
    public void SetTheme(Theme _theme)
    {
        theme = _theme.ToString();
    }

    private void SetTheme(string _theme)
    {
        theme = _theme;
    }
    public void SetMeasurementUnits(MeasurementUnits _measurementUnits)
    {
        measurementUnits = _measurementUnits.ToString();
    }

    private void SetMeasurementUnits(string _measurementUnits)
    {
        measurementUnits = _measurementUnits;
    }

    public void SetHeight(float _height) {
        if (_height < 50)
        {
            throw new Exception("Height must be greater than 50!");
        } else if (_height > 300)
        {
            throw new Exception("Height must be less than 300!");
        }
        if (height == _height) return;

        height = _height;
    }

    public void SetWeight(float _weight) {
        if (_weight < 0)
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

    public string GetUsername() { return username; }
    public string GetEmail() { return email; }
    public GoalType GetGoal() { return goal; }
    public ActivityType GetActivity() { return activity; }
    public float GetHeight() { return height; }
    public float GetWeight() { return weight; }
    public SexType GetSex() { return sex; }
    public int GetAge() { return age; }
    public string GetTheme() { return theme; }
    public string GetLanguage() { return language; }
    public string GetMeasurementUnits() { return measurementUnits; }


    public void Show()
    {
        Debug.Log("User: " + JsonUtility.ToJson(this));
    }

    static public void SetUserDataWithSnapshot(DataSnapshot snapshot)
    {
        Instance.SetUsername(snapshot.Child("username").Value.ToString());
        Instance.SetEmail(snapshot.Child("email").Value.ToString());
        Instance.SetGoal((GoalType)Convert.ToInt32(snapshot.Child("goal").Value));
        Instance.SetActivity((ActivityType)Convert.ToInt32(snapshot.Child("activity").Value));
        Instance.SetHeight(Convert.ToSingle(snapshot.Child("height").Value));
        Instance.SetWeight(Convert.ToSingle(snapshot.Child("weight").Value));
        Instance.SetSex((SexType)Convert.ToInt32(snapshot.Child("sex").Value));
        Instance.SetAge(Convert.ToInt32(snapshot.Child("age").Value));
        Instance.SetLanguage(snapshot.Child("language").Value.ToString());
        Instance.SetTheme(snapshot.Child("theme").Value.ToString());
        Instance.SetMeasurementUnits(snapshot.Child("measurementUnits").Value.ToString());

        Instance.caloriesEaten = Convert.ToInt32(snapshot.Child("caloriesEaten").Value);
        Instance.caloriesNeeded = Convert.ToInt32(snapshot.Child("caloriesNeeded").Value);
        Instance.protsEaten = Convert.ToInt32(snapshot.Child("protsEaten").Value);
        Instance.protsNeeded = Convert.ToInt32(snapshot.Child("protsNeeded").Value);
        Instance.fatsEaten = Convert.ToInt32(snapshot.Child("fatsEaten").Value);
        Instance.fatsNeeded = Convert.ToInt32(snapshot.Child("fatsNeeded").Value);
        Instance.carbsEaten = Convert.ToInt32(snapshot.Child("carbsEaten").Value);
        Instance.carbsNeeded = Convert.ToInt32(snapshot.Child("carbsNeeded").Value);
        Instance.waterDrunk = Convert.ToInt32(snapshot.Child("waterDrunk").Value);
        Instance.waterNeeded = Convert.ToInt32(snapshot.Child("waterNeeded").Value);
    }

    static public void AddToEaten(MacrosInfo macrosInfo)
    {
        Instance.caloriesEaten += macrosInfo.calories;
        Instance.protsEaten += macrosInfo.prots;
        Instance.fatsEaten += macrosInfo.fats;
        Instance.carbsEaten += macrosInfo.carbs;

        Instance.UpdateUserMacros();
    }

    static public void SubtractFromEatem(MacrosInfo macrosInfo)
    {
        Instance.caloriesEaten -= macrosInfo.calories;
        Instance.protsEaten -= macrosInfo.prots;
        Instance.fatsEaten -= macrosInfo.fats;
        Instance.carbsEaten -= macrosInfo.carbs;

        if (Instance.caloriesEaten < 0) Instance.caloriesEaten = 0;
        if (Instance.protsEaten < 0) Instance.protsEaten = 0;
        if (Instance.fatsEaten < 0) Instance.fatsEaten = 0;
        if (Instance.carbsEaten < 0) Instance.carbsEaten = 0;

        Instance.UpdateUserMacros();
    }

    public void UpdateUserMacros()
    {
        StartCoroutine(FirebaseManager.UpdateUserValue("caloriesEaten", Instance.CaloriesEaten));
        StartCoroutine(FirebaseManager.UpdateUserValue("protsEaten", Instance.ProtsEaten));
        StartCoroutine(FirebaseManager.UpdateUserValue("fatsEaten", Instance.FatsEaten));
        StartCoroutine(FirebaseManager.UpdateUserValue("carbsEaten", Instance.CarbsEaten));
    }

    static public void AddWater(int waterAmount)
    {
        Instance.WaterDrunk += waterAmount;
    }

    static public void SetWater(int waterAmount)
    {
        Instance.WaterDrunk = waterAmount;
    }
}
