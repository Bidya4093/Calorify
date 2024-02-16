
using Firebase.Database;
using System;

public class products
{
    public int product_id;
    public string name ;
    public string type ;
    public string nutri_score;
    public float calories ;
    public float prots ;
    public float fats ;
    public float carbs ;
    public string vuforia_id ;

    public products()
    {

    }

    public products(int product_id, string name, string type, string nutri_score, float calories, float prots, float fats, float carbs, string vuforia_id)
    {
        this.product_id = product_id;
        this.name = name;
        this.type = type;
        this.nutri_score = nutri_score;
        this.calories = calories;
        this.prots = prots;
        this.fats = fats;
        this.carbs = carbs;
        this.vuforia_id = vuforia_id;
    }

    public products(DataSnapshot snapshot)
    {
        product_id = Convert.ToInt32(snapshot.Child("product_id").Value);
        name = snapshot.Child("name").Value.ToString();
        type = snapshot.Child("type").Value.ToString();
        nutri_score = snapshot.Child("nutri_score").Value.ToString();
        calories = Convert.ToSingle(snapshot.Child("calories").Value);
        prots = Convert.ToSingle(snapshot.Child("prots").Value);
        fats = Convert.ToSingle(snapshot.Child("fats").Value);
        carbs = Convert.ToSingle(snapshot.Child("carbs").Value);
        vuforia_id = snapshot.Child("vuforia_id").Value.ToString();
    }

    public override string ToString()
    {
        return $"{product_id}, {name}, {type}, {nutri_score}, {calories}, {prots}, {fats}, {carbs}, {vuforia_id};";
    }
}
