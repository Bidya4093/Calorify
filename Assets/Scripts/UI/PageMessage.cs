
using System;
using UnityEngine.UIElements;

public class PageMessage : MessageComponent
{
    private float swipeInitVerticalThreshold = 40;
    protected bool isScrollStarted = false;

    public PageMessage() : base() { }

    public PageMessage(string _message, DateTime _date, bool _isNew) : base(_message, _date, _isNew) { }

    public PageMessage(Notification notification) : base(notification) { }

    public override void Init(string _message, DateTime _date, bool _isNew)
    {
        base.Init(_message, _date, _isNew);

        Message.CheckEmptyList();
        if (Message.empty)
            style.marginBottom = 30;
        Message.messages.Add(this);
    }

    public override void OnPointerDownEvent(PointerDownEvent evt)
    {
        base.OnPointerDownEvent(evt);
        DisableScroll();
    }

    public override void OnPointerUpEvent(PointerUpEvent evt)
    {
        isScrollStarted = false;

        base.OnPointerUpEvent(evt);

        // Вертаємо можливість руху скроллів.
        EnableScroll(null);
    }

    public override void OnPointerMoveEvent(PointerMoveEvent evt)
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

        UpdateTranslate(evt);

        // Записуємо обраховану позицію елементам
        SetTranslate();
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

    public virtual void DisableScroll()
    {
        // Виключаємо можливість руху скроллів.
        Message.messageScroll.contentContainer.RegisterCallback<PointerMoveEvent>(PreventScroll);
        isScrollStarted = false;
    }

    public override void Delete()
    {
        base.Delete();
        Message.notificationDBManager.DeleteRecordById(id);
        Message.messages.Remove(this);
        if (Message.messageList.Query<PageMessage>().Last() != null)
            Message.messageList.Query<PageMessage>().Last().style.marginBottom = 30;

        Message.CheckEmptyList();
    }
}

/** Todo
 * Bug: When the swipe starts and moves on another message then the position of the current item is frozen 
 * and other message starts swiping.
 * 
 * 
 */