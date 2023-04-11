using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }

    public void Job()
    {
        audioSource.PlayOneShot(buttonSound);
    }
}
