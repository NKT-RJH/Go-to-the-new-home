using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    // ��ŷ ������ �����ϴ� ����Ʈ
    public static List<RankData> ranks = new List<RankData>();
    // ��ŷ �ٸ� �����ϴ� ��ġ
    [SerializeField] private Transform grid;
    // ��ŷ �� ������
    [SerializeField] private GameObject rankBar;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        string beforeString = null;
        // grid�� ���Ŀ� �°� ��ŷ �ٸ� ����
        for (int count = 0; count < ranks.Count; count++)
        {
            if (ranks[count].name.Equals(beforeString)) continue;
            Transform rankBarTransform = Instantiate(rankBar, grid).transform;
            rankBarTransform.Find("Rank").GetComponent<Text>().text = (count + 1) + "��";
            rankBarTransform.Find("Name").GetComponent<Text>().text = ranks[count].name;
            rankBarTransform.Find("Score").GetComponent<Text>().text = ranks[count].score + "��";
            beforeString = ranks[count].name;
        }
    }

    // ����Ʈ�� ���� �߰�
    public static void Add(string name, int score)
    {
        ranks.Add(new RankData(name, score));
        // ������ �������� �������� ����
        ranks = ranks.OrderByDescending(x => x.score).ToList();
    }

    // PlayerPrefs�� ����
    public static void Save()
    {
        // ���� ������ ����
        PlayerPrefs.DeleteAll();
        // ���ο� ������ �߰�
        for (int count = 0; count < ranks.Count; count++)
        {
            PlayerPrefs.SetString(count + "s", ranks[count].name);
            PlayerPrefs.SetInt(count.ToString(), ranks[count].score);
        }
        // ����
        PlayerPrefs.Save();
    }

    // ��ŷ �ʱ�ȭ
    public void Remove()
    {
        // ��ŷ ���� �ʱ�ȭ
        ranks = new List<RankData>();
        // ����
        Save();
        // ������ ��ŷ �� ����
        int childCount = grid.childCount;
        for (int count = 0; count < childCount; count++)
        {
            Destroy(grid.GetChild(count).gameObject);
        }
    }

    // PlayerPrefs ���̺� ������ �ҷ�����
    private void Load()
    {
        // PlayerPrefs�� ���̺� ������ �ִٸ� �ҷ�����
        for (int count = 0; PlayerPrefs.HasKey(count.ToString()); count++)
        {
            Add(PlayerPrefs.GetString(count + "s"), PlayerPrefs.GetInt(count.ToString()));
        }
    }
}
