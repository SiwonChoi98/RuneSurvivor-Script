using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

[System.Serializable]
public class MonsterData //1. json�� ���� Ŭ���� ����
{
    public int id;
    public string name; //���� �̸�
    public string resist; //���� �Ӽ�
    public string type; //Ÿ��
    public int damage;
    public int curHealth; //���� ����ü��
    public int maxHealth; //���� �ִ�ü��     
    public int speed; //���� �̵��ӵ�
    public int expLevel; //���� ����ġ�ܰ�

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
    public Monster[] normalMonsters; //�븻���͵�
    public Monster[] patternMonster; //���ϸ��͵�
    public Monster[] bossMonster; //�������͵�

    public List<MonsterData> monsterDatasList = new List<MonsterData>(); //���� ������ ����Ʈ

    void Awake()
    {
        //���� �ʱ� ������
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

        //������ �߰� �� ���
        //JsonData jsonMonsterData = JsonMapper.ToJson(monsterDatasList); //json���� ��ȯ�ؼ� ���� //json������ string���θ� �����
        //File.WriteAllText(Application.streamingAssetsPath + "/MonsterData.json", jsonMonsterData.ToString());


        //json���Ͽ��� �ٲ��ټ��ְ� ���
        string jsonMonsterDataString = File.ReadAllText(Application.streamingAssetsPath + "/MonsterData.json");
        JsonData MonsterData2 = JsonMapper.ToObject(jsonMonsterDataString); //json������ string�̿��� ���ڿ��� �ٽ� PlayerData ���� �°� ��ȯ��.
                
        for(int i=0; i<monsterDatasList.Count; i++) //���� ����Ʈ ������ ����
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
        
        for(int i=0; i<normalMonsters.Length; i++) //�븻���� ������ ����
        {
            normalMonsters[i].MonsterName = monsterDatasList[i].name;
            normalMonsters[i].MonsterResist = monsterDatasList[i].resist; //���� �Ӽ�
            normalMonsters[i].MonsterType = monsterDatasList[i].type; //������ �븻, ����, �������͸� �и�
            normalMonsters[i].MonsterCurHealth = monsterDatasList[i].curHealth;
            normalMonsters[i].MonsterMaxHealth = monsterDatasList[i].maxHealth;
            normalMonsters[i].MonsterDamage = monsterDatasList[i].damage; //���� ������(���ݷ�)
            normalMonsters[i].MonsterSpeed = monsterDatasList[i].speed;
            normalMonsters[i].MonsterExpLevel = monsterDatasList[i].expLevel;
        }
        for (int i = 0; i < patternMonster.Length; i++) //���ϸ��� ������ ����
        {
            patternMonster[i].MonsterName = monsterDatasList[i+6].name;
            patternMonster[i].MonsterResist = monsterDatasList[i+6].resist; //���� �Ӽ�
            patternMonster[i].MonsterType = monsterDatasList[i+6].type; //������ �븻, ����, �������͸� �и�
            patternMonster[i].MonsterCurHealth = monsterDatasList[i+6].curHealth;
            patternMonster[i].MonsterMaxHealth = monsterDatasList[i+6].maxHealth;
            patternMonster[i].MonsterDamage = monsterDatasList[i+6].damage; //���� ������(���ݷ�)
            patternMonster[i].MonsterSpeed = monsterDatasList[i+6].speed; //���� ������(���ݷ�)
            patternMonster[i].MonsterExpLevel = monsterDatasList[i + 6].expLevel; //���� ����ġ�ܰ�

        }
        for (int i = 0; i < bossMonster.Length; i++) //�������� ������ ����
        {
            bossMonster[i].MonsterName = monsterDatasList[i + 9].name; 
            bossMonster[i].MonsterResist = monsterDatasList[i + 9].resist;  //���� �Ӽ�
            bossMonster[i].MonsterType = monsterDatasList[i + 9].type;  //������ �븻, ����, �������͸� �и�
            bossMonster[i].MonsterCurHealth = monsterDatasList[i + 9].curHealth; 
            bossMonster[i].MonsterMaxHealth = monsterDatasList[i + 9].maxHealth; 
            bossMonster[i].MonsterDamage = monsterDatasList[i + 9].damage; //���� ������(���ݷ�)
            bossMonster[i].MonsterSpeed = monsterDatasList[i + 9].speed; //���� ������(���ݷ�)
            bossMonster[i].MonsterExpLevel = monsterDatasList[i + 9].expLevel; //���� ����ġ�ܰ�
        }


    }


}
