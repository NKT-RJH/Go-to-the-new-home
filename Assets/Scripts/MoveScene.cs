using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void Job(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
