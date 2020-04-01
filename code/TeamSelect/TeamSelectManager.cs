using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeamSelectManager : MonoBehaviour
{
    int isClicked = 0;    // 이동할 페이지
    int cursor = 0;       // 현재 페이지
    int maxTeam;          // 선택가능한 팀 개수 
    GameObject gameManager;
    GameManagerScript gameManagerScript;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        maxTeam = gameManagerScript.td.teamData.Count -1;
        ChangeColor(gameManagerScript.td.teamData[0].teamColor);
        ChangeName(gameManagerScript.td.teamData[0].teamName);
        ChangeEx(gameManagerScript.td.teamData[0].teamExplanation);
        ChangeImage(Resources.Load<Sprite>("graphic/Team_icon/" + gameManagerScript.td.teamData[0].teamIndex));
    }


    void Update()
    {
        // 좌우 클릭시 페이지 이동
        if(isClicked == 1)                
        {
            isClicked = 0;
            if(cursor != maxTeam)
            {
                cursor++;
                ChangeColor(gameManagerScript.td.teamData[cursor].teamColor);
                ChangeName(gameManagerScript.td.teamData[cursor].teamName);
                ChangeEx(gameManagerScript.td.teamData[cursor].teamExplanation);
                ChangeImage(Resources.Load<Sprite>("graphic/Team_icon/" + gameManagerScript.td.teamData[cursor].teamIndex));
            }
        }
        else if(isClicked == -1)
        {
            isClicked = 0;
            if (cursor != 0)
            {
                cursor--;
                ChangeColor(gameManagerScript.td.teamData[cursor].teamColor);
                ChangeName(gameManagerScript.td.teamData[cursor].teamName);
                ChangeEx(gameManagerScript.td.teamData[cursor].teamExplanation);
                ChangeImage(Resources.Load<Sprite>("graphic/Team_icon/" + gameManagerScript.td.teamData[cursor].teamIndex));
            }
        }
    }

    public void ChangeColor(Color teamColor)   //배경색 변경
    {
        GameObject backGround = GameObject.Find("BackGround");
        backGround.GetComponent<SpriteRenderer>().color = teamColor;
    }

    public void ChangeName(string teamName)     //이름 변셩
    {
        GameObject teamNameText = GameObject.Find("TeamName");
        teamNameText.GetComponent<Text>().text = teamName;
    }
    public void ChangeEx(string teamEx)         //팀 설명 변경 
    {
        GameObject teamExText = GameObject.Find("TeamExplanation");
        teamExText.GetComponent<Text>().text = teamEx;
    }

    public void ChangeImage(Sprite teamImage)   //팀 로고 변경
    {
        GameObject teamNameText = GameObject.Find("TeamLogo");
        teamNameText.GetComponent<SpriteRenderer>().sprite = teamImage;
    }

    //좌우 버튼 클릭
    public void NextButton()                     
    {
        isClicked = 1;
    }

    public void PreviousButton()
    {
        isClicked =  - 1;
    }


    //나의팀 선택 버튼 클릭
    public void SelectMyTeam()
    {
        gameManagerScript.myTeam = new Team(gameManagerScript.td.teamData[cursor]);     // 나의 팀으로 저장후 
        SceneManager.LoadScene("ComTeamSelectScene");                         // 상대방 팀 선택 화면으로 이동
    }

    //상대팀 선택 버튼 클릭
    public void SelectComTeam()
    {
        gameManagerScript.comTeam = new Team(gameManagerScript.td.teamData[cursor]);    //  상대 팀으로 저장후 
        SceneManager.LoadScene("TeamSettingScene");                                     // 전술 선택 화면으로 이동
    }
}