using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up, Space.Self);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
