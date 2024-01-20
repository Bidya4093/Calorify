using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MacrosManager : MonoBehaviour
{
    static public int caloriesNeeded = 0;
    static public int fatsNeeded = 0;
    static public int carbsNeeded = 0;
    static public int protsNeeded = 0;

    static public int caloriesEaten = 0;
    static public int fatsEaten = 0;
    static public int carbsEaten = 0;
    static public int protsEaten = 0;

    public FirebaseUser user;
    private DatabaseReference DBreference;

    private void Start()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(LoadUserMacrosData());
    }


    static public void CalculateUserNeeds()
    {
        int calories = 0;
        double activityCoefficient = 1.2f;
        SexType sex = User.Instance.GetSex();
        ActivityType activity = User.Instance.GetActivity();
        float weight = User.Instance.GetWeight();
        float height = User.Instance.GetHeight();
        int age = User.Instance.GetAge();


        // BMR - basal metabolic rate
        if (sex == SexType.Female)
        {
            calories = (int)(655.1 + (9.563 * weight) + (1.85 * height) - (4.676 * age));
        } else if (sex == SexType.Male)
        {
            calories = (int)(66.47 + (13.75 * weight) + (5.003 * height) - (6.755 * age));
        }

        if (activity == ActivityType.None) activityCoefficient = 1.2f;
        else if (activity == ActivityType.Regular) activityCoefficient = 1.375f;
        else if (activity == ActivityType.Often) activityCoefficient = 1.55f;

        // AMR - active metabolic rate
        caloriesNeeded = (int)(calories * activityCoefficient);

        carbsNeeded = (int)(0.5f * calories / 4);
        fatsNeeded = (int)(0.25f * calories / 4);
        protsNeeded = (int)(0.3f * calories / 9);

        User.Instance.CaloriesNeeded = caloriesNeeded;
        User.Instance.CarbsNeeded = carbsNeeded;
        User.Instance.FatsNeeded = fatsNeeded;
        User.Instance.ProtsNeeded = protsNeeded;
    }

    public IEnumerator LoadUserMacrosData()
    {
        Task<DataSnapshot> DBTask = DBreference.Child("users").Child(user.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet

        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            caloriesEaten = Convert.ToInt32(snapshot.Child("caloriesEaten").Value);
            caloriesNeeded = Convert.ToInt32(snapshot.Child("caloriesNeeded").Value);
            protsEaten = Convert.ToInt32(snapshot.Child("protsEaten").Value);
            protsNeeded = Convert.ToInt32(snapshot.Child("protsNeeded").Value);
            fatsEaten = Convert.ToInt32(snapshot.Child("fatsEaten").Value);
            fatsNeeded = Convert.ToInt32(snapshot.Child("fatsNeeded").Value);
            carbsEaten = Convert.ToInt32(snapshot.Child("carbsEaten").Value);
            carbsNeeded = Convert.ToInt32(snapshot.Child("carbsNeeded").Value);

        }
    }
}

/*
 * 
 * 
 * 
 * Step 1: Calculate Your BMR (basal metabolic rate)
 * For women, BMR = 655.1 + (9.563 x weight in kg) + (1.850 x height in cm) - (4.676 x age in years)
 * For men, BMR = 66.47 + (13.75 x weight in kg) + (5.003 x height in cm) - (6.755 x age in years)
 * 
 * Step 2: Calculate Your AMR (active metabolic rate)
 * Sedentary (little or no exercise): AMR = BMR x 1.2
 * Lightly active (exercise 1–3 days/week): AMR = BMR x 1.375
 * Moderately active (exercise 3–5 days/week): AMR = BMR x 1.55
 * 
 * 
 * Active (exercise 6–7 days/week): AMR = BMR x 1.725
 * Very active (hard exercise 6–7 days/week): AMR = BMR x 1.9
 * 
 * 
 * Carbohydrates: 45%-65% of total calories
 * Protein: 10%-35% of total calories
 * Fat: 20%-35% of total calories
 * 
 * 1 gram of carbohydrate = 4 calories
 * 1 gram of protein = 4 calories
 * 1 gram of fat = 9 calories
 * 
 * Carbohydrate: 45% x 1,500 calories / 4 calories = 168.75 grams per day
 * Protein: 25% x 1,500 calories / 4 calories =  93.75 grams per day
 * Fat: 30% x 1,500 calories / 9 calories = 50 grams per day
 * 
*/
