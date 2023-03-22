using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [Header("�Ŵ���")]
    public static GameManager instance; //���� Ŭ���� ���� 
    public SoundManager soundManager;
    public ButtonManager uiManager;
    
    [Header("�÷��̾� ����")]
    [SerializeField] private Image playerHealthImage; //�÷��̾� ü���̹���
    [SerializeField] private Transform playerHealthImageTrans; //�÷��̾� ü���̹��� ��ġ
    public GameObject waterLevel1PosTrans; //��1���� ĳ���� ����ٴϰ�
    public Player player;
    [SerializeField] private Image playerExpImage;
    [SerializeField] private Transform area; //�� �̵� �Ÿ�
    [Header("��ų ������ġ ")]
    public SkillCreatePos skillCreatePos;
    [Header("���� ����")]
    public Weapon weapon;
    [Header("��ġ(�ð�,���� óġ��)")]
    private float _gameTime;
    private float _maxGameTime = 20f * 60f;
    private int _monsterCount; //���� ��������
    public int MonsterCount{get => _monsterCount; set => _monsterCount = value;} //������Ƽȭ
    public float GameTime { get => _gameTime; set => _gameTime = value; }
    [Header("����")]
    public GameObject store;
    public bool isStore = false;
    [SerializeField] private RectTransform storeRt;
    [Header("���ӿ���")]
    [SerializeField] private GameObject gameOverPanel;
    public bool isGameStart = false;
    [Header("�ؽ�Ʈ")]
    [SerializeField] private Text gameOverBattleTimeText; //���ӿ�����ƲŸ��
    [SerializeField] private Text monsterCountText; //���� �������� �ؽ�Ʈ
    [SerializeField] private Text gameTimeTxt; //�ð� �ؽ�Ʈ
    [SerializeField] private Text playerExpText; //�÷��̾� ���׷��̵� �ؽ�Ʈ                             
    [SerializeField] private Text[] slotsLevelText; //��ų���� �� �ؽ�Ʈ
    [SerializeField] private Text playerLevelText; //�÷��̾� ���� �ؽ�Ʈ
    [Header("������ ���� �� ����")]
    public MonsterDatas monsterDatas;
    [Header("���� ����")]
    public MonsterPool monsterPool;
    public Spawner spawner;
    private void Awake()
    {
        MonsterCount = 0; //���� ���� Ƚ��
        soundManager.backGroundSound.Play();
        //GameManager�� ������ gameObject�� �� �ϳ��� �����ϰ� ó��
        if (instance == null) { instance = this; }
        else if(instance != this) { Destroy(gameObject); }//gameObject�� ���� scripts�� ���Ե� GameObject�� �ǹ��Ѵ�. //�ߺ��ɰ�� ������Ʈ ����
        //DontDestroyOnLoad(gameObject); //ȭ�� ��ȯ�� �Ͼ�� ������Ʈ�� �����ϴ°� �����Ѵ�. DontDestroyOnLoad �̰ɷ� ���ϸ� ����ȯ�� ������

        ShopOpen();//���� ���� �� �ٷ� ���� ����
    }

    private void LateUpdate()
    {
        area.position = player.transform.position; //ĳ���� �ֺ� �þ� area
        PlayTime(); 
        UiText();
        LevelUp();
    }
    //�÷��� �ð� �Լ�
    private void PlayTime()
    {
        int hour = (int)(_gameTime / 3600);
        int min = (int)((_gameTime - hour * 3600) / 60);
        int second = (int)(_gameTime % 60);

        if (player.IsLive && isGameStart)
        {
            _gameTime += Time.deltaTime; //���� ���̸� �÷��� �ð� ��� �÷��ش�.
                                              //battlePlayTimeTxt.text = Mathf.Round(battlePlayTime).ToString(); //�Ѱ��ӽð�����̶� �ٸ���� Round : �ε��Ҽ��� ���� �ݿø� ��Ų��.
            gameTimeTxt.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second); //���� �ð�
            gameOverBattleTimeText.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second); //(���ӿ���)���� �ð�
        }
        if (_gameTime > _maxGameTime)
        {
            _gameTime = _maxGameTime;
        }
    }
    //UI �̹���,�ؽ�Ʈ �Լ�
    private void UiText()
    {
        playerHealthImage.fillAmount = Mathf.Lerp(playerHealthImage.fillAmount, (float)player.PlayerCurHealth / player.PlayerMaxHealth / 1 / 1, Time.deltaTime * 5); //�÷��̾� ü��
        playerHealthImageTrans.position = player.transform.position + new Vector3(0, 3f, 0); //�÷��̾� ü�� ��ġ �÷��̾� �Ӹ�����
        playerExpImage.fillAmount = Mathf.Lerp(playerExpImage.fillAmount, (float)player.PlayerCurExp / player.PlayerMaxExp / 1 / 1, Time.deltaTime * 5); //�÷��̾� ����ġ ��
        waterLevel1PosTrans.transform.position = player.transform.position + new Vector3(0, 0.5f, 0); //��1���� ������

        playerExpText.text = player.PlayerCurExp + "/" + player.PlayerMaxExp; //�÷��̾� ����ġ �ؽ�Ʈ
        playerLevelText.text = player.PlayerLevel + ""; //�÷��̾� ���� �ؽ�Ʈ
        monsterCountText.text = _monsterCount+""; //���� ���� �ؽ�Ʈ

        slotsLevelText[0].text = "LEVEL :" + weapon.inventory.slots[0].skillLevel;
        slotsLevelText[1].text = "LEVEL :" + weapon.inventory.slots[1].skillLevel;
        slotsLevelText[2].text = "LEVEL :" + weapon.inventory.slots[2].skillLevel;
    }
    //ĳ���� ������ �̺�Ʈ �Լ�
    private void LevelUp()
    {
        if (player.PlayerCurExp >= player.PlayerMaxExp) //���� ���׷��̵尡 �ִ�ġ �̸� �÷��̾� �������ϰ� ������
        {
            player.PlayerLevel++; //�÷��̾� ������
            player.particleSystems[0].Play(); //������ ��ƼŬ �߻�
            player.PlayerCurExp = 0;
            //player.PlayerMaxExp += 3 * player.PlayerLevel; //������ �����ϱ� ���׷��̵� ��ġ �÷��ֱ� (�� ���� ��ƾ��ϰ� ) //�����ʿ�
            ShopOpen();
        }
        if (isStore) { Time.timeScale = 0; }//������ ���µǸ� �ٸ��� �����. �׸��� ������ Ŭ���� �Ǹ� �ٽ� �����δ�.
        else { Time.timeScale = 1; } //�������� �� �̵� ���߱�
    }
    //(ĳ���� ������ �̺�Ʈ �Լ�) �������� �̺�Ʈ �Լ�
    private void ShopOpen()
    {
        isStore = true;
        store.SetActive(true);
        uiManager.PanelFadeIn(uiManager.storePanelCanGroup);
    }
    //(player.cs(OnTrigger)) ���ӿ��� �̺�Ʈ �Լ�
    public void StopGame()
    {
        SoundManager.Instance.backGroundSound.Stop();
        SoundManager.Instance.deathPanelSound.Play();
        gameOverPanel.SetActive(true);
    }
}
