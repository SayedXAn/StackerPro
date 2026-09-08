using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject); 
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore()
    {
        score++;
    }

    public void SubtractScore()
    {
        score--;
    }

    public void ResetScore()
    {
        score = 0;
    }


}
