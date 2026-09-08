using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "box")
        {
            Destroy(collision.gameObject);
            ScoreManager.Instance.SubtractScore();
            UIManager.Instance.UpdateScoreText();
        }
    }
}
