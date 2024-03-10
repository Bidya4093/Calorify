using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SwipeMenu : MonoBehaviour
{
    private VisualElement mainRoot;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private VisualElement mainContainer;
    private VisualElement homeContainer;
    private TemplateContainer activityTemplate;

    private VisualElement btnSlider;

    private ScrollView homeScroll;
    private ScrollView activityScroll;

    private float swipeDistanceThreshold = 300;
    private float velocityThreshold = 150;
    private float maxDeltaPositionX;
    private float swipeInitThreshold = 40;
    private float sliderCurrentTranslate;
    private float homeCurrentTranslate;
    private float activityCurrentTranslate;
    private StyleLength sliderCurrentLeft;

    private float padding = 6;
    private float activityTranslateXPercent = 100;
    private float homeTranslateXPercent = 0;
    private float sliderTranslateXPercent = 0;
    private bool isSwipeStarted = false;
    private bool isScrollStarted = false;

    private float homeTranslateMax = 25;
    private float activityTranslateMax = 100;
    void Start()
    {
        mainRoot = GetComponent<UIDocument>().rootVisualElement;
        mainContainer = mainRoot.Q<VisualElement>("MainContainer");
        homeContainer = mainRoot.Q<VisualElement>("HomeContainer");
        activityTemplate = mainRoot.Q<TemplateContainer>("ActivityTemplate");

        homeScroll = homeContainer.Q<ScrollView>("HistoryScroll");
        activityScroll = activityTemplate.Q<ScrollView>("HistoryScroll");

        btnSlider = mainRoot.Q<VisualElement>("HomeNavBtnSlider");

        mainContainer.RegisterCallback<GeometryChangedEvent>(Init);
        mainContainer.RegisterCallback<PointerDownEvent>(OnPointerDownEvent, TrickleDown.TrickleDown);
        mainContainer.RegisterCallback<PointerUpEvent>(OnPointerUpEvent, TrickleDown.TrickleDown);
        mainContainer.RegisterCallback<PointerMoveEvent>(OnPointerMoveEvent, TrickleDown.TrickleDown);
    }

    private void Init(GeometryChangedEvent evt)
    {
        // �������� �������� �������� ����� ��������.
        swipeDistanceThreshold = mainRoot.resolvedStyle.width / 2;
        activityTranslateXPercent = activityTemplate.resolvedStyle.translate.x / activityTemplate.resolvedStyle.width * 100;
        homeTranslateXPercent = homeContainer.resolvedStyle.translate.x / homeContainer.resolvedStyle.width * 100;
        sliderTranslateXPercent = btnSlider.resolvedStyle.translate.x / btnSlider.resolvedStyle.width * 100;
        padding = btnSlider.parent.resolvedStyle.paddingLeft;
        mainContainer.UnregisterCallback<GeometryChangedEvent>(Init);
    }

    private void OnActivityTranstitionEnd(TransitionEndEvent evt)
    {
        // ��������� �������� �������� ���������, ���� ��������� �������
        if (activityTranslateXPercent == 0)
        {
            activityTemplate.style.visibility = Visibility.Visible;
        }
        else if (activityTranslateXPercent == 100)
        {
            activityTemplate.PlaceInFront(homeContainer);
            activityTemplate.style.visibility = Visibility.Hidden;
        }
        activityTemplate.UnregisterCallback<TransitionEndEvent>(OnActivityTranstitionEnd);
    }

    private void OnHomeTranstitionEnd(TransitionEndEvent evt)
    {
        // ��������� �������� �������� ����������, ���� ��������� �������.
        if (homeTranslateXPercent == 0 )
        {
            homeContainer.style.visibility = Visibility.Visible;
        }
        else if (homeTranslateXPercent == -100)
        {
            homeContainer.PlaceInFront(activityTemplate);
            homeContainer.style.visibility = Visibility.Hidden;
        }
        homeContainer.UnregisterCallback<TransitionEndEvent>(OnHomeTranstitionEnd);
    }

    private void OnPointerMoveEvent(PointerMoveEvent evt)
    {
        // ������������ ���� ��������� ������ � ������.
        // ���� �� �������� ������ �� ���� - ���������� �����, ���� �� ���������� - �����.
        if ((startTouchPosition.x - swipeInitThreshold > evt.position.x ||
            startTouchPosition.x + swipeInitThreshold < evt.position.x) && !isScrollStarted)
        {
            if (!isSwipeStarted) StartSwipe();
        }
        else if ((startTouchPosition.y - swipeInitThreshold > evt.position.y ||
            startTouchPosition.y + swipeInitThreshold < evt.position.y) && !isSwipeStarted)
        {
            if (!isScrollStarted) EnableScroll(evt);
            return;
        }
        else return;

        // ������� ���� ��� ���� �������� ��������� ����
        if ((btnSlider.transform.position.x - evt.deltaPosition.x) < 0)
        {
            sliderTranslateXPercent = 0;
        }
        else if ((btnSlider.transform.position.x + btnSlider.resolvedStyle.width - evt.deltaPosition.x) > btnSlider.parent.resolvedStyle.width)
        {
            sliderTranslateXPercent = 100;
        }
        else
        {
            // ���� � ����� ����, ��� ��������� ������� ������� ���� �����������
            sliderTranslateXPercent -= ((evt.deltaPosition.x) / btnSlider.parent.resolvedStyle.width) * 50;
        }

        // ������� ���� ��� ���� �������� ��������� �� ����������
        if ((homeContainer.transform.position.x + homeContainer.resolvedStyle.width + evt.deltaPosition.x) > homeContainer.resolvedStyle.width)
        {
            homeTranslateXPercent = 0;
            activityTranslateXPercent = 100;
        }
        else if (activityTemplate.worldBound.x + evt.deltaPosition.x < 0)
        {
            activityTranslateXPercent = 0;
            homeTranslateXPercent = -100;
        }
        else
        {
            // ���� � ����� ����, ��� ��������� ������� ������� ���� �����������
            activityTranslateXPercent += (evt.deltaPosition.x / activityTemplate.resolvedStyle.width) * activityTranslateMax;
            homeTranslateXPercent += (evt.deltaPosition.x / homeContainer.resolvedStyle.width) * homeTranslateMax;
        }

        if (maxDeltaPositionX < Mathf.Abs(evt.deltaPosition.x))
            maxDeltaPositionX = Mathf.Abs(evt.deltaPosition.x);
        // �������� ���������� ������� ���������
        SetTranslate();
    }

    private void StartSwipe()
    {
        homeContainer.style.visibility = Visibility.Visible;
        activityTemplate.style.visibility = Visibility.Visible;
        
        isSwipeStarted = true;

        // �����'������� ������� ������� ��������
        sliderCurrentTranslate = sliderTranslateXPercent;
        homeCurrentTranslate = homeTranslateXPercent;
        activityCurrentTranslate = activityTranslateXPercent;
        sliderCurrentLeft = btnSlider.style.left;
    }

    private void EnableScroll(PointerMoveEvent evt)
    {
        // �������� ��������� ���� �������.
        homeScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);
        activityScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);

        isScrollStarted = true;
    }

    private void DisableScroll()
    {
        // ��������� ��������� ���� �������.
        homeScroll.contentContainer.RegisterCallback<PointerMoveEvent>(PreventScroll);
        activityScroll.contentContainer.RegisterCallback<PointerMoveEvent>(PreventScroll);
        isScrollStarted = false;
    }

    private void PreventScroll(PointerMoveEvent evt)
    {
        if (isSwipeStarted || !isScrollStarted) evt.StopPropagation();
    }
    private void OnPointerDownEvent(PointerDownEvent evt)
    {
        startTouchPosition = evt.position;
        maxDeltaPositionX = 0;
        DisableScroll();
    }

    private void OnPointerUpEvent(PointerUpEvent evt)
    {
        isScrollStarted = false;

        if (!isSwipeStarted) return;
        isSwipeStarted = false;

        // ��������� ������� ������.
        endTouchPosition = evt.position;
        float swipeDistance = (endTouchPosition - startTouchPosition).magnitude;
        // �������� ������������� ��������� �������� ��������.
        homeContainer.RegisterCallback<TransitionEndEvent>(OnHomeTranstitionEnd);
        activityTemplate.RegisterCallback<TransitionEndEvent>(OnActivityTranstitionEnd);

        // ������� ��������� ���� �������.
        homeScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);
        activityScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);

        if (swipeDistance > swipeDistanceThreshold || maxDeltaPositionX > velocityThreshold)
        {
            // ����� �������, ���������� � ��� ������� ������� �����.
            if (endTouchPosition.x > startTouchPosition.x)
            {
                // �������� 䳿 ��� swipe ������, ������������� �� �������� ����������
                ToRationPanel();
            }
            else
            {
                // �������� 䳿 ��� swipe ����, ������������� �� �������� ���������
                ToActivityPanel();
            }
        }
        else
        {
            // ����� �� �������, ��������� �������� �� ��� ����.
            btnSlider.style.left = sliderCurrentLeft;
            sliderTranslateXPercent = sliderCurrentTranslate;
            activityTranslateXPercent = activityCurrentTranslate;
            homeTranslateXPercent = homeCurrentTranslate;
        }

        SetTranslate();
    }

    public void ToActivityPanel()
    {
        btnSlider.style.left = -padding;
        sliderTranslateXPercent = 100;
        homeTranslateXPercent = -100;
        activityTranslateXPercent = 0;
        homeTranslateMax = 100;
        activityTranslateMax = 25;
        SetTranslate();
    }

    public void ToRationPanel()
    {
        btnSlider.style.left = padding;
        sliderTranslateXPercent = 0;
        homeTranslateXPercent = 0;
        activityTranslateXPercent = 100;
        homeTranslateMax = 25;
        activityTranslateMax = 100;

        SetTranslate();
    }

    private void SetTranslate()
    {
        // �������� ���������� ������� ���������
        btnSlider.style.translate = new Translate(Length.Percent(sliderTranslateXPercent), 0);
        activityTemplate.style.translate = new Translate(Length.Percent(activityTranslateXPercent), 0);
        homeContainer.style.translate = new Translate(Length.Percent(homeTranslateXPercent), 0);
    }
}

// Todo:
/*
 * �������� ����� ���� ����������� ������� ����� ������ - ��������
 * 
 * ���������� ����� �������� ������� (��������, ���������) - ��������
 * 
 * �������� ��������� ������������� ��� ������� ������
 * ���������: ������������� � 40 ������ ���������� ���, �� ����� ��� � ����������, ��� ����, �� ���������� �� ����� ������� ��������.
 * 
 * �� ������� ��������: �������������, ���� ������� � �������, �� � ������ �����, 
 * � ���� ���� ������� � ���������� ������� ���������� �����. - ��������� ����������
 * 
*/