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
        // Записуємо необхідні початкові стани елементів.
        swipeDistanceThreshold = mainRoot.resolvedStyle.width / 2;
        activityTranslateXPercent = activityTemplate.resolvedStyle.translate.x / activityTemplate.resolvedStyle.width * 100;
        homeTranslateXPercent = homeContainer.resolvedStyle.translate.x / homeContainer.resolvedStyle.width * 100;
        sliderTranslateXPercent = btnSlider.resolvedStyle.translate.x / btnSlider.resolvedStyle.width * 100;
        padding = btnSlider.parent.resolvedStyle.paddingLeft;
        mainContainer.UnregisterCallback<GeometryChangedEvent>(Init);
    }

    private void OnActivityTranstitionEnd(TransitionEndEvent evt)
    {
        // Визначаємо видимість панельки активності, коли закінчився перехід
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
        // Визначаємо видимість панельки харчування, коли закінчився перехід.
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
        // Встановлюємо поріг активації свайпу і скролу.
        // Якщо по вертикалі вийшли за поріг - активовуємо скрол, якщо по горизонталі - свайп.
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

        // Ставимо ліміти для руху слайдера верхнього меню
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
            // Якщо в межах лімітів, тоді оновлюємо позицію відносно руху користувача
            sliderTranslateXPercent -= ((evt.deltaPosition.x) / btnSlider.parent.resolvedStyle.width) * 50;
        }

        // Ставимо ліміти для руху панельок активності та харчування
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
            // Якщо в межах лімітів, тоді оновлюємо позицію відносно руху користувача
            activityTranslateXPercent += (evt.deltaPosition.x / activityTemplate.resolvedStyle.width) * activityTranslateMax;
            homeTranslateXPercent += (evt.deltaPosition.x / homeContainer.resolvedStyle.width) * homeTranslateMax;
        }

        if (maxDeltaPositionX < Mathf.Abs(evt.deltaPosition.x))
            maxDeltaPositionX = Mathf.Abs(evt.deltaPosition.x);
        // Записуємо обраховану позицію елементам
        SetTranslate();
    }

    private void StartSwipe()
    {
        homeContainer.style.visibility = Visibility.Visible;
        activityTemplate.style.visibility = Visibility.Visible;
        
        isSwipeStarted = true;

        // Запам'ятовуємо стартові позиції елементів
        sliderCurrentTranslate = sliderTranslateXPercent;
        homeCurrentTranslate = homeTranslateXPercent;
        activityCurrentTranslate = activityTranslateXPercent;
        sliderCurrentLeft = btnSlider.style.left;
    }

    private void EnableScroll(PointerMoveEvent evt)
    {
        // Включаємо можливість руху скроллів.
        homeScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);
        activityScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);

        isScrollStarted = true;
    }

    private void DisableScroll()
    {
        // Виключаємо можливість руху скроллів.
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

        // Знаходимо відстань свайпу.
        endTouchPosition = evt.position;
        float swipeDistance = (endTouchPosition - startTouchPosition).magnitude;
        // Починаємо відслідковувати закінчення плавного переходу.
        homeContainer.RegisterCallback<TransitionEndEvent>(OnHomeTranstitionEnd);
        activityTemplate.RegisterCallback<TransitionEndEvent>(OnActivityTranstitionEnd);

        // Вертаємо можливість руху скроллів.
        homeScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);
        activityScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);

        if (swipeDistance > swipeDistanceThreshold || maxDeltaPositionX > velocityThreshold)
        {
            // Свайп відбувся, перевіряємо в яку сторону відбувся свайп.
            if (endTouchPosition.x > startTouchPosition.x)
            {
                // Виконати дії для swipe вправо, переключаємось на панельку харчування
                ToRationPanel();
            }
            else
            {
                // Виконати дії для swipe вліво, переключаємось на панельку активності
                ToActivityPanel();
            }
        }
        else
        {
            // Свайп не відбувся, повертаємо елементи на свої місця.
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
        // Записуємо обраховану позицію елементам
        btnSlider.style.translate = new Translate(Length.Percent(sliderTranslateXPercent), 0);
        activityTemplate.style.translate = new Translate(Length.Percent(activityTranslateXPercent), 0);
        homeContainer.style.translate = new Translate(Length.Percent(homeTranslateXPercent), 0);
    }
}

// Todo:
/*
 * Починати свайп після проходження кількості певної пікселів - виконано
 * 
 * Адекватний спосіб керувати скролом (включити, виключити) - виконано
 * 
 * Прибрати початкове подьоргування при початку скролу
 * Пояснення: подьоргування в 40 пікселів спричинене тим, що скрол хоч і виключений, але змінні, що відповідають за змінну значень працюють.
 * 
 * Не потрібна поведінка: відтворююється, коли свайпаєш в сторону, що є глухим кутом, 
 * а потім коли свайпаєш в протилежну сторону починається свайп. - самостійно виконалось
 * 
*/