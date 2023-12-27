using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowPopUp : MonoBehaviour
{
    private VisualElement scanPanel;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        scanPanel = root.Q<VisualElement>("ScanPanelContainer");
    }
    public void Show()
    {
        //scanPanel.SetEnabled(true);
        scanPanel.style.display = DisplayStyle.Flex;
        Debug.Log("Crisps detected");
    }

    public void Hide()
    {
        //scanPanel.SetEnabled(false);
        scanPanel.style.display = DisplayStyle.None;
    }
}
