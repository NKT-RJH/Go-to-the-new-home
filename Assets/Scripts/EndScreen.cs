using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI enemyScore;
    [SerializeField] private TextMeshProUGUI itemScore;
    [SerializeField] private TextMeshProUGUI timeScore;
    [SerializeField] private TextMeshProUGUI hpScore;
    [SerializeField] private TextMeshProUGUI gageScore;

    private void OnEnable()
    {
        TimeManager.wholeTime += TimeManager.time;
        TimeManager.isStart = false;
        FindObjectOfType<PlayerHit>().gameObject.GetComponent<CircleCollider2D>().enabled = false;
        FindObjectOfType<PlayerHit>().enabled = false;
        FindObjectOfType<PlayerSkill>().enabled = false;
        FindObjectOfType<PlayerStatus>().enabled = false;
        FindObjectOfType<MovePlayer>().enabled = false;
        FindObjectOfType<ShootPlayerBullet>().enabled = false;

        Calculate();

        time.text = "�ҿ�ð� : " + string.Format("{0}�� {1}��", (int)(TimeManager.time / 60), (int)(TimeManager.time % 60));
        score.text = "�� ���� : " + ScoreManager.score;
        enemyScore.text = "�� óġ ���� : " + ScoreManager.enemyScore;
        itemScore.text = "������ ȹ�� ���� : " + ScoreManager.itemScore;
        timeScore.text = "�ð� ���ʽ� ���� : " + ScoreManager.timeScore;
        hpScore.text = "������ ���ʽ� ���� : " + ScoreManager.hpScore;
        gageScore.text = "���� ���ʽ� ���� : " + ScoreManager.gageScore;
    }

    private void Calculate()
    {
        ScoreManager.timeScore = PlayerStatus.hp <= 0 || PlayerStatus.gage <= 0 ? 0 : (int)Mathf.Clamp(2000 + (180 - TimeManager.time) * 50, 0, 5000);
        ScoreManager.hpScore = PlayerStatus.hp <= 0 || PlayerStatus.gage <= 0 ? 0 : (int)(PlayerStatus.hp / PlayerStatus.maxHP * 1000);
        ScoreManager.gageScore = PlayerStatus.hp <= 0 || PlayerStatus.gage <= 0 ? 0 : (int)(PlayerStatus.gage / PlayerStatus.maxGage * 1000);

        ScoreManager.score += ScoreManager.enemyScore + ScoreManager.itemScore + ScoreManager.timeScore + ScoreManager.hpScore + ScoreManager.gageScore;
    }
}
