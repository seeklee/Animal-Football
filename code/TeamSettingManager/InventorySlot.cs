using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Player player;      // 선수 정보
    public GameObject imageObject;    //이미지 출력 오브젝트
    public GameObject effect;         //이펙트 출력 오브젝트
    GameObject positionText;          // 선수 포지션 표시 텍스트 화면
    GameObject nameText;              // 선수 이름 표시 텍스트 화면
    GameObject strangthText;          // 선수 몸싸움 표시 텍스트 화면
    GameObject weightText;            // 선수 체중 표시 텍스트 화면
    GameObject heightText;            // 선수 신장 표시 텍스트 화면
    GameObject powerText;             // 선수 파워 표시 텍스트 화면
    GameObject skillText;             // 선수 스킬 표시 텍스트 화면
    GameObject costText;              // 선수 코스트 표시 텍스트 화면


    private void Start()
    {
        positionText = GameObject.Find("Position");
        costText = GameObject.Find("Cost");
        nameText = GameObject.Find("PlayerName");
        strangthText = GameObject.Find("Strangth");
        weightText = GameObject.Find("Weight");
        heightText = GameObject.Find("Height");
        powerText = GameObject.Find("Power");
        skillText = GameObject.Find("Skill");
    }
    public void AddPlayer(Player _player)   // 인벤토리 선수 추가 함수
    {
        player = _player;
        imageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("graphic/Sample_icon/" + _player.playerIndex);
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == this.gameObject)                          //인벤토리 선수가 클릭 되었을 경우
                {
                    effect.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
                    TeamSettingManager.isInventoryClicked = 1;
                    TeamSettingManager.selectedSlotName = this.name;
                    positionText.GetComponent<Text>().text = this.player.position.ToString();
                    costText.GetComponent<Text>().text = this.player.cost.ToString()+"성";
                    nameText.GetComponent<Text>().text = this.player.name.ToString();
                    strangthText.GetComponent<Text>().text = this.player.strength.ToString();
                    weightText.GetComponent<Text>().text = this.player.weight.ToString();
                    heightText.GetComponent<Text>().text = this.player.height.ToString();
                    powerText.GetComponent<Text>().text = this.player.power.ToString();
                    skillText.GetComponent<Text>().text = this.player.skill.ToString();
                }
                else                                                                   //  그외의 것이 클릭 되었을 경우
                {
                    effect.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, 0.0f);
                }
            }
        }
    }
}
