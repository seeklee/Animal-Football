using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public void NewGame()   //둘이서 놀기 버튼 클릭시
    {
        SceneManager.LoadScene("MyTeamSelectScene");
    }

    public void EndGame()   // 게임 종료 버튼 클릭시
    {
        Application.Quit();
    }
}
