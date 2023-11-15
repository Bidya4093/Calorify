using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAdder : MonoBehaviour
{
    void Start()
    {
        Item juice = new Item(42, 10, 1, 1, "Сік");
        Item crisps = new Item(520, 53, 6, 32, "Чіпси");
        Item straw = new Item(520, 53, 6, 32, "Соломка");
        Item mayo = new Item(616, 3, 1, 67, "Маїнез");

        FoodStatic.AddItem(juice);
        FoodStatic.AddItem(crisps);
        FoodStatic.AddItem(straw);
        FoodStatic.AddItem(mayo);
    }
}
