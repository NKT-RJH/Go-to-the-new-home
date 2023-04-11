using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float speed;
    [SerializeField] private float maxY;

    private float originY;

    private void Start()
    {
        originY = transform.position.y;
    }

    private void Update()
    {
        if (transform.position.y <= maxY)
        {
            transform.position = new Vector3(transform.position.x, originY);
        }
        transform.position += Time.deltaTime * speed * Vector3.down;
    }
}
