using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class QuestionaireBoosterController
{
    private ClickEventHandlerManager buttonClickManager = new();

    private Button answerButton;
    private Button questionButton;

    private List<MCQAnswer> wrongAnswers;

    Action<string> onRemoveAnswer;

    public QuestionaireBoosterController(VisualElement root, Action<string> answerCallback, EventCallback<ClickEvent> questionCallback)
    {
        answerButton = root.Q<Button>("ButtonAnswer");
        questionButton = root.Q<Button>("ButtonQuestion");

        onRemoveAnswer = answerCallback;

        buttonClickManager.Register(answerButton, evt => SelectAnswerToRemove());
        buttonClickManager.Register(questionButton, questionCallback);
    }

    public void InitializeBoosters(MCQAnswer[] currentAnswers,string answerId)
    {
        EnableBoosters();
        wrongAnswers = new List<MCQAnswer>(currentAnswers).FindAll(answer => answer.id != answerId); ; 
    }

    public void DisableBoosters()
    {
        answerButton.SetEnabled(false);
        questionButton.SetEnabled(false);
    }

    public void EnableBoosters()
    {
        answerButton.SetEnabled(true);
        questionButton.SetEnabled(true);
    }

    private void SelectAnswerToRemove()
    {
        int randomIdx = UnityEngine.Random.Range(0, wrongAnswers.Count);

        onRemoveAnswer.Invoke(wrongAnswers[randomIdx].id);

        wrongAnswers.RemoveAt(randomIdx);

        if(wrongAnswers.Count == 1)
        {
            answerButton.SetEnabled(false);
        }
    }
}
