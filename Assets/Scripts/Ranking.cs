using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public static List<RankData> ranks = new List<RankData>();
    [SerializeField] private Transform grid;
    [SerializeField] private GameObject rankBar;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        string beforeString = null;
        for (int count = 0; count < ranks.Count; count++)
        {
            if (ranks[count].name.Equals(beforeString)) continue;
            Transform rankBarTransform = Instantiate(rankBar, grid).transform;
            rankBarTransform.Find("Rank").GetComponent<Text>().text = (count + 1) + "µî";
            rankBarTransform.Find("Name").GetComponent<Text>().text = ranks[count].name;
            rankBarTransform.Find("Score").GetComponent<Text>().text = ranks[count].score + "Á¡";
            beforeString = ranks[count].name;
        }
    }

    public static void Add(string name, int score)
    {
        ranks.Add(new RankData(name, score));
        ranks = ranks.OrderByDescending(x => x.score).ToList();
    }

    public static void Save()
    {
        PlayerPrefs.DeleteAll();
        for (int count = 0; count < ranks.Count; count++)
        {
            PlayerPrefs.SetString(count + "s", ranks[count].name);
            PlayerPrefs.SetInt(count.ToString(), ranks[count].score);
        }
        PlayerPrefs.Save();
    }

    public void Remove()
    {
        ranks = new List<RankData>();
        Save();
        int childCount = grid.childCount;
        for (int count = 0; count < childCount; count++)
        {
            Destroy(grid.GetChild(count).gameObject);
        }
    }

    private void Load()
    {
        for (int count = 0; PlayerPrefs.HasKey(count.ToString()); count++)
        {
            Add(PlayerPrefs.GetString(count + "s"), PlayerPrefs.GetInt(count.ToString()));
        }
    }
}
