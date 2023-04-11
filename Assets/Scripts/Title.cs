using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private GameObject rankingScreen;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private RectTransform content;

    private void Start()
    {
        StartCoroutine(ShowFadeOut());
    }

    private IEnumerator ShowFadeOut()
    {
        blackScreen.gameObject.SetActive(true);
        for (int count = 255; count >= 0; count-=3)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, count / 255f);
            yield return null;
        }
        blackScreen.gameObject.SetActive(false);
    }

    public void OpenRankingScreen()
    {
        rankingScreen.SetActive(true);
        content.anchoredPosition = Vector2.zero;
    }

    public void CloseRankingScreen()
    {
        rankingScreen.SetActive(false);
    }

    public void OpenHelpScreen()
    {
        helpScreen.SetActive(true);
    }

    public void CloseHelpScreen()
    {
        helpScreen.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
