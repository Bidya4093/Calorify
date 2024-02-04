using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HistoryProductPanel : MonoBehaviour
{
    static public VisualElement productPanelRoot;
    static public VisualElement productPanelBackground;

    static public Button deleteBtn;
    static public IntegerField massInput;
    static public Button saveChangesBtn;

    static public Label caloriesLabel;
    static public Label protsLabel;
    static public Label fatsLabel;
    static public Label carbsLabel;

    static public VisualElement nutriScoreBadge;
    static public Label nutriScoreLabel;
    static public Label title;
    static public Label dateLabel;

    void Start()
    {
        productPanelRoot = GetComponent<UIDocument>().rootVisualElement;
        productPanelBackground = productPanelRoot.Q<VisualElement>("ProductPanelBackground");

        deleteBtn = productPanelRoot.Q<Button>("ProductPanelDeleteBtn");
        massInput = productPanelRoot.Q<IntegerField>("ProductPanelWeightInput");
        saveChangesBtn = productPanelRoot.Q<Button>("ProductPanelBtn");

        caloriesLabel = productPanelRoot.Q<Label>("MacrosCaloriesValue");
        protsLabel = productPanelRoot.Q<Label>("MacrosProtsValue");
        fatsLabel = productPanelRoot.Q<Label>("MacrosFatsValue");
        carbsLabel = productPanelRoot.Q<Label>("MacrosCarbsValue");

        nutriScoreBadge = productPanelRoot.Q<VisualElement>("HistoryItemScore");
        nutriScoreLabel = nutriScoreBadge.Q<Label>();
        title = productPanelRoot.Q<Label>("ProductPanelTitle");
        dateLabel = productPanelRoot.Q<Label>("ProductPanelDate");
    }
}
