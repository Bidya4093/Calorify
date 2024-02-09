using UnityEngine.UIElements;

public class WaterButton : Button
{
    public new class UxmlFactory : UxmlFactory<WaterButton, UxmlTraits> { }
    public new class UxmlTraits : Button.UxmlTraits
    {
        UxmlIntAttributeDescription m_WaterAmountAttr = new UxmlIntAttributeDescription { name = "water-amount", defaultValue = 0 };
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            WaterButton ate = ve as WaterButton;
            ate.waterAmount = m_WaterAmountAttr.GetValueFromBag(bag, cc);
        }
    }

    public WaterButton() {
        AddToClassList("water__add-btn");
        text = "Water button";
    }

    public WaterButton(string _text)
    {
        AddToClassList("water__add-btn");
        text = _text;
    }

    public int m_WaterAmount;

    public int waterAmount
    {
        // The progress property is exposed in C#.
        get => m_WaterAmount;
        set
        {
            text = $"+ {value} мл.";
            m_WaterAmount = value;
        }
    }
}