using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    int calories, carbs, prots, fats;
    string name;

    public int Calories { get { return calories; } }
    public int Carbs { get { return carbs; } }
    public int Prots { get { return prots; } }
    public int Fats { get { return fats; } }
    public string Name { get { return name; } }

    public Item(int _calories, int _carbs, int _prots, int _fats, string _name)
    {
        this.calories = _calories;
        this.carbs = _carbs;
        this.prots = _prots;
        this.fats = _fats;
        this.name = _name;
    }
}
