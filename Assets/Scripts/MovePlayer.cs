using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // 이동 속도 값 설정
    [SerializeField] private float speed;

    private void Update()
    {
        Vector3 path = Vector3.zero;

        // W, S, A, D 각각 상, 하, 좌, 우 입력을 받음
        if (Input.GetKey(KeyCode.W))
        {
            path += Vector3.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            path += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            path += Vector3.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            path += Vector3.right;
        }

        path.Normalize();

        transform.position += Time.deltaTime * speed * path;

        // 게임 화면 밖으로 나가지 않게 하기
        Vector3 pathScreen = Camera.main.WorldToScreenPoint(transform.position);
        if (pathScreen.x < 55 || pathScreen.x > Screen.width - 55 || pathScreen.y < 50 || pathScreen.y > Screen.height - 50)
        {
            transform.position -= Time.deltaTime * speed * path;
        }
    }
}
