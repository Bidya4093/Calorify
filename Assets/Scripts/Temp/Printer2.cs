using Firebase.Auth;
using UnityEngine;

public class Printer2 : MonoBehaviour
{
    private void Start()
    {
        ActivitiesManager activitiesManager = new ActivitiesManager();

        foreach(Activity activity in activitiesManager.activitiesList)
        {
            Debug.Log(activity);
        }
    }
}
