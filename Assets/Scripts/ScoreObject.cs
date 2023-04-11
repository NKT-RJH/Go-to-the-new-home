using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObject : MonoBehaviour
{
    public int score;
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }

    private void OnBecameInvisible()
    {
        try
        {
            Vector3 path = Camera.main.WorldToScreenPoint(transform.position);

            if (path.x < 0 || path.x > Screen.width || path.y < 0 || path.y > Screen.height)
            {
                Destroy(gameObject);
            }
        }
        catch (System.NullReferenceException)
        {
            return;
        }
    }
}
