using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject scoreObject;
    [SerializeField] private GameObject[] items = new GameObject[6];
    [SerializeField] private bool dropScoreObject;
    [SerializeField] private bool dropItem;
    [SerializeField] private bool dontUseEffect;
    [Range(0, 100)] [SerializeField] private int spawnItemValue;
    public bool follow;

    protected EnemyStatus enemyStatus = null;

    public float speed;

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

        if (follow)
        {
            transform.rotation = LookAt(playerTransform.position);
        }
    }

    protected virtual void Update()
    {
        if (enemyStatus.hp <= 0)
        {
            Death();
        }

        transform.Translate(speed * Time.deltaTime * Vector3.up, Space.Self);
    }

    protected Quaternion LookAt(Vector3 target)
    {
        Vector3 path = target - transform.position;

        float value = Mathf.Atan2(path.y, path.x) * Mathf.Rad2Deg;

        return Quaternion.AngleAxis(value - 90, Vector3.forward);
    }

    protected Bullet ShootBullet(GameObject bullet, Vector3 path, Quaternion angle, float damage, float speed)
    {
        Bullet bulletObject = Instantiate(bullet, path, angle).GetComponent<Bullet>();
        bulletObject.GetComponent<EnemyStatus>().damage = damage;
        bulletObject.speed = speed;

        return bulletObject;
    }

    private void HitEffect()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(JobHitEffect());
    }

    private IEnumerator JobHitEffect()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        coroutine = null;
    }

    private void Death()
    {
        ScoreManager.enemyScore += enemyStatus.score;

        int value = Random.Range(0, 100);

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
        if (collision.CompareTag("Bullet"))
        {
            enemyStatus.hp -= ShootPlayerBullet.damage;
            if (!dontUseEffect)
            {
                HitEffect();
            }
            Destroy(collision.gameObject);
            if (enemyStatus.hp <= 0)
            {
                Death();
            }
        }
    }
}
