using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductPanel : MonoBehaviour
{
    private VisualElement productPanelBackground;
    private VisualElement productPanelRoot;

    void Start()
    {
        productPanelRoot = GetComponent<UIDocument>().rootVisualElement;
        productPanelBackground = productPanelRoot.Q<VisualElement>("ProductPanelBackground");

        //productPanelBackground.RegisterCallback<ClickEvent>(CloseProductPanel);
    }

    private void CloseProductPanel(ClickEvent evt)
    {
        productPanelRoot.style.display = DisplayStyle.None;
    }
}
