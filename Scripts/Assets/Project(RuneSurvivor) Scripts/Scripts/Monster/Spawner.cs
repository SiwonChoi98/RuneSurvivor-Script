using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;
    private float _timer; //몬스터 소환시간 
    private float _levelUpTime = 60f;
    [SerializeField] private int _level = 1;
    [SerializeField] private float _spawnTime = 0.5f;
    [SerializeField] private float _levelTime;
    [SerializeField] private bool isPattern = true; //몬스터패턴 가능한 지
    [SerializeField] private GameObject bossWarningText; //보스 소환 텍스트
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        if (!GameManager.instance.isGameStart)
            return;
        _timer += Time.deltaTime;
        LevelUp();       
        if (GameManager.instance.player.IsLive) //캐릭터가 살아있을때만 소환
        {
            if (_timer > _spawnTime)
            {
                _timer = 0;
                NormalMonsterSpawn(); 
            }
            if (isPattern) { StartCoroutine(LevelPattern(_level)); }
            
        }
       
    }

    //레벨에 따른 일반 몬스터 소환 함수
    private void NormalMonsterSpawn()
    {
        int ran = Random.Range(0, 5); 
        int ran2 = Random.Range(0, 5);
        Vector3 vec = new Vector3(ran, 0, ran2); //조금씩 위치를 다르게 생성

        GameObject monster = GameManager.instance.monsterPool.Get(Random.Range(0, _level));
        monster.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].transform.position + vec; //몬스터 생성될때 조금씩 위치 다르게 생성
    }
    //시간에 따른 레벨업 함수
    private void LevelUp()
    {
        _levelTime += Time.deltaTime;
        if (_levelTime > _levelUpTime) //레벨업 타임 지나면 레벨업
        {
            _spawnTime -= 0.02f;
            _levelTime = 0;
            _level++;
            if(_level >= 2) { isPattern = true; }//레벨업할때마다 패턴 true로 바꿔준다. //단 3레벨 이상부터
        }
        if (_level >= GameManager.instance.monsterPool.prefabs.Length)
        {
            _level = GameManager.instance.monsterPool.prefabs.Length;
        }
    }

    //레벨별 패턴 몬스터 소환 함수
    private IEnumerator LevelPattern(int level) 
    {
        switch (level)
        {
            case 2:
                MonsterMiddleBoss().MonsterCurHealth = 150; //1분~2분사이 미들보스
                break;
            case 3:
                for (int i = 0; i < 2; i++)
                {
                    MonsterPattern1(); //2~3분사이 박쥐패턴
                    yield return new WaitForSeconds(3f);
                }
                break;
            case 4:
                MonsterPattern2();
                MonsterMiddleBoss().MonsterCurHealth = 300; //3~4분 사이 미들보스 
                break;
            case 6:
                MonsterBoss().MonsterCurHealth = 500; //5~6분사이 보스소환
                break;
            case 8:
                MonsterMiddleBoss().MonsterCurHealth = 400; //7~8분 사이 미들보스 
                for (int i = 0; i < 4; i++)
                {
                    MonsterPattern1(); 
                    yield return new WaitForSeconds(3f);
                }
                break;
            case 9:
                MonsterPattern2();
                for (int i = 0; i < 5; i++)
                {
                    MonsterPattern1();
                    yield return new WaitForSeconds(3f);
                }
                break;
            case 10:
                MonsterPattern3();
                MonsterMiddleBoss().MonsterCurHealth = 800; //9~10분사이 미들보스
                break;
            case 11:
                MonsterBoss().MonsterCurHealth = 2500; //10~11분사이 보스소환
                break;
            case 12:
                for (int i = 0; i < 4; i++)
                {
                    MonsterPattern3();
                    yield return new WaitForSeconds(3f);
                }
                break;
            case 13:
                MonsterPattern2();
                MonsterMiddleBoss().MonsterCurHealth = 1500; //10~11분사이 미들보스
                break;
            case 14:
                MonsterPattern2();
                for (int i = 0; i < 7; i++)
                {
                    MonsterPattern1();
                    yield return new WaitForSeconds(2f);
                }
                break;
            default:
                break;
        }
    }
    //박쥐패턴 소환정의
    public void MonsterPattern1()
    {
        int ran = Random.Range(1, GameManager.instance.spawner.spawnPoint.Length);
        for (int i = 0; i < 10; i++)
        {
            int x = Random.Range(-2, 2);
            int z = Random.Range(-2, 2);
            Vector3 ranVec = new Vector3(x, 0, z); //몬스터 생성 존에서 다르게 나오게 하려고 위치 지정

            GameObject monster = GameManager.instance.monsterPool.GetPattern(0);
            monster.transform.position = GameManager.instance.spawner.spawnPoint[ran].transform.position + ranVec; //몬스터 생성될때 조금씩 위치 다르게 생성
        }
        isPattern = false;
    }
    //랜덤 위치 박쥐패턴 소환정의
    public void MonsterPattern2()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject monster = GameManager.instance.monsterPool.GetPattern(1);
            monster.transform.position = GameManager.instance.spawner.spawnPoint[Random.Range(1, GameManager.instance.spawner.spawnPoint.Length)].transform.position;
        }
        isPattern = false;
    }
    //자폭몬스터패턴 소환정의
    public void MonsterPattern3()
    {
        GameObject monster = GameManager.instance.monsterPool.GetPattern(2);
        monster.transform.position = GameManager.instance.spawner.spawnPoint[Random.Range(1, GameManager.instance.spawner.spawnPoint.Length)].transform.position;

        isPattern = false;
    } 
    //중간보스 소환정의
    public Monster MonsterMiddleBoss()
    {
        Monster instantMonster = Instantiate(GameManager.instance.monsterDatas.bossMonster[0]);
        instantMonster.transform.position = GameManager.instance.spawner.spawnPoint[Random.Range(1, GameManager.instance.spawner.spawnPoint.Length)].transform.position;
        //---타겟지정---                                     
        Monster monster = instantMonster.GetComponent<Monster>();
        monster.target = GameManager.instance.player.transform;
        monster.manager = GameManager.instance;

        isPattern = false;
        return instantMonster;

    }
    private IEnumerator MonsterBossWarningText()
    {
        bossWarningText.SetActive(true);
        yield return new WaitForSeconds(2f);
        bossWarningText.SetActive(false);
    }
    //보스 소환정의
    public Monster MonsterBoss()
    {
        StartCoroutine("MonsterBossWarningText");
        Monster instantMonster = Instantiate(GameManager.instance.monsterDatas.bossMonster[1], GameManager.instance.spawner.spawnPoint[1].position, GameManager.instance.spawner.spawnPoint[1].rotation);
        Monster monster = instantMonster.GetComponent<Monster>();
        monster.target = GameManager.instance.player.transform;
        monster.manager = GameManager.instance;
        isPattern = false;
        return instantMonster;
    }
}