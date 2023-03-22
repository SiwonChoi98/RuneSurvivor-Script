using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [Header("매니저")]
    public static GameManager instance; //현재 클래스 선언 
    public SoundManager soundManager;
    public ButtonManager uiManager;
    
    [Header("플레이어 관련")]
    [SerializeField] private Image playerHealthImage; //플레이어 체력이미지
    [SerializeField] private Transform playerHealthImageTrans; //플레이어 체력이미지 위치
    public GameObject waterLevel1PosTrans; //물1레벨 캐릭터 따라다니게
    public Player player;
    [SerializeField] private Image playerExpImage;
    [SerializeField] private Transform area; //맵 이동 거리
    [Header("스킬 생성위치 ")]
    public SkillCreatePos skillCreatePos;
    [Header("무기 관련")]
    public Weapon weapon;
    [Header("수치(시간,몬스터 처치수)")]
    private float _gameTime;
    private float _maxGameTime = 20f * 60f;
    private int _monsterCount; //몬스터 잡은갯수
    public int MonsterCount{get => _monsterCount; set => _monsterCount = value;} //프로퍼티화
    public float GameTime { get => _gameTime; set => _gameTime = value; }
    [Header("상점")]
    public GameObject store;
    public bool isStore = false;
    [SerializeField] private RectTransform storeRt;
    [Header("게임오버")]
    [SerializeField] private GameObject gameOverPanel;
    public bool isGameStart = false;
    [Header("텍스트")]
    [SerializeField] private Text gameOverBattleTimeText; //게임오버배틀타임
    [SerializeField] private Text monsterCountText; //몬스터 잡은갯수 텍스트
    [SerializeField] private Text gameTimeTxt; //시간 텍스트
    [SerializeField] private Text playerExpText; //플레이어 업그레이드 텍스트                             
    [SerializeField] private Text[] slotsLevelText; //스킬레벨 별 텍스트
    [SerializeField] private Text playerLevelText; //플레이어 레벨 텍스트
    [Header("몬스터의 생성 및 정보")]
    public MonsterDatas monsterDatas;
    [Header("몬스터 패턴")]
    public MonsterPool monsterPool;
    public Spawner spawner;
    private void Awake()
    {
        MonsterCount = 0; //잡은 몬스터 횟수
        soundManager.backGroundSound.Play();
        //GameManager를 포함한 gameObject는 단 하나만 존재하게 처리
        if (instance == null) { instance = this; }
        else if(instance != this) { Destroy(gameObject); }//gameObject는 현재 scripts가 포함된 GameObject를 의미한다. //중복될경우 오브젝트 제거
        //DontDestroyOnLoad(gameObject); //화면 전환이 일어나도 오브젝트가 삭제하는걸 방지한다. DontDestroyOnLoad 이걸로 안하면 씬전환시 삭제됨

        ShopOpen();//게임 시작 시 바로 상점 오픈
    }

    private void LateUpdate()
    {
        area.position = player.transform.position; //캐릭터 주변 시야 area
        PlayTime(); 
        UiText();
        LevelUp();
    }
    //플레이 시간 함수
    private void PlayTime()
    {
        int hour = (int)(_gameTime / 3600);
        int min = (int)((_gameTime - hour * 3600) / 60);
        int second = (int)(_gameTime % 60);

        if (player.IsLive && isGameStart)
        {
            _gameTime += Time.deltaTime; //전투 중이면 플레이 시간 계속 올려준다.
                                              //battlePlayTimeTxt.text = Mathf.Round(battlePlayTime).ToString(); //총게임시간계산이랑 다른방식 Round : 부동소수점 값을 반올림 시킨다.
            gameTimeTxt.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second); //전투 시간
            gameOverBattleTimeText.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second); //(게임오버)전투 시간
        }
        if (_gameTime > _maxGameTime)
        {
            _gameTime = _maxGameTime;
        }
    }
    //UI 이미지,텍스트 함수
    private void UiText()
    {
        playerHealthImage.fillAmount = Mathf.Lerp(playerHealthImage.fillAmount, (float)player.PlayerCurHealth / player.PlayerMaxHealth / 1 / 1, Time.deltaTime * 5); //플레이어 체력
        playerHealthImageTrans.position = player.transform.position + new Vector3(0, 3f, 0); //플레이어 체력 위치 플레이어 머리위에
        playerExpImage.fillAmount = Mathf.Lerp(playerExpImage.fillAmount, (float)player.PlayerCurExp / player.PlayerMaxExp / 1 / 1, Time.deltaTime * 5); //플레이어 경험치 바
        waterLevel1PosTrans.transform.position = player.transform.position + new Vector3(0, 0.5f, 0); //물1레벨 포지션

        playerExpText.text = player.PlayerCurExp + "/" + player.PlayerMaxExp; //플레이어 경험치 텍스트
        playerLevelText.text = player.PlayerLevel + ""; //플레이어 레벨 텍스트
        monsterCountText.text = _monsterCount+""; //몬스터 갯수 텍스트

        slotsLevelText[0].text = "LEVEL :" + weapon.inventory.slots[0].skillLevel;
        slotsLevelText[1].text = "LEVEL :" + weapon.inventory.slots[1].skillLevel;
        slotsLevelText[2].text = "LEVEL :" + weapon.inventory.slots[2].skillLevel;
    }
    //캐릭터 레벨업 이벤트 함수
    private void LevelUp()
    {
        if (player.PlayerCurExp >= player.PlayerMaxExp) //현재 업그레이드가 최대치 이면 플레이어 레벨업하고 샵오픈
        {
            player.PlayerLevel++; //플레이어 레벨업
            player.particleSystems[0].Play(); //레벨업 파티클 발생
            player.PlayerCurExp = 0;
            //player.PlayerMaxExp += 3 * player.PlayerLevel; //레벨업 했으니까 업그레이드 수치 올려주기 (더 많이 모아야하게 ) //조정필요
            ShopOpen();
        }
        if (isStore) { Time.timeScale = 0; }//상점이 오픈되면 다른건 멈춘다. 그리고 상점이 클로즈 되면 다시 움직인다.
        else { Time.timeScale = 1; } //상점오픈 시 이동 멈추기
    }
    //(캐릭터 레벨업 이벤트 함수) 상점오픈 이벤트 함수
    private void ShopOpen()
    {
        isStore = true;
        store.SetActive(true);
        uiManager.PanelFadeIn(uiManager.storePanelCanGroup);
    }
    //(player.cs(OnTrigger)) 게임오버 이벤트 함수
    public void StopGame()
    {
        SoundManager.Instance.backGroundSound.Stop();
        SoundManager.Instance.deathPanelSound.Play();
        gameOverPanel.SetActive(true);
    }
}
