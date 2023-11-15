using System;
using TMPro;
using UnityEngine;
using static PanelChanger;

public class SignUp : MonoBehaviour
{
    // Поля вводу даних
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField heightInput;
    public TMP_InputField weightInput;
    public TMP_Text ErrorOutput;

    void Start()
    {
        if (ErrorOutput.text.Length != 0)
        {
            ErrorOutput.text = "";
        }
    }
    public void GetUserDataFromPanel()
    {
        switch (currentAuthPanelIndex)
        {
            case 1:
                // sign up panel
                try
                {
                    User.SetEmail(emailInput.text);
                    User.SetPassword(passwordInput.text);

                    if (ErrorOutput.text.Length != 0)
                    {
                        ErrorOutput.text = "";
                    }
                } catch (Exception e)
                {
                    ErrorOutput.text = e.Message;
                    throw e;
                }

                break;
            case 4:
                // name panel
                try
                {
                    User.SetUsername(usernameInput.text);

                    if (ErrorOutput.text.Length != 0)
                    {
                        ErrorOutput.text = "";
                    }
                }
                catch (Exception e)
                {
                    ErrorOutput.text = e.Message;
                    throw e;
                }

                break;
            case 5:
                // height panel
                try
                {
                    User.SetHeight(heightInput.text);

                    if (ErrorOutput.text.Length != 0)
                    {
                        ErrorOutput.text = "";
                    }
                }
                catch (Exception e)
                {
                    ErrorOutput.text = e.Message;
                    throw e;
                }
                break;
            case 6:
                // weight panel
                try
                {
                    User.SetWeight(weightInput.text);

                    if (ErrorOutput.text.Length != 0)
                    {
                        ErrorOutput.text = "";
                    }
                }
                catch (Exception e)
                {
                    ErrorOutput.text = e.Message;
                    throw e;
                }
                break;
            case 7:
                //DBWorking.RegisterUser();

                usernameInput.text = "";
                emailInput.text = "";
                passwordInput.text = "";
                weightInput.text = "";
                heightInput.text = "";

                if (ErrorOutput.text.Length != 0)
                {
                    ErrorOutput.text = "";
                }

                break;
        }
    }

    public void SetGoal(short option)
    {
        User.SetGoal(option);
    }

    public void SetActivity(short option)
    {
        User.SetActivity(option);
    }
}

// Todo: 
// Виводити помилки у інтерфейс