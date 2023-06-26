using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public class Enemy : MonoBehaviour
{
    // ȿ����
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;
    // ��� �� �����Ǵ� ���� ������Ʈ
    [SerializeField] private GameObject scoreObject;
    // ��� �� �����Ǵ� �������� ������Ʈ �迭
    [SerializeField] private GameObject[] items = new GameObject[6];
    // �����Ǵ� ���� ������Ʈ ���� ����
    [SerializeField] private bool dropScoreObject;
    // �����Ǵ� ������ ���� ����
    [SerializeField] private bool dropItem;
    // ȿ�� ��� ����
    [SerializeField] private bool dontUseEffect;
    // ������ Ȯ�� ����
    [Range(0, 100)] [SerializeField] private int spawnItemValue;
    // �÷��̾ �����ϴ��� ����
    public bool follow;

    // ���� �ɷ�ġ�� �����ϴ� ����
    protected EnemyStatus enemyStatus = null;

    // �̵� �ӵ� ����
    public float speed;

    // �ǰ� ��, ���� ������ ���ϴ� �ڷ�ƾ�� �����ϴ� ����
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

        // �÷��̾� ������Ʈ�� �����Ѵٸ�
        if (follow)
        {
            // �÷��̾ �ٶ󺸵��� ����
            transform.rotation = LookAt(playerTransform.position);
        }
    }

    protected virtual void Update()
    {
        // ü���� 0 �̶��
        if (enemyStatus.hp <= 0)
        {
            // ���
            Death();
        }

        // ������ ������ �ӵ��� ���� �̵�
        transform.Translate(speed * Time.deltaTime * Vector3.up, Space.Self);
    }

    // �޾ƿ� Vector3 ���� �ٶ󺸰� �ϴ� �Լ�
    protected Quaternion LookAt(Vector3 target)
    {
        Vector3 path = target - transform.position;

        float value = Mathf.Atan2(path.y, path.x) * Mathf.Rad2Deg;

        return Quaternion.AngleAxis(value - 90, Vector3.forward);
    }

    // �Ѿ��� ������, �ӵ�, ������ �����Ͽ� �����ϴ� �Լ�
    protected Bullet ShootBullet(GameObject bullet, Vector3 path, Quaternion angle, float damage, float speed)
    {
        Bullet bulletObject = Instantiate(bullet, path, angle).GetComponent<Bullet>();
        bulletObject.GetComponent<EnemyStatus>().damage = damage;
        bulletObject.speed = speed;

        return bulletObject;
    }

    // �ǰ� �� ȿ�� ��� �ڷ�ƾ�� �����ϴ� �Լ�
    private void HitEffect()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(JobHitEffect());
    }

    // �ǰ� �� ȿ�� ���
    private IEnumerator JobHitEffect()
    {
        // 0.1�ʰ� ���������� ��ȭ
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        coroutine = null;
    }

    // ��� ��, ȿ��
    private void Death()
    {
        // ���� �߰�
        ScoreManager.enemyScore += enemyStatus.score;

        int value = Random.Range(0, 100);

        // ���� ������ ������ ����
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

    // ���� ȭ�� ������ �����ٸ� ������Ʈ ����
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
        // �÷��̾� �Ѿ˿� �ǰ� ��
        if (collision.CompareTag("Bullet"))
        {
            // ü�� ����
            enemyStatus.hp -= ShootPlayerBullet.damage;
            if (!dontUseEffect)
            {
                // ȿ�� ���
                HitEffect();
            }
            // �Ѿ� ������Ʈ ����
            Destroy(collision.gameObject);
            // ü���� 0 ���϶��
            if (enemyStatus.hp <= 0)
            {
                // ���
                Death();
            }
        }
    }
}
