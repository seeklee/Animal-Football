using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    float weight;    // 유닛 체중
    float power;     // 유닛 파워
    float speed;     // 유닛 스피드
    public string skill = "none";     // 유닛 스킬
    public int skillLevel = 1;        // 유닛 스킬 레벨
    float powerRatio;                 // 파워 비율 
    public GameObject Image;          // 유닛 오브젝트 이미지
    public GameObject gameManager;    
    RoundScript gameManagerScript;
    int collisionFlag = 0;             // 충돌 여부 확인
    int skillCount = 1;                // 스킬 사용 여부 확인


    void Start()
    {
        gameManager = GameObject.Find("RoundDirector");
        gameManagerScript = gameManager.GetComponent<RoundScript>();
    }


    public void MovePlayer()
    {
        if (skill == "rocketRush")   // rocketRush 스킬이 있을 경우 이동대신 스킬을 사용함
        {
            collisionFlag = 1;
            StartCoroutine(RocketRush());
        }
        else       // 통상적 이동
        {
            // this.GetComponent<Rigidbody2D>().AddForce(this.transform.localRotation * this.transform.up * 0.5f *  powerRatio * power, ForceMode2D.Force);
            this.GetComponent<Rigidbody2D>().AddForce(this.transform.up * 0.5f * powerRatio * power, ForceMode2D.Force);
        }
    }

    // 유닛 초기화 
    public void SetPlayer(Team team, Player player, Vector3 _position, Quaternion _direction ,float _power)
    {
        Image.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("graphic/Sample_icon/" + player.playerIndex);
        this.transform.localPosition = _position;
        this.GetComponent<SpriteRenderer>().color = team.teamColor;
        this.transform.localRotation = _direction;
        this.GetComponent<Rigidbody2D>().mass = player.weight;
        this.transform.localScale = new Vector3(player.height, player.height, player.height);
        this.GetComponent<Rigidbody2D>().drag = player.strength;
        power = player.power; 
        skill = player.skill;
        skillLevel = player.skill_level;
        powerRatio = 1;  

        if (skill == "tolerance")  // tolerance 스킬을 가지고 있을 경우 태그를 변경
        {
            this.tag = "Tolerance";
        }
        else if (this.tag == "HomeTeam" && skill == "spy")   // spy 스킬을 가지고 있을 경우 태그를 변경
        {
            this.tag = "AwayTeam";
        }
        else if (this.tag == "AwayTeam" && skill == "spy")   // spy 스킬을 가지고 있을 경우 태그를 변경
        {
            this.tag = "HomeTeam";
        }
    }

    //Skill_WallAttack 스킬 함수
    IEnumerator Skill_WallAttack(Vector3 hisPosition)
    {
        yield return new WaitForSeconds(0.01f);
        Vector3 curPosition = this.transform.position;
        if (this.GetComponent<Rigidbody2D>().velocity.magnitude < 20.0f)
        {
            this.GetComponent<Rigidbody2D>().AddForce((curPosition - hisPosition).normalized * 1000f, ForceMode2D.Force);
        }
        Debug.Log(this.GetComponent<Rigidbody2D>().velocity.magnitude);
    }

    //MetalWall 스킬 함수
    IEnumerator MetalWall()
    {
        yield return new WaitForSeconds(1.0f / skillLevel);
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
    }

    //RocketRush 스킬 함수
    IEnumerator RocketRush()
    {
        Debug.Log("발동");
        while (collisionFlag == 1)
        {
            Debug.Log("로켓러쉬");
            this.GetComponent<Rigidbody2D>().AddForce(this.transform.localRotation * Vector3.right * powerRatio * power, ForceMode2D.Force);
            yield return new WaitForSeconds(0.2f);
        }
    }

    //Lamp 스킬 함수
    IEnumerator Lamp()
    {
        float ori = this.transform.localScale.x;
        float maxSize = Mathf.Min(ori + 0.2f, 0.8f);
        this.transform.localScale = new Vector3(maxSize, maxSize, 1);
        yield return new WaitForSeconds(3f);
        this.transform.localScale = new Vector3(ori, ori, 1);
    }

    //MagicHand 스킬 함수
    void MagicHand()
    {
        this.GetComponent<Rigidbody2D>().velocity += new Vector2(5.0f * skillLevel, 0.0f);
    }

    //Spacial 스킬 함수(상대 관점)
    IEnumerator SpacialYou(int level)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("필살기너");
        Debug.Log("이전속도" + this.GetComponent<Rigidbody2D>().velocity);
        float speedX = Mathf.Min(this.GetComponent<Rigidbody2D>().velocity.x * 2, 20.0f);
        float speedY = Mathf.Min(this.GetComponent<Rigidbody2D>().velocity.y * 2, 20.0f);
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);
        Debug.Log("이후속도" + this.GetComponent<Rigidbody2D>().velocity);
    }

    //Spacial 스킬 함수(사용자 관점)
    void SpacialMe()
    {
        Debug.Log("필살기나");
        this.GetComponent<Rigidbody2D>().mass = 1.0f;
        this.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
        this.GetComponent<Rigidbody2D>().drag = 0.1f;
    }

    //LockDown 스킬 함수
    void LockDown()
    {
        Debug.Log("락다운");
        if (skillCount > 0)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            skillCount--;
        }
    }

    //Smash 스킬 함수
    IEnumerator Smash(int level)
    {
        Debug.Log("스매쉬");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("이전속도" + this.GetComponent<Rigidbody2D>().velocity);
        float speedX = Mathf.Min(this.GetComponent<Rigidbody2D>().velocity.x * 2, 20.0f);
        float speedY = Mathf.Min(this.GetComponent<Rigidbody2D>().velocity.y * 2, 20.0f);
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);
        Debug.Log("이후속도" + this.GetComponent<Rigidbody2D>().velocity);

    }

    //OutOfControl 스킬 함수
    IEnumerator OutOfControl(Vector3 hisPosition)
    {
        yield return new WaitForSeconds(0.01f);
        Vector3 curPosition = this.transform.position;
        Debug.Log("통제불능");
        if (this.GetComponent<Rigidbody2D>().velocity.magnitude < 20.0f)
        {
            this.GetComponent<Rigidbody2D>().AddForce((curPosition - hisPosition).normalized * 1500f, ForceMode2D.Force);
        }
    }

    //Guardian 스킬 함수
    void Guardian(int level)
    {
        this.GetComponent<Rigidbody2D>().drag += 1;
    }

    //Assist 스킬 함수
    IEnumerator Assist(int level)
    {
        Debug.Log("스매쉬");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("이전속도" + this.GetComponent<Rigidbody2D>().velocity);
        float speedX = Mathf.Min(this.GetComponent<Rigidbody2D>().velocity.x * 2, 20.0f);
        float speedY = Mathf.Min(this.GetComponent<Rigidbody2D>().velocity.y * 2, 20.0f);
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);
        Debug.Log("이후속도" + this.GetComponent<Rigidbody2D>().velocity);

    }

    //FightChicken 스킬 함수
    void FightChicken(int level)
    {
        this.GetComponent<Rigidbody2D>().drag -= 1;
    }

    //Gum 스킬 함수
    IEnumerator Gum()
    {
        float oriDrag = this.GetComponent<Rigidbody2D>().drag;
        if (this.GetComponent<Rigidbody2D>().velocity.magnitude > 10.0f)
        {
            this.GetComponent<Rigidbody2D>().drag += 10;
        }
        yield return new WaitForSeconds(1f);
        this.GetComponent<Rigidbody2D>().drag = oriDrag;
    }

    //Dodge 스킬 함수
    void Dodge(GameObject other)
    {
        if (skillCount > 0)
        {
            Debug.Log("회피발동");
            Vector3 myPosition = this.transform.position;
            Vector3 myVelocity = this.GetComponent<Rigidbody2D>().velocity;
            this.transform.position = other.transform.position;
            this.GetComponent<Rigidbody2D>().velocity = other.GetComponent<Rigidbody2D>().velocity;
            other.transform.position = myPosition;
            other.GetComponent<Rigidbody2D>().velocity = myVelocity;
            skillCount--;
        }
    }

    //PickPocket 스킬 함수
    void PickPocket()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0.0f);
        this.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
    }

    //Perfect 스킬 함수 
    void Perfect()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        this.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
    }

    //SelfStudy 스킬 함수 
    void SelfStudy()
    {
        float ori = this.transform.localScale.x;
        float maxSize = Mathf.Min(ori + 0.02f, 0.8f);
        this.transform.localScale = new Vector3(maxSize, maxSize, 1);
    }

    //Painless 스킬 함수 
    void Painless()
    {
        this.GetComponent<Rigidbody2D>().drag += 0.1f;
        this.GetComponent<Rigidbody2D>().mass += 1.0f;
    }




    // 충돌 관리 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = this.transform.position;

        collisionFlag = 0;

        if (collision.gameObject.name == "Home")  // 홈팀 벽에 충돌할 경우
        {
            gameManagerScript.GetScore(0); // 점수 획득
            Destroy(gameObject);           //유닛 파괴
        }
        else if (collision.gameObject.name == "Away")  // 어웨이팀 벽에 충돌할 경우
        {
            gameManagerScript.GetScore(1);    // 점수 획득
            Destroy(gameObject);              //유닛 파괴
        }
        else if (collision.gameObject.name == "Left")  //사이드 벽에 충돌할 경우 스킬발동
        {
            if (skill == "wallAttack")   
            {
                StartCoroutine(Skill_WallAttack(hitPosition));
            }
            else if (skill == "pickPocket")   
            {
                PickPocket();
            }
            else if (skill == "perfect")      
            {
                Perfect();
            }
            else if (skill == "selfStudy")       
            {
                SelfStudy();
            }
        }
        else if (collision.gameObject.name == "Right")  //사이드 벽에 충돌할 경우  스킬발동
        {
            if (skill == "wallAttack")      
            {
                StartCoroutine(Skill_WallAttack(hitPosition));
            }
            else if (skill == "pickPocket")     
            {
                PickPocket();
            }
            else if (skill == "perfect")          
            {
                Perfect();
            }
            else if (skill == "selfStudy")       
            {
                SelfStudy();
            }
        }
        else if (collision.gameObject.tag == "Tolerance")   //Tolerance 스킬발동
        {
            Debug.Log("강한내성 발동");
        }
        else if (collision.gameObject.tag == this.tag)       // 같은 팀끼리 부딪쳤을 경우 스킬발동 
        {
            if (skill == "metalWall")
            {
                StartCoroutine(MetalWall());
            }
            else if (skill == "secrifice")
            {
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
            else if (skill == "special")
            {
                SpacialMe();
            }
            else if (skill == "lockDown")
            {
                LockDown();
            }
            else if (skill == "outOfControl")
            {
                StartCoroutine(OutOfControl(hitPosition));
            }
            else if (skill == "gum")
            {
                StartCoroutine(Gum());
            }
            else if (skill == "dodge")
            {
                Dodge(collision.gameObject);
            }
            else if (skill == "painless")
            {
                Painless();
            }



            if (collision.gameObject.GetComponent<UnitScript>().skill == "lamp")
            {
                StartCoroutine(Lamp());
            }
            else if (collision.gameObject.GetComponent<UnitScript>().skill == "magicHand")
            {
                MagicHand();
            }
            else if (collision.gameObject.GetComponent<UnitScript>().skill == "guardian")
            {
                Guardian(collision.gameObject.GetComponent<UnitScript>().skillLevel);
            }
            else if (collision.gameObject.GetComponent<UnitScript>().skill == "assist")
            {
                StartCoroutine(Assist(collision.gameObject.GetComponent<UnitScript>().skillLevel));
            }
        }
        else if (collision.gameObject.tag != this.tag)         // 서로 다른 팀끼리 부딪쳤을 경우 스킬발동 
        {

            if (skill == "metalWall")
            {
                StartCoroutine(MetalWall());
            }
            else if (skill == "secrifice")
            {
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
            else if (skill == "lockDown")
            {
                LockDown();
            }
            else if (skill == "outOfControl")
            {
                StartCoroutine(OutOfControl(hitPosition));
                //Skill_WallAttack(skillLevel);
            }
            else if (skill == "gum")
            {
                StartCoroutine(Gum());
            }
            else if (skill == "dodge")
            {
                Dodge(collision.gameObject);
            }
            else if (skill == "painless")
            {
                Painless();
            }

            if (collision.gameObject.GetComponent<UnitScript>().skill == "special")
            {
                StartCoroutine(SpacialYou(collision.gameObject.GetComponent<UnitScript>().skillLevel));
            }
            else if (collision.gameObject.GetComponent<UnitScript>().skill == "smash")
            {
                StartCoroutine(Smash(collision.gameObject.GetComponent<UnitScript>().skillLevel));
            }
            else if (collision.gameObject.GetComponent<UnitScript>().skill == "fightChicken")
            {
                FightChicken(collision.gameObject.GetComponent<UnitScript>().skillLevel);
            }
        }
    }
}
