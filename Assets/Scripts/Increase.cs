using UnityEngine;

public class Increase : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSize;

    private void Update()
    {
        if (transform.localScale.x >= maxSize)
        {
            Destroy(gameObject);
        }

        transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime * speed;
    }
}
