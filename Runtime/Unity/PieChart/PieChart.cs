using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitLibrary.Runtime.Unity.PieChart {
    public class PieChart : BindableElement
    {
        float m_Radius = 100.0f;
        float m_Value = 40.0f;

        VisualElement m_Chart;

        public float radius
        {
            get => m_Radius;
            set
            {
                m_Radius = value;
                m_Chart.style.height = diameter;
                m_Chart.style.width = diameter;
                m_Chart.MarkDirtyRepaint();
            }
        }

        public float diameter => m_Radius * 2.0f;

        public float value {
            get { return m_Value; }
            set { m_Value = value; MarkDirtyRepaint(); }
        }

        public PieChart() {
            m_Chart = this;
            generateVisualContent += DrawCanvas;
        }
        
        // public PieChart(string label) : this()
        // {
        //     this.label = label;
        // }

        void DrawCanvas(MeshGenerationContext ctx)
        {
            var painter = ctx.painter2D;
            painter.strokeColor = Color.white;
            painter.fillColor = Color.white;

            var percentage = m_Value;

            var percentages = new float[] {
                percentage, 100 - percentage
            };
            var colors = new Color32[] {
                new Color32(182,235,122,255),
                new Color32(251,120,19,255)
            };
            float angle = 0.0f;
            float anglePct = 0.0f;
            int k = 0;
            foreach (var pct in percentages)
            {
                anglePct += 360.0f * (pct / 100);

                painter.fillColor = colors[k++];
                painter.BeginPath();
                painter.MoveTo(new Vector2(m_Radius, m_Radius));
                painter.Arc(new Vector2(m_Radius, m_Radius), m_Radius, angle, anglePct);
                painter.Fill();

                angle = anglePct;
            }
        }
        
        public new class UxmlFactory : UxmlFactory<PieChart, UxmlTraits> { }
        
        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlFloatAttributeDescription m_Radius = new UxmlFloatAttributeDescription { name = "radius", defaultValue = 100.0f };
            UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription { name = "value", defaultValue = 40.0f };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                
                var pieChart = (PieChart)ve;
                pieChart.radius = m_Radius.GetValueFromBag(bag, cc);
                pieChart.value = m_Value.GetValueFromBag(bag, cc);
            }
            
        }
    }
}