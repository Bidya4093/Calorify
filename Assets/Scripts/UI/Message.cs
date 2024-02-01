using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Message : MonoBehaviour

{
    private Button closeBtn;
    public GameObject mainObjectPage;
    private VisualElement messageRoot;
    private VisualElement mainRoot;
    private VisualElement mainBg;


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
}
