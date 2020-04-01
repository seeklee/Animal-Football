using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class GameManagerScript : MonoBehaviour
{
    public Team myTeam;
    public Team comTeam;
    public TeamData td;   //전체 팀데이터
    public PlayerData pd;  //전체 선수 데이터

    void Awake()
    {
        DontDestroyOnLoad(gameObject);   //항상 로드 시킴
    }
    void Start()
    {
        LoadPlayerData();   // 파일 형태의 선수 데이터 로드
        LoadTeamData();     // 파일 형태의 팀 데이터 로드 
    }


    [ContextMenu("Team to json data")]            
    public void SaveTeamData()             //json 형태로 팀정보 저장
    {
        string fileName = "TeamDB.json";
        string jsonData = JsonUtility.ToJson(td);
        FileStream fileStream = new FileStream(fileName, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();
    }

    [ContextMenu("Team from json data")]
    public void LoadTeamData()               //json 형태로 팀정보 로드
    {
        string fileName = "TeamDB.json";
        FileStream fileStream = new FileStream(fileName, FileMode.Open);
        byte[] byteData = new byte[fileStream.Length];
        fileStream.Read(byteData, 0, byteData.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(byteData);
        td = JsonUtility.FromJson<TeamData>(jsonData);
    }

    [ContextMenu("Team load from CSVData")]    //csv 형태로 팀정보 로드
    public void TeamEditData()
    {
        string fileName = "data/team2";
        TextAsset csvData = Resources.Load(fileName) as TextAsset;
        string allData = csvData.text;
        string[] dataList = allData.Split('\n');
        td = new TeamData();
        for (int i = 3; i < dataList.Length; i++)
        {
            string[] teamData = dataList[i].Split(',');
            if (teamData[0] != "")
            {
                string[] colorStr = teamData[3].Split('-');
                Color colorRGB = new Color(float.Parse(colorStr[0])/255f, float.Parse(colorStr[1]) / 255f, float.Parse(colorStr[2]) / 255f);
                Team tmp = new Team(int.Parse(teamData[0]), teamData[1], teamData[2], colorRGB);
                td.teamData.Add(tmp);
            }
        }
        
        string fileName2 = "data/team_join2";
        TextAsset csvData2 = Resources.Load(fileName2) as TextAsset;
        string allData2 = csvData2.text;
        string[] dataList2 = allData2.Split('\n');
        for (int i = 3; i < dataList2.Length; i++)
        {
            string[] relationData = dataList2[i].Split(',');
            if (relationData[0] != "")
            {
                td.teamData[int.Parse(relationData[1])].AddPlayer(pd.playerData[int.Parse(relationData[0])]);
            }
        }
    }


    [ContextMenu("Clear TeamData")]
    public void ClearTeamData()   //팀데이터 초기화
    {
        td = new TeamData();
    }


    [ContextMenu("Player to json data")]
    public void SavePlayerData()      //json 형태로 선수 데이터 저장
    {
        string fileName = "PlayerDB.json";
        string jsonData = JsonUtility.ToJson(pd);
        FileStream fileStream = new FileStream(fileName, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();
    }
    [ContextMenu("Player from json data")]
    public void LoadPlayerData()      //json 형태로 선수 데이터 로드
    {
        string fileName = "PlayerDB.json";
        FileStream fileStream = new FileStream(fileName, FileMode.Open);
        byte[] byteData = new byte[fileStream.Length];
        fileStream.Read(byteData, 0, byteData.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(byteData);
        pd = JsonUtility.FromJson<PlayerData>(jsonData);
    }


    [ContextMenu("Player load from CSVData")]
    public void LoadPlayerCsvData()    //csv 형태로 선수 데이터 로드
    {
        string fileName = "data/player2";
        TextAsset csvData = Resources.Load(fileName) as TextAsset;
        string allData = csvData.text;
        string[] dataList = allData.Split('\n');
        pd = new PlayerData();
        for (int i=3;i<dataList.Length;i++)
        {
            string[] playerData = dataList[i].Split(',');
            if(playerData[0] != "")
            {
                pd.playerData.Add(new Player(int.Parse(playerData[0]), playerData[1], playerData[2], int.Parse(playerData[3]), float.Parse(playerData[4]), float.Parse(playerData[5]), float.Parse(playerData[6]), float.Parse(playerData[7]), playerData[8], int.Parse(playerData[9]), int.Parse(playerData[10])));
            }
        }
    }

    [ContextMenu("Clear PlayerData")]
    public void ClearPlayerData()        // 선수 데이터 초기화
    {
        pd = new PlayerData();
    }
}




[System.Serializable]
public class PlayerData                  // 선수 데이터 
{
    public List<Player> playerData;

    public PlayerData()
    {
        playerData = new List<Player>();
    }
}

[System.Serializable]
public class Player                     // 선수
{
    public bool isUsed;                 // 선수 존재여부 확인
    public int playerIndex;             // 선수 인덱스
    public string name;                 // 선수 이름
    public string position;             // 선수 포지션 
    public int level;                   // 선수 등급 
    public float weight;                // 선수 몸무게
    public float height;                // 선수 키
    public float strength;              // 선수 몸싸움
    public float power;                 // 선수 파워
    public string skill;                // 선수 스킬
    public int skill_level;             // 선수 스킬레벨
    public int cost;                    // 선수 코스트


    //선수 생성
    public Player(int _playerIndex, string _name, string _position, int _level, float _weight,float _height, float _strength, float _power, string _skill, int _skill_level, int _cost)  
    {
        isUsed = true;
        playerIndex = _playerIndex;
        name = _name;
        position = _position;
        level = _level;
        weight = _weight;
        height = _height;
        strength = _strength;
        power = _power;
        skill = _skill;
        skill_level = _skill_level;
        cost = _cost;
    }
    
    //선수 생성(스킬 없음)
    public Player(int _playerIndex, string _name, string _position, int _level, float _weight, float _height, float _strength, float _power)
    {
        isUsed = true;
        playerIndex = _playerIndex;
        name = _name;
        position = _position;
        level = _level;
        weight = _weight;
        height = _height;
        strength = _strength;
        power = _power;
        skill = "none";
        skill_level = 1;
        cost = 1;

    }
    public Player()
    {
        isUsed = false;
    }
}



[System.Serializable]
public class TeamData           //팀 데이터
{
    public List<Team> teamData;
    public TeamData()
    {
        teamData = new List<Team>();
    }
}


[System.Serializable]
public class Team                       // 팀 
{
    public int teamIndex;               // 팀 인덱스
    public string teamName;             // 팀 이름
    public string teamExplanation;      // 팀 설명
    public Color teamColor;             // 팀 색깔
    public List<Player> teamPlayer;     // 팀 소속 선수
    public TeamTactic teamTactics;      // 팀 전술 


    public Team(int _teamIndex, string _teamName, string _teamExplanation, Color _teamColor)     // 팀생성
    {
        teamIndex = _teamIndex;
        teamName = _teamName;
        teamExplanation = _teamExplanation;
        teamColor = _teamColor;
        teamPlayer = new List<Player>();
    }
    public Team(Team _selectTeam)             //팀 복사
    {
        teamIndex = _selectTeam.teamIndex;
        teamName = _selectTeam.teamName;
        teamExplanation = _selectTeam.teamExplanation;
        teamColor = _selectTeam.teamColor;
        teamPlayer = new List<Player>();
        for (int i = 0; i < _selectTeam.teamPlayer.Count; i++)
        {
            teamPlayer.Add(_selectTeam.teamPlayer[i]);
        }
    }
    public Team()
    {
        teamPlayer = new List<Player>();
    }

    public void AddPlayer(Player player)             // 팀 선수 추가
    {
        Player tmp = new Player();
        tmp.isUsed = true;
        tmp.playerIndex = player.playerIndex;
        tmp.name = player.name;
        tmp.position = player.position;
        tmp.level = player.level;
        tmp.weight = player.weight;
        tmp.height = player.height;
        tmp.power = player.power;
        tmp.strength = player.strength;
        tmp.skill = player.skill;
        tmp.skill_level = player.skill_level;
        tmp.cost = player.cost;
        teamPlayer.Add(tmp);
    }

    public void SubPlayer(int index)               // 팀 선수 삭제   
    {
        teamPlayer.RemoveAt(index);
    }
}

public class PlayerTactic                          // 개인 전술 
{
    public Player player;                         // 선수
    public Quaternion direction;                  // 선수 이동방향
    public Vector3 position;                      // 선수 포지션
    public float power;                           // 선수 파워
    public PlayerTactic(Player _player, Quaternion _direction, Vector3 _position, float _power)   //개인 전술 설정
    {
        player = _player;
        direction = _direction;
        position = _position;
        power = _power;
    }
    public PlayerTactic()
    {

    }
}

public class TeamTactic                                   // 팀 전술 
{
    public int teamTacticIndex;                                            // 전술 번호
    public PlayerTactic[] teamTactic = new PlayerTactic[5];                // 개인전술 * (출전 선수수:5) = 팀전술 
    public TeamTactic(int _teamTacticIndex)
    {
        teamTacticIndex = _teamTacticIndex;
    }
}
