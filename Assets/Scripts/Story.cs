using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Story : MonoBehaviour
{
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Text storyText;
    [SerializeField] private string[] texts;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(ShowStory());
    }

    private IEnumerator ShowStory()
    {
        yield return new WaitForSeconds(1.5f);

        for (float count = 255; count > 0; count -= 0.3f)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, count / 255f);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        for (float count = 0; count <= 155; count += 0.4f)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, count / 255f);
            yield return null;
        }

        for (int count = 0; count < texts.Length; count++)
        {
            foreach (char character in texts[count])
            {
                audioSource.PlayOneShot(typingSound);
                storyText.text += character;
                yield return new WaitForSeconds(0.1f);
            }
            storyText.text += "\n";
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Stage1");
    }
}
