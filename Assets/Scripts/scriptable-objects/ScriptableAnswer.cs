using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Questions/AnswerObect", order = 2)]
public class ScriptableAnswer : ScriptableObject
{
    public string id;
    public string shortT;
    public string longT;
}
