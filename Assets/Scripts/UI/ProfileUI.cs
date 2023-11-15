using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{

    public TMP_InputField profileNameInput;
    public TMP_InputField profileHeightInput;
    public TMP_InputField profileWeightInput;
    public Toggle[] profileGoalToggles;
    public ToggleGroup profileGoalToggleGroup;

    void Start()
    {
        Debug.Log("Profile Started");
        profileNameInput.text = User.GetUsername();
        profileHeightInput.text = User.GetHeight().ToString();
        profileWeightInput.text = User.GetWeight().ToString();
        profileGoalToggles[User.GetGoal()].isOn = true;

        profileNameInput.onEndEdit.AddListener(UpdateUsername);
        profileHeightInput.onEndEdit.AddListener(UpdateHeight);
        profileWeightInput.onEndEdit.AddListener(UpdateWeight);
    }


    public void UpdateUsername(string username)
    {
        try
        {
            User.SetUsername(username);
        }
        catch
        {
            profileNameInput.text = User.GetUsername();
        }
    }

    public void UpdateHeight(string height)
    {

        Debug.Log("Height: " + height);

        try
        {
            User.SetHeight(float.Parse(height));
        }
        catch
        {
            profileHeightInput.text = User.GetHeight().ToString();
        }
    }

    public void UpdateWeight(string weight)
    {
        Debug.Log("Weight: " + weight);

        try
        {
            User.SetWeight(float.Parse(weight));
        }
        catch
        {
            profileWeightInput.text = User.GetWeight().ToString();
        }
    }

    public void UpdateGoal(short goal)
    {
        Debug.Log("GoalType: " + goal);
        User.SetGoal(goal);
    }
}

// Зберігати зміненні дані в профілю
// Записувати дані після зміни в базу даних