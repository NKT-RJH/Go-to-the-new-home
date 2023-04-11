using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Vector2 path;
    [SerializeField] private float angle;
    [SerializeField] private float maxTime;
    [SerializeField] private float minTime;
    [SerializeField] private StopSpawn sP;

    private float spawnTime;
    private float countTime;

    public bool stopSpawn;

    private void Start()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        if (!TimeManager.isStart) return;
        if (stopSpawn) return;

        countTime += Time.deltaTime;

        if (countTime >= spawnTime)
        {
            countTime = 0;
            spawnTime = Random.Range(minTime, maxTime);

            Instantiate(enemy, new Vector3(path.x == 0 ? transform.position.x : Random.Range(-path.x, path.x), path.y == 0 ? transform.position.y : Random.Range(-path.y, path.y)), enemy.GetComponent<Enemy>().follow ? Quaternion.identity : Quaternion.Euler(0, 0, angle));
            if (sP != null)
            {
                sP.enabled = true;
            }
        }
    }
}
