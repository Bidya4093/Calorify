using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyUILibrary
{
    public class Circle : VisualElement
    {
        // Define a factory class to expose this control to UXML.
        public new class UxmlFactory : UxmlFactory<Circle, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            // The progress property is exposed to UXML.
            UxmlFloatAttributeDescription m_CircleAttribute = new UxmlFloatAttributeDescription()
            {
                name = "sweep"
            };


            // Use the Init method to assign the value of the progress UXML attribute to the C# progress property.
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                (ve as Circle).sweep = m_CircleAttribute.GetValueFromBag(bag, cc);

            }
        }

        // These are USS class names for the control overall and the label.
        public static readonly string ussClassName = "circle";

        // These objects allow C# code to access custom USS properties.
        static CustomStyleProperty<Color> s_CircleColor = new CustomStyleProperty<Color>("--circle-color");
        static CustomStyleProperty<float> s_CircleLineWidth = new CustomStyleProperty<float>("--circle-line-width");


        Color m_CircleColor = Color.gray;
        private float m_CircleLineWidth = 10.0f;

        // This is the number that the Label displays as a percentage.
        [Range(0.0f, 100.0f)]
        float m_Sweep;

        // A value between 0 and 100
        public float sweep
        {
            get => m_Sweep;
            set
            {
                if (value >= 0 || value <= 100)
                {
                    m_Sweep = value;
                }
                MarkDirtyRepaint();
            }
        }

        public Circle()
        {
            AddToClassList(ussClassName);

            // Register a callback after custom style resolution.
            RegisterCallback<CustomStyleResolvedEvent>(evt => CustomStylesResolved(evt));

            // Register a callback to generate the visual content of the control.
            generateVisualContent += GenerateVisualContent;

            sweep = 0.0f;
        }

        static void CustomStylesResolved(CustomStyleResolvedEvent evt)
        {
            Circle element = (Circle)evt.currentTarget;
            element.UpdateCustomStyles();
        }

        // After the custom colors are resolved, this method uses them to color the meshes and (if necessary) repaint
        // the control.
        void UpdateCustomStyles()
        {
            bool repaint = false;
            if (customStyle.TryGetValue(s_CircleColor, out m_CircleColor))
                repaint = true;


            if (repaint)
                MarkDirtyRepaint();
        }

        void GenerateVisualContent(MeshGenerationContext context)
        {
            float width = contentRect.width;
            float height = contentRect.height;
            float lineWidth = m_CircleLineWidth;

            if (customStyle.TryGetValue(s_CircleLineWidth, out m_CircleLineWidth))
                lineWidth = m_CircleLineWidth;
            var painter = context.painter2D;
            painter.lineWidth = lineWidth;
            painter.lineCap = LineCap.Round;

            // Draw the circle
            painter.strokeColor = m_CircleColor;
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), (width-lineWidth) * 0.5f, 0.0f, -360.0f*((sweep / 100.0f)));
            painter.Stroke();

        }
    }
}