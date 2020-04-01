using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TeamSettingManager : MonoBehaviour
{
    const int MAXPLAYERS = 5;                       //최대 필드 플레이어수
    public List<InventorySlot> slots;               //인벤토리 슬롯 리스트
    public Transform tf;                            //인벤토리 부모 오브젝트 위치
    public InventorySlot slot;                      //인벤토리 슬롯
    GameObject gameManager;                                     
    GameManagerScript gameManagerScript;
    public Team setTeam;                             // 관리할 팀
    formation[] guideFormation = new formation[MAXPLAYERS];          // 포메이션 정보
    public int formationIndex = 0;                                   // 포메이션 인덱스
    public GameObject formationName;                                 // 포메이션 이름
    public GameObject[] players = new GameObject[MAXPLAYERS];        // 필드플레이어 리스트

    public static int isInventoryClicked;                         //인벤토리가 선택되었는지
    public static string selectedSlotName;                        //선택된 인벤토리 슬롯 이름



    void Start()
    {
        isInventoryClicked = 0;

        // 초기 포메이션 설정 
        guideFormation[0] = new formation("4-1", -1.1f, -0.1f, -0.4f, -0.1f, 0.4f, -0.1f, 1.1f, -0.1f, 0f, -0.8f);
        guideFormation[1] = new formation("3-2", -0.7f, -0.1f, 0.0f, -0.1f, 0.7f, -0.1f, -0.4f, -0.8f, 0.4f, -0.8f);
        guideFormation[2] = new formation("2-1-2", -0.8f, -0.1f, 0.8f, -0.1f, 0f, -0.5f, -0.8f, -0.8f, 0.8f, -0.8f);
        guideFormation[3] = new formation("2-3", -0.4f, -0.1f, 0.4f, -0.1f, -0.7f, -0.8f, 0f, -0.8f, 0.7f, -0.8f);
        guideFormation[4] = new formation("1-4", 0.0f, -0.1f, -1.1f, -0.8f, -0.4f, -0.8f, 0.4f, -0.8f, 1.1f, -0.8f);

        // 설정할 팀 로드
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        if (SceneManager.GetActiveScene().name == "TeamSettingScene")
        {
            setTeam = gameManagerScript.myTeam;
        }
        else
        {
            setTeam = gameManagerScript.comTeam;
        }
        for (int i = 0; i < MAXPLAYERS; i++)
        {
            players[i].GetComponent<SpriteRenderer>().color = setTeam.teamColor;
            players[i].GetComponent<PlayerSetting>().myColor = setTeam.teamColor;
        }
        formationName.GetComponent<Text>().text = guideFormation[formationIndex].name;
        for (int i = 0; i < MAXPLAYERS; i++)
        {
            players[i].transform.localPosition = new Vector3(guideFormation[formationIndex].positions[i].x, guideFormation[formationIndex].positions[i].y);
        }
        GameObject.Find("Background").GetComponent<SpriteRenderer>().color = setTeam.teamColor;

        ResetButtonClick();
    }


    // 포메이션 변경함수 
    public void FormationButtonClick()
    {
        formationIndex++;
        if (formationIndex == MAXPLAYERS)
        {
            formationIndex = 0;
        }
        formationName.GetComponent<Text>().text = guideFormation[formationIndex].name;
        for (int i = 0; i < MAXPLAYERS; i++)
        {
            players[i].transform.localPosition = new Vector3(guideFormation[formationIndex].positions[i].x, guideFormation[formationIndex].positions[i].y);
        }
    }

    //  작전 초기화 함수 
    public void ResetButtonClick()
    {
        for (int i = 0; i < MAXPLAYERS; i++)
        {
            players[i].GetComponent<PlayerSetting>().Reset();
        }

        Transform[] soltList = tf.GetComponentsInChildren<Transform>(true);
        if (soltList != null)
        {
            for (int i = 0; i < soltList.Length; i++)
            {
                if (soltList[i] != tf)
                {
                    Destroy(soltList[i].gameObject);
                }
            }
        }

        for (int i = 0; i < setTeam.teamPlayer.Count; i++)
        {
            InventorySlot newSlot = Instantiate(slot);
            newSlot.name = "Slot" + i;
            newSlot.transform.parent = tf;
            slots.Add(newSlot.GetComponent<InventorySlot>());
            newSlot.AddPlayer(setTeam.teamPlayer[i]);
            newSlot.transform.localScale = new Vector3(1, 1, 0);
        }
    }

    // 플레이 버튼 클릭 
    public void PlayButtonClick()
    {
        TeamTactic myTeamTactic = new TeamTactic(0);
        for (int i = 0; i < MAXPLAYERS; i++)
        {
            PlayerTactic tactic = new PlayerTactic(players[i].GetComponent<PlayerSetting>().player, players[i].GetComponent<PlayerSetting>().transform.localRotation, players[i].transform.localPosition, players[i].GetComponent<PlayerSetting>().direction.transform.localScale.x);
            myTeamTactic.teamTactic[i] = tactic;
        }
        setTeam.teamTactics = myTeamTactic;
        if (SceneManager.GetActiveScene().name == "TeamSettingScene")
        {
            SceneManager.LoadScene("ComTeamSettingScene");
        }
        else
        {

            SceneManager.LoadScene("MainGameScene");
        }

    }
}

// 포메이션 구조체 

public struct formation {
    public string name;           // 포메이션 이름
    public position[] positions;  // 포지션 위치
    public formation(string _name, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, float x5, float y5)
    {
        positions = new position[5];
        name = _name;
        positions[0].x = x1;
        positions[0].y = y1;
        positions[1].x = x2;
        positions[1].y = y2;
        positions[2].x = x3;
        positions[2].y = y3;
        positions[3].x = x4;
        positions[3].y = y4;
        positions[4].x = x5;
        positions[4].y = y5;
    }
}

//포지션 구조체 
public struct position
{
    public float x;    // x좌표 위치
    public float y;    // y좌표 위치 
    public position(int _x,int _y)
    {
        x = _x;
        y = _y;
    }
}


