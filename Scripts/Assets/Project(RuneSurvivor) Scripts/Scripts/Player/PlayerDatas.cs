using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
[System.Serializable]
public class PlayerData //1. json�� ���� Ŭ���� ����
{
    public int Level; //�÷��̾� ����
    public int CurHealth; //�÷��̾� ����ü��
    public int MaxHealth; //�÷��̾� �ִ�ü��
    public int CurExp; //�÷��̾� ���� ����ġ
    public int MaxExp; //�÷��̾� �ִ� ����ġ
    public int Speed; //�÷��̾� �̵��ӵ�
    public float FireRate; //�÷��̾� ���ݼӵ�
    public int CriProbability; //�÷��̾� ũ��Ƽ�� Ȯ��
    public float AttackRange; //�÷��̾� ���� �����Ÿ�

    public PlayerData(int level, int curHealth, int maxHealth, int curExp, int maxExp, int speed, float fireRate, int criProbability, float attackRange)
    {
        Level = level;
        CurHealth = curHealth;
        MaxHealth = maxHealth;
        CurExp = curExp;
        MaxExp = maxExp;
        Speed = speed;
        FireRate = fireRate;
        CriProbability = criProbability;
        AttackRange = attackRange;
    }
}
public class PlayerDatas : MonoBehaviour
{
    public Player player;
    void Awake()
    {
        //�ʱ� �÷��̾� ������
        PlayerData playerData = new PlayerData(1, 300, 300, 0, 10, 6, 1f, 10, 30);
        //������ �߰� �� ���
        //string jsonPlayerData = JsonUtility.ToJson(playerData); //json���� ��ȯ�ؼ� ���� //json������ string���θ� �����
        //File.WriteAllText(Application.streamingAssetsPath + "/PlayerData.json", jsonPlayerData);

        //json���Ͽ��� ���� �� ���
        string jsonPlayerDataString = File.ReadAllText(Application.streamingAssetsPath + "/PlayerData.json");
        PlayerData playerData2 = JsonUtility.FromJson<PlayerData>(jsonPlayerDataString); //json������ string�̿��� ���ڿ��� �ٽ� PlayerData ���� �°� ��ȯ��.

        player.PlayerCurHealth = playerData2.CurHealth; //ü��
        player.PlayerMaxHealth = playerData2.MaxHealth; //�ִ�ü��
        player.PlayerCurExp = playerData2.CurExp; //����ġ
        player.PlayerMaxExp = playerData2.MaxExp; //�ִ����ġ
        player.PlayerMoveSpeed = playerData2.Speed; //�̵��ӵ�
        player.PlayerCriProbability = playerData2.CriProbability; //ũ��Ƽ��Ȯ��
        player.PlayerLevel = playerData2.Level; //����
        player.PlayerRange = playerData2.AttackRange; //�����Ÿ�
        //player.PlayerRate = playerData2.FireRate; //���ݼӵ�

    }
}
