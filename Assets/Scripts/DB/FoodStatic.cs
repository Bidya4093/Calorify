using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStatic
{
    static List<Item> items = new List<Item>();

    public static void AddItem(Item item)
    {
        items.Add(item);
    }

    public static void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public static Item GetItem(int index)
    {
        return items[index];
    }
}
