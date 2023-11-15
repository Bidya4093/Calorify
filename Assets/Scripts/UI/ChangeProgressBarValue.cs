using System.Collections;
using System.Collections.Generic;
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

    public void Update()
    {
        float percentage;
        percentage = (float)User.CaloriesEaten / (float)User.CaloriesNeeded;

        caloriesProgressBar.fillAmount = percentage;
        caloriesScanProgressBar.fillAmount = percentage;
        caloriesPercent.text = $"{Mathf.RoundToInt(percentage * 100f)}%";
        caloriesScanPercent.text = $"{Mathf.RoundToInt(percentage * 100f)}%";
        caloriesText.text = $"{User.CaloriesEaten}/{User.CaloriesNeeded} ккал.";

        protsProgressBar.fillAmount = (float)User.ProtsEaten / (float)User.ProtsNeeded;
        fatsProgressBar.fillAmount = (float) (User.FatsEaten / (float)User.FatsNeeded);
        carbsProgressBar.fillAmount = (float)(User.CarbsEaten / (float)User.CarbsNeeded);
    }

    public void AddProductData(int calories, int carbs, int fats, int prots)
    {
        User.ProtsEaten += prots;
        User.CaloriesEaten += calories;
        User.CarbsEaten += carbs;
        User.FatsEaten += fats;
    }
}
