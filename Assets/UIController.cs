using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class UIController : MonoBehaviour
{
    public Button scanButton;
    public GameObject scanPanel;
    //public UIDocument UIDocument;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        scanButton = root.Q<Button>("scan-btn");

        scanButton.clicked += scanButtonPressed;
    }

    // Update is called once per frame
    void scanButtonPressed()
    {
        Debug.Log("Button Pressed;");
        scanPanel.SetActive(true);
        //GetComponent<UIDocument>().rootVisualElement.SetEnabled
        //UIDocument
        GetComponent<UIDocument>().gameObject.SetActive(false);
    }
}
