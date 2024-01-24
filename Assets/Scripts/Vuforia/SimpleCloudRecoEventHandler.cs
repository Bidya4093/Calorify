using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;
using static Vuforia.CloudRecoBehaviour;
using Vuforia;
using UnityEngine;
using Vuforia;

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    CloudRecoBehaviour mCloudRecoBehaviour;
    bool mIsScanning = false;
    string mTargetMetadata = "";

    public ImageTargetBehaviour ImageTargetTemplate;

    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        Debug.Log(mCloudRecoBehaviour);
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        Debug.Log(mCloudRecoBehaviour);
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());

    }
    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        if (scanning)
        {
            // Clear all known targets
        }
    }

    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        // Видобути назву об'єкта з метаданих
        Debug.Log("Result: " + cloudRecoSearchResult);
        Debug.Log("Meta data id : " + cloudRecoSearchResult.UniqueTargetId);
        Debug.Log("Meta data tracking rating: " + cloudRecoSearchResult.TrackingRating);
        Debug.Log("Meta data target name: " + cloudRecoSearchResult.TargetName);


        Debug.Log("Meta data: " + cloudRecoSearchResult.MetaData);

        string targetName = cloudRecoSearchResult.TargetName;

        if (!string.IsNullOrEmpty(targetName))
        {
            // Вивести інформацію на основі назви об'єкта
            DisplayInformationForTarget(targetName);

            // Зупинити сканування, вимкнувши поведінку
            mCloudRecoBehaviour.enabled = false;

            // Побудувати доповнення на основі об'єкта
            if (ImageTargetTemplate)
            {
                // Увімкнути новий результат з тією самою ImageTargetBehaviour
                mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
            }
        }
    }

    // Власний метод для відображення інформації на основі назви об'єкта
    void DisplayInformationForTarget(string targetName)
    {
        // Відобразити інформацію на основі назви об'єкта
        switch (targetName)
        {
            case "lays_123":
                Debug.Log("Відображення інформації для Об'єкта1");
                
                break;
            case "grechka":
                Debug.Log("калорії: 159");
                // виводить інформацію
                break;
           
            default:
                Debug.Log("Невідома назва об'єкта: " + targetName);
                break;
        }
    }


    // Here we handle a cloud target recognition event
    //public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    //{
    //    // Store the target metadata
    //    mTargetMetadata = cloudRecoSearchResult.MetaData;

    //    // Stop the scanning by disabling the behaviour
    //    mCloudRecoBehaviour.enabled = false;
    //    // Build augmentation based on target 



    //    if (ImageTargetTemplate)
    //    {
    //        /* Enable the new result with the same ImageTargetBehaviour: */
    //        mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
    //    }
    //}

    void OnGUI()
    {
        // Display current 'scanning' status
        GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
        // If not scanning, show button
        // so that user can restart cloud scanning

        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
            {
                // Reset Behaviour
                mCloudRecoBehaviour.enabled = true;
                mTargetMetadata = "";
            }
        }
    }
}




