using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    public static int enemyScore;
    public static int timeScore;
    public static int itemScore;
    public static int hpScore;
    public static int gageScore;

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        enemyScore = 0;
        timeScore = 0;
        itemScore = 0;
        hpScore = 0;
        gageScore = 0;

        scoreText.text = score + "Á¡";
    }

    private void Update()
    {
        scoreText.text = (score + enemyScore + itemScore) + "Á¡";
    }
}
