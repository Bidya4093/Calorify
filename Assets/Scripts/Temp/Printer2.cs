using Firebase.Auth;
using Unity.VisualScripting;
using UnityEngine;

public class Printer2 : MonoBehaviour
{
    private void Start()
    {
        ProductsLoader productsLoader = new ProductsLoader();

        foreach (products product in productsLoader.dishes)
        {
            Debug.Log(product);
        }
    }
}
