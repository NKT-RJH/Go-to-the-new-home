using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static float hp = 100;
    public static float maxHP = 100;
    public static float gage = 100;
    public static float maxGage = 100;
    public static bool godMode = false;

    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private Image hpImage;
    [SerializeField] private Image gageImage;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI gageText;
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
        if (gameEnd) return;

        if (hp <= 0)
        {
            hp = 0;
            Death();
        }
        else if (hp > maxHP)
        {
            hp = maxHP;
        }
        if (gage <= 0)
        {
            gage = 0;
            Death();
        }
        if (gage > maxGage)
        {
            gage = maxGage;
        }

        hpImage.fillAmount = hp / maxHP;
        gageImage.fillAmount = gage / maxGage;
        hpText.text = hp + "/" + maxHP;
        gageText.text = gage + "/" + maxGage;
    }

    private void Death()
    {
        audioSource.PlayOneShot(deathSound);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Instantiate(endScreen, GameObject.Find("Canvas").transform);
        FindObjectOfType<PlayerHit>().enabled = false;
        FindObjectOfType<PlayerSkill>().enabled = false;
        FindObjectOfType<ShootPlayerBullet>().enabled = false;
        FindObjectOfType<MovePlayer>().enabled = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Repair"))
        {
            audioSource.PlayOneShot(itemSound);
            hp = Mathf.Clamp(hp + 30, 0, maxHP);
            Destroy(collision.gameObject);
            ScoreManager.itemScore += 150;
        }
        if (collision.CompareTag("HealGage"))
        {
            audioSource.PlayOneShot(itemSound);
            gage = Mathf.Clamp(gage + 30, 0, maxGage);
            Destroy(collision.gameObject);
            ScoreManager.itemScore += 150;
        }
    }
}
