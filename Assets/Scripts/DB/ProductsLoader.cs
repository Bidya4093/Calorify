using Firebase.Auth;
using Firebase;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class ProductsLoader
{
    public List<products> dishes;
    public DatabaseReference DBreference;

    public ProductsLoader() 
    {
        //var dataService = new DataService("productsdb.db", true);
        //var products = dataService.GetConnection().Table<products>();
        //dishes = new List<products>();
        //ToList(products);
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
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

    public async Task<List<products>> IncludeSubstringAsync(string substr)
    {
        List<products> returnList = new List<products>();

        // Отримуємо дані з Firebase
        DataSnapshot snapshot = await DBreference.Child("products").GetValueAsync();

        // Перевіряємо, чи є дані
        if (snapshot.Exists)
        {
            // Проходимося по всіх даних
            foreach (var childSnapshot in snapshot.Children)
            {
                // Отримуємо дані продукту
                var productData = childSnapshot.Value as Dictionary<string, object>;

                // Перевіряємо, чи ім'я продукту містить підстроку
                if (productData.ContainsKey("name") && productData["name"].ToString().ToLower().Contains(substr.ToLower()))
                {
                    // Створюємо об'єкт продукту і додаємо його до списку
                    products product = new products(childSnapshot);
                    returnList.Add(product);
                }
            }
        }

        return returnList;
    }


    // returns dish by its id. If not found - returns null
    public async Task<products> GetById(int id)
        {
        products product = null;
        try
        {
            DataSnapshot snapshot = await DBreference.Child("products").OrderByChild("product_id").EqualTo(id).GetValueAsync();

            if (snapshot.Exists)
            {
                // Loop through the snapshot to access the found items.
                foreach (var childSnapshot in snapshot.Children)
                {
                    // Access the data of the found item.
                    product = new products(childSnapshot);
                    break; // Assuming you only need the first product
                }
            }
            else
            {
                Debug.Log("No item found with the specified value.");
            }
        }
        catch (AggregateException ex)
        {
            Debug.LogWarning($"Error occurred while querying the database: {ex.InnerException}");
        }
        Debug.Log(product);
        return product;
    }

    public async Task<products> GetByVuforiaIdAsync(string vuforia_id)
    {
        products product = null;
        try
        {
            DataSnapshot snapshot = await DBreference.Child("products").OrderByChild("vuforia_id").EqualTo(vuforia_id).GetValueAsync();

            if (snapshot.Exists)
            {
                // Loop through the snapshot to access the found items.
                foreach (var childSnapshot in snapshot.Children)
                {
                    // Access the data of the found item.
                    Debug.Log(childSnapshot.Key + ": " + childSnapshot.GetRawJsonValue());
                    product = new products(childSnapshot);
                    break; // Assuming you only need the first product
                }
            }
            else
            {
                Debug.Log("No item found with the specified value.");
            }
        }
        catch (AggregateException ex)
        {
            Debug.LogWarning($"Error occurred while querying the database: {ex.InnerException}");
        }
        Debug.Log(product);
        return product;
    }
}
