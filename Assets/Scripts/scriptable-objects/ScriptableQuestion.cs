using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Questions/QuestionObect", order = 1)]
public class ScriptableQuestion : ScriptableObject
{
    public string id;
    public string shortT;
    public string longT;
    public string[] answerIds;
    public string correctAnswer;
    public float maxTime;
}
