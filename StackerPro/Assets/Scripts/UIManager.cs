using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;

    public static UIManager Instance { get; private set; }

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
    private void Start()
    {
        UpdateScoreText();
    }
    public void UpdateScoreText()
    {
        int score = ScoreManager.Instance.GetScore();
        scoreText.text = "score\n" + score.ToString();
    }
}
