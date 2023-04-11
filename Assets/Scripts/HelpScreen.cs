using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{
    [SerializeField] private List<GameObject> screens = new List<GameObject>();

    private int currentScreen = 0;

    private void Update()
    {
        for (int count = 0; count < screens.Count; count++)
        {
            screens[count].SetActive(false);
        }

        screens[currentScreen].SetActive(true);
    }

    public void GotoLeftScreen()
    {
        currentScreen = Mathf.Clamp(currentScreen - 1, 0, screens.Count - 1);
    }

    public void GotoRightScreen()
    {
        currentScreen = Mathf.Clamp(currentScreen + 1, 0, screens.Count - 1);
    }
}
