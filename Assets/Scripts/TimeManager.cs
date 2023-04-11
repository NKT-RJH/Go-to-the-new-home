using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    public static float time;
    public static float wholeTime;
    public static bool isStart;

    private void Start()
    {
        time = 0;
        isStart = false;
    }

    private void Update()
    {
        if (!isStart) return;

        timeText.text = string.Format("{0}:{1}", (int)(time / 60), (int)(time % 60));

        time += Time.deltaTime;
    }
}
