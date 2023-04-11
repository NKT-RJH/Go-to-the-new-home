using UnityEngine;

public class Item : MonoBehaviour
{
    [Range(0, 5)][SerializeField] private int index;
    [SerializeField] private float speed;

    private void Start()
    {
        switch (index)
        {
            case 0:
                tag = "Weapon";
                break;
            case 1:
                tag = "GodMode";
                break;
            case 2:
                tag = "Repair";
                break;
            case 3:
                tag = "HealGage";
                break;
            case 4:
                tag = "CoolDown";
                break;
            case 5:
                tag = "GiveGage";
                break;
            default:
                break;
        }
    }

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
