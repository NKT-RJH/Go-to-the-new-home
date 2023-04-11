using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class End : MonoBehaviour
{
    [SerializeField] private AudioClip textSound;
    [SerializeField] private GameObject[] objectsToAnimation;
    [SerializeField] private Text wholeTimeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Text nameText;
    [SerializeField] private GameObject warning;

    private AudioSource audioSource;
    private Coroutine coroutine = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        wholeTimeText.text = "총 소요시간 : " + string.Format("{0}분 {1}초", (int)(TimeManager.wholeTime / 60), (int)(TimeManager.wholeTime % 60));

        scoreText.text = "당신의 점수 : " + ScoreManager.score + "점";

        StartCoroutine(ShowAnimation());
    }


    private IEnumerator ShowAnimation()
    {
        foreach (GameObject obj in objectsToAnimation)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToAnimation)
        {
            obj.SetActive(true);
            audioSource.PlayOneShot(textSound);
            yield return new WaitForSeconds(2);
        }
    }

    private void Warning()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(JobWarning());
    }

    private IEnumerator JobWarning()
    {
        warning.SetActive(true);

        yield return new WaitForSeconds(2);

        warning.SetActive(false);

        coroutine = null;
    }

    public void SubmitAndMove()
    {
        if ((nameText.text.StartsWith(" ") && nameText.text.Length == 1) || nameText.text.Length <= 1)
        {
            Warning();
            return;
        }

        Ranking.Add(nameText.text, ScoreManager.score);
        Ranking.Save();
        Ranking.ranks = new List<RankData>();

        ClearValues();

        SceneManager.LoadScene("Title");
    }

    private void ClearValues()
    {
        ScoreManager.score = 0;
        ScoreManager.enemyScore = 0;
        ScoreManager.itemScore = 0;
        ScoreManager.timeScore = 0;
        ScoreManager.hpScore = 0;
        ScoreManager.gageScore = 0;
        PlayerStatus.godMode = false;
        PlayerStatus.hp = 100;
        PlayerStatus.gage = 100;
        ShootPlayerBullet.level = 1;
        CheatKey.stage = 1;
        TimeManager.wholeTime = 0;
        TimeManager.time = 0;
        TimeManager.isStart = false;
    }
}
