using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SwipeMenu : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public UIDocument document;
    private VisualElement homeContainer;
    private Button rationBtn;
    private Button activityBtn;


    public float swipeDistanceThreshold = 50f;

    void Start()
    {
        //document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;
        homeContainer = root.Q<VisualElement>("HomeContainer");
        rationBtn = root.Q<Button>("RationBtn");
        activityBtn = root.Q<Button>("ActivityBtn");


        homeContainer.RegisterCallback<PointerDownEvent>(OnPointerDownEvent, TrickleDown.TrickleDown);
        homeContainer.RegisterCallback<PointerUpEvent>(OnPointerUpEvent, TrickleDown.TrickleDown);

    }

    private void OnPointerDownEvent(PointerDownEvent evt)
    {
        startTouchPosition = evt.position;
    }

    private void OnPointerUpEvent(PointerUpEvent evt)
    {
        endTouchPosition = evt.position;
        float swipeDistance = (endTouchPosition - startTouchPosition).magnitude;

        if (swipeDistance > swipeDistanceThreshold)
        {
            if (endTouchPosition.x > startTouchPosition.x)
            {
                // Виконати дії для swipe вправо
                Debug.Log("Swipe to right");
                rationBtn.ToggleInClassList("active");
                activityBtn.ToggleInClassList("active");
            }
            else
            {
                // Виконати дії для swipe вліво
                Debug.Log("Swipe to left");
                rationBtn.ToggleInClassList("active");
                activityBtn.ToggleInClassList("active");

            }
        }
    }
}
