using System;
using UnityEngine.UIElements;
using UnityEngine;

public class MessageComponent : VisualElement
{

    private Shadow shadowContainer;
    private VisualElement messageContainer;
    private Label dateLabel;
    private Label messageLabel;
    private Shadow newBadgeShadow;
    private Label newBadgeLabel;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private float swipeDistanceThreshold = 300;
    private float swipeInitHorizontalThreshold = 80;
    private float swipeInitVerticalThreshold = 40;
    private bool isSwipeStarted = false;
    private bool isScrollStarted = false;
    private float translateXPercent = 0;
    private float currentTranslate;


    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_MessageAttr = new UxmlStringAttributeDescription { name = "message", defaultValue = "Ви споживаєте надмірну к-сть жирів, це може бути <color=#FF3333>шкідливо для вашого здоров" };
        UxmlBoolAttributeDescription m_IsNewAttr = new UxmlBoolAttributeDescription { name = "isNew", defaultValue = false };
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            MessageComponent ate = ve as MessageComponent;
            ate.message = m_MessageAttr.GetValueFromBag(bag, cc);
            ate.isNew = m_IsNewAttr.GetValueFromBag(bag, cc);
        }
    }
    public new class UxmlFactory : UxmlFactory<MessageComponent, UxmlTraits> { }

    public readonly string ussContainer = "message-item__container";
    public readonly string ussShadow = "message-item__shadow";
    public readonly string ussItem = "message-item";
    public readonly string ussDate = "message-item__date";
    public readonly string ussText = "message-item__text";
    public readonly string ussNewBadgeShadow = "message-item__new-shadow";
    public readonly string ussNewBadge = "message-item__new";

    string m_Message;
    DateTime m_Date;
    bool m_IsNew;
    public int id;
    public bool isReviewed = false;

    public string message
    {
        get => m_Message;
        set
        {

            messageLabel.text = value;
            m_Message = value;
        }
    }
    public DateTime date
    {
        get => m_Date;
        set
        {
            dateLabel.text = value.ToString("HH:mm     dd.MM.yyyy");
            m_Date = value;
        }
    }
    public bool isNew
    {
        get => m_IsNew;
        set
        {

            m_IsNew = value;
            UpdateNewBadge(value);
        }
    }


    public MessageComponent() {
        id = Message.messages.Count;
        Init("Нове сповіщення", DateTime.Now, m_IsNew);
    }

    public MessageComponent(string _message, DateTime _date, bool _isNew)
    {
        id = Message.messages.Count;
        Init(_message, _date, _isNew);
    }

    public MessageComponent(Notification notification)
    {
        Init(notification.message, DateTime.Parse(notification.date), Convert.ToBoolean(notification.is_new));
        id = notification.notification_id;
    }

    public void Init(string _message, DateTime _date, bool _isNew)
    {
        name = "MessageItemContainer";
        AddToClassList(ussContainer);

        shadowContainer = new Shadow();
        shadowContainer.name = "MessageItemShadow";
        shadowContainer.AddToClassList(ussShadow);
        shadowContainer.AddToClassList("shadow-45");
        hierarchy.Add(shadowContainer);

        messageContainer = new VisualElement();
        messageContainer.name = "MessageItem";
        messageContainer.AddToClassList(ussItem);
        shadowContainer.Add(messageContainer);

        dateLabel = new Label(_date.ToString("HH:mm     dd.MM.yyyy"));
        dateLabel.name = "MessageItemDate";
        dateLabel.AddToClassList(ussDate);
        messageContainer.Add(dateLabel);

        messageLabel = new Label(_message);
        messageLabel.name = "MessageItemText";
        messageLabel.AddToClassList(ussText);
        messageContainer.Add(messageLabel);

        message = _message;
        date = _date;
        isNew = _isNew;


        Message.CheckEmptyList();
        if (Message.empty)
            style.marginBottom = 30;

        Message.messageList.Insert(0, this);
        Message.messages.Add(this);

        // Зареєструвати калбек видалення свайпом.
        RegisterCallback<PointerDownEvent>(OnPointerDownEvent, TrickleDown.TrickleDown);
        RegisterCallback<PointerMoveEvent>(OnPointerMoveEvent, TrickleDown.TrickleDown);
        RegisterCallback<PointerUpEvent>(OnPointerUpEvent, TrickleDown.TrickleDown);
    }

    private void OnTransitionEndEvent(TransitionEndEvent evt)
    {
        // Визначаємо видимість панельки харчування, коли закінчився перехід.
        if (translateXPercent == 100)
            Delete();

        UnregisterCallback<TransitionEndEvent>(OnTransitionEndEvent);
    }

    private void OnPointerDownEvent(PointerDownEvent evt)
    {
        startTouchPosition = evt.position;
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
        RegisterCallback<TransitionEndEvent>(OnTransitionEndEvent);

        // Вертаємо можливість руху скроллів.
        Message.messageScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);

        if (swipeDistance > swipeDistanceThreshold)
        {
            // Свайп відбувся, перевіряємо в яку сторону відбувся свайп.
            if (endTouchPosition.x > startTouchPosition.x)
            {
                // Виконати дії для swipe вправо, дотягуємо елемент.
                translateXPercent = 100;
                SetTranslate();
            }
        }
        else
        {
            // Свайп не відбувся, повертаємо елементи на свої місця.
            translateXPercent = currentTranslate;
        }

        SetTranslate();
    }

    private void OnPointerMoveEvent(PointerMoveEvent evt)
    {
        // Встановлюємо поріг активації свайпу і скролу.
        // Якщо по вертикалі вийшли за поріг - активовуємо скрол, якщо по горизонталі - свайп.
        if ((startTouchPosition.x - swipeInitHorizontalThreshold > evt.position.x ||
            startTouchPosition.x + swipeInitHorizontalThreshold < evt.position.x) && !isScrollStarted)
        {
            if (!isSwipeStarted) StartSwipe();
        }
        else if ((startTouchPosition.y - swipeInitVerticalThreshold > evt.position.y ||
            startTouchPosition.y + swipeInitVerticalThreshold < evt.position.y) && !isSwipeStarted)
        {
            if (!isScrollStarted) EnableScroll(evt);
            return;
        }
        else return;
        
        if (worldBound.x + evt.deltaPosition.x < 0)
        {
            translateXPercent = 0;
        }
        else
        {
            // Якщо в межах лімітів, тоді оновлюємо позицію відносно руху користувача
            translateXPercent += (evt.deltaPosition.x / resolvedStyle.width) * 100;
        }

        // Записуємо обраховану позицію елементам
        SetTranslate();
    }

    private void StartSwipe()
    {
        isSwipeStarted = true;

        // Запам'ятовуємо стартові позиції елементів
        currentTranslate = translateXPercent;
    }

    private void EnableScroll(PointerMoveEvent evt)
    {
        // Включаємо можливість руху скроллів.
        Message.messageScroll.contentContainer.UnregisterCallback<PointerMoveEvent>(PreventScroll);
        isScrollStarted = true;
    }


    private void PreventScroll(PointerMoveEvent evt)
    {
        if (isSwipeStarted || !isScrollStarted) evt.StopPropagation();
    }

    private void DisableScroll()
    {
        // Виключаємо можливість руху скроллів.
        Message.messageScroll.contentContainer.RegisterCallback<PointerMoveEvent>(PreventScroll);
        isScrollStarted = false;
    }

    private void SetTranslate()
    {
        // Записуємо обраховану позицію елементу
        style.translate = new Translate(Length.Percent(translateXPercent), 0);
    }


    public void UpdateNewBadge(bool _isNew)
    {
        if (_isNew)
        {
            if (!Contains(newBadgeShadow))
            {
                newBadgeShadow = new Shadow();
                newBadgeShadow.name = "MessageItemNewShadow";
                newBadgeShadow.AddToClassList(ussNewBadgeShadow);
                messageContainer.Add(newBadgeShadow);

                newBadgeLabel = new Label("NEW");
                newBadgeLabel.name = "MessageItemNew";
                newBadgeLabel.AddToClassList(ussNewBadge);
                newBadgeShadow.Add(newBadgeLabel);
            }

        }
        else
        {
            if (Contains(newBadgeShadow)) 
                newBadgeShadow.RemoveFromHierarchy();
        }
    }

    private void Delete()
    {
        Message.notificationDBManager.DeleteRecordById(id);
        RemoveFromHierarchy();
        Message.messages.Remove(this);
        if (Message.messageList.Query<MessageComponent>().Last() != null)
            Message.messageList.Query<MessageComponent>().Last().style.marginBottom = 30;

        Message.CheckEmptyList();
    }

    public void DeleteWithAnimation()
    {
        translateXPercent = 100;
        SetTranslate();

        //Починаємо відслідковувати закінчення плавного переходу.
        RegisterCallback<TransitionEndEvent>(OnTransitionEndEvent);
    }
}

/** Todo
 * Bug: When the swipe starts and moves on another message then the position of the current item is frozen 
 * and other message starts swiping.
 * 
 * 
 */