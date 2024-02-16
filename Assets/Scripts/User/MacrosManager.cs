using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;

public class MacrosInfo
{
    public int calories;
    public int carbs;
    public int fats;
    public int prots;
}

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
    }

    static public void CalculateUserNeeds()
    {
        int calories = 0;
        float activityCoefficient = 1.2f;
        float goalCoefficient = 1f;
        SexType sex = User.Instance.GetSex();
        ActivityType activity = User.Instance.GetActivity();
        float weight = User.Instance.GetWeight();
        float height = User.Instance.GetHeight();
        int age = User.Instance.GetAge();
        GoalType goal = User.Instance.GetGoal();

        // BMR - basal metabolic rate
        if (sex == SexType.Female)
        {
            calories = (int)((10 * weight) + (6.25 * height) - (5 * age)) - 161;
        } else if (sex == SexType.Male)
        {
            calories = (int)((10 * weight) + (6.25 * height) - (5 * age) + 5);
        }

        if (activity == ActivityType.None) activityCoefficient = 1.2f;
        else if (activity == ActivityType.Regular) activityCoefficient = 1.375f;
        else if (activity == ActivityType.Often) activityCoefficient = 1.55f;

        if (goal == GoalType.LoseWeight) goalCoefficient = 0.9f;
        else if (goal == GoalType.PutOnWeight) goalCoefficient = 1.2f;
        
        // AMR - active metabolic rate
        caloriesNeeded = (int)(calories * activityCoefficient * goalCoefficient);



        carbsNeeded = (int)(0.5f * caloriesNeeded / 4);
        fatsNeeded = (int)(0.25f * caloriesNeeded / 4);
        protsNeeded = (int)(0.3f * caloriesNeeded / 9);

        User.Instance.CaloriesNeeded = caloriesNeeded;
        User.Instance.CarbsNeeded = carbsNeeded;
        User.Instance.FatsNeeded = fatsNeeded;
        User.Instance.ProtsNeeded = protsNeeded;
    }
    
    static public async Task<MacrosInfo> CalculateMacrosByMass(int mass, products product)
    {
        MacrosInfo macrosInfo = new MacrosInfo();

        //products product = await new ProductsLoader().GetById(productId);
        macrosInfo.calories = (int)(product.calories * (mass / 100f));
        macrosInfo.prots = (int)(product.prots * (mass / 100f));
        macrosInfo.fats = (int)(product.fats * (mass / 100f));
        macrosInfo.carbs = (int)(product.carbs * (mass / 100f));
        return macrosInfo;
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
