using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHit : MonoBehaviour
{
    // ������ ������Ʈ�� �浹�Ͽ��� ���� ����
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip scoreObjectSound;

    private SpriteRenderer spriteRenderer;
    // ���� ��尡 �� 3��(������ �ǰ� ��, ��ų ��üȭ, ������ �Ͻù���)�� �ֱ⿡ �̸� ���� �����ϱ� ���� �ڷ�ƾ
    private Coroutine coroutine1 = null;
    private Coroutine coroutine2 = null;
    private Coroutine coroutine3 = null;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // �ǰ� ��, �����̸� ���������� ����� �ڷ�ƾ�� �����ϴ� �Լ�
    private void HitEffect()
    {
        // �̹� �ش� ������ �������̶�� �����ϰ�
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
        }
        // ���ο� ���� ����
        coroutine1 = StartCoroutine(JobHitEffect());
    }

    // �ǰ� ��, �����̸� ���������� ����� �ڷ�ƾ
    private IEnumerator JobHitEffect()
    {
        // �������� Ȱ��ȭ
        PlayerStatus.godMode = true;

        // �÷��̾��� SpriteRenderer�� ������ �����Ͽ� �����̰� ����� �ڵ�
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

        // �������� ��Ȱ��ȭ
        PlayerStatus.godMode = false;

        // ���� ������ �����⿡ �������� ����
        coroutine1 = null;
    }
    
    // ������ �Ͻù��� �ڷ�ƾ�� �����ϴ� �Լ�
    private void GodMode()
    {
        // �ٸ� ���������� �������̶�� ��� ��Ȱ��ȭ ��
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
        // ������ �Ͻù��� ����
        coroutine2 = StartCoroutine(JobGodMode());
    }

    // ������ �Ͻù���
    private IEnumerator JobGodMode()
    {
        // 5�ʰ� SpriteRenderer�� �Ķ������� ���ϸ� �������� ����
        PlayerStatus.godMode = true;

        spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(5);

        spriteRenderer.color = Color.white;

        PlayerStatus.godMode = false;

        // ���� ������ �����⿡ �������� ����
        coroutine2 = null;
    }

    // ��ų ��üȭ �������� �ڷ�ƾ�� �����ϴ� �Լ�
    public void BeWater()
    {
        // �ٸ� ���������� ���� ���̶�� ���� ��
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
            coroutine1 = null;
        }
        if (coroutine3 != null)
        {
            StopCoroutine(coroutine3);
        }
        // ��ų ��üȭ ���� ����
        coroutine3 = StartCoroutine(JobBeWater());
    }

    // ��ų ��üȭ ����
    private IEnumerator JobBeWater()
    {
        // 0.75�ʰ� �ʷϻ����� ���� ���� ����
        PlayerStatus.godMode = true;

        spriteRenderer.color = Color.green;

        yield return new WaitForSeconds(0.75f);

        spriteRenderer.color = Color.white;

        PlayerStatus.godMode = false;

        // ���� ������ �����⿡ �������� ����
        coroutine3 = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ͻ� ���� �����ۿ� ����� ��
        if (collision.CompareTag("GodMode"))
        {
            // ȿ���� ��� ��
            audioSource.PlayOneShot(itemSound);
            // �Ͻ� ���� Ȱ��ȭ
            GodMode();
            // �Ͻ� ���� ������ ����
            Destroy(collision.gameObject);
            // ���� ȹ��
            ScoreManager.itemScore += 150;
            return;
        }

        // ���� ������Ʈ�� ����� ��
        if (collision.CompareTag("ScoreObject"))
        {
            // ȿ���� ��� ��
            audioSource.PlayOneShot(scoreObjectSound);
            // ���� ȹ��
            ScoreManager.score += collision.GetComponent<ScoreObject>().score;
            // ���� ������Ʈ ����
            Destroy(collision.gameObject);
        }

        // ���� ������ �ƴ� ��
        if (!PlayerStatus.godMode)
        {
            // ������ �ǰ� ��
            if (collision.CompareTag("Enemy"))
            {
                // ȿ���� ��� ��
                audioSource.PlayOneShot(hitSound);
                // �÷��̾� HP ����
                PlayerStatus.hp -= collision.GetComponent<EnemyStatus>().damage;
                // �ǰ� �������� ����
                HitEffect();
                // ������ ����� �Ѿ��̶�� ����
                if (collision.GetComponent<Bullet>())
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
