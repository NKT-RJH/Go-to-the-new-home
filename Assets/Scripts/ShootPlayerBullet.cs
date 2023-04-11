using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShootPlayerBullet : MonoBehaviour
{
    public static float damage = 1;
    public static int level = 1;
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip bulletSound;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject strongBullet;
    [SerializeField] private GameObject[] levelImages = new GameObject[4];

    [SerializeField] private float delay;

    private bool upgrade = false;

    private float countTime;

    private Vector3[,] levelPath = { { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero }, { Vector2.right * 0.2f, Vector2.left * 0.2f, Vector2.zero, Vector2.zero }, { Vector2.zero, Vector2.left * 0.3f, Vector2.right * 0.3f, Vector2.zero }, { Vector2.left * 0.1f, Vector2.right * 0.1f, Vector2.left * 0.3f, Vector2.right * 0.3f } };

    private AudioSource audioSource;

    private Coroutine coroutine = null;

    private void Awake()
    {
        for (int count = 0; count < levelImages.Length; count++)
        {
            levelImages[count].SetActive(false);
        }
        levelImages[level - 1].SetActive(true);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        for (int count = 0; count < levelImages.Length; count++)
        {
            levelImages[count].SetActive(false);
        }
        levelImages[level - 1].SetActive(true);

        if (Input.GetKey(KeyCode.J))
        {
            countTime += Time.deltaTime;

            if (countTime >= delay)
            {
                countTime = 0;

                audioSource.PlayOneShot(bulletSound);
                
                for (int count = 0; count < level; count++)
                {
                    if (upgrade)
                    {
                        Instantiate(strongBullet, transform.position + levelPath[level - 1, count] + Vector3.up * 0.5f, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(bullet, transform.position + levelPath[level - 1, count] + Vector3.up * 0.5f, Quaternion.identity);
                    }
                }
            }
        }
    }

    public void PowerUP(float time)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(JobPowerUP(time));
    }

    private IEnumerator JobPowerUP(float time)
    {
        damage = 2;
        upgrade = true;
        yield return new WaitForSeconds(time);
        upgrade = false;
        damage = 1;

        coroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            if (level < 4)
            {
                audioSource.PlayOneShot(itemSound);
                level++;
                Destroy(collision.gameObject);
                ScoreManager.itemScore += 150;
            }
        }
    }
}
