using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.eulerAngles += Vector3.forward * speed * Time.deltaTime;
    }
}
