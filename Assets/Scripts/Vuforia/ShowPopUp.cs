using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowPopUp : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textMeshPro;

    public void Show(int index)
    {
        panel.SetActive(true);
        textMeshPro.text = index.ToString();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
