using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelChanger : MonoBehaviour
{
    public GameObject[] mainPanels;
    public GameObject[] authPanels;
    static public int currentAuthPanelIndex = 0;

    public GameObject signupPanel;
    public GameObject loginPanel;
    public GameObject menuPanel;

    public Button profileButton;
    public Button nextButton;
    public Button previousButton;
    public Sprite openProfileSprite;
    public Sprite closeProfileSprite;

    public TMP_Text ErrorOutput;


    public void LoadPanelByIndex(int index)
    {
        for(int i = 0; i < mainPanels.Length; i++)
        {
            if (index != i)
            {
                mainPanels[i].SetActive(false);
            }
            else if (index == 2) { 
                break; 
            }
            else
            {
                mainPanels[i].SetActive(true);
            }
        }

        if (index == 2)
        {
            mainPanels[2].SetActive(!mainPanels[2].activeSelf);
            mainPanels[0].SetActive(!mainPanels[0].activeSelf);
            menuPanel.SetActive(!menuPanel.activeSelf);

            profileButton.image.sprite = profileButton.image.sprite == closeProfileSprite ? openProfileSprite : closeProfileSprite;
        }

        profileButton.gameObject.SetActive(index == 0 || index == 2);
    }

    public void ToAuthPanel(GameObject authPanel)
    {

        authPanels[0].SetActive(false); // Welcome panel
        authPanel.SetActive(true);
        previousButton.gameObject.SetActive(true);

        if (authPanel == signupPanel)
        {
            nextButton.gameObject.SetActive(true);
        }

        currentAuthPanelIndex++;

    }

    public void SwapAuthPanel()
    {
        loginPanel.SetActive(!loginPanel.activeSelf);
        signupPanel.SetActive(!signupPanel.activeSelf);

        if (signupPanel.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }
        else if (loginPanel.activeSelf)
        {
            nextButton.gameObject.SetActive(false);
        }

    }

    public void ToNextPanel()
    {
        authPanels[currentAuthPanelIndex].SetActive(false);
        currentAuthPanelIndex++;
        if (currentAuthPanelIndex == authPanels.Length)
        {
            nextButton.gameObject.SetActive(false);
            SceneManager.LoadScene("MainScreen");
            return;
        }
        authPanels[currentAuthPanelIndex].SetActive(true);
        if (ErrorOutput.text.Length != 0)
        {
            ErrorOutput.text = "";
        }
    }

    public void ToPreviousPanel()
    {
        authPanels[currentAuthPanelIndex].SetActive(false);

        if (loginPanel.activeSelf && currentAuthPanelIndex == 1)
            loginPanel.SetActive(false);

        currentAuthPanelIndex--;
        authPanels[currentAuthPanelIndex].SetActive(true);

        if (currentAuthPanelIndex == 0)
        {
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
        }
        if (ErrorOutput.text.Length != 0)
        {
            ErrorOutput.text = "";
        }
    }
}
