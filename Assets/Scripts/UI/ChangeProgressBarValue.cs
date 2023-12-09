using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeProgressBarValue : MonoBehaviour
{
    public Image caloriesProgressBar;
    public Image caloriesScanProgressBar;
    public TextMeshProUGUI caloriesText;
    public TextMeshProUGUI caloriesPercent;
    public TextMeshProUGUI caloriesScanPercent;
    public Image protsProgressBar;
    public Image fatsProgressBar;
    public Image carbsProgressBar;

    public FirebaseUser User;
    private DatabaseReference DBreference;

    void Start()
    {
        User = FirebaseManager.firebaseUser;
        DBreference = FirebaseManager.DBreference;
        //StartCoroutine(UpdateCircleBars());
    }


    public void AddProductData(int calories, int carbs, int fats, int prots)
    {
        MacrosManager.protsEaten += prots;
        MacrosManager.caloriesEaten += calories;
        MacrosManager.carbsEaten += carbs;
        MacrosManager.fatsEaten += fats;

        User = FirebaseManager.firebaseUser;
        DBreference = FirebaseManager.DBreference;

        DBreference.Child("users").Child(User.UserId).Child("protsEaten").SetValueAsync(MacrosManager.protsEaten);
        DBreference.Child("users").Child(User.UserId).Child("caloriesEaten").SetValueAsync(MacrosManager.caloriesEaten);
        DBreference.Child("users").Child(User.UserId).Child("carbsEaten").SetValueAsync(MacrosManager.carbsEaten);
        DBreference.Child("users").Child(User.UserId).Child("fatsEaten").SetValueAsync(MacrosManager.fatsEaten);
    }

    void Update()
    {
        float percentage = MacrosManager.caloriesEaten / (float)MacrosManager.caloriesNeeded;
        //Debug.Log($"{percentage}");
        //Debug.Log($"{MacrosManager.caloriesEaten} {MacrosManager.caloriesNeeded} {MacrosManager.protsEaten} {MacrosManager.protsNeeded}");
        caloriesProgressBar.fillAmount = percentage;
        caloriesScanProgressBar.fillAmount = percentage;
        caloriesPercent.text = $"{Mathf.RoundToInt(percentage * 100f)}%";
        caloriesScanPercent.text = $"{Mathf.RoundToInt(percentage * 100f)}%";
        caloriesText.text = $"{MacrosManager.caloriesEaten}/{MacrosManager.caloriesNeeded} ккал.";
        protsProgressBar.fillAmount = (float)MacrosManager.protsEaten / (float)MacrosManager.protsNeeded;
        fatsProgressBar.fillAmount = (float)MacrosManager.fatsEaten / (float)MacrosManager.fatsNeeded;
        carbsProgressBar.fillAmount = (float)MacrosManager.carbsEaten / (float)MacrosManager.carbsNeeded;
    }
}
