using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnddingScript : MonoBehaviour
{
    public GameObject score;          // 스코어 출력 오브젝트
    public GameObject victory;        // 승패 여부 출력 오브젝트
    public GameObject winTeamLogo;    // 승리팀 로고 출력 오브젝트
    public GameObject backGround;     // 배경 오브젝트 
    int gameResult = 0;

    void Start()
    {
        score.GetComponent<Text>().text = PlayScript.homeScore + " : " + PlayScript.awayScore;
        gameResult = PlayScript.homeScore - PlayScript.awayScore;
        Destroy(GameObject.Find("GameDirector"));
        if (gameResult > 0)
        {
            victory.GetComponent<Text>().text = "승리";
            int logoIndex = GameObject.Find("GameManager").GetComponent<GameManagerScript>().myTeam.teamIndex;
            winTeamLogo.GetComponent<Image>().sprite = Resources.Load<Sprite>("graphic/Team_icon/" + logoIndex);
            backGround.GetComponent<SpriteRenderer>().color = GameObject.Find("GameManager").GetComponent<GameManagerScript>().myTeam.teamColor;
        }
        else if(gameResult == 0)
        {
            victory.GetComponent<Text>().text = "무승부";
            int logoIndex = GameObject.Find("GameManager").GetComponent<GameManagerScript>().myTeam.teamIndex;
            winTeamLogo.GetComponent<Image>().sprite = Resources.Load<Sprite>("graphic/Etc/draw");
            backGround.GetComponent<SpriteRenderer>().color = new Color(43f/255f,105f/255f,145/255f);
        }
        else
        {
            victory.GetComponent<Text>().text = "패배";
            int logoIndex = GameObject.Find("GameManager").GetComponent<GameManagerScript>().comTeam.teamIndex;
            winTeamLogo.GetComponent<Image>().sprite = Resources.Load<Sprite>("graphic/Team_icon/" + logoIndex);
            backGround.GetComponent<SpriteRenderer>().color = GameObject.Find("GameManager").GetComponent<GameManagerScript>().comTeam.teamColor;
        }
    }

    void Update()
    {
        
    }

    // 팀선택 버튼 클릭시 씬이동 
    public void TeamSelectButton()
    {
        SceneManager.LoadScene("MyTeamSelectScene");
    }

    // 메인화면 버튼 클릭시 씬이동 
    public void MainButton()
    {
        SceneManager.LoadScene("GameStart");
    }
}
