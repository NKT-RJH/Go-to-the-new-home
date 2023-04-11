using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private int upgrade;
    [SerializeField] private float maxY;
    [SerializeField] private GameObject bossHPBar;
    [SerializeField] private GameObject endScreen;

    private Image bossHPBarImage = null;
    private TextMeshProUGUI bossHPBarText;

    private Coroutine coroutine = null;

    private float countTime;
    private float patternTime;

    protected override void Start()
    {
        base.Start();

        if (upgrade == 0)
        {
            patternTime = Random.Range(3f, 5);
        }
        else if (upgrade == 2)
        {
            patternTime = Random.Range(1f, 3);
        }
        else if (upgrade == 1)
        {
            patternTime = Random.Range(2f, 4);
        }
    }

    protected override void Update()
    {
        if (transform.position.y <= maxY)
        {
            if (bossHPBarImage == null)
            {
                bossHPBarImage = Instantiate(bossHPBar).transform.Find("BossHPBar").Find("HPBar").GetComponent<Image>();
                bossHPBarText = bossHPBarImage.transform.parent.Find("HPText").GetComponent<TextMeshProUGUI>();
            }
            else
            {
                bossHPBarText.text = enemyStatus.hp + "/" + enemyStatus.maxHP;
                bossHPBarImage.fillAmount = enemyStatus.hp / enemyStatus.maxHP;
                countTime += Time.deltaTime;
                if (countTime >= patternTime)
                {
                    countTime = 0;
                    if (upgrade == 1)
                    {
                        patternTime = Random.Range(2f, 4);
                    }
                    else if (upgrade == 2)
                    {
                        patternTime = Random.Range(1f, 3);
                    }
                    else
                    {
                        patternTime = Random.Range(3f, 5);
                    }



                    switch (Random.Range(0, 3 + upgrade))
                    {
                        case 0:
                            Pattern1();
                            break;
                        case 1:
                            Pattern2();
                            break;
                        case 2:
                            Pattern3();
                            break;
                        case 3:
                            Pattern4();
                            break;
                        case 4:
                            Pattern5();
                            break;
                        default:
                            break;
                    }
                }
            }

            return;
        }

        base.Update();
    }

    private void Pattern1()
    {
        for (int angle = 0; angle < 360; angle += 20)
        {
            ShootBullet(bullet, transform.position, Quaternion.Euler(0, 0, angle), enemyStatus.damage / 2, 3);
        }
    }
    private void Pattern2()
    {
        StartCoroutine(JobPattern2());
    }
    private IEnumerator JobPattern2()
    {
        for (int angle = 0; angle < 360; angle += 10)
        {
            ShootBullet(bullet, transform.position, Quaternion.Euler(0, 0, angle), enemyStatus.damage / 2, 3);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void Pattern3()
    {
        int value = Random.Range(130, 211);
        for (int angle = 0; angle < 360; angle += 5)
        {
            if (angle >= value && angle <= value + 20) continue;
            ShootBullet(bullet, transform.position, Quaternion.Euler(0, 0, angle), enemyStatus.damage / 2, 3);
        }
    }

    private void Pattern4()
    {
        StartCoroutine(JobPattern4());
    }

    private IEnumerator JobPattern4()
    {
        for (int angle = 0; angle <= 180; angle += 5)
        {
            ShootBullet(bullet, transform.position, Quaternion.Euler(0, 0, angle), enemyStatus.damage / 2, 4);
        }

        yield return new WaitForSeconds(1);

        for (int angle = 190; angle < 360; angle += 5)
        {
            ShootBullet(bullet, transform.position, Quaternion.Euler(0, 0, angle), enemyStatus.damage / 2, 4);
        }
    }

    private void Pattern5()
    {
        if (coroutine != null) return;

        coroutine = StartCoroutine(JobPattern5());
    }

    private IEnumerator JobPattern5()
    {
        int angle = Random.Range(120, 241);
        for (float time = 0; time < 10; time += 0.1f)
        {
            ShootBullet(bullet, transform.position, Quaternion.Euler(0, 0, angle), enemyStatus.damage / 2, 3);
            yield return new WaitForSeconds(0.1f);
        }

        coroutine = null;
    }

    private void OnDestroy()
    {
        if (PlayerStatus.hp <= 0 || PlayerStatus.gage <= 0) return;
        if (bossHPBarImage != null)
        {
            Destroy(bossHPBarImage.transform.parent.parent.gameObject);
        }
        Instantiate(endScreen, GameObject.FindGameObjectWithTag("Canvas").transform);
    }
}
