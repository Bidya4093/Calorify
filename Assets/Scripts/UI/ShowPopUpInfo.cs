using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            ProductsLoader productsLoader = new ProductsLoader();
            products item = productsLoader.GetById(Int32.Parse(index.text));

            if (inputMass.text.Length == 0) mass = 100;
            else mass = Int32.Parse(inputMass.text);

            calculatedCals = (int)(item.calories * (mass / 100f));
            calculatedProts = (int)(item.prots * (mass / 100f));
            calculatedFats = (int)(item.fats * (mass / 100f));
            calculatedCarbs = (int)(item.carbs * (mass / 100f));

            itemName.text = item.name;
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


        float percentage = (float)MacrosManager.caloriesEaten / (float)MacrosManager.caloriesNeeded;

        caloriesScanProgressBar.fillAmount = percentage;
        caloriesScanPercent.text = $"{Mathf.RoundToInt(percentage * 100f)}%";

    }

}
