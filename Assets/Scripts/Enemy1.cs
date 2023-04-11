using UnityEngine;

public class Enemy1 : Enemy
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float delay;

    private float countTime;

    protected override void Update()
    {
        base.Update();

        countTime += Time.deltaTime;

        if (countTime >= delay)
        {
            countTime = 0;

            if (playerTransform != null)
            {
                ShootBullet(bullet, transform.position, LookAt(playerTransform.position), enemyStatus.damage / 2, 3);
            }
        }
    }
}