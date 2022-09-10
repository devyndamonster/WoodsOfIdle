using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class SimpleProgressBar : VisualElement
    {
        public float Progress { get; set; }
        
        public new class UxmlFactory : UxmlFactory<SimpleProgressBar, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlFloatAttributeDescription ProgressAttr = new UxmlFloatAttributeDescription { name = "Progress", defaultValue = 0.5f };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(visualElement, bag, context);
                var progressBar = visualElement as SimpleProgressBar;

                progressBar.Progress = ProgressAttr.GetValueFromBag(bag, context);
                progressBar.Clear();

                VisualElement barElement = new VisualElement();
                progressBar.Add(barElement);
                progressBar.SetProgress(progressBar.Progress);
            }
        }

        public void SetProgress(float progress)
        {
            Progress = progress;
            VisualElement barElement = Children().First();
            barElement.style.width = new StyleLength(Length.Percent(Progress * 100));
        }
    }
}
