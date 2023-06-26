using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    // �÷��̾��� ����
    public static float hp = 100;
    public static float maxHP = 100;
    public static float gage = 100;
    public static float maxGage = 100;
    public static bool godMode = false;

    // ȿ����
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;
    // ü�� UI ��
    [SerializeField] private Image hpImage;
    // ���� UI ��
    [SerializeField] private Image gageImage;
    // ü�� UI �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI hpText;
    // ���� UI �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI gageText;
    // ���� ���� ȭ�� ������Ʈ
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
        // ������ ����Ǿ��ٸ� Update�� �Ʒ� �ڵ带 �������� ����
        if (gameEnd) return;

        // ü�� ���¿� ���� ��� ����
        if (hp <= 0)
        {
            // ü���� 0 ���϶�� ��� �Լ� ����
            hp = 0;
            Death();
        }
        else if (hp > maxHP)
        {
            // ü���� �ִ� ü���� �ʰ��Ѵٸ� �ִ� ü������ �� ����
            hp = maxHP;
        }
        // ���ᰡ 0 ���϶�� ��� �Լ� ����
        if (gage <= 0)
        {
            gage = 0;
            Death();
        }
        // ���ᰡ �ִ� ���Ḧ �ʰ��Ѵٸ� �ִ� ���ᰪ���� ����
        if (gage > maxGage)
        {
            gage = maxGage;
        }

        // ü�°� ���� UI �������� ȭ�鿡 ���
        hpImage.fillAmount = hp / maxHP;
        gageImage.fillAmount = gage / maxGage;
        hpText.text = hp + "/" + maxHP;
        gageText.text = gage + "/" + maxGage;
    }

    // ��� ��, ȿ��
    private void Death()
    {
        // ��� ���� ���
        audioSource.PlayOneShot(deathSound);
        // ����Ʈ ���
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        // ���� ���� ��ũ�� ����
        Instantiate(endScreen, GameObject.Find("Canvas").transform);
        // �÷��̾�� ���õ� ��ũ��Ʈ ��Ȱ��ȭ
        FindObjectOfType<PlayerHit>().enabled = false;
        FindObjectOfType<PlayerSkill>().enabled = false;
        FindObjectOfType<ShootPlayerBullet>().enabled = false;
        FindObjectOfType<MovePlayer>().enabled = false;
        // �÷��̾� ������Ʈ ����
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ü�� ȸ�� ������ �ǰ� ��
        if (collision.CompareTag("Repair"))
        {
            // ȿ���� ���
            audioSource.PlayOneShot(itemSound);
            // ü�� ȸ��
            hp = Mathf.Clamp(hp + 30, 0, maxHP);
            // ������ ����
            Destroy(collision.gameObject);
            // ���� ����
            ScoreManager.itemScore += 150;
        }
        // ���� ȸ�� ������ �ǰ� ��
        if (collision.CompareTag("HealGage"))
        {
            // ȿ���� ���
            audioSource.PlayOneShot(itemSound);
            // ���� ȸ��
            gage = Mathf.Clamp(gage + 30, 0, maxGage);
            // ������ ����
            Destroy(collision.gameObject);
            // ���� ����
            ScoreManager.itemScore += 150;
        }
    }
}
