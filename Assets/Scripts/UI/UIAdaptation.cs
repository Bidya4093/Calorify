using UnityEngine;
using UnityEngine.UI;

public class UIAdaptation : MonoBehaviour
{
    public RectTransform[] elements;
    private Vector2[] initialPositions;
    private Vector2 screenResolution;

    void Awake()
    {
        // Зберігаємо початкові позиції елементів
        initialPositions = new Vector2[elements.Length];
        for (int i = 0; i < elements.Length; i++)
        {
            initialPositions[i] = elements[i].anchoredPosition;
        }
        // Зберігаємо розмір екрану
        screenResolution = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        // Отримуємо поточний розмір екрану
        Vector2 currentResolution = new Vector2(Screen.width, Screen.height);
        // Обчислюємо коефіцієнти масштабування
        float scaleX = currentResolution.x / screenResolution.x;
        float scaleY = currentResolution.y / screenResolution.y;
        // Застосовуємо коефіцієнти масштабування до елементів
        for (int i = 0; i < elements.Length; i++)
        {
            // Обчислюємо нові позиції елементів
            float newX = initialPositions[i].x * scaleX;
            float newY = initialPositions[i].y * scaleY;
            Vector2 newPosition = new Vector2(newX, newY);
            // Застосовуємо нові позиції і масштаби до елементів
            elements[i].anchoredPosition = newPosition;
            elements[i].localScale = new Vector2(scaleX, scaleY);
        }
    }
}
