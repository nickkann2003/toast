using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New TN Score", menuName = "Minigames/Toast Ninja/Score", order = 55)]
public class ToastNinjaScore : ScriptableObject
{
    [SerializeField, ReadOnly]
    private int score;
    [SerializeField, ReadOnly]
    private int highScore;

    [SerializeField]
    private int bombsToLose;
    [SerializeField, ReadOnly]
    private int bombsHit;

    public int Score {  get { return score; } }
    public int HighScore { get {  return highScore; } }
    public int BombsHit { get {  return bombsHit; } }

    [Button]
    private void FullReset()
    {
        score = 0;
        highScore = 0;
        bombsHit = 0;
    }

    public void AddPoints(int points)
    {
        score += points;

        if (score > highScore)
        {
            highScore = score;
        }
    }

    public void BombHit()
    {
        bombsHit++;

        if (bombsHit >= bombsToLose)
        {
            Raycast.Instance?.hand.Drop();
        }
    }

    public void GameEnd()
    {
        score = 0;
        bombsHit = 0;
    }

    private void OnEnable()
    {
        GameEnd();
    }
}