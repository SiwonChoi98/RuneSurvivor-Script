using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
[System.Serializable]
public class PlayerData //1. json에 넣을 클래스 생성
{
    public int Level; //플레이어 레벨
    public int CurHealth; //플레이어 현재체력
    public int MaxHealth; //플레이어 최대체력
    public int CurExp; //플레이어 현재 경험치
    public int MaxExp; //플레이어 최대 경험치
    public int Speed; //플레이어 이동속도
    public float FireRate; //플레이어 공격속도
    public int CriProbability; //플레이어 크리티컬 확률
    public float AttackRange; //플레이어 공격 사정거리

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
        //초기 플레이어 데이터
        PlayerData playerData = new PlayerData(1, 300, 300, 0, 10, 6, 1f, 10, 30);
        //데이터 추가 시 사용
        //string jsonPlayerData = JsonUtility.ToJson(playerData); //json으로 변환해서 저장 //json파일은 string으로만 저장됨
        //File.WriteAllText(Application.streamingAssetsPath + "/PlayerData.json", jsonPlayerData);

        //json파일에서 수정 시 사용
        string jsonPlayerDataString = File.ReadAllText(Application.streamingAssetsPath + "/PlayerData.json");
        PlayerData playerData2 = JsonUtility.FromJson<PlayerData>(jsonPlayerDataString); //json형태의 string이였던 문자열이 다시 PlayerData 형에 맞게 변환됨.

        player.PlayerCurHealth = playerData2.CurHealth; //체력
        player.PlayerMaxHealth = playerData2.MaxHealth; //최대체력
        player.PlayerCurExp = playerData2.CurExp; //경험치
        player.PlayerMaxExp = playerData2.MaxExp; //최대경험치
        player.PlayerMoveSpeed = playerData2.Speed; //이동속도
        player.PlayerCriProbability = playerData2.CriProbability; //크리티컬확률
        player.PlayerLevel = playerData2.Level; //레벨
        player.PlayerRange = playerData2.AttackRange; //사정거리
        //player.PlayerRate = playerData2.FireRate; //공격속도

    }
}
