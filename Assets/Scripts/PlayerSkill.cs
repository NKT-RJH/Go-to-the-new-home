using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerSkill : MonoBehaviour
{
    // 아이템 사용 시 사운드
    [SerializeField] private AudioClip itemSound;
    // 스킬 쿨타임 값
    public float repairCoolTime;
    public float bombCoolTime;
    public float powerUPCoolTime;
    public float beWaterCoolTime;
    public float sunPowerCoolTime;
    [SerializeField] private GameObject dontUseGageEffect;
    // 각각의 스킬 사용 시 사운드
    [SerializeField] private AudioClip repairSound;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip powerUPSound;
    [SerializeField] private AudioClip beWaterSound;
    [SerializeField] private AudioClip sunPowerSound;
    [SerializeField] private GameObject bombEffect;
    // 각각의 스킬 이미지
    [SerializeField] private Image repairImage;
    [SerializeField] private Image bombImage;
    [SerializeField] private Image powerUPImage;
    [SerializeField] private Image beWaterImage;
    [SerializeField] private Image sunPowerImage;
    // 각각의 스킬 쿨타임을 표시하는 텍스트
    [SerializeField] private TextMeshProUGUI repairText;
    [SerializeField] private TextMeshProUGUI bombText;
    [SerializeField] private TextMeshProUGUI powerUPText;
    [SerializeField] private TextMeshProUGUI beWaterText;
    [SerializeField] private TextMeshProUGUI sunPowerText;
    // 경고문구를 표시하는 텍스트
    [SerializeField] private GameObject warning;

    // 현재 사용한 스킬의 이름을 저장하는 변수
    private string skillName;

    // 다음 스킬은 연료를 소비하는지를 결정하는 bool변수
    private bool dontUseGage;

    // 각각의 스킬 쿨타임을 카운트하는 변수
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

        #region 스킬을 사용하여 쿨타임이 생겼다면, 비활성화 표시 및 재사용까지 남은 시간을 화면에 보여줌
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

        #region 각각의 키 입력 시 스킬 실행
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

    // 경고 문구 코루틴을 실행하는 함수
    private void Warning()
    {
        // 이미 경고 문구 실행 시, 종료 후
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
        }
        // 새로운 경고 문구 실행
        coroutine1 = StartCoroutine(JobWarning());
    }

    // 경고 문구 실행
    private IEnumerator JobWarning()
    {
        // 스킬 이름에 따라 텍스트를 2초간 다르게 출력
        warning.GetComponent<Text>().text = "아직 " + skillName + "을(를) 사용할 수 없습니다!";

        warning.SetActive(true);

        yield return new WaitForSeconds(2);

        warning.SetActive(false);

        coroutine1 = null;
    }

    // 스킬 수리
    private void Repair()
    {
        // 쿨타임이라면 경고 문구 실행 후 중지
        if (countRepairCoolTime > 0)
        {
            skillName = "수리";
            Warning();
            return;
        }

        // 효과음 출력
        audioSource.PlayOneShot(repairSound);

        // 연료를 사용하지 않는 상태인지 확인
        if (dontUseGage)
        {
            // 사용하지 않는다면 bool값을 false로 설정
            dontUseGage = false;
        }
        else
        {
            // 사용한다면 연료 소모
            PlayerStatus.gage -= 20;
        }

        // 쿨타임 지정
        countRepairCoolTime = repairCoolTime;

        // 플레이어 체력 회복
        PlayerStatus.hp = Mathf.Clamp(PlayerStatus.hp + 20, 0, PlayerStatus.maxHP);
    }

    // 스킬 폭탄
    private void Bomb()
    {
        if (countBombCoolTime > 0)
        {
            skillName = "폭탄";
            Warning();
            return;
        }

        audioSource.PlayOneShot(bombSound);

        // 폭탄 효과 실행
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

        // 적이 생성한 모든 총알 삭제 또한 적의 HP 10 감소
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

    // 스킬 총알 강화
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

        // 총알의 데미지를 5로 상승
        shootPlayerBullet.PowerUP(5);
    }

    // 스킬 액체화
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

        // PlayerHit 스크립트에서 액체화 무적판정 실행
        playerHit.BeWater();
    }
    
    // 스킬 태양광 회복
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
            // 연료 대신 체력 감소
            PlayerStatus.hp -= 15;
        }

        countSunPowerCoolTime = sunPowerCoolTime;

        // 이미 태양광 회복 실행 중이라면 중지 후
        if (coroutine2 != null)
        {
            StopCoroutine(coroutine2);
        }
        // 새로운 태양광 회복 실행
        coroutine2 = StartCoroutine(JobSunPower());
    }

    // 태양광 회복 코루틴
    private IEnumerator JobSunPower()
    {
        // 초당 2씩 체력 회복
        for (int count = 0; count < 10; count++)
        {
            PlayerStatus.gage = Mathf.Clamp(PlayerStatus.gage + 1, 0, PlayerStatus.maxGage);
            yield return new WaitForSeconds(0.5f);
        }

        coroutine2 = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 다음 스킬은 연료를 사용하지 않게 만드는 아이템에 피격당했다면
        if (collision.CompareTag("GiveGage"))
        {
            // 효과음 출력
            audioSource.PlayOneShot(itemSound);
            // 다음 스킬은 연료 소모를 하지 않음
            dontUseGage = true;
            // 아이템 삭제
            Destroy(collision.gameObject);
        }

        // 쿨타임 감소 아이템에 피격당했다면
        if (collision.CompareTag("CoolDown"))
        {
            // 효과음 출력
            audioSource.PlayOneShot(itemSound);
            // 모든 스킬의 쿨타임 50% 감소
            countBeWaterCoolTime /= 2;
            countBombCoolTime /= 2;
            countPowerUPCoolTime /= 2;
            countRepairCoolTime /= 2;
            countSunPowerCoolTime /= 2;
            // 아이템 삭제
            Destroy(collision.gameObject);
        }
    }
}
