using UnityEngine;
using Vuforia;

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    static CloudRecoBehaviour mCloudRecoBehaviour;
    bool mIsScanning = false;
    string mTargetId = "";

    public ImageTargetBehaviour ImageTargetTemplate;

    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
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

        // �������� ����� ��'���� � ���������
        Debug.Log("Meta data id : " + cloudRecoSearchResult.UniqueTargetId);
        Debug.Log("Meta data target name: " + cloudRecoSearchResult.TargetName);
        mTargetId = cloudRecoSearchResult.UniqueTargetId;
        string targetName = cloudRecoSearchResult.TargetName;


        if (!string.IsNullOrEmpty(targetName))
        {
            // ������� ���������� �� ����� ����� ��'����
            try
            {
                GameObject.Find("MainPage").GetComponent<ScanPanelManager>().LoadProductData(mTargetId);

            }
            catch (System.Exception ex)
            {
                GUIStyle style = new GUIStyle("sdfa");
                style.fontSize = 60;
                GUI.Label(new Rect((Screen.width - 600) / 2, (Screen.height - 150) / 2, 600, 150), ex.Message, style);
            }

            // �������� ����������, ��������� ��������
            //mCloudRecoBehaviour.enabled = false;

            // ���������� ���������� �� ����� ��'����
            //if (ImageTargetTemplate)
            //{
            //    // �������� ����� ��������� � �� ����� ImageTargetBehaviour
            //    mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
            //}
        }
    }

    public void ResetBehaviour()
    {
        mTargetId = "";
        mCloudRecoBehaviour.enabled = true;
    }
}




