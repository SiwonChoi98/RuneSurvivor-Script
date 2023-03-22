using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;
    private float _timer; //���� ��ȯ�ð� 
    private float _levelUpTime = 60f;
    [SerializeField] private int _level = 1;
    [SerializeField] private float _spawnTime = 0.5f;
    [SerializeField] private float _levelTime;
    [SerializeField] private bool isPattern = true; //�������� ������ ��
    [SerializeField] private GameObject bossWarningText; //���� ��ȯ �ؽ�Ʈ
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
        if (GameManager.instance.player.IsLive) //ĳ���Ͱ� ����������� ��ȯ
        {
            if (_timer > _spawnTime)
            {
                _timer = 0;
                NormalMonsterSpawn(); 
            }
            if (isPattern) { StartCoroutine(LevelPattern(_level)); }
            
        }
       
    }

    //������ ���� �Ϲ� ���� ��ȯ �Լ�
    private void NormalMonsterSpawn()
    {
        int ran = Random.Range(0, 5); 
        int ran2 = Random.Range(0, 5);
        Vector3 vec = new Vector3(ran, 0, ran2); //���ݾ� ��ġ�� �ٸ��� ����

        GameObject monster = GameManager.instance.monsterPool.Get(Random.Range(0, _level));
        monster.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].transform.position + vec; //���� �����ɶ� ���ݾ� ��ġ �ٸ��� ����
    }
    //�ð��� ���� ������ �Լ�
    private void LevelUp()
    {
        _levelTime += Time.deltaTime;
        if (_levelTime > _levelUpTime) //������ Ÿ�� ������ ������
        {
            _spawnTime -= 0.02f;
            _levelTime = 0;
            _level++;
            if(_level >= 2) { isPattern = true; }//�������Ҷ����� ���� true�� �ٲ��ش�. //�� 3���� �̻����
        }
        if (_level >= GameManager.instance.monsterPool.prefabs.Length)
        {
            _level = GameManager.instance.monsterPool.prefabs.Length;
        }
    }

    //������ ���� ���� ��ȯ �Լ�
    private IEnumerator LevelPattern(int level) 
    {
        switch (level)
        {
            case 2:
                MonsterMiddleBoss().MonsterCurHealth = 150; //1��~2�л��� �̵麸��
                break;
            case 3:
                for (int i = 0; i < 2; i++)
                {
                    MonsterPattern1(); //2~3�л��� ��������
                    yield return new WaitForSeconds(3f);
                }
                break;
            case 4:
                MonsterPattern2();
                MonsterMiddleBoss().MonsterCurHealth = 300; //3~4�� ���� �̵麸�� 
                break;
            case 6:
                MonsterBoss().MonsterCurHealth = 500; //5~6�л��� ������ȯ
                break;
            case 8:
                MonsterMiddleBoss().MonsterCurHealth = 400; //7~8�� ���� �̵麸�� 
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
                MonsterMiddleBoss().MonsterCurHealth = 800; //9~10�л��� �̵麸��
                break;
            case 11:
                MonsterBoss().MonsterCurHealth = 2500; //10~11�л��� ������ȯ
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
                MonsterMiddleBoss().MonsterCurHealth = 1500; //10~11�л��� �̵麸��
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
    //�������� ��ȯ����
    public void MonsterPattern1()
    {
        int ran = Random.Range(1, GameManager.instance.spawner.spawnPoint.Length);
        for (int i = 0; i < 10; i++)
        {
            int x = Random.Range(-2, 2);
            int z = Random.Range(-2, 2);
            Vector3 ranVec = new Vector3(x, 0, z); //���� ���� ������ �ٸ��� ������ �Ϸ��� ��ġ ����

            GameObject monster = GameManager.instance.monsterPool.GetPattern(0);
            monster.transform.position = GameManager.instance.spawner.spawnPoint[ran].transform.position + ranVec; //���� �����ɶ� ���ݾ� ��ġ �ٸ��� ����
        }
        isPattern = false;
    }
    //���� ��ġ �������� ��ȯ����
    public void MonsterPattern2()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject monster = GameManager.instance.monsterPool.GetPattern(1);
            monster.transform.position = GameManager.instance.spawner.spawnPoint[Random.Range(1, GameManager.instance.spawner.spawnPoint.Length)].transform.position;
        }
        isPattern = false;
    }
    //������������ ��ȯ����
    public void MonsterPattern3()
    {
        GameObject monster = GameManager.instance.monsterPool.GetPattern(2);
        monster.transform.position = GameManager.instance.spawner.spawnPoint[Random.Range(1, GameManager.instance.spawner.spawnPoint.Length)].transform.position;

        isPattern = false;
    } 
    //�߰����� ��ȯ����
    public Monster MonsterMiddleBoss()
    {
        Monster instantMonster = Instantiate(GameManager.instance.monsterDatas.bossMonster[0]);
        instantMonster.transform.position = GameManager.instance.spawner.spawnPoint[Random.Range(1, GameManager.instance.spawner.spawnPoint.Length)].transform.position;
        //---Ÿ������---                                     
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
    //���� ��ȯ����
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