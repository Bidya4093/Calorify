using System.Collections.Generic;

public class ProductsLoader
{
<<<<<<< Updated upstream
    public List<products> dishes;

    public ProductsLoader() 
    {
        var dataService = new DataService("productsdb.db", true);
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

=======
    public DatabaseReference DBreference;

    public ProductsLoader() 
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
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


>>>>>>> Stashed changes
    // returns dish by its id. If not found - returns null
    public products GetById(int id)
    {
        foreach (products dish in dishes)
        {
            if (dish.product_id == id) return dish;
        }
        return null;
    }

    public products GetByVuforiaId(string vuforia_id)
    {
        foreach (products dish in dishes)
        {
            if (dish.vuforia_id == vuforia_id) return dish;
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
