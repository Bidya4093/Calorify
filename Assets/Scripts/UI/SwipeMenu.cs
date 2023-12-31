using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SwipeMenu : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public UIDocument document;
    private VisualElement mainContainer;
    private VisualElement homeContainer;
    private VisualElement activityContainer;
    private TemplateContainer activityTemplate;

    private Button rationBtn;
    private Button activityBtn;

    public float swipeDistanceThreshold = 50f;

    void Start()
    {
        //document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;
        mainContainer = root.Q<VisualElement>("MainContainer");
        homeContainer = root.Q<VisualElement>("HomeContainer");
        activityContainer = root.Q<VisualElement>("ActivityContainer");
        activityTemplate = root.Q<TemplateContainer>("ActivityTemplate");

        rationBtn = root.Q<Button>("RationBtn");
        activityBtn = root.Q<Button>("ActivityBtn");
        mainContainer.RegisterCallback<PointerDownEvent>(OnPointerDownEvent, TrickleDown.TrickleDown);
        mainContainer.RegisterCallback<PointerUpEvent>(OnPointerUpEvent, TrickleDown.TrickleDown);
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
                rationBtn.AddToClassList("active");
                activityBtn.RemoveFromClassList("active");
                homeContainer.style.display = DisplayStyle.Flex;
                activityTemplate.style.display = DisplayStyle.None;


            }
            else
            {
                // Виконати дії для swipe вліво
                Debug.Log("Swipe to left");
                rationBtn.RemoveFromClassList("active");
                activityBtn.AddToClassList("active");
                homeContainer.style.display = DisplayStyle.None;
                activityTemplate.style.display = DisplayStyle.Flex;
            }
        }
    }
}
