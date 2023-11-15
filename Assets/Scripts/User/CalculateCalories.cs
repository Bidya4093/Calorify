using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCalories : MonoBehaviour
{
    void Start()
    {
        int calories, fats, carbs, prots;
        float activityCoefficient;

        if (User.GetActivity() == 0) activityCoefficient = 1f;
        else if (User.GetActivity() == 1) activityCoefficient = 1.3f;
        else if (User.GetActivity() == 2) activityCoefficient = 1.5f;
        else activityCoefficient = 1f;

        calories = (int)((0.063 * (float)User.GetWeight() + 2.896) * 240 * activityCoefficient);
        fats = (int)(calories * 0.25);
        carbs = (int)(calories * 0.55);
        prots = (int)(calories * 0.20);

        User.CaloriesNeeded = calories;
        User.FatsNeeded = fats;
        User.CarbsNeeded = carbs;
        User.ProtsNeeded = prots;

        Debug.Log($"Carbs: {carbs}");
    }
}
