using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyList : MonoBehaviour
{
    public GameObject list;
    public TextMeshProUGUI emptyText;
    public TMP_FontAsset fontAsset;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI calories;
    public TextMeshProUGUI index;
    public TMP_InputField inputMasa;
    public Sprite[] foodImages;

    void Start()
    {
        if (list.transform.childCount == 0)
        {
            emptyText.text = "ѕусто";
        }
        else
        {
            emptyText.text = string.Empty;
        }
    }

    void Update()
    {
        if (list.transform.childCount == 0)
        {
            emptyText.text = "ѕусто";
        }
        else
        {
            emptyText.text = string.Empty;
        }
    }

    public void AddItem()
    {
        GameObject dish = new GameObject("Dish");
        GameObject image = new GameObject("Image");
        GameObject line = new GameObject("Line");
        GameObject name = new GameObject("Name");
        GameObject mass = new GameObject("Mass");
        GameObject caloriesObj = new GameObject("Calories");

        string nameStr = itemName.text;
        string caloriesStr = calories.text + " ккал";
        string masaStr = inputMasa.text.Length == 0 || Int16.Parse(inputMasa.text) <= 0 ? "100 г" : inputMasa.text + " г";

        // Dish panel
        RectTransform dishTrans = dish.AddComponent<RectTransform>();
        dishTrans.transform.SetParent(list.transform); // setting parent
        dishTrans.anchoredPosition = new Vector2(0.5f, 0.5f); // setting position
        dishTrans.sizeDelta = new Vector2(1080, 350);
        dish.AddComponent<CanvasRenderer>();
        dish.transform.SetParent(list.transform);

        // Image
        RectTransform imageTrans = image.AddComponent<RectTransform>();
        imageTrans.transform.SetParent(dish.transform);
        imageTrans.sizeDelta = new Vector2(200, 200);
        imageTrans.localPosition = new Vector2(-340f, 0f);

        Image foodImage = image.AddComponent<Image>();
        foodImage.sprite = foodImages[Int32.Parse(index.text)];
        image.transform.SetParent(dish.transform);

        // Name
        RectTransform nameTrans = name.AddComponent<RectTransform>();
        nameTrans.transform.SetParent(dish.transform);
        nameTrans.sizeDelta = new Vector2(600, 100);
        nameTrans.localPosition = new Vector2(150f, 70f);

        TextMeshProUGUI nameText = name.AddComponent<TextMeshProUGUI>();
        nameText.fontStyle = FontStyles.Bold;
        nameText.fontSize = 80;
        nameText.text = nameStr;
        nameText.font = fontAsset;
        nameText.color = Color.black;
        name.transform.SetParent(dish.transform);

        // Mass
        RectTransform massTrans = mass.AddComponent<RectTransform>();
        massTrans.transform.SetParent(dish.transform);
        massTrans.sizeDelta = new Vector2(400, 100);
        massTrans.localPosition = new Vector2(50f, -15f);

        TextMeshProUGUI massText = mass.AddComponent<TextMeshProUGUI>();
        massText.fontSize = 60;
        massText.text = masaStr;
        massText.font = fontAsset;
        massText.color = Color.black;
        mass.transform.SetParent(dish.transform);

        // Calories
        RectTransform caloriesTrans = caloriesObj.AddComponent<RectTransform>();
        caloriesTrans.transform.SetParent(dish.transform);
        caloriesTrans.sizeDelta = new Vector2(400, 100);
        caloriesTrans.localPosition = new Vector2(50f, -80f);

        TextMeshProUGUI caloriesText = caloriesObj.AddComponent<TextMeshProUGUI>();
        caloriesText.fontSize = 60;
        caloriesText.text = caloriesStr;
        caloriesText.font = fontAsset;
        caloriesText.color = Color.black;
        caloriesObj.transform.SetParent(dish.transform);

        // Line
        RectTransform lineTrans = line.AddComponent<RectTransform>();
        lineTrans.transform.SetParent(dish.transform);
        lineTrans.sizeDelta = new Vector2(1080, 3);
        lineTrans.localPosition = new Vector2(0f, -175f);

        Image lineImage = line.AddComponent<Image>();
        lineImage.color = Color.black;
        line.transform.SetParent(dish.transform);
    }
}
