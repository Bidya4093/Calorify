using System.Collections.Generic;
using UnityEngine;

public class ProductsLoader
{
    public List<products> dishes;

    public ProductsLoader() 
    {
        var dataService = new DataService("productsdb.db");
        var products = dataService.GetConnection().Table<products>();
        dishes = new List<products>();
        ToList(products);
    }

    // returns list of dishes that include given substring in its name
    public List<products> IncludeSubstring(string substr) 
    {
        List<products> returnList = new List<products>();
        foreach(products dish in dishes)
        {
            if(dish.name.ToLower().Contains(substr.ToLower())) returnList.Add(dish);
        }
        return returnList;
    }

    // returns dish by its id. If not found - returns null
    public products GetById(int id)
    {
        foreach (products dish in dishes)
        {
            if (dish.product_id == id) return dish;
        }
        return null;
    }

    private void ToList(IEnumerable<products> products)
    {
        foreach (var product in products)
        {
            dishes.Add(product);
        }
    }
}
