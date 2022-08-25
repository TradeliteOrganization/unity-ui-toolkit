using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionairePanelController : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset questionChoice;
    [SerializeField]
    private ScriptableQuestion[] questions;
    [SerializeField]
    private ScriptableAnswer[] answers;

    private VisualElement rootElement;

    private QuestionaireAnswerController answerController;
    private QuestionaireBoosterController boosterController;
    private TimerController timerController;

    private MCQQuestion currentQuestion;
    private MCQAnswer[] currentAnswers;

    private int questionIndex = 0;
    private bool questionInProgress = false;
    private float time = 0.0f;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        VisualElement answerContainer = rootElement.Q<VisualElement>("AnswerContainer");
        answerController = new QuestionaireAnswerController(answerContainer, questionChoice);

        timerController = new TimerController(rootElement.Q<ProgressBar>("Timer"));

        boosterController = new QuestionaireBoosterController(rootElement, OnRemoveAnswer, OnContiueClicked);

        StartQuestion();
    }

    private void Update()
    {
        if (questionInProgress)
        {
            time += Time.deltaTime;
            timerController.SetTimerValue(time);
        }
    }

    private void StartQuestion()
    {
        if (questionIndex >= questions.Length)
        {
            questionIndex = 0;
        }

        ScriptableQuestion questionObj = questions[questionIndex];

        currentQuestion = new MCQQuestion
        {
            id = questionObj.id,
            shortT = questionObj.shortT,
            longT = questionObj.longT,
            answerIds = questionObj.answerIds,
            correctAnswer = questionObj.correctAnswer,
            maxTime = questionObj.maxTime
        };

        rootElement.Q<Label>("QuestionText").text = currentQuestion.shortT;

        currentAnswers = GetAnswers(currentQuestion);

        answerController.Initialize(currentAnswers,OnAnswerClicked, OnContiueClicked);
        timerController.SetTimerRange(currentQuestion.maxTime);
        boosterController.InitializeBoosters(currentAnswers, currentQuestion.correctAnswer);

        time = 0.0f;
        questionInProgress = true;
        questionIndex++;
    }

    private MCQAnswer[] GetAnswers(MCQQuestion question)
    {
        List<ScriptableAnswer> answerList = new List<ScriptableAnswer>(answers);
        List<MCQAnswer> answersForQuestion = new List<MCQAnswer>();
        foreach (string answerId in question.answerIds)
        {
            var foundAnswer = answerList.Find(answer => answer.id == answerId);

            if (foundAnswer == null) continue;

            answersForQuestion.Add(new MCQAnswer
            {
                id = foundAnswer.id,
                shortT = foundAnswer.shortT,
                longT = foundAnswer.longT,
            }
            );
        }

        return answersForQuestion.ToArray();
    }

    private void OnAnswerClicked(ClickEvent evt, string answerId)
    {
        boosterController.DisableBoosters();

        VisualElement answerContainer = (VisualElement)evt.currentTarget;
        TemplateContainer parentContainer = (TemplateContainer)answerContainer.parent;

        CheckAnswer(parentContainer, answerId);

        answerController.ChoiceSelected(parentContainer);
    }

    private void OnContiueClicked(ClickEvent evt)
    {
        StartQuestion();
    }

    private void OnRemoveAnswer(string id)
    {
        answerController.RemoveOneAnswer(id);
    }

    private void CheckAnswer(VisualElement choiceContainer, string selectedAnswerId)
    {
        if (selectedAnswerId == currentQuestion.correctAnswer)
        {
            choiceContainer.AddToClassList("Good");
        }
        else
        {
            choiceContainer.AddToClassList("Bad");
        }
    }
}
