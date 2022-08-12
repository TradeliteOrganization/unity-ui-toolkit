using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionairePanelController : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset questionChoice;

    void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        var questionaireMainController = new QuestionaireMainController();
        questionaireMainController.InitializeQuestionaire(uiDocument.rootVisualElement, questionChoice);
    }
}
