using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    // 랭킹 정보를 저장하는 리스트
    public static List<RankData> ranks = new List<RankData>();
    // 랭킹 바를 생성하는 위치
    [SerializeField] private Transform grid;
    // 랭킹 바 프리팹
    [SerializeField] private GameObject rankBar;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        string beforeString = null;
        // grid에 형식에 맞게 랭킹 바를 생성
        for (int count = 0; count < ranks.Count; count++)
        {
            if (ranks[count].name.Equals(beforeString)) continue;
            Transform rankBarTransform = Instantiate(rankBar, grid).transform;
            rankBarTransform.Find("Rank").GetComponent<Text>().text = (count + 1) + "등";
            rankBarTransform.Find("Name").GetComponent<Text>().text = ranks[count].name;
            rankBarTransform.Find("Score").GetComponent<Text>().text = ranks[count].score + "점";
            beforeString = ranks[count].name;
        }
    }

    // 리스트에 정보 추가
    public static void Add(string name, int score)
    {
        ranks.Add(new RankData(name, score));
        // 점수를 기준으로 내림차순 정렬
        ranks = ranks.OrderByDescending(x => x.score).ToList();
    }

    // PlayerPrefs로 저장
    public static void Save()
    {
        // 기존 데이터 삭제
        PlayerPrefs.DeleteAll();
        // 새로운 데이터 추가
        for (int count = 0; count < ranks.Count; count++)
        {
            PlayerPrefs.SetString(count + "s", ranks[count].name);
            PlayerPrefs.SetInt(count.ToString(), ranks[count].score);
        }
        // 저장
        PlayerPrefs.Save();
    }

    // 랭킹 초기화
    public void Remove()
    {
        // 랭킹 정보 초기화
        ranks = new List<RankData>();
        // 저장
        Save();
        // 생성된 랭킹 바 삭제
        int childCount = grid.childCount;
        for (int count = 0; count < childCount; count++)
        {
            Destroy(grid.GetChild(count).gameObject);
        }
    }

    // PlayerPrefs 세이브 데이터 불러오기
    private void Load()
    {
        // PlayerPrefs에 세이브 정보가 있다면 불러오기
        for (int count = 0; PlayerPrefs.HasKey(count.ToString()); count++)
        {
            Add(PlayerPrefs.GetString(count + "s"), PlayerPrefs.GetInt(count.ToString()));
        }
    }
}
