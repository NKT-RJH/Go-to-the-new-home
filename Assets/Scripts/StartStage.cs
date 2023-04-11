using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage : MonoBehaviour
{
    [SerializeField] private GameObject startText;
    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(Job());
    }

    private IEnumerator Job()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        FindObjectOfType<PlayerHit>().enabled = false;
        FindObjectOfType<PlayerSkill>().enabled = false;
        FindObjectOfType<PlayerStatus>().enabled = false;
        FindObjectOfType<MovePlayer>().enabled = false;
        FindObjectOfType<ShootPlayerBullet>().enabled = false;

        MoveBackground moveBackground = FindObjectOfType<MoveBackground>();
        float originSpeed = moveBackground.speed;
        for (float speed = originSpeed + 50; speed >= originSpeed; speed -= Time.deltaTime * 20)
        {
            moveBackground.speed = speed;
            yield return null;
        }

        moveBackground.speed = originSpeed;

        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }

        FindObjectOfType<PlayerHit>().enabled = true;
        FindObjectOfType<PlayerSkill>().enabled = true;
        FindObjectOfType<PlayerStatus>().enabled = true;
        FindObjectOfType<MovePlayer>().enabled = true;
        FindObjectOfType<ShootPlayerBullet>().enabled = true;

        TimeManager.isStart = true;

        startText.SetActive(true);

        yield return new WaitForSeconds(3);

        startText.SetActive(false);
    }
}
