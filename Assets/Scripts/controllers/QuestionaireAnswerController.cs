using System;
using UnityEngine.UIElements;

public class QuestionaireAnswerController
{
    private VisualElement answerContainer;
    private VisualTreeAsset answerChoiceAsset;

    private ClickEventHandlerManager eventChoiceHandlerManager = new();
    private ClickEventHandlerManager eventContinueHandlerManager = new();

    public QuestionaireAnswerController(VisualElement container, VisualTreeAsset asset)
    {
        answerContainer = container;
        answerChoiceAsset = asset;
    }

    public void Initialize(MCQAnswer[]answers, Action<ClickEvent, string>choiceClickCallback, EventCallback<ClickEvent> continueCallback)
    {
        answerContainer.Clear();
        eventChoiceHandlerManager.UnregisterAll();
        eventContinueHandlerManager.UnregisterAll();

        for (int idx = 0; idx < answers.Length; idx++)
        {
            TemplateContainer choiceTemplate = createAnswerChoiceTemplate(answers[idx].id);
            PopulateAnswerChoice(choiceTemplate, answers[idx], idx);
            InitializeAnwserDescription(choiceTemplate, answers[idx]);
            SetEventCallbacks(choiceTemplate, answers[idx], choiceClickCallback, continueCallback);
        }
    }

    public void ChoiceSelected(TemplateContainer choiceContainer)
    {
        EnableDescription(choiceContainer);
        HideUnselectedQuestions(choiceContainer.name);
    }

    public void RemoveOneAnswer(string answerId)
    {
        foreach (var answerElement in answerContainer.Children())
        {
            FloatElement(answerElement);

            if (answerElement.name == "Answer_" + answerId)
            {
                answerElement.AddToClassList("Hide");
                answerElement.RegisterCallback<TransitionEndEvent>(RemoveElement);
            }
        }
    }

    private TemplateContainer createAnswerChoiceTemplate(string id)
    {
        TemplateContainer choiceTemplate = answerChoiceAsset.Instantiate();
        choiceTemplate.name = "Answer_" + id;

        choiceTemplate.AddToClassList("Spacer");

        answerContainer.Add(choiceTemplate);

        return choiceTemplate;
    }

    private void PopulateAnswerChoice(TemplateContainer choiceTemplate, MCQAnswer answer, int idx)
    {
        InitializeAnwserDescription(choiceTemplate, answer);

        Label id = choiceTemplate.Q<Label>("Id");
        id.text = ((char)(idx + 65)).ToString().ToUpper(); ;

        Label answerText = choiceTemplate.Q<Label>("Text");
        answerText.text = answer.shortT;
    }

    private void InitializeAnwserDescription(TemplateContainer choiceTemplate, MCQAnswer answer)
    {
        TemplateContainer descriptionContainer = choiceTemplate.Q<TemplateContainer>("AnswerDescription");

        descriptionContainer.Q<Label>("DescriptionText").text = answer.longT;
    }

    private void SetEventCallbacks(TemplateContainer choiceTemplate, MCQAnswer answer, Action<ClickEvent, string> choiceClickCallback, EventCallback<ClickEvent> continueCallback)
    {
        VisualElement answerContainer = choiceTemplate.Q<VisualElement>("Container");
        Button button = choiceTemplate.Q<Button>("ContinueButton");

        eventChoiceHandlerManager.Register(answerContainer, evt => choiceClickCallback.Invoke(evt, answer.id));
        eventContinueHandlerManager.Register(button, continueCallback);
    }

    private void EnableDescription(VisualElement choiceContainer)
    {
        TemplateContainer descriptionContainer = choiceContainer.Q<TemplateContainer>("AnswerDescription");
        descriptionContainer.AddToClassList("Selected");
    }

    private void HideUnselectedQuestions(string answerName)
    {
        foreach (var answerElement in answerContainer.Children())
        {
            FloatElement(answerElement);

            if (answerElement.name != answerName)
            {
                answerElement.AddToClassList("Hide");
                answerElement.RegisterCallback<TransitionEndEvent>(RemoveElement);
            }
            else
            {
                answerElement.style.top = 0;
            }
        }
    }

    private void FloatElement(VisualElement element)
    {
        element.style.top = element.layout.position.y - 20;
        element.style.position = Position.Absolute;
    }

    private void RemoveElement(TransitionEndEvent evt)
    {
       ((TemplateContainer)evt.currentTarget).RemoveFromHierarchy();
    }
}
