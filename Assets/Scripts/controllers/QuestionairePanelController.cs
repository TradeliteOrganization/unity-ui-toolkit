using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionairePanelController : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset questionChoice;

    private QuestionaireMainController questionController;

    private float time = 0.0f;

    void OnEnable()
    {
        // Initialize the character list controller
        questionController = new QuestionaireMainController();
        questionController.onStart += StartQuestion;
        StartQuestion();
    }

    void StartQuestion()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        questionController.InitializeQuestionaire(uiDocument.rootVisualElement, questionChoice);
        time = 0.0f;
    }

    void Update()
    {
        if (questionController.InProgress)
        {
            time += Time.deltaTime;
            questionController.UpdateTimer(time);
        }
    }

}
