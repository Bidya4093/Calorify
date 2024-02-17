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

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_MessageAttr = new UxmlStringAttributeDescription { name = "message", defaultValue = "" };
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


    public readonly string ussShadow = "message-item__shadow";
    public readonly string ussItem = "message-item";
    public readonly string ussDate = "message-item__date";
    public readonly string ussText = "message-item__text";
    public readonly string ussNewBadgeShadow = "message-item__new-shadow";
    public readonly string ussNewBadge = "message-item__new";

    string m_Message;
    DateTime m_Date;
    bool m_IsNew;

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
        Init("Нове сповіщення", DateTime.Now, true);
    }

    public MessageComponent(string _message, DateTime _date, bool _isNew)
    {
        Init(_message, _date, _isNew);
    }

    public void Init(string _message, DateTime _date, bool _isNew)
    {
        name = "MessageItemContainer";

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

        //Debug.Log(parent.childCount);

        //Debug.Log();
        if (Message.empty)
            style.marginBottom = 0;


        // Зареєструвати калбек видалення свайпом.
        RegisterCallback<PointerDownEvent>(OnPointerDownEvent, TrickleDown.TrickleDown);
        RegisterCallback<PointerUpEvent>(OnPointerUpEvent, TrickleDown.TrickleDown);
        RegisterCallback<PointerMoveEvent>(OnPointerMoveEvent, TrickleDown.TrickleDown);
        RegisterCallback<TransitionEndEvent>(OnTransitonEndEvent);
        RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanelEvent);
    }

    private void OnDetachFromPanelEvent(DetachFromPanelEvent evt)
    {
        Debug.Log((evt.originPanel as VisualElement));
        Debug.Log((evt.originPanel as VisualElement).childCount);
    }

    private void OnTransitonEndEvent(TransitionEndEvent evt)
    {
        Delete();

    }

    private void OnPointerMoveEvent(PointerMoveEvent evt)
    {
        throw new NotImplementedException();
    }

    private void OnPointerUpEvent(PointerUpEvent evt)
    {
        throw new NotImplementedException();
    }

    private void OnPointerDownEvent(PointerDownEvent evt)
    {
        throw new NotImplementedException();
    }

    public void UpdateNewBadge(bool _isNew)
    {
        if (_isNew)
        {
            newBadgeShadow = new Shadow();
            newBadgeShadow.name = "MessageItemNewShadow";
            newBadgeShadow.AddToClassList(ussNewBadgeShadow);
            messageContainer.Add(newBadgeShadow);

            newBadgeLabel = new Label("NEW");
            newBadgeLabel.name = "MessageItemNew";
            newBadgeLabel.AddToClassList(ussNewBadge);
            newBadgeShadow.Add(newBadgeLabel);
        } else
        {
            if (Contains(newBadgeShadow))
            {
                newBadgeShadow.RemoveFromHierarchy();
            }
        }
    }

    public void Delete()
    {
        RemoveFromHierarchy();
    }
}