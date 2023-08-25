using UnityEngine;

public class DecisionButton : MonoBehaviour
{
    public int mentalHealthChange;
    public int fatigueChange;
    public float averageChange;
    public int nextQuestionIndex;

    public GameManager gameManager;

    public void OnButtonPress()
    {
        gameManager.MakeDecision(mentalHealthChange, fatigueChange, averageChange, nextQuestionIndex);
    }
}
