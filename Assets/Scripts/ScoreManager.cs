using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public UnityEvent<int> onScoreChanged;   // hook UI text here
    public UnityEvent<string> onFeedback;    // hook a floating "+10" text here

    void Awake() => Instance = this;

    public void CorrectOrder(bool wasVague, bool wasFast)
    {
        int gain = wasVague ? 25 : 10;
        if (wasFast) gain += 5;
        score += gain;
        onFeedback?.Invoke($"+{gain}");
        onScoreChanged?.Invoke(score);
    }

    public void WrongItem()
    {
        score = Mathf.Max(0, score - 5);
        onFeedback?.Invoke("-5 Wrong item");
        onScoreChanged?.Invoke(score);
    }

    public void MissedOrder()
    {
        score = Mathf.Max(0, score - 10);
        onFeedback?.Invoke("-10 Missed");
        onScoreChanged?.Invoke(score);
    }

    public string GetRank()
    {
        if (score >= 200)
        {
            return "Olympian";
        }

        if (score >= 120)
        {
            return "Demigod";
        }

        if (score >= 50)
        {
            return "Hero";
        }

        return "Mortal";
    }
}