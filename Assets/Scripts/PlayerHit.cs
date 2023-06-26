using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHit : MonoBehaviour
{
    // 각각의 오브젝트에 충돌하였을 때의 사운드
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip scoreObjectSound;

    private SpriteRenderer spriteRenderer;
    // 무적 모드가 총 3개(적에게 피격 시, 스킬 액체화, 아이템 일시무적)가 있기에 이를 각각 관리하기 위한 코루틴
    private Coroutine coroutine1 = null;
    private Coroutine coroutine2 = null;
    private Coroutine coroutine3 = null;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 피격 시, 깜빡이며 무적판정이 생기는 코루틴을 실행하는 함수
    private void HitEffect()
    {
        // 이미 해당 무적이 실행중이라면 종료하고
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
        }
        // 새로운 무적 실행
        coroutine1 = StartCoroutine(JobHitEffect());
    }

    // 피격 시, 깜빡이며 무적판정이 생기는 코루틴
    private IEnumerator JobHitEffect()
    {
        // 무적판정 활성화
        PlayerStatus.godMode = true;

        // 플레이어의 SpriteRenderer의 투명도를 변경하여 깜빡이게 만드는 코드
        Color spriteColor = spriteRenderer.color;
        for (int count = 0; count < 10; count++)
        {
            if (spriteColor.a != 1)
            {
                spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);
            }
            else
            {
                spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 150 / 255f);
            }
            spriteRenderer.color = spriteColor;

            yield return new WaitForSeconds(0.1f);
        }

        // 무적판정 비활성화
        PlayerStatus.godMode = false;

        // 무적 판정이 끝났기에 변수에서 제거
        coroutine1 = null;
    }
    
    // 아이템 일시무적 코루틴을 실행하는 함수
    private void GodMode()
    {
        // 다른 무적판정이 실행중이라면 모두 비활성화 후
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
            coroutine1 = null;
        }
        if (coroutine2 != null)
        {
            StopCoroutine(coroutine2);
        }
        if (coroutine3 != null)
        {
            StopCoroutine(coroutine3);
            coroutine3 = null;
        }
        // 아이템 일시무적 실행
        coroutine2 = StartCoroutine(JobGodMode());
    }

    // 아이템 일시무적
    private IEnumerator JobGodMode()
    {
        // 5초간 SpriteRenderer가 파란색으로 변하며 무적판정 실행
        PlayerStatus.godMode = true;

        spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(5);

        spriteRenderer.color = Color.white;

        PlayerStatus.godMode = false;

        // 무적 판정이 끝났기에 변수에서 제거
        coroutine2 = null;
    }

    // 스킬 액체화 무적판정 코루틴을 실행하는 함수
    public void BeWater()
    {
        // 다른 무적판정이 실행 중이라면 종료 후
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
            coroutine1 = null;
        }
        if (coroutine3 != null)
        {
            StopCoroutine(coroutine3);
        }
        // 스킬 액체화 무적 실행
        coroutine3 = StartCoroutine(JobBeWater());
    }

    // 스킬 액체화 무적
    private IEnumerator JobBeWater()
    {
        // 0.75초간 초록색으로 무적 판정 실행
        PlayerStatus.godMode = true;

        spriteRenderer.color = Color.green;

        yield return new WaitForSeconds(0.75f);

        spriteRenderer.color = Color.white;

        PlayerStatus.godMode = false;

        // 무적 판정이 끝났기에 변수에서 제거
        coroutine3 = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 일시 무적 아이템에 닿았을 떄
        if (collision.CompareTag("GodMode"))
        {
            // 효과음 출력 후
            audioSource.PlayOneShot(itemSound);
            // 일시 무적 활성화
            GodMode();
            // 일시 무적 아이템 삭제
            Destroy(collision.gameObject);
            // 점수 획득
            ScoreManager.itemScore += 150;
            return;
        }

        // 점수 오브젝트에 닿았을 때
        if (collision.CompareTag("ScoreObject"))
        {
            // 효과음 출력 후
            audioSource.PlayOneShot(scoreObjectSound);
            // 점수 획득
            ScoreManager.score += collision.GetComponent<ScoreObject>().score;
            // 점수 오브젝트 삭제
            Destroy(collision.gameObject);
        }

        // 무적 판정이 아닐 때
        if (!PlayerStatus.godMode)
        {
            // 적에게 피격 시
            if (collision.CompareTag("Enemy"))
            {
                // 효과음 출력 후
                audioSource.PlayOneShot(hitSound);
                // 플레이어 HP 감소
                PlayerStatus.hp -= collision.GetComponent<EnemyStatus>().damage;
                // 피격 무적판정 실행
                HitEffect();
                // 공격한 대상이 총알이라면 삭제
                if (collision.GetComponent<Bullet>())
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
