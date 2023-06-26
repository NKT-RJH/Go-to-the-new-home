using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public class Enemy : MonoBehaviour
{
    // 효과음
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;
    // 사망 시 생성되는 점수 오브젝트
    [SerializeField] private GameObject scoreObject;
    // 사망 시 생성되는 아이템의 오브젝트 배열
    [SerializeField] private GameObject[] items = new GameObject[6];
    // 생성되는 점수 오브젝트 종류 설정
    [SerializeField] private bool dropScoreObject;
    // 생성되는 아이템 종류 설정
    [SerializeField] private bool dropItem;
    // 효과 사용 여부
    [SerializeField] private bool dontUseEffect;
    // 아이템 확률 설정
    [Range(0, 100)] [SerializeField] private int spawnItemValue;
    // 플레이어를 추적하는지 설정
    public bool follow;

    // 적의 능력치를 관리하는 변수
    protected EnemyStatus enemyStatus = null;

    // 이동 속도 설정
    public float speed;

    // 피격 시, 빨간 색으로 변하는 코루틴을 관리하는 변수
    private Coroutine coroutine = null;

    private SpriteRenderer spriteRenderer;

    protected Transform playerTransform;

    private AudioSource audioSource;

    protected virtual void Start()
    {
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        enemyStatus = GetComponent<EnemyStatus>();

        if (!dontUseEffect)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        playerTransform = FindObjectOfType<PlayerStatus>().transform;

        // 플레이어 오브젝트를 추적한다면
        if (follow)
        {
            // 플레이어를 바라보도록 설정
            transform.rotation = LookAt(playerTransform.position);
        }
    }

    protected virtual void Update()
    {
        // 체력이 0 이라면
        if (enemyStatus.hp <= 0)
        {
            // 사망
            Death();
        }

        // 정해진 각도와 속도에 따라 이동
        transform.Translate(speed * Time.deltaTime * Vector3.up, Space.Self);
    }

    // 받아온 Vector3 값을 바라보게 하는 함수
    protected Quaternion LookAt(Vector3 target)
    {
        Vector3 path = target - transform.position;

        float value = Mathf.Atan2(path.y, path.x) * Mathf.Rad2Deg;

        return Quaternion.AngleAxis(value - 90, Vector3.forward);
    }

    // 총알을 데미지, 속도, 각도를 지정하여 생성하는 함수
    protected Bullet ShootBullet(GameObject bullet, Vector3 path, Quaternion angle, float damage, float speed)
    {
        Bullet bulletObject = Instantiate(bullet, path, angle).GetComponent<Bullet>();
        bulletObject.GetComponent<EnemyStatus>().damage = damage;
        bulletObject.speed = speed;

        return bulletObject;
    }

    // 피격 시 효과 출력 코루틴을 실행하는 함수
    private void HitEffect()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(JobHitEffect());
    }

    // 피격 시 효과 출력
    private IEnumerator JobHitEffect()
    {
        // 0.1초간 빨간색으로 변화
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        coroutine = null;
    }

    // 사망 시, 효과
    private void Death()
    {
        // 점수 추가
        ScoreManager.enemyScore += enemyStatus.score;

        int value = Random.Range(0, 100);

        // 랜덤 값으로 아이템 생성
        if (dropItem && value < spawnItemValue)
        {
            int newValue = Random.Range(0, items.Length);
            Instantiate(items[newValue], transform.position, Quaternion.identity);
            if (dropScoreObject)
            {
                Instantiate(scoreObject, transform.position + Vector3.right * 1.5f, Quaternion.identity);
            }
        }
        else
        {
            if (dropScoreObject)
            {
                Instantiate(scoreObject, transform.position, Quaternion.identity);
            }
        }
        audioSource.PlayOneShot(deathSound);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // 게임 화면 밖으로 나갔다면 오브젝트 삭제
    private void OnBecameInvisible()
    {
        try
        {
            Vector3 path = Camera.main.WorldToScreenPoint(transform.position);

            if (path.x < 0 || path.x > Screen.width || path.y < 0 || path.y > Screen.height)
            {
                Destroy(gameObject);
            }
        }
        catch (System.NullReferenceException)
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 총알에 피격 시
        if (collision.CompareTag("Bullet"))
        {
            // 체력 감소
            enemyStatus.hp -= ShootPlayerBullet.damage;
            if (!dontUseEffect)
            {
                // 효과 출력
                HitEffect();
            }
            // 총알 오브젝트 삭제
            Destroy(collision.gameObject);
            // 체력이 0 이하라면
            if (enemyStatus.hp <= 0)
            {
                // 사망
                Death();
            }
        }
    }
}
