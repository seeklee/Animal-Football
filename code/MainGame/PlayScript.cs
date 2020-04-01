using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour
{
    public static int homeScore;         // 홈팀 스코어
    public static int awayScore;         // 어웨이팀 스코어 
    public float playTime;               // 게임 플레이 시간
    const float  ANDTIME = 30.0f;         // 게임 종료 시간

    public static PlayScript instance;   // 오브젝트 유지

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        PlayScript.instance = this;
    }

    void Start()
    {
        // 초기화 
        homeScore = 0;
        awayScore = 0;
        playTime = 0;
    }

    void Update()
    {
        // 시간 경과시 종료 화면으로
        if(playTime >= ANDTIME)
        {
            SceneManager.LoadScene("ResultScene");
        }
    }
}
