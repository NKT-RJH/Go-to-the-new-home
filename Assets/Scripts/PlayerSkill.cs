using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private AudioClip itemSound;
    public float repairCoolTime;
    public float bombCoolTime;
    public float powerUPCoolTime;
    public float beWaterCoolTime;
    public float sunPowerCoolTime;
    [SerializeField] private GameObject dontUseGageEffect;
    [SerializeField] private AudioClip repairSound;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip powerUPSound;
    [SerializeField] private AudioClip beWaterSound;
    [SerializeField] private AudioClip sunPowerSound;
    [SerializeField] private GameObject bombEffect;
    [SerializeField] private Image repairImage;
    [SerializeField] private Image bombImage;
    [SerializeField] private Image powerUPImage;
    [SerializeField] private Image beWaterImage;
    [SerializeField] private Image sunPowerImage;
    [SerializeField] private TextMeshProUGUI repairText;
    [SerializeField] private TextMeshProUGUI bombText;
    [SerializeField] private TextMeshProUGUI powerUPText;
    [SerializeField] private TextMeshProUGUI beWaterText;
    [SerializeField] private TextMeshProUGUI sunPowerText;
    [SerializeField] private GameObject warning;

    private string skillName;

    private bool dontUseGage;

    public float countRepairCoolTime;
    public float countBombCoolTime;
    public float countPowerUPCoolTime;
    public float countBeWaterCoolTime;
    public float countSunPowerCoolTime;

    private PlayerHit playerHit;
    private ShootPlayerBullet shootPlayerBullet;
    private Coroutine coroutine1 = null;
    private Coroutine coroutine2 = null;
    private AudioSource audioSource;

    private void Start()
    {
        playerHit = GetComponent<PlayerHit>();
        shootPlayerBullet = GetComponent<ShootPlayerBullet>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        dontUseGageEffect.SetActive(dontUseGage);

        if (countRepairCoolTime > 0)
        {
            countRepairCoolTime -= Time.deltaTime;
            repairImage.gameObject.SetActive(true);
            repairText.gameObject.SetActive(true);
            repairImage.fillAmount = countRepairCoolTime / repairCoolTime;
            repairText.text = ((int)countRepairCoolTime + 1).ToString();
        }
        else
        {
            repairImage.gameObject.SetActive(false);
            repairText.gameObject.SetActive(false);
        }
        if (countBombCoolTime > 0)
        {
            countBombCoolTime -= Time.deltaTime;
            bombImage.gameObject.SetActive(true);
            bombText.gameObject.SetActive(true);
            bombImage.fillAmount = countBombCoolTime / bombCoolTime;
            bombText.text = ((int)countBombCoolTime + 1).ToString();
        }
        else
        {
            bombImage.gameObject.SetActive(false);
            bombText.gameObject.SetActive(false);
        }
        if (countPowerUPCoolTime > 0)
        {
            countPowerUPCoolTime -= Time.deltaTime;
            powerUPImage.gameObject.SetActive(true);
            powerUPText.gameObject.SetActive(true);
            powerUPImage.fillAmount = countPowerUPCoolTime / powerUPCoolTime;
            powerUPText.text = ((int)countPowerUPCoolTime + 1).ToString();
        }
        else
        {
            powerUPImage.gameObject.SetActive(false);
            powerUPText.gameObject.SetActive(false);
        }
        if (countBeWaterCoolTime > 0)
        {
            countBeWaterCoolTime -= Time.deltaTime;
            beWaterImage.gameObject.SetActive(true);
            beWaterText.gameObject.SetActive(true);
            beWaterImage.fillAmount = countBeWaterCoolTime / beWaterCoolTime;
            beWaterText.text = ((int)countBeWaterCoolTime + 1).ToString();
        }
        else
        {
            beWaterImage.gameObject.SetActive(false);
            beWaterText.gameObject.SetActive(false);
        }
        if (countSunPowerCoolTime > 0)
        {
            countSunPowerCoolTime -= Time.deltaTime;
            sunPowerImage.gameObject.SetActive(true);
            sunPowerText.gameObject.SetActive(true);
            sunPowerImage.fillAmount = countSunPowerCoolTime / sunPowerCoolTime;
            sunPowerText.text = ((int)countSunPowerCoolTime + 1).ToString();
        }
        else
        {
            sunPowerImage.gameObject.SetActive(false);
            sunPowerText.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Repair();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Bomb();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            PowerUP();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            BeWater();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SunPower();
        }
    }

    private void Warning()
    {
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
        }
        coroutine1 = StartCoroutine(JobWarning());
    }

    private IEnumerator JobWarning()
    {
        warning.GetComponent<Text>().text = "아직 " + skillName + "을(를) 사용할 수 없습니다!";

        warning.SetActive(true);

        yield return new WaitForSeconds(2);

        warning.SetActive(false);

        coroutine1 = null;
    }

    private void Repair()
    {
        if (countRepairCoolTime > 0)
        {
            skillName = "수리";
            Warning();
            return;
        }

        audioSource.PlayOneShot(repairSound);

        if (dontUseGage)
        {
            dontUseGage = false;
        }
        else
        {
            PlayerStatus.gage -= 20;
        }

        countRepairCoolTime = repairCoolTime;

        PlayerStatus.hp = Mathf.Clamp(PlayerStatus.hp + 20, 0, PlayerStatus.maxHP);
    }

    private void Bomb()
    {
        if (countBombCoolTime > 0)
        {
            skillName = "폭탄";
            Warning();
            return;
        }

        audioSource.PlayOneShot(bombSound);

        Instantiate(bombEffect, transform.position, Quaternion.identity);

        if (dontUseGage)
        {
            dontUseGage = false;
        }
        else
        {
            PlayerStatus.gage -= 15;
        }

        countBombCoolTime = bombCoolTime;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<Bullet>())
            {
                Destroy(enemy);
            }
            else
            {
                enemy.GetComponent<EnemyStatus>().hp -= 10;
            }
        }
    }

    private void PowerUP()
    {
        if (countPowerUPCoolTime > 0)
        {
            skillName = "강화";
            Warning();
            return;
        }

        audioSource.PlayOneShot(powerUPSound);

        if (dontUseGage)
        {
            dontUseGage = false;
        }
        else
        {
            PlayerStatus.gage -= 5;
        }

        countPowerUPCoolTime = powerUPCoolTime;

        shootPlayerBullet.PowerUP(5);
    }

    private void BeWater()
    {
        if (countBeWaterCoolTime > 0)
        {
            skillName = "액체화";
            Warning();
            return;
        }

        audioSource.PlayOneShot(beWaterSound);

        if (dontUseGage)
        {
            dontUseGage = false;
        }
        else
        {
            PlayerStatus.gage -= 30;
        }

        countBeWaterCoolTime = beWaterCoolTime;

        playerHit.BeWater();
    }

    private void SunPower()
    {
        if (countSunPowerCoolTime > 0)
        {
            skillName = "태양광 회복";
            Warning();
            return;
        }

        audioSource.PlayOneShot(sunPowerSound);

        if (dontUseGage)
        {
            dontUseGage = false;
        }
        else
        {
            PlayerStatus.hp -= 15;
        }

        countSunPowerCoolTime = sunPowerCoolTime;

        if (coroutine2 != null)
        {
            StopCoroutine(coroutine2);
        }
        coroutine2 = StartCoroutine(JobSunPower());
    }

    private IEnumerator JobSunPower()
    {
        for (int count = 0; count < 10; count++)
        {
            PlayerStatus.gage = Mathf.Clamp(PlayerStatus.gage + 1, 0, PlayerStatus.maxGage);
            yield return new WaitForSeconds(0.5f);
        }

        coroutine2 = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GiveGage"))
        {
            audioSource.PlayOneShot(itemSound);
            dontUseGage = true;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("CoolDown"))
        {
            audioSource.PlayOneShot(itemSound);
            countBeWaterCoolTime /= 2;
            countBombCoolTime /= 2;
            countPowerUPCoolTime /= 2;
            countRepairCoolTime /= 2;
            countSunPowerCoolTime /= 2;
            Destroy(collision.gameObject);
        }
    }
}
