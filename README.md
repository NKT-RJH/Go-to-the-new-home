# Go-to-the-new-home

<h4>2023년 지방기능경기대회 게임 개발 부문 (새로운 행성을 찾아 떠나는 2D 슈팅 게임)</h4>

<hr class='hr-solid'/>

<h4>게임 시연</h4>

<A href="https://youtu.be/2nEvY-u_gfs"> 게임 시연 영상 </A><br>

<hr class='hr-solid'/>

<h3>게임 구조</h3>

<details>
<summary><i>인 게임 이미지</i></summary>
<br>
  - 타이틀<br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/478a6ee4-a56f-43a8-b145-b22b01abb0d2"><br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/81544c5f-b319-4762-ac51-44489369cc0c"><br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/c8791972-3bc9-40f4-91f5-4b8cba0f7c84"><br>
    <br>
  - 스토리<br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/92ad55d4-efa4-4483-b5fd-fc5f075bdd5d"><br>
    <br>
  - 플레이<br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/b3dbceb6-66c3-42c5-be8d-acae83da5b4e"><br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/f36b0672-ac68-484b-9eaf-03b7e4dcbd37"><br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/94de6877-3f77-4192-bc40-00733e50547a"><br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/48106e10-7b4e-4d86-bbc7-60b491dd7713"><br>
    <img width="640" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/df76d981-92d9-4a44-ab2d-2b4bfb902f51"><br>
  <br>
</details>

<img width="700" alt="image" src="https://github.com/NKT-RJH/Go-to-the-new-home/assets/80941288/bbc04ce0-3d29-4ff9-ba3f-11222a5bd5bb"><br>

<hr class='hr-solid'/>

<h3>주요 코드</h3>
<b>MovePlayer</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 플레이어의 이동을 담당하는 스크립트입니다.<br>
&nbsp;&nbsp;&nbsp;&nbsp;● W, A, S, D로 상하좌우로 이동할 수 있고, 게임 화면에서만 이동할 수 있습니다.
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // 이동 속도 값 설정
    [SerializeField] private float speed;

    private void Update()
    {
        Vector3 path = Vector3.zero;

        // W, S, A, D 각각 상, 하, 좌, 우 입력을 받음
        if (Input.GetKey(KeyCode.W))
        {
            path += Vector3.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            path += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            path += Vector3.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            path += Vector3.right;
        }

        path.Normalize();

        transform.position += Time.deltaTime * speed * path;

        // 게임 화면 밖으로 나가지 않게 하기
        Vector3 pathScreen = Camera.main.WorldToScreenPoint(transform.position);
        if (pathScreen.x < 55 || pathScreen.x > Screen.width - 55 || pathScreen.y < 50 || pathScreen.y > Screen.height - 50)
        {
            transform.position -= Time.deltaTime * speed * path;
        }
    }
}
  ```
</details><br>

<b>PlayerHit</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 플레이어의 피격을 담당하는 스크립트입니다.<br>
&nbsp;&nbsp;&nbsp;&nbsp;● 적, 아이템, 점수 오브젝트에 닿았을 때 수행하는 동작도 포함하며, 무적 판정도 관리합니다.
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHit : MonoBehaviour
{
    // 각각의 오브젝트에 충돌하였을 때의 사운드
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip scoreObjectSound;

    private SpriteRenderer spriteRenderer;
    // 무적 모드가 총 3개(적에게 피격 시, 스킬 액체화, 아이템 일시무적)가 있기에 이를 각각 관리하기 위한 코루틴
    private Coroutine coroutine1 = null;
    private Coroutine coroutine2 = null;
    private Coroutine coroutine3 = null;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 피격 시, 깜빡이며 무적판정이 생기는 코루틴을 실행하는 함수
    private void HitEffect()
    {
        // 이미 해당 무적이 실행중이라면 종료하고
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
        }
        // 새로운 무적 실행
        coroutine1 = StartCoroutine(JobHitEffect());
    }

    // 피격 시, 깜빡이며 무적판정이 생기는 코루틴
    private IEnumerator JobHitEffect()
    {
        // 무적판정 활성화
        PlayerStatus.godMode = true;

        // 플레이어의 SpriteRenderer의 투명도를 변경하여 깜빡이게 만드는 코드
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

        // 무적판정 비활성화
        PlayerStatus.godMode = false;

        // 무적 판정이 끝났기에 변수에서 제거
        coroutine1 = null;
    }
    
    // 아이템 일시무적 코루틴을 실행하는 함수
    private void GodMode()
    {
        // 다른 무적판정이 실행중이라면 모두 비활성화 후
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
        // 아이템 일시무적 실행
        coroutine2 = StartCoroutine(JobGodMode());
    }

    // 아이템 일시무적
    private IEnumerator JobGodMode()
    {
        // 5초간 SpriteRenderer가 파란색으로 변하며 무적판정 실행
        PlayerStatus.godMode = true;

        spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(5);

        spriteRenderer.color = Color.white;

        PlayerStatus.godMode = false;

        // 무적 판정이 끝났기에 변수에서 제거
        coroutine2 = null;
    }

    // 스킬 액체화 무적판정 코루틴을 실행하는 함수
    public void BeWater()
    {
        // 다른 무적판정이 실행 중이라면 종료 후
        if (coroutine1 != null)
        {
            StopCoroutine(coroutine1);
            coroutine1 = null;
        }
        if (coroutine3 != null)
        {
            StopCoroutine(coroutine3);
        }
        // 스킬 액체화 무적 실행
        coroutine3 = StartCoroutine(JobBeWater());
    }

    // 스킬 액체화 무적
    private IEnumerator JobBeWater()
    {
        // 0.75초간 초록색으로 무적 판정 실행
        PlayerStatus.godMode = true;

        spriteRenderer.color = Color.green;

        yield return new WaitForSeconds(0.75f);

        spriteRenderer.color = Color.white;

        PlayerStatus.godMode = false;

        // 무적 판정이 끝났기에 변수에서 제거
        coroutine3 = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 일시 무적 아이템에 닿았을 떄
        if (collision.CompareTag("GodMode"))
        {
            // 효과음 출력 후
            audioSource.PlayOneShot(itemSound);
            // 일시 무적 활성화
            GodMode();
            // 일시 무적 아이템 삭제
            Destroy(collision.gameObject);
            // 점수 획득
            ScoreManager.itemScore += 150;
            return;
        }

        // 점수 오브젝트에 닿았을 때
        if (collision.CompareTag("ScoreObject"))
        {
            // 효과음 출력 후
            audioSource.PlayOneShot(scoreObjectSound);
            // 점수 획득
            ScoreManager.score += collision.GetComponent<ScoreObject>().score;
            // 점수 오브젝트 삭제
            Destroy(collision.gameObject);
        }

        // 무적 판정이 아닐 때
        if (!PlayerStatus.godMode)
        {
            // 적에게 피격 시
            if (collision.CompareTag("Enemy"))
            {
                // 효과음 출력 후
                audioSource.PlayOneShot(hitSound);
                // 플레이어 HP 감소
                PlayerStatus.hp -= collision.GetComponent<EnemyStatus>().damage;
                // 피격 무적판정 실행
                HitEffect();
                // 공격한 대상이 총알이라면 삭제
                if (collision.GetComponent<Bullet>())
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
  ```
</details><br>

<b>PlayerSkill</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 플레이어의 스킬을 관리하는 스크립트입니다.
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
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
  ```
</details>

<b>PlayerStatus</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 플레이어의 스텟을 관리하는 스크립트입니다.<br>
&nbsp;&nbsp;&nbsp;&nbsp;● 추가로 사망 효과, 체력 회복 및 연료 회복 아이템 피격 시 명령 수행을 관리합니다.
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    // 플레이어의 스텟
    public static float hp = 100;
    public static float maxHP = 100;
    public static float gage = 100;
    public static float maxGage = 100;
    public static bool godMode = false;

    // 효과음
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;
    // 체력 UI 바
    [SerializeField] private Image hpImage;
    // 연료 UI 바
    [SerializeField] private Image gageImage;
    // 체력 UI 텍스트
    [SerializeField] private TextMeshProUGUI hpText;
    // 연료 UI 텍스트
    [SerializeField] private TextMeshProUGUI gageText;
    // 게임 종료 화면 오브젝트
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
        // 게임이 종료되었다면 Update가 아래 코드를 실행하지 않음
        if (gameEnd) return;

        // 체력 상태에 따른 명령 실행
        if (hp <= 0)
        {
            // 체력이 0 이하라면 사망 함수 실행
            hp = 0;
            Death();
        }
        else if (hp > maxHP)
        {
            // 체력이 최대 체력을 초과한다면 최대 체력으로 값 조정
            hp = maxHP;
        }
        // 연료가 0 이하라면 사망 함수 실행
        if (gage <= 0)
        {
            gage = 0;
            Death();
        }
        // 연료가 최대 연료를 초과한다면 최대 연료값으로 조정
        if (gage > maxGage)
        {
            gage = maxGage;
        }

        // 체력과 연료 UI 형식으로 화면에 출력
        hpImage.fillAmount = hp / maxHP;
        gageImage.fillAmount = gage / maxGage;
        hpText.text = hp + "/" + maxHP;
        gageText.text = gage + "/" + maxGage;
    }

    // 사망 시, 효과
    private void Death()
    {
        // 사망 사운드 출력
        audioSource.PlayOneShot(deathSound);
        // 이펙트 출력
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        // 게임 종료 스크린 실행
        Instantiate(endScreen, GameObject.Find("Canvas").transform);
        // 플레이어와 관련된 스크립트 비활성화
        FindObjectOfType<PlayerHit>().enabled = false;
        FindObjectOfType<PlayerSkill>().enabled = false;
        FindObjectOfType<ShootPlayerBullet>().enabled = false;
        FindObjectOfType<MovePlayer>().enabled = false;
        // 플레이어 오브젝트 삭제
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 체력 회복 아이템 피격 시
        if (collision.CompareTag("Repair"))
        {
            // 효과음 출력
            audioSource.PlayOneShot(itemSound);
            // 체력 회복
            hp = Mathf.Clamp(hp + 30, 0, maxHP);
            // 아이템 삭제
            Destroy(collision.gameObject);
            // 점수 증가
            ScoreManager.itemScore += 150;
        }
        // 연료 회복 아이템 피격 시
        if (collision.CompareTag("HealGage"))
        {
            // 효과음 출력
            audioSource.PlayOneShot(itemSound);
            // 연료 회복
            gage = Mathf.Clamp(gage + 30, 0, maxGage);
            // 아이템 삭제
            Destroy(collision.gameObject);
            // 점수 증가
            ScoreManager.itemScore += 150;
        }
    }
}
  ```
</details>

<b>Enemy</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 적을 관리하는 스크립트입니다.<br>
&nbsp;&nbsp;&nbsp;&nbsp;● 스테이터스, 이동, 피격, 공격, 사망 등의 기능이 있습니다.
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
public class Enemy : MonoBehaviour
{
    // 효과음
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;
    // 사망 시 생성되는 점수 오브젝트
    [SerializeField] private GameObject scoreObject;
    // 사망 시 생성되는 아이템의 오브젝트 배열
    [SerializeField] private GameObject[] items = new GameObject[6];
    // 생성되는 점수 오브젝트 종류 설정
    [SerializeField] private bool dropScoreObject;
    // 생성되는 아이템 종류 설정
    [SerializeField] private bool dropItem;
    // 효과 사용 여부
    [SerializeField] private bool dontUseEffect;
    // 아이템 확률 설정
    [Range(0, 100)] [SerializeField] private int spawnItemValue;
    // 플레이어를 추적하는지 설정
    public bool follow;

    // 적의 능력치를 관리하는 변수
    protected EnemyStatus enemyStatus = null;

    // 이동 속도 설정
    public float speed;

    // 피격 시, 빨간 색으로 변하는 코루틴을 관리하는 변수
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

        // 플레이어 오브젝트를 추적한다면
        if (follow)
        {
            // 플레이어를 바라보도록 설정
            transform.rotation = LookAt(playerTransform.position);
        }
    }

    protected virtual void Update()
    {
        // 체력이 0 이라면
        if (enemyStatus.hp <= 0)
        {
            // 사망
            Death();
        }

        // 정해진 각도와 속도에 따라 이동
        transform.Translate(speed * Time.deltaTime * Vector3.up, Space.Self);
    }

    // 받아온 Vector3 값을 바라보게 하는 함수
    protected Quaternion LookAt(Vector3 target)
    {
        Vector3 path = target - transform.position;

        float value = Mathf.Atan2(path.y, path.x) * Mathf.Rad2Deg;

        return Quaternion.AngleAxis(value - 90, Vector3.forward);
    }

    // 총알을 데미지, 속도, 각도를 지정하여 생성하는 함수
    protected Bullet ShootBullet(GameObject bullet, Vector3 path, Quaternion angle, float damage, float speed)
    {
        Bullet bulletObject = Instantiate(bullet, path, angle).GetComponent<Bullet>();
        bulletObject.GetComponent<EnemyStatus>().damage = damage;
        bulletObject.speed = speed;

        return bulletObject;
    }

    // 피격 시 효과 출력 코루틴을 실행하는 함수
    private void HitEffect()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(JobHitEffect());
    }

    // 피격 시 효과 출력
    private IEnumerator JobHitEffect()
    {
        // 0.1초간 빨간색으로 변화
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

        coroutine = null;
    }

    // 사망 시, 효과
    private void Death()
    {
        // 점수 추가
        ScoreManager.enemyScore += enemyStatus.score;

        int value = Random.Range(0, 100);

        // 랜덤 값으로 아이템 생성
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

    // 게임 화면 밖으로 나갔다면 오브젝트 삭제
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
        // 플레이어 총알에 피격 시
        if (collision.CompareTag("Bullet"))
        {
            // 체력 감소
            enemyStatus.hp -= ShootPlayerBullet.damage;
            if (!dontUseEffect)
            {
                // 효과 출력
                HitEffect();
            }
            // 총알 오브젝트 삭제
            Destroy(collision.gameObject);
            // 체력이 0 이하라면
            if (enemyStatus.hp <= 0)
            {
                // 사망
                Death();
            }
        }
    }
}
  ```
</details>

<b>Ranking</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 랭킹을 관리하는 스크립트입니다.<br>
&nbsp;&nbsp;&nbsp;&nbsp;● 추가로 랭킹 정보를 PlayerPrefs를 이용하여 저장, 초기화, 불러오기 등의 기능을 수행할 수 있습니다.
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    // 랭킹 정보를 저장하는 리스트
    public static List<RankData> ranks = new List<RankData>();
    // 랭킹 바를 생성하는 위치
    [SerializeField] private Transform grid;
    // 랭킹 바 프리팹
    [SerializeField] private GameObject rankBar;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        string beforeString = null;
        // grid에 형식에 맞게 랭킹 바를 생성
        for (int count = 0; count < ranks.Count; count++)
        {
            if (ranks[count].name.Equals(beforeString)) continue;
            Transform rankBarTransform = Instantiate(rankBar, grid).transform;
            rankBarTransform.Find("Rank").GetComponent<Text>().text = (count + 1) + "등";
            rankBarTransform.Find("Name").GetComponent<Text>().text = ranks[count].name;
            rankBarTransform.Find("Score").GetComponent<Text>().text = ranks[count].score + "점";
            beforeString = ranks[count].name;
        }
    }

    // 리스트에 정보 추가
    public static void Add(string name, int score)
    {
        ranks.Add(new RankData(name, score));
        // 점수를 기준으로 내림차순 정렬
        ranks = ranks.OrderByDescending(x => x.score).ToList();
    }

    // PlayerPrefs로 저장
    public static void Save()
    {
        // 기존 데이터 삭제
        PlayerPrefs.DeleteAll();
        // 새로운 데이터 추가
        for (int count = 0; count < ranks.Count; count++)
        {
            PlayerPrefs.SetString(count + "s", ranks[count].name);
            PlayerPrefs.SetInt(count.ToString(), ranks[count].score);
        }
        // 저장
        PlayerPrefs.Save();
    }

    // 랭킹 초기화
    public void Remove()
    {
        // 랭킹 정보 초기화
        ranks = new List<RankData>();
        // 저장
        Save();
        // 생성된 랭킹 바 삭제
        int childCount = grid.childCount;
        for (int count = 0; count < childCount; count++)
        {
            Destroy(grid.GetChild(count).gameObject);
        }
    }

    // PlayerPrefs 세이브 데이터 불러오기
    private void Load()
    {
        // PlayerPrefs에 세이브 정보가 있다면 불러오기
        for (int count = 0; PlayerPrefs.HasKey(count.ToString()); count++)
        {
            Add(PlayerPrefs.GetString(count + "s"), PlayerPrefs.GetInt(count.ToString()));
        }
    }
}
  ```
</details>
