using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionaireMainController
{
    VisualTreeAsset questionaireMain;

    private MCQQuestion question = new MCQQuestion { id = "111", shortT = "What is red?", answerIds = new string[] { "111-222", "111-333", "111-444", "111-555" } };
    private MCQAnswer[] answers = new MCQAnswer[] {
            new MCQAnswer { id = "111-222", shortT = "Red is dead" },
            new MCQAnswer { id = "111-333", shortT = "Red is blue" },
            new MCQAnswer { id = "111-444", shortT = "Red is hot" },
            new MCQAnswer { id = "111-555", shortT = "Red is ice" },
        };

    public void InitializeQuestionaire(VisualElement root, VisualTreeAsset what)
    {
        this.questionaireMain = what;

        root.Q<Label>("QuestionText").text = this.question.shortT;
        for (int idx = 0; idx < this.question.answerIds.Length; idx++)
        {
            setAnswerText(root, question.id, idx, this.answers[idx]);
        }
    }

    ClickEventHandlerManager eventHandlerManager = new ClickEventHandlerManager();

    public void setAnswerText(VisualElement root, string questionId, int index, MCQAnswer answer) {
        TemplateContainer templateContainer = root.Q<TemplateContainer>("Answer_" + index);
        Label label = templateContainer.Q<Label>("Text");
        label.text = answer.shortT;

        eventHandlerManager.Register(templateContainer, evt => OnClick(evt, questionId, answer.id));
        // templateContainer.RegisterCallback<ClickEvent>(evt => OnClick(evt, questionId, answer.id));
    }

    public void OnClick(ClickEvent evt, string questionId, string answerId) {
        TemplateContainer templateContainer = (TemplateContainer) evt.currentTarget;
        Label label = templateContainer.Q<Label>("Text");
        UnityEngine.Debug.Log($"Answer clicked: {questionId} => {answerId} / {label.text}...");

        // this.eventHandlerManager.UnregisterAll();

        if (answerId == this.answers[2].id) {
            templateContainer.AddToClassList("Good");
        } else {
            templateContainer.AddToClassList("Bad");
        }
    }
}
