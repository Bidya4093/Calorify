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


    void Start()
    {
        messageRoot = GetComponent<UIDocument>().rootVisualElement;
        mainRoot = mainObjectPage.GetComponent<UIDocument>().rootVisualElement;

        closeBtn = messageRoot.Q<Button>("CloseBtn");
        closeBtn.RegisterCallback<ClickEvent>(CloseMessagePage);
    }

    private void CloseMessagePage(ClickEvent evt)
    {
        messageRoot.style.display = DisplayStyle.None;
        mainRoot.style.display = DisplayStyle.Flex;
    }
}
