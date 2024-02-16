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

    //public Task<products> GetByVuforiaId(string vuforia_id)
    //{
    //    products product = null;
    //    Task<DataSnapshot> DBTask = DBreference.Child("products").OrderByChild("vuforia_id").EqualTo(vuforia_id).GetValueAsync();

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning($"Error occurred while querying the database with  {DBTask.Exception}.");
    //    }
    //    else
    //    {
    //        DataSnapshot snapshot = DBTask.Result;
    //        if (snapshot.Exists)
    //        {
    //            // Loop through the snapshot to access the found items.
    //            foreach (var childSnapshot in snapshot.Children)
    //            {
    //                // Access the data of the found item.
    //                Debug.Log(childSnapshot.Key + ": " + childSnapshot.GetRawJsonValue());
    //                Debug.Log(new products(childSnapshot));
    //                product = new products(childSnapshot);
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("No item found with the specified value.");
    //        }
    //    }
    //    return product;
    //}

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


    private void ToList(IEnumerable<products> products)
    {
        foreach (var product in products)
        {
            dishes.Add(product);
        }
    }
}
