using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public VisualTreeAsset SaveSelectOption;

    private UIDocument rootDocument;
    private VisualElement rootElement;

    private void Awake()
    {
        rootDocument = GetComponent<UIDocument>();
        rootElement = rootDocument.rootVisualElement;

        VisualElement saveSelectOption = SaveSelectOption.CloneTree();
    }
}
