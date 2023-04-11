using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHit : MonoBehaviour
{
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip scoreObjectSound;

    private SpriteRenderer spriteRenderer;
    private Coroutine coroutine1 = null;
    private Coroutine coroutine2 = null;
    private Coroutine coroutine3 = null;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void HitEffect()
    {
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
        }
        coroutine1 = StartCoroutine(JobHitEffect());
    }

    private IEnumerator JobHitEffect()
    {
        PlayerStatus.godMode = true;

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

        PlayerStatus.godMode = false;

        coroutine1 = null;
    }

    private void GodMode()
    {
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
        coroutine2 = StartCoroutine(JobGodMode());
    }

    private IEnumerator JobGodMode()
    {
        PlayerStatus.godMode = true;

        spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(5);

        spriteRenderer.color = Color.white;

        PlayerStatus.godMode = false;

        coroutine2 = null;
    }

    public void BeWater()
    {
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
            coroutine1 = null;
        }
        if (coroutine3 != null)
        {
            StopCoroutine(coroutine3);
        }
        coroutine3 = StartCoroutine(JobBeWater());
    }

    private IEnumerator JobBeWater()
    {
        PlayerStatus.godMode = true;

        spriteRenderer.color = Color.green;

        yield return new WaitForSeconds(0.75f);

        spriteRenderer.color = Color.white;

        PlayerStatus.godMode = false;

        coroutine3 = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GodMode"))
        {
            audioSource.PlayOneShot(itemSound);
            GodMode();
            Destroy(collision.gameObject);
            ScoreManager.itemScore += 150;
            return;
        }

        if (collision.CompareTag("ScoreObject"))
        {
            audioSource.PlayOneShot(scoreObjectSound);
            ScoreManager.score += collision.GetComponent<ScoreObject>().score;
            Destroy(collision.gameObject);
        }

        if (!PlayerStatus.godMode)
        {
            if (collision.CompareTag("Enemy"))
            {
                audioSource.PlayOneShot(hitSound);
                PlayerStatus.hp -= collision.GetComponent<EnemyStatus>().damage;
                HitEffect();
                if (collision.GetComponent<Bullet>())
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
