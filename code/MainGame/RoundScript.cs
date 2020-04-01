using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundScript : MonoBehaviour
{
    public GameObject time;                          // 시간 표시 오브젝트 
    public GameObject homeTeam;                      // 홈팀 오브젝트   
    public GameObject awayTeam;                      // 어웨이팀 오브젝트 
    Vector3 centor = new Vector3(-1.5f,0.0f,0.0f);
    public Team myTeam;                       // 홈팀 
    public Team comTeam;                      // 어웨이팀 
    public Transform homeGround;              // 홈팀 필드
    public Transform awayGround;              // 어웨이팀 필드
    float waitTime = 2.0f;                           // 라운드 시작시 딜레이 타임
    GameObject[] myUnit = new GameObject[5];       // 홈팀 플레이어 유닛
    GameObject[] comUnit = new GameObject[5];      // 어웨이팀 플레이어 유닛
    int isHit= 0;
    float roundTime;                        // 라운드 경과시간
    float playTime;                         // 게임 경과시간
    public GameObject homeTeamName;             //홈팀 이름 텍스트 출력 
    public GameObject awayTeamName;             //어웨이팀 이름 텍스트 출력
    public GameObject homeTeamLogo;             //홈팀 로고 표시
    public GameObject awayTeamLogo;             //어웨이팀 로고 표시
    public GameObject homeTeamScore;            //홈팀 스코어 출력
    public GameObject awayTeamScore;            //어웨이팀 스코어 출력


    void Start()
    {
        //초기화 
        playTime = GameObject.Find("GameDirector").GetComponent<PlayScript>().playTime;
        roundTime = 0;
        isHit = 1;
        waitTime = 2.0f;
        myTeam = GameObject.Find("GameManager").GetComponent<GameManagerScript>().myTeam;
        comTeam = GameObject.Find("GameManager").GetComponent<GameManagerScript>().comTeam;
        for (int i=0;i<5;i++)
        {
            var unit = Instantiate(homeTeam);
            unit.transform.parent = homeGround;
            unit.GetComponent<UnitScript>().SetPlayer(myTeam,myTeam.teamTactics.teamTactic[i].player, myTeam.teamTactics.teamTactic[i].position, myTeam.teamTactics.teamTactic[i].direction, myTeam.teamTactics.teamTactic[i].power);
            myUnit[i] = unit;
        }
        for (int i = 0; i < 5; i++)
        {
            var unit = Instantiate(awayTeam);
            unit.transform.parent = awayGround;
            unit.GetComponent<UnitScript>().SetPlayer(comTeam,comTeam.teamTactics.teamTactic[i].player, comTeam.teamTactics.teamTactic[i].position, comTeam.teamTactics.teamTactic[i].direction, comTeam.teamTactics.teamTactic[i].power);
            comUnit[i] = unit;
        }


        // UI 초기화
        homeTeamName.GetComponent<Text>().text = myTeam.teamName;
        awayTeamName.GetComponent<Text>().text = comTeam.teamName;
        homeTeamLogo.GetComponent<Image>().sprite = Resources.Load<Sprite>("graphic/Team_icon/" + myTeam.teamIndex);
        awayTeamLogo.GetComponent<Image>().sprite = Resources.Load<Sprite>("graphic/Team_icon/" + comTeam.teamIndex);
        homeTeamScore.GetComponent<Text>().text = PlayScript.homeScore.ToString();
        awayTeamScore.GetComponent<Text>().text = PlayScript.awayScore.ToString();
        GameObject.Find("Background").GetComponent<SpriteRenderer>().color = myTeam.teamColor;
    }


    void Update()
    {
        waitTime -= Time.deltaTime;
        roundTime += Time.deltaTime;

        //일정시간 경과시 유닛이동 시작 
        if (waitTime < 0.0f && isHit ==1)
        {
            isHit = 0;
            for (int i = 0; i < 5; i++)
            {
                myUnit[i].GetComponent<UnitScript>().MovePlayer();
                comUnit[i].GetComponent<UnitScript>().MovePlayer();
            }
        }

        // 경과시간 텍스트 업데이트 및 출력
        time.GetComponent<Text>().text = (roundTime + playTime).ToString("00");
        if (roundTime > 15.0f)
        {
            GameObject.Find("GameDirector").GetComponent<PlayScript>().playTime += 15.0f;
            SceneManager.LoadScene("TeamSettingScene");
        }
    }

    // 경기 스코어 출력하는 함수
    public void GetScore(int type)
    {
        if(type == 0)
        {
            PlayScript.homeScore++;
            homeTeamScore.GetComponent<Text>().text = PlayScript.homeScore.ToString();
        }
        else
        {
            PlayScript.awayScore++;
            awayTeamScore.GetComponent<Text>().text = PlayScript.awayScore.ToString();
        }
    }
}
