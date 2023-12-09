using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{
    public void ShowPassword(TMP_InputField input)
    {
        if (input.contentType == TMP_InputField.ContentType.Password)
        {
            input.contentType = TMP_InputField.ContentType.Standard;
        }
        else if (input.contentType == TMP_InputField.ContentType.Standard)
        {
            input.contentType = TMP_InputField.ContentType.Password;
        }
        input.Select();
    }
}
