

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PushMessage : MessageComponent
{

    public readonly string ussPushContainer = "message-item__container--push";

    public PushMessage() : base() { }

    public PushMessage(string _message, DateTime _date, bool _isNew) : base(_message, _date, _isNew) { }

    public PushMessage(Notification notification) : base(notification) { }


    public override void Init(string _message, DateTime _date, bool _isNew)
    {
        base.Init(_message, _date, _isNew);

        AddToClassList(ussPushContainer);
        RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
    }

    public void OnGeometryChangedEvent(GeometryChangedEvent evt)
    {
        style.translate = new Translate(0, Length.Percent(100));
    }

    public override void SetTranslate()
    {
        // Записуємо обраховану позицію елементу
        style.translate = new Translate(Length.Percent(translateXPercent), Length.Percent(100));
    }

    public override void Delete()
    {
        base.Delete();
        InnerPushMessageManager innerPushMessageManager = GameObject.Find("PushMessageContainer").GetComponent<InnerPushMessageManager>();
        innerPushMessageManager.UpdateQueue();
    }
}