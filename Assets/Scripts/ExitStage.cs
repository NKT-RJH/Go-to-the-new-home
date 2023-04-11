using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitStage : MonoBehaviour
{
    [SerializeField] private GameObject exitScreen;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!TimeManager.isStart) return;
        if (exitScreen.activeSelf)
        {
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Title");
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                audioSource.Play();
                exitScreen.SetActive(false);
            }
        }
        else
        {
            Time.timeScale = 1;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                audioSource.Pause();
                exitScreen.SetActive(true);
            }
        }
    }
}
