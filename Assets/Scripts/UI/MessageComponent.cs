using System;
using UnityEngine.UIElements;
using UnityEngine;

public class MessageComponent : VisualElement
{
    protected Shadow shadowContainer;
    protected VisualElement messageContainer;
    protected Label dateLabel;
    protected Label messageLabel;
    protected Shadow newBadgeShadow;
    protected Label newBadgeLabel;

    protected Vector2 startTouchPosition;
    protected Vector2 endTouchPosition;

    protected float swipeDistanceThreshold = 300;
    protected float swipeInitHorizontalThreshold = 80;
    protected bool isSwipeStarted = false;
    protected float translateXPercent = 0;
    protected float currentTranslate;

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

    public virtual void Init(string _message, DateTime _date, bool _isNew)
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

        // Зареєструвати калбек видалення свайпом.
        RegisterCallback<PointerDownEvent>(OnPointerDownEvent, TrickleDown.TrickleDown);
        RegisterCallback<PointerMoveEvent>(OnPointerMoveEvent, TrickleDown.TrickleDown);
        RegisterCallback<PointerUpEvent>(OnPointerUpEvent, TrickleDown.TrickleDown);
    }

    public void OnTransitionEndEvent(TransitionEndEvent evt)
    {
        // Визначаємо видимість панельки харчування, коли закінчився перехід.
        if (translateXPercent == 100)
            Delete();

        UnregisterCallback<TransitionEndEvent>(OnTransitionEndEvent);
    }

    public virtual void OnPointerDownEvent(PointerDownEvent evt)
    {
        startTouchPosition = evt.position;
    }

    public virtual void OnPointerUpEvent(PointerUpEvent evt)
    {
        if (!isSwipeStarted) return;
        isSwipeStarted = false;

        // Знаходимо відстань свайпу.
        endTouchPosition = evt.position;
        float swipeDistance = (endTouchPosition - startTouchPosition).magnitude;

        // Починаємо відслідковувати закінчення плавного переходу.
        RegisterCallback<TransitionEndEvent>(OnTransitionEndEvent);

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

    public void UpdateTranslate(PointerMoveEvent evt)
    {
        if (worldBound.x + evt.deltaPosition.x < 0)
        {
            translateXPercent = 0;
        }
        else
        {
            // Якщо в межах лімітів, тоді оновлюємо позицію відносно руху користувача
            translateXPercent += (evt.deltaPosition.x / resolvedStyle.width) * 100;
        }
    }

    public virtual void OnPointerMoveEvent(PointerMoveEvent evt)
    {
        // Встановлюємо поріг активації свайпу і скролу.
        // Якщо по вертикалі вийшли за поріг - активовуємо скрол, якщо по горизонталі - свайп.
        if ((startTouchPosition.x - swipeInitHorizontalThreshold > evt.position.x ||
            startTouchPosition.x + swipeInitHorizontalThreshold < evt.position.x))
        {
            if (!isSwipeStarted) StartSwipe();
        }
        else return;

        UpdateTranslate(evt);

        // Записуємо обраховану позицію елементам
        SetTranslate();
    }

    public virtual void StartSwipe()
    {
        isSwipeStarted = true;

        // Запам'ятовуємо стартові позиції елементів
        currentTranslate = translateXPercent;
    }

    public virtual void SetTranslate()
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
            Message.messageIconState.visible = true;
        }
        else
        {
            if (Contains(newBadgeShadow)) 
                newBadgeShadow.RemoveFromHierarchy();
        }
    }

    public virtual void Delete()
    {
        RemoveFromHierarchy();
    }

    public virtual void DeleteWithAnimation()
    {
        translateXPercent = 100;
        SetTranslate();
    }
}

/** Todo
 * Bug: When the swipe starts and moves on another message then the position of the current item is frozen 
 * and other message starts swiping.
 * 
 * 
 */