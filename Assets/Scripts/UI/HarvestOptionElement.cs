using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class HarvestOptionElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<HarvestOptionElement, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(visualElement, bag, context);
                var harvestOption = visualElement as HarvestOptionElement;
                
                harvestOption.Clear();

                VisualElement harvestElementContainer = new VisualElement();
                harvestOption.Add(harvestElementContainer);

                Button harvestButton = new Button();
                harvestElementContainer.Add(harvestButton);
                
                SimpleProgressBar progressBar = new SimpleProgressBar();
                progressBar.AddToClassList("HarvestProgressBar");
                harvestOption.Add(progressBar);
            }
        }
    }
}
