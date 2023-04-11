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

        time.text = "소요시간 : " + string.Format("{0}분 {1}초", (int)(TimeManager.time / 60), (int)(TimeManager.time % 60));
        score.text = "총 점수 : " + ScoreManager.score;
        enemyScore.text = "적 처치 점수 : " + ScoreManager.enemyScore;
        itemScore.text = "아이템 획득 점수 : " + ScoreManager.itemScore;
        timeScore.text = "시간 보너스 점수 : " + ScoreManager.timeScore;
        hpScore.text = "내구도 보너스 점수 : " + ScoreManager.hpScore;
        gageScore.text = "연료 보너스 점수 : " + ScoreManager.gageScore;
    }

    private void Calculate()
    {
        ScoreManager.timeScore = PlayerStatus.hp <= 0 || PlayerStatus.gage <= 0 ? 0 : (int)Mathf.Clamp(2000 + (180 - TimeManager.time) * 50, 0, 5000);
        ScoreManager.hpScore = PlayerStatus.hp <= 0 || PlayerStatus.gage <= 0 ? 0 : (int)(PlayerStatus.hp / PlayerStatus.maxHP * 1000);
        ScoreManager.gageScore = PlayerStatus.hp <= 0 || PlayerStatus.gage <= 0 ? 0 : (int)(PlayerStatus.gage / PlayerStatus.maxGage * 1000);

        ScoreManager.score += ScoreManager.enemyScore + ScoreManager.itemScore + ScoreManager.timeScore + ScoreManager.hpScore + ScoreManager.gageScore;
    }
}
