using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerSkill : MonoBehaviour
{
    // ������ ��� �� ����
    [SerializeField] private AudioClip itemSound;
    // ��ų ��Ÿ�� ��
    public float repairCoolTime;
    public float bombCoolTime;
    public float powerUPCoolTime;
    public float beWaterCoolTime;
    public float sunPowerCoolTime;
    [SerializeField] private GameObject dontUseGageEffect;
    // ������ ��ų ��� �� ����
    [SerializeField] private AudioClip repairSound;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip powerUPSound;
    [SerializeField] private AudioClip beWaterSound;
    [SerializeField] private AudioClip sunPowerSound;
    [SerializeField] private GameObject bombEffect;
    // ������ ��ų �̹���
    [SerializeField] private Image repairImage;
    [SerializeField] private Image bombImage;
    [SerializeField] private Image powerUPImage;
    [SerializeField] private Image beWaterImage;
    [SerializeField] private Image sunPowerImage;
    // ������ ��ų ��Ÿ���� ǥ���ϴ� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI repairText;
    [SerializeField] private TextMeshProUGUI bombText;
    [SerializeField] private TextMeshProUGUI powerUPText;
    [SerializeField] private TextMeshProUGUI beWaterText;
    [SerializeField] private TextMeshProUGUI sunPowerText;
    // ������� ǥ���ϴ� �ؽ�Ʈ
    [SerializeField] private GameObject warning;

    // ���� ����� ��ų�� �̸��� �����ϴ� ����
    private string skillName;

    // ���� ��ų�� ���Ḧ �Һ��ϴ����� �����ϴ� bool����
    private bool dontUseGage;

    // ������ ��ų ��Ÿ���� ī��Ʈ�ϴ� ����
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

        #region ��ų�� ����Ͽ� ��Ÿ���� ����ٸ�, ��Ȱ��ȭ ǥ�� �� ������� ���� �ð��� ȭ�鿡 ������
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
        #endregion

        #region ������ Ű �Է� �� ��ų ����
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
        #endregion
    }

    // ��� ���� �ڷ�ƾ�� �����ϴ� �Լ�
    private void Warning()
    {
        // �̹� ��� ���� ���� ��, ���� ��
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
        }
        // ���ο� ��� ���� ����
        coroutine1 = StartCoroutine(JobWarning());
    }

    // ��� ���� ����
    private IEnumerator JobWarning()
    {
        // ��ų �̸��� ���� �ؽ�Ʈ�� 2�ʰ� �ٸ��� ���
        warning.GetComponent<Text>().text = "���� " + skillName + "��(��) ����� �� �����ϴ�!";

        warning.SetActive(true);

        yield return new WaitForSeconds(2);

        warning.SetActive(false);

        coroutine1 = null;
    }

    // ��ų ����
    private void Repair()
    {
        // ��Ÿ���̶�� ��� ���� ���� �� ����
        if (countRepairCoolTime > 0)
        {
            skillName = "����";
            Warning();
            return;
        }

        // ȿ���� ���
        audioSource.PlayOneShot(repairSound);

        // ���Ḧ ������� �ʴ� �������� Ȯ��
        if (dontUseGage)
        {
            // ������� �ʴ´ٸ� bool���� false�� ����
            dontUseGage = false;
        }
        else
        {
            // ����Ѵٸ� ���� �Ҹ�
            PlayerStatus.gage -= 20;
        }

        // ��Ÿ�� ����
        countRepairCoolTime = repairCoolTime;

        // �÷��̾� ü�� ȸ��
        PlayerStatus.hp = Mathf.Clamp(PlayerStatus.hp + 20, 0, PlayerStatus.maxHP);
    }

    // ��ų ��ź
    private void Bomb()
    {
        if (countBombCoolTime > 0)
        {
            skillName = "��ź";
            Warning();
            return;
        }

        audioSource.PlayOneShot(bombSound);

        // ��ź ȿ�� ����
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

        // ���� ������ ��� �Ѿ� ���� ���� ���� HP 10 ����
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

    // ��ų �Ѿ� ��ȭ
    private void PowerUP()
    {
        if (countPowerUPCoolTime > 0)
        {
            skillName = "��ȭ";
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

        // �Ѿ��� �������� 5�� ���
        shootPlayerBullet.PowerUP(5);
    }

    // ��ų ��üȭ
    private void BeWater()
    {
        if (countBeWaterCoolTime > 0)
        {
            skillName = "��üȭ";
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

        // PlayerHit ��ũ��Ʈ���� ��üȭ �������� ����
        playerHit.BeWater();
    }
    
    // ��ų �¾籤 ȸ��
    private void SunPower()
    {
        if (countSunPowerCoolTime > 0)
        {
            skillName = "�¾籤 ȸ��";
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
            // ���� ��� ü�� ����
            PlayerStatus.hp -= 15;
        }

        countSunPowerCoolTime = sunPowerCoolTime;

        // �̹� �¾籤 ȸ�� ���� ���̶�� ���� ��
        if (coroutine2 != null)
        {
            StopCoroutine(coroutine2);
        }
        // ���ο� �¾籤 ȸ�� ����
        coroutine2 = StartCoroutine(JobSunPower());
    }

    // �¾籤 ȸ�� �ڷ�ƾ
    private IEnumerator JobSunPower()
    {
        // �ʴ� 2�� ü�� ȸ��
        for (int count = 0; count < 10; count++)
        {
            PlayerStatus.gage = Mathf.Clamp(PlayerStatus.gage + 1, 0, PlayerStatus.maxGage);
            yield return new WaitForSeconds(0.5f);
        }

        coroutine2 = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ��ų�� ���Ḧ ������� �ʰ� ����� �����ۿ� �ǰݴ��ߴٸ�
        if (collision.CompareTag("GiveGage"))
        {
            // ȿ���� ���
            audioSource.PlayOneShot(itemSound);
            // ���� ��ų�� ���� �Ҹ� ���� ����
            dontUseGage = true;
            // ������ ����
            Destroy(collision.gameObject);
        }

        // ��Ÿ�� ���� �����ۿ� �ǰݴ��ߴٸ�
        if (collision.CompareTag("CoolDown"))
        {
            // ȿ���� ���
            audioSource.PlayOneShot(itemSound);
            // ��� ��ų�� ��Ÿ�� 50% ����
            countBeWaterCoolTime /= 2;
            countBombCoolTime /= 2;
            countPowerUPCoolTime /= 2;
            countRepairCoolTime /= 2;
            countSunPowerCoolTime /= 2;
            // ������ ����
            Destroy(collision.gameObject);
        }
    }
}
