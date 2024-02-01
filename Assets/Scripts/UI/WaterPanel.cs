using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterPanel : MonoBehaviour
{
    private VisualElement waterPanelBackground;
    private VisualElement waterPanelRoot;

    private ProgressBar waterProgressBar;

    private Label waterRemainsValueLabel;

    private IntegerField waterPanelInput;
    private Label waterPanelInputPrefix;

    private List<WaterButton> waterAddBtns;

    void Start()
    {
        waterPanelRoot = GetComponent<UIDocument>().rootVisualElement;
        waterPanelBackground = waterPanelRoot.Q<VisualElement>("WaterPanelBackground");
        waterProgressBar = waterPanelRoot.Q<ProgressBar>("WaterProgressBar");
        waterPanelInput = waterPanelRoot.Q<IntegerField>("WaterPanelInput");
        waterPanelInputPrefix = waterPanelRoot.Q<Label>("WaterPanelInputPrefix");
        waterRemainsValueLabel = waterPanelRoot.Q<Label>("WaterPanelRemainsValue");

        waterAddBtns = waterPanelRoot.Query<WaterButton>("WaterButton").ToList();

        waterAddBtns.ForEach(btn =>
        {
            btn.RegisterCallback<ClickEvent>(AddWater);
        });

        waterPanelBackground.RegisterCallback<ClickEvent>(CloseWaterPanel);

        waterPanelInput.RegisterValueChangedCallback(UpdateWater);
        waterProgressBar.RegisterValueChangedCallback(ChangeWaterProgress);

    }

    private void ChangeWaterProgress(ChangeEvent<float> evt)
    {
        VisualElement progressBarProgress = (evt.currentTarget as ProgressBar).Query(className: "unity-progress-bar__progress");
        progressBarProgress.style.width = Length.Percent(evt.newValue);
    }

    private void CloseWaterPanel(ClickEvent evt)
    {
        waterPanelRoot.style.display = DisplayStyle.None;
    }

    private void AddWater(ClickEvent evt)
    {
        int waterAmount = (evt.target as WaterButton).waterAmount;
        User.AddWater(waterAmount);
        StartCoroutine(FirebaseManager.UpdateUserValue("waterDrunk", User.Instance.WaterDrunk));
        DataManager.RenderWaterData();
        RenderWaterData();
    }
    private void UpdateWater(ChangeEvent<int> evt)
    {
        User.SetWater(evt.newValue);
        StartCoroutine(FirebaseManager.UpdateUserValue("waterDrunk", User.Instance.WaterDrunk));
        DataManager.RenderWaterData();
        RenderWaterData();
    }

    public void LoadWaterData()
    {
        RenderWaterData();
        waterPanelInputPrefix.text = $"/ {User.Instance.WaterNeeded} мл.";
    }

    public void RenderWaterData()
    {
        waterRemainsValueLabel.text = User.Instance.WaterNeeded - User.Instance.WaterDrunk + " мл.";
        waterProgressBar.value = ((float)User.Instance.WaterDrunk / (float)User.Instance.WaterNeeded) * 100f;
        waterPanelInput.value = User.Instance.WaterDrunk;
    }
}
