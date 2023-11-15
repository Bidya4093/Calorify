using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPopUpInfo : MonoBehaviour
{
    public TextMeshProUGUI index;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI calories;
    public TextMeshProUGUI prots;
    public TextMeshProUGUI carbs;
    public TextMeshProUGUI fats;

    public Image caloriesScanProgressBar;
    public TextMeshProUGUI caloriesScanPercent;


    public TMP_InputField inputMass;

    int calculatedCals, calculatedProts, calculatedFats, calculatedCarbs, mass;

    private void FixedUpdate()
    {
        if (index.text.Length > 0)
        {
            Item item = FoodStatic.GetItem(Int32.Parse(index.text));

            if (inputMass.text.Length == 0) mass = 100;
            else mass = Int32.Parse(inputMass.text);

            calculatedCals = (int)(item.Calories * (mass / 100f));
            calculatedProts = (int)(item.Prots * (mass / 100f));
            calculatedFats = (int)(item.Fats * (mass / 100f));
            calculatedCarbs = (int)(item.Carbs * (mass / 100f));

            itemName.text = item.Name;
            calories.text = $"{calculatedCals.ToString()}";
            prots.text = $"{calculatedProts.ToString()}";
            carbs.text = $"{calculatedCarbs.ToString()}";
            fats.text = $"{calculatedFats.ToString()}";

        }
    }

    public void AddToRatio()
    {
        ChangeProgressBarValue progressBar = new ChangeProgressBarValue();
        progressBar.AddProductData(calculatedCals, calculatedCarbs, calculatedFats, calculatedProts);

        float percentage = (float)User.CaloriesEaten / (float)User.CaloriesNeeded;

        caloriesScanProgressBar.fillAmount = percentage;
        caloriesScanPercent.text = $"{Mathf.RoundToInt(percentage * 100f)}%";

    }
}
