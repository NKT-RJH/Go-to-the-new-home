using UnityEngine;

public class Enemy2 : Enemy
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

            ShootBullet(bullet, transform.position, transform.rotation, enemyStatus.damage / 2, 4);
        }
    }
}
