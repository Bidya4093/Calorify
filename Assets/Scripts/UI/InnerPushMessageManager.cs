using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class InnerPushMessageManager : MonoBehaviour
{
    static private List<PushMessage> queue = new List<PushMessage>();
    private VisualElement root;
    static private VisualElement pushMessageContainer = null;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        pushMessageContainer = root.Q<VisualElement>("PushMessageContainer");
    }

    public void AddToQueue(PushMessage message)
    {
        queue.Add(message);
        if (pushMessageContainer.childCount == 0 && queue.Count == 1)
        {
            pushMessageContainer.Add(message);
        }
        Debug.Log(queue.Count);
    }

    public void OpenMessage()
    {

    }

    public async void UpdateQueue()
    {
        await Task.Delay(1000);
        NotificationDBManager notificationDBManager = new NotificationDBManager();
        notificationDBManager.UpdateStateViewedAsPushMessage(queue[0].id, true);
        queue.RemoveAt(0);
        if (queue.Count > 0)
        {
            pushMessageContainer.Add(queue[0]);
        }
    }
}
