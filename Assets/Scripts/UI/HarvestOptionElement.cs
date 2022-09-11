using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class HarvestOptionElement : VisualElement
    {
        public string HarvestOptionText { get; set; }
        public Button HarvestButton { get; set; }
        public SimpleProgressBar ProgressBar { get; set; }

        public new class UxmlFactory : UxmlFactory<HarvestOptionElement, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription HarvestOptionTextAttr = new UxmlStringAttributeDescription { name = "Option_Text", defaultValue = "Harvest" };
            UxmlIntAttributeDescription SampleImageCountAttr = new UxmlIntAttributeDescription { name = "Sample_Image_Count", defaultValue = 3 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(visualElement, bag, context);
                var harvestOption = visualElement as HarvestOptionElement;

                harvestOption.HarvestOptionText = HarvestOptionTextAttr.GetValueFromBag(bag, context);

                harvestOption.Clear();

                VisualElement upperContainer = new VisualElement();
                upperContainer.name = "HarvestOptionUpperContainer";
                upperContainer.AddToClassList("HarvestOptionUpperContainer");
                harvestOption.Add(upperContainer);

                Button harvestButton = new Button();
                harvestButton.name = "HarvestOptionButton";
                harvestButton.text = harvestOption.HarvestOptionText;
                harvestButton.AddToClassList("GameMenuNavButton");
                upperContainer.Add(harvestButton);
                harvestOption.HarvestButton = harvestButton;

                VisualElement itemIconContainer = new VisualElement();
                itemIconContainer.name = "HarvestOptionItemContainer";
                itemIconContainer.AddToClassList("HarvestOptionItemContainer");
                upperContainer.Add(itemIconContainer);

                AddSampleIcons(harvestOption, SampleImageCountAttr.GetValueFromBag(bag, context));
                
                SimpleProgressBar progressBar = new SimpleProgressBar();
                progressBar.name = "HarvestOptionProgressBar";
                progressBar.AddToClassList("HarvestProgressBar");
                harvestOption.Add(progressBar);
                harvestOption.ProgressBar = progressBar;
            }

            private void AddSampleIcons(HarvestOptionElement harvestOption, int sampleImageCount)
            {
                List<Texture2D> textures = new List<Texture2D>();

                for (int i = 0; i < sampleImageCount; i++)
                {
                    textures.Add(null);
                }
                
                harvestOption.SetItemIcons(textures);
            }
        }
        
        public void SetItemIcons(IEnumerable<Texture> icons)
        {
            VisualElement iconContainer = this.Q<VisualElement>("HarvestOptionItemContainer");
            iconContainer.Clear();

            foreach (Texture icon in icons)
            {
                Image iconImage = new Image();
                iconImage.image = icon;
                iconImage.AddToClassList("HarvestOptionItemIcon");
                iconContainer.Add(iconImage);
            }
        }
    }
}
