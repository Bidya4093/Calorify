using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Message : MonoBehaviour

{
    public GameObject mainObjectPage;
    static private VisualElement messageRoot;
    static private VisualElement mainRoot;
    private VisualElement mainBg;
    private Button closeBtn;
    private Button deleteAllBtn;
    static private VisualElement messageEmptyContainer;
    static public VisualElement messageList;
    static public ScrollView messageScroll;
    static public List<MessageComponent> messages = new List<MessageComponent>();
    static public bool empty = true;
    private float maxScrollValue = 0;
    static public NotificationDBManager notificationDBManager;
    void Start()
    {
        messageRoot = GetComponent<UIDocument>().rootVisualElement;
        mainRoot = mainObjectPage.GetComponent<UIDocument>().rootVisualElement;
        mainBg = mainRoot.Q<VisualElement>("MainBackground");
        messageEmptyContainer = messageRoot.Q<VisualElement>("MessageEmptyContainer");
        messageList = messageRoot.Q<VisualElement>("MessageScrollContainer");
        messageScroll = messageRoot.Q<ScrollView>("MessageScroll");

        closeBtn = messageRoot.Q<Button>("CloseBtn");
        deleteAllBtn = messageRoot.Q<Button>("MessageDeleteAllBtn");
        closeBtn.RegisterCallback<ClickEvent>(CloseMessagePage);
        deleteAllBtn.RegisterCallback<ClickEvent>(DeleteAllMessages);
        messageRoot.RegisterCallback<TransitionEndEvent>(HandleMessageSlideInEnd);
        // Ціль по калоріях <color=#33B333>виконано</color>
        // Ви споживаєте надмірну к-сть жирів, це може бути <color=#FF3333>шкідливо для вашого здоров
        notificationDBManager = new NotificationDBManager();

        Render();

        messageScroll.verticalScroller.valueChanged += Reviewing;
        messageScroll.RegisterCallback<ChangeEvent<float>>(Reviewing);
    }

    private void Reviewing(ChangeEvent<float> evt)
    {

        if (Math.Abs(evt.newValue - evt.previousValue) < 0.05) {
            Debug.Log("Scroll previous value" + evt.previousValue);
            Debug.Log("Scroll new value" + evt.newValue);
        }
    }

    public void Render()
    {

        List<Notification> notifications = notificationDBManager.GetCurrentUserHistory();

        notifications.ForEach(item => { new MessageComponent(item); });
    }

    private void DeleteAllMessages(ClickEvent evt)
    {
        StartCoroutine(DeleteAllMessagesCoroutine(evt));
        deleteAllBtn.visible = false;
    }

    private IEnumerator DeleteAllMessagesCoroutine(ClickEvent evt)
    {
        foreach (MessageComponent message in messages)
        {
            yield return new WaitForSeconds(0.05f);
            message.DeleteWithAnimation();
        }
    }

    private void Reviewing(float value)
    {
        if (value > maxScrollValue)
            maxScrollValue = value;
        //Debug.Log("Scroll value" + value);
    }

    private void CloseMessagePage(ClickEvent evt)
    {
        mainRoot.style.display = DisplayStyle.Flex;
        messageRoot.RemoveFromClassList("message-template--slide-in");
        mainRoot.RemoveFromClassList("home-template--slide-out-left");
        mainBg.RemoveFromClassList("main-bg--active");

        foreach (MessageComponent message in messages.Where(m => m.isNew == true))
        {
            if (message.localBound.yMin - maxScrollValue < messageScroll.resolvedStyle.height)
            {
                message.isNew = false;
                notificationDBManager.UpdateStateNew(message.id, message.isNew);
            }
        }

        maxScrollValue = 0;
        messageScroll.scrollOffset = Vector2.zero;
    }

    private void HandleMessageSlideInEnd(TransitionEndEvent evt)
    {
        if (!messageRoot.ClassListContains("message-template--slide-in"))
        {
            messageRoot.style.display = DisplayStyle.None;
            mainBg.style.display = DisplayStyle.None;
        }
    }

    static public bool CheckEmptyList()
    {
        messageEmptyContainer = messageRoot.Q<VisualElement>("MessageEmptyContainer");

        if (messageList.Query<MessageComponent>().ToList().Count == 0)
        {
            messageEmptyContainer.style.display = DisplayStyle.Flex;
            empty = true;
        }
        else
        {
            messageEmptyContainer.style.display = DisplayStyle.None;
            empty = false;
        }
        return empty;
    }
}
