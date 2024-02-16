using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class ManualAddProductItem : ProductItem
{

    public ManualAddProductItem(products _product) : base(_product) { }

    public async override Task Init(string _name, int mass, int calories, string nutri_score)
    {
        await base.Init(_name, mass, calories, nutri_score);

        RegisterCallback<ClickEvent>(OpenProductPanel);
    }

    private void OpenProductPanel(ClickEvent evt)
    {
        ProductPanel productPanel = GameObject.Find("ProductPanel").GetComponent<ProductPanel>();
        productPanel.Show();

        GameObject.Find("MainPage").GetComponent<ManualAdd>().LoadProductData(product.product_id);
    }
}
