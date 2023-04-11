using UnityEngine;

public class Enemy3 : Enemy
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float delay;

    private float countTime;

    private float[] bulletPath = { 0, -15, 15 };

    protected override void Update()
    {
        base.Update();

        countTime += Time.deltaTime;

        if (countTime >= delay)
        {
            countTime = 0;

            if (playerTransform != null)
            {
                for (int count = 0; count < bulletPath.Length; count++)
                {
                    Bullet bulletObject = ShootBullet(bullet, transform.position, LookAt(playerTransform.position), enemyStatus.damage / 2, 3);
                    bulletObject.transform.eulerAngles += Vector3.forward * bulletPath[count];
                }
            }
        }
    }
}
