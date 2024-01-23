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
        Debug.Log("Workign");
        scanPanel.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        scanPanel.style.display = DisplayStyle.None;
    }
}
