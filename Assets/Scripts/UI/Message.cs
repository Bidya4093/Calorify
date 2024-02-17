using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Message : MonoBehaviour

{
    private Button closeBtn;
    public GameObject mainObjectPage;
    private VisualElement messageRoot;
    static private VisualElement mainRoot;
    private VisualElement mainBg;
    static private VisualElement messageEmptyContainer;
    static public VisualElement messageList;
    //public List<MessageComponent> messageList;
    static public bool empty;


    void Start()
    {
        messageRoot = GetComponent<UIDocument>().rootVisualElement;
        mainRoot = mainObjectPage.GetComponent<UIDocument>().rootVisualElement;
        mainBg = mainRoot.Q<VisualElement>("MainBackground");

        closeBtn = messageRoot.Q<Button>("CloseBtn");
        closeBtn.RegisterCallback<ClickEvent>(CloseMessagePage);
        messageRoot.RegisterCallback<TransitionEndEvent>(HandleMessageSlideInEnd);
    }

    private void CloseMessagePage(ClickEvent evt)
    {
        mainRoot.style.display = DisplayStyle.Flex;
        messageRoot.RemoveFromClassList("message-template--slide-in");
        mainRoot.RemoveFromClassList("home-template--slide-out-left");
        mainBg.RemoveFromClassList("main-bg--active");
    }

    private void HandleMessageSlideInEnd(TransitionEndEvent evt)
    {
        if (!messageRoot.ClassListContains("message-template--slide-in"))
        {
            messageRoot.style.display = DisplayStyle.None;
            mainBg.style.display = DisplayStyle.None;
        }
    }

    static public void CheckEmptyList()
    {
        messageEmptyContainer = mainRoot.Q<VisualElement>("MessageEmptyContainer");

        if (messageList.Query<MessageComponent>("MessageItemContainer").ToList().Count == 0)
        {
            messageEmptyContainer.style.display = DisplayStyle.Flex;
            empty = true;
        }
        else
        {
            messageEmptyContainer.style.display = DisplayStyle.None;
            empty = false;
        }
    }
}
