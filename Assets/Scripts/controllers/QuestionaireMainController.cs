using UnityEngine.UIElements;

public class QuestionaireMainController
{
    private VisualTreeAsset questionaireMain;
    private VisualElement rootElement;
    private ProgressBar timer;

    private ClickEventHandlerManager eventHandlerManager = new ClickEventHandlerManager();

    private bool inProgress = false;
    public bool InProgress
    {
        get { return inProgress; }
    }

    private MCQQuestion question = new MCQQuestion
    { id = "111", shortT = "What is red?", answerIds = new string[] { "111-222", "111-333", "111-444", "111-555" }, correctAnswer = "111-4444", maxTime = 30 };
    private MCQAnswer[] answers = new MCQAnswer[] {
            new MCQAnswer { id = "111-222", shortT = "Red is dead", longT = "This is example text explaning the definition of answer \"Red is DEAD\"." },
            new MCQAnswer { id = "111-333", shortT = "Red is blue", longT = "This is example text explaning the definition of answer \"Red is BLUE\"." },
            new MCQAnswer { id = "111-444", shortT = "Red is hot", longT = "This is example text explaning the definition of answer \"Red is HOT\"." },
            new MCQAnswer { id = "111-555", shortT = "Red is ice", longT = "This is example text explaning the definition of answer \"Red is ICE\"." },
        };

    public delegate void startDelegate();
    public startDelegate onStart;

    private void InitializeDescription(TemplateContainer templateContainer, MCQAnswer answer)
    {
        TemplateContainer descriptionContainer = templateContainer.Q<TemplateContainer>("AnswerDescription");
        Button button = descriptionContainer.Q<Button>("ContinueButton");

        descriptionContainer.Q<Label>("DescriptionText").text = answer.longT;

        button.RegisterCallback<ClickEvent>(RestartQuestion);
    }

    private void HideUnselectedQuestions(string selectedAnswerId)
    {
        for (int idx = 0; idx < question.answerIds.Length; idx++)
        {
            if (selectedAnswerId != question.answerIds[idx])
            {
                TemplateContainer choiceTemplate = rootElement.Q<TemplateContainer>("Answer_" + idx);
                choiceTemplate.AddToClassList("Hide");
                choiceTemplate.RegisterCallback<TransitionEndEvent>(RemoveElement);
            }
        };
    }

    private void CheckAnswer(VisualElement choiceContainer, string selectedAnswerId)
    { 
        if (selectedAnswerId == question.correctAnswer)
        {
            choiceContainer.AddToClassList("Good");
        }
        else
        {
            choiceContainer.AddToClassList("Bad");
        }
    }

    private void EnableDescription (VisualElement choiceContainer)
    {
        TemplateContainer descriptionContainer = choiceContainer.Q<TemplateContainer>("AnswerDescription");
      

        descriptionContainer.AddToClassList("Selected");
    }

    private void RestartQuestion(ClickEvent evt)
    {
        Button button = (Button)evt.currentTarget;
        button.UnregisterCallback<ClickEvent>(RestartQuestion);
        for (int idx = 0; idx < question.answerIds.Length; idx++)
        {
            TemplateContainer choiceTemplate = rootElement.Q<TemplateContainer>("Answer_" + idx);
            choiceTemplate.ClearClassList();
            choiceTemplate.UnregisterCallback<TransitionEndEvent>(RemoveElement);

            TemplateContainer descriptionContainer = choiceTemplate.Q<TemplateContainer>("AnswerDescription");
            descriptionContainer.ClearClassList();
        };

        onStart.Invoke();
    }

    private void RemoveElement(TransitionEndEvent evt)
    {
        TemplateContainer choiceContainer = (TemplateContainer)evt.currentTarget;

        choiceContainer.AddToClassList("Removed");
    }

    public void InitializeQuestionaire(VisualElement root, VisualTreeAsset visualTree)
    {
        questionaireMain = visualTree;

        root.Q<Label>("QuestionText").text = question.shortT;
        for (int idx = 0; idx < question.answerIds.Length; idx++)
        {
            InitializeAnswer(root, question.id, idx, answers[idx]);
        }

        timer = root.Q<ProgressBar>("Timer");

        timer.lowValue = 0;
        timer.highValue = question.maxTime;
        rootElement = root;

        inProgress = true;
    }

    public void InitializeAnswer(VisualElement root, string questionId, int index, MCQAnswer answer) {
        TemplateContainer templateContainer = root.Q<TemplateContainer>("Answer_" + index);
        InitializeDescription(templateContainer, answer);
        VisualElement AnswerContainer = templateContainer.Q<VisualElement>("Container");
        Label label = templateContainer.Q<Label>("Text");
        label.text = answer.shortT;

        eventHandlerManager.Register(AnswerContainer, evt => OnClick(evt, questionId, answer.id));
    }

    public void OnClick(ClickEvent evt, string questionId, string answerId) {
        VisualElement answerContainer = (VisualElement)evt.currentTarget;
        TemplateContainer parentContainer = (TemplateContainer)answerContainer.parent;
        Label label = parentContainer.Q<Label>("Text");
       
        UnityEngine.Debug.Log($"Answer clicked: {questionId} => {answerId} / {label.text}...");

        eventHandlerManager.UnregisterAll();

        CheckAnswer(parentContainer, answerId);

        EnableDescription(parentContainer);

        HideUnselectedQuestions(answerId);

        inProgress = false;
    }

    public void UpdateTimer (float time)
    {
        timer.value = time;

        if (time >= question.maxTime)
        {
            // TIMES UP
            UnityEngine.Debug.Log("TIMES UP!");
            inProgress = false;
        };
    }
}
