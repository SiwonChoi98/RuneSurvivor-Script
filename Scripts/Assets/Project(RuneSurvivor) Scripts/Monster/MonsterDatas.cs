using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

[System.Serializable]
public class MonsterData //1. json에 넣을 클래스 생성
{
    public int id;
    public string name; //몬스터 이름
    public string resist; //몬스터 속성
    public string type; //타입
    public int damage;
    public int curHealth; //몬스터 현재체력
    public int maxHealth; //몬스터 최대체력     
    public int speed; //몬스터 이동속도
    public int expLevel; //몬스터 경험치단계

    public MonsterData(int monsterid, string monstername, string monsterresist, string monstertype, int monsterdamage, int monstercurHealth, int monstermaxHealth, int monsterspeed, int monsterexpLevel)
    {
        id = monsterid;
        name = monstername;
        resist = monsterresist;
        type = monstertype;
        damage = monsterdamage;
        curHealth = monstercurHealth;
        maxHealth = monstermaxHealth;
        speed = monsterspeed;
        expLevel = monsterexpLevel;
    }
}
public class MonsterDatas : MonoBehaviour
{
    public Monster[] normalMonsters; //노말몬스터들
    public Monster[] patternMonster; //패턴몬스터들
    public Monster[] bossMonster; //보스몬스터들

    public List<MonsterData> monsterDatasList = new List<MonsterData>(); //몬스터 데이터 리스트

    void Awake()
    {
        //몬스터 초기 데이터
        monsterDatasList.Add(new MonsterData(0, "skeleton", "Rock", "NormalMonster", 10, 10, 10, 4, 0));
        monsterDatasList.Add(new MonsterData(1, "dragon", "Fire", "NormalMonster", 10, 30, 30, 5,0));    
        monsterDatasList.Add(new MonsterData(2, "evilmage", "Water", "NormalMonster", 10, 15, 15, 4,0)); 
        monsterDatasList.Add(new MonsterData(3, "slime", "Water", "NormalMonster", 10, 10, 10, 5,0));
        monsterDatasList.Add(new MonsterData(4, "spider", "Rock", "NormalMonster", 10, 30, 30, 4,0));
        monsterDatasList.Add(new MonsterData(5, "turtleshell", "Rock", "NormalMonster", 10, 50, 50, 2,1));
        monsterDatasList.Add(new MonsterData(6, "bat", "Rock", "Pattern1Monster", 10, 10, 10, 10,0));
        monsterDatasList.Add(new MonsterData(7, "batC", "Lightning", "Pattern2Monster", 10, 10, 10, 10,0));
        monsterDatasList.Add(new MonsterData(8, "dragonD", "Fire", "Pattern3Monster", 50, 30, 30, 10,1));
        monsterDatasList.Add(new MonsterData(9, "orc", "Fire", "MiddleBoss", 10, 300, 300, 4,2));
        monsterDatasList.Add(new MonsterData(10, "golem", "Lightning", "Boss", 20, 1000, 1000, 4,2));

        //데이터 추가 시 사용
        //JsonData jsonMonsterData = JsonMapper.ToJson(monsterDatasList); //json으로 변환해서 저장 //json파일은 string으로만 저장됨
        //File.WriteAllText(Application.streamingAssetsPath + "/MonsterData.json", jsonMonsterData.ToString());


        //json파일에서 바꿔줄수있게 사용
        string jsonMonsterDataString = File.ReadAllText(Application.streamingAssetsPath + "/MonsterData.json");
        JsonData MonsterData2 = JsonMapper.ToObject(jsonMonsterDataString); //json형태의 string이였던 문자열이 다시 PlayerData 형에 맞게 변환됨.
                
        for(int i=0; i<monsterDatasList.Count; i++) //몬스터 리스트 데이터 저장
        {
            monsterDatasList[i].id = (int)MonsterData2[i]["id"];
            monsterDatasList[i].name = MonsterData2[i]["name"].ToString();
            monsterDatasList[i].resist = MonsterData2[i]["resist"].ToString();
            monsterDatasList[i].type = MonsterData2[i]["type"].ToString();
            monsterDatasList[i].curHealth = (int)MonsterData2[i]["curHealth"];
            monsterDatasList[i].maxHealth = (int)MonsterData2[i]["maxHealth"];
            monsterDatasList[i].damage = (int)MonsterData2[i]["damage"];
            monsterDatasList[i].speed = (int)MonsterData2[i]["speed"];
            monsterDatasList[i].expLevel = (int)MonsterData2[i]["expLevel"];

        }
        
        for(int i=0; i<normalMonsters.Length; i++) //노말몬스터 데이터 저장
        {
            normalMonsters[i].MonsterName = monsterDatasList[i].name;
            normalMonsters[i].MonsterResist = monsterDatasList[i].resist; //몬스터 속성
            normalMonsters[i].MonsterType = monsterDatasList[i].type; //몬스터의 노말, 패턴, 보스몬스터를 분리
            normalMonsters[i].MonsterCurHealth = monsterDatasList[i].curHealth;
            normalMonsters[i].MonsterMaxHealth = monsterDatasList[i].maxHealth;
            normalMonsters[i].MonsterDamage = monsterDatasList[i].damage; //몬스터 데미지(공격력)
            normalMonsters[i].MonsterSpeed = monsterDatasList[i].speed;
            normalMonsters[i].MonsterExpLevel = monsterDatasList[i].expLevel;
        }
        for (int i = 0; i < patternMonster.Length; i++) //패턴몬스터 데이터 저장
        {
            patternMonster[i].MonsterName = monsterDatasList[i+6].name;
            patternMonster[i].MonsterResist = monsterDatasList[i+6].resist; //몬스터 속성
            patternMonster[i].MonsterType = monsterDatasList[i+6].type; //몬스터의 노말, 패턴, 보스몬스터를 분리
            patternMonster[i].MonsterCurHealth = monsterDatasList[i+6].curHealth;
            patternMonster[i].MonsterMaxHealth = monsterDatasList[i+6].maxHealth;
            patternMonster[i].MonsterDamage = monsterDatasList[i+6].damage; //몬스터 데미지(공격력)
            patternMonster[i].MonsterSpeed = monsterDatasList[i+6].speed; //몬스터 데미지(공격력)
            patternMonster[i].MonsterExpLevel = monsterDatasList[i + 6].expLevel; //몬스터 경험치단계

        }
        for (int i = 0; i < bossMonster.Length; i++) //보스몬스터 데이터 저장
        {
            bossMonster[i].MonsterName = monsterDatasList[i + 9].name; 
            bossMonster[i].MonsterResist = monsterDatasList[i + 9].resist;  //몬스터 속성
            bossMonster[i].MonsterType = monsterDatasList[i + 9].type;  //몬스터의 노말, 패턴, 보스몬스터를 분리
            bossMonster[i].MonsterCurHealth = monsterDatasList[i + 9].curHealth; 
            bossMonster[i].MonsterMaxHealth = monsterDatasList[i + 9].maxHealth; 
            bossMonster[i].MonsterDamage = monsterDatasList[i + 9].damage; //몬스터 데미지(공격력)
            bossMonster[i].MonsterSpeed = monsterDatasList[i + 9].speed; //몬스터 데미지(공격력)
            bossMonster[i].MonsterExpLevel = monsterDatasList[i + 9].expLevel; //몬스터 경험치단계
        }


    }


}
