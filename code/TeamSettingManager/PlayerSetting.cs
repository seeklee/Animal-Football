using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetting : MonoBehaviour
{
    public Player player;              // 저장될 선수 정보
    public GameObject direction;       // 저장될 선수 이동방향 정보
    public GameObject Image;           // 저장될 선수 이미지 정보
    public int isClicked;              // 클릭 상태 확인
    public Color myColor;              // 팀 색깔 정보
    GameObject nameText;               // 선수 이름 표시 텍스트 화면
    GameObject strangthText;           // 선수 몸싸움 표시 텍스트 화면 
    GameObject weightText;             // 선수 체중 표시 텍스트 화면
    GameObject heightText;             // 선수 신장 표시 텍스트 화면
    GameObject powerText;              // 선수 파워 표시 텍스트 화면
    GameObject skillText;              // 선수 스킬 표시 텍스트 화면
    GameObject costText;               // 선수 코스트 표시 텍스트 화면
    GameObject positionText;           // 선수 포지션 표시 텍스트 화면


    void Start()
    {
        isClicked = 0;
        positionText = GameObject.Find("Position");
        costText = GameObject.Find("Cost");
        nameText = GameObject.Find("PlayerName");
        strangthText = GameObject.Find("Strangth");
        weightText = GameObject.Find("Weight");
        heightText = GameObject.Find("Height");
        powerText = GameObject.Find("Power");
        skillText = GameObject.Find("Skill");
    }

    private void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (isClicked == 0)                        // 필드 플레이어가 클릭이 안된 상태
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == this.gameObject)   // 필드 플레이어가 클릭 됐을 경우
                    {
                        this.GetComponent<SpriteRenderer>().color = new Color(myColor.r, myColor.g, myColor.b, 0.5f);   
                        isClicked = 1;                                       
                    }
                    else
                    {
                        isClicked = 0;
                    }

                }
                else
                {
                    isClicked = 0;
                }
            }
        }
        else                                // 필드 플레이어가 클릭이 된 상태
        {
            if (TeamSettingManager.isInventoryClicked == 1)         //인벤토리가 선택된 상태 
            {
                player = GameObject.Find(TeamSettingManager.selectedSlotName).GetComponent<InventorySlot>().player;
                Image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("graphic/Sample_icon/" + player.playerIndex);
                this.GetComponent<SpriteRenderer>().color = myColor;
                this.transform.localScale = new Vector3(player.height, player.height, player.height);
                Destroy(GameObject.Find(TeamSettingManager.selectedSlotName));
                isClicked = 0;
                TeamSettingManager.isInventoryClicked = 0;
                costText.GetComponent<Text>().text = null;
                nameText.GetComponent<Text>().text = null;
                strangthText.GetComponent<Text>().text = null;
                weightText.GetComponent<Text>().text = null;
                heightText.GetComponent<Text>().text = null;
                powerText.GetComponent<Text>().text = null;
                skillText.GetComponent<Text>().text = null;
                positionText.GetComponent<Text>().text = null;
            }
            else                        //인벤토리가 선택되지 않은 상태
            {
                
                Vector2 mouse_pos = new Vector2(pos.x - direction.transform.position.x, pos.y - direction.transform.position.y);
                float rad = Mathf.Atan2(mouse_pos.x, mouse_pos.y);
                float mouse_rotate = (rad * 180) / Mathf.PI;

                float vectorSize = Mathf.Min(1.0f, mouse_pos.magnitude);
                direction.transform.localScale = new Vector3(vectorSize, vectorSize);
                this.transform.localEulerAngles = new Vector3(0, 0, (-mouse_rotate));
                if (Input.GetMouseButtonDown(0))
                {
                    this.GetComponent<SpriteRenderer>().color = myColor;
                    isClicked = 0;
                }
                if(player.isUsed == true)
                {
                    costText.GetComponent<Text>().text = player.cost.ToString();
                    nameText.GetComponent<Text>().text = player.name.ToString();
                    strangthText.GetComponent<Text>().text = player.strength.ToString();
                    weightText.GetComponent<Text>().text = player.weight.ToString();
                    heightText.GetComponent<Text>().text = player.height.ToString();
                    powerText.GetComponent<Text>().text = player.power.ToString();
                    skillText.GetComponent<Text>().text = player.skill.ToString();
                    positionText.GetComponent<Text>().text = player.position.ToString();
                }
            }
        }
    }
    public void Reset()      // 필드 플레이어 설정들을 모두 초기화 
    {
        direction.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        direction.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        player = new Player();
        Image.GetComponent<SpriteRenderer>().sprite = null;
    }
}
