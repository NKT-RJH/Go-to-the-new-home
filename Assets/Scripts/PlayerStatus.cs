using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    // 플레이어의 스텟
    public static float hp = 100;
    public static float maxHP = 100;
    public static float gage = 100;
    public static float maxGage = 100;
    public static bool godMode = false;

    // 효과음
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;
    // 체력 UI 바
    [SerializeField] private Image hpImage;
    // 연료 UI 바
    [SerializeField] private Image gageImage;
    // 체력 UI 텍스트
    [SerializeField] private TextMeshProUGUI hpText;
    // 연료 UI 텍스트
    [SerializeField] private TextMeshProUGUI gageText;
    // 게임 종료 화면 오브젝트
    [SerializeField] private GameObject endScreen;

    private bool gameEnd;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        godMode = false;
    }

    private void Update()
    {
        // 게임이 종료되었다면 Update가 아래 코드를 실행하지 않음
        if (gameEnd) return;

        // 체력 상태에 따른 명령 실행
        if (hp <= 0)
        {
            // 체력이 0 이하라면 사망 함수 실행
            hp = 0;
            Death();
        }
        else if (hp > maxHP)
        {
            // 체력이 최대 체력을 초과한다면 최대 체력으로 값 조정
            hp = maxHP;
        }
        // 연료가 0 이하라면 사망 함수 실행
        if (gage <= 0)
        {
            gage = 0;
            Death();
        }
        // 연료가 최대 연료를 초과한다면 최대 연료값으로 조정
        if (gage > maxGage)
        {
            gage = maxGage;
        }

        // 체력과 연료 UI 형식으로 화면에 출력
        hpImage.fillAmount = hp / maxHP;
        gageImage.fillAmount = gage / maxGage;
        hpText.text = hp + "/" + maxHP;
        gageText.text = gage + "/" + maxGage;
    }

    // 사망 시, 효과
    private void Death()
    {
        // 사망 사운드 출력
        audioSource.PlayOneShot(deathSound);
        // 이펙트 출력
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        // 게임 종료 스크린 실행
        Instantiate(endScreen, GameObject.Find("Canvas").transform);
        // 플레이어와 관련된 스크립트 비활성화
        FindObjectOfType<PlayerHit>().enabled = false;
        FindObjectOfType<PlayerSkill>().enabled = false;
        FindObjectOfType<ShootPlayerBullet>().enabled = false;
        FindObjectOfType<MovePlayer>().enabled = false;
        // 플레이어 오브젝트 삭제
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 체력 회복 아이템 피격 시
        if (collision.CompareTag("Repair"))
        {
            // 효과음 출력
            audioSource.PlayOneShot(itemSound);
            // 체력 회복
            hp = Mathf.Clamp(hp + 30, 0, maxHP);
            // 아이템 삭제
            Destroy(collision.gameObject);
            // 점수 증가
            ScoreManager.itemScore += 150;
        }
        // 연료 회복 아이템 피격 시
        if (collision.CompareTag("HealGage"))
        {
            // 효과음 출력
            audioSource.PlayOneShot(itemSound);
            // 연료 회복
            gage = Mathf.Clamp(gage + 30, 0, maxGage);
            // 아이템 삭제
            Destroy(collision.gameObject);
            // 점수 증가
            ScoreManager.itemScore += 150;
        }
    }
}
