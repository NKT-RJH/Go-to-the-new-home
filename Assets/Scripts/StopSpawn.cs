using UnityEngine;

public class StopSpawn : MonoBehaviour
{
    private void OnEnable()
    {
        foreach (SpawnEnemy spawnEnemy in FindObjectsOfType<SpawnEnemy>())
        {
            spawnEnemy.stopSpawn = true;
        }
    }
}
