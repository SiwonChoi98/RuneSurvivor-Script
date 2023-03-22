using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public enum ExpLevel { First, Second, Third }; //경험치 레벨
   
    [Header("몬스터 데이터")]
    [SerializeField] private string _name; //몬스터 이름
    [SerializeField] private string _resist; //몬스터 속성
    [SerializeField] private string _type; //몬스터의 노말, 패턴, 보스몬스터를 분리
    [SerializeField] private int _curHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage; //몬스터 데미지(공격력)
    [SerializeField] private int _speed; //몬스터 이동속도
    [SerializeField] private int _expLevel; //몬스터 경험치 단계
    public string MonsterName { get => _name; set => _name = value; }
    public string MonsterResist { get => _resist; set => _resist = value; }
    public string MonsterType { get => _type; set => _type = value; }
    public int MonsterCurHealth { get => _curHealth; set => _curHealth = value; }
    public int MonsterMaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int MonsterDamage { get => _damage; set => _damage = value; }
    public int MonsterSpeed { get => _speed; set => _speed = value; }
    public int MonsterExpLevel { get => _expLevel; set => _expLevel = value; }

    
    [Header("몬스터 이동")]
    public GameManager manager;
    public Transform target; // 몬스터가 지정한 타겟
    protected Rigidbody rigid;
    protected Vector3 dirVec;
    protected Vector3 nextVec;
    [Header("몬스터 데미지")]
    [SerializeField] private GameObject dmgText; //일반 데미지 텍스트
    [SerializeField] private Transform dmgTextPos;
    [SerializeField] private GameObject dmgTextCritical; //크리티컬 데미지 텍스트

    private BoxCollider meleeArea; // 몬스터 콜라이더
    protected Animator anim;
    [Header("경험치,회복")]
    [SerializeField] private List<ExpData> expDatas;
    [SerializeField] private GameObject expPrefab;
    [SerializeField] private HealthItemData healthItemDatas;
    [SerializeField] private GameObject healthPrefab; //회복아이템 
    private int _healthCreatePercentage = 1; //회복아이템 나올 확률 //50분의 1로 만들예정
    protected bool isLook = true;
    [Header("몬스터 번개1레벨")]
    public bool _isLightningLevel1 = true; //번개속성 1레벨 지속데미지 위한 변수
    protected float lightningTime = 0.5f;
    protected float currentLightningTime;
    private void Awake()
    {
        meleeArea = GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rigid.position = new Vector3(0,0,0);
        currentLightningTime = lightningTime;
    }
    //번개1레벨 피격 상태 
    #region
    private void Update()
    {
        IsLightningBool();
    }
    private void IsLightningBool()
    {
        if (!_isLightningLevel1)
        {
            currentLightningTime -= Time.deltaTime;
            if (currentLightningTime < 0)
            {
                currentLightningTime = lightningTime;
                _isLightningLevel1 = true;
            }
        }
    } //번개1레벨 맞고 다시 공격할수 있는 상태
    #endregion
    //이동 함수
    #region

    private void FixedUpdate()
    {
        Move();
    }
    public virtual void Move() 
    {
     

    }
    protected IEnumerator IsLook()
    {
        transform.LookAt(target.position);
        yield return new WaitForSeconds(1.5f);
        isLook = false;
    }
    #endregion 
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { StartCoroutine(BoxOnOff()); }
        if (other.CompareTag("Attack(Ice)")) //아이스공격
        {
            bool isCri = false;
            int ranDamage = Random.Range(0, 5);
            Skill IceAttack = other.GetComponent<Skill>();
            
            if (!IsCritical(isCri))
            {
                _curHealth -= IceAttack.skillDamage + ranDamage;
                DamageText(IceAttack.skillDamage + ranDamage);
            }
            else
            {
                _curHealth -= IceAttack.skillDamage + 10 + ranDamage;
                DamageTextCri(IceAttack.skillDamage + 10 + ranDamage);
            }
            MonsterHit();


        }  //물 공격
        if (other.CompareTag("Attack(Fire)")) //파이어공격
        {
            bool isCri = false;
            int ranDamage = Random.Range(0, 5);
            Skill fireAttack = other.GetComponent<Skill>();
            
            if (!IsCritical(isCri))
            {
                DamageText(fireAttack.skillDamage + ranDamage);
                _curHealth -= fireAttack.skillDamage + ranDamage;
            }
            else
            {
                DamageTextCri(fireAttack.skillDamage + 10 + ranDamage);
                _curHealth -= fireAttack.skillDamage + 10 + ranDamage;
            }

            MonsterHit();

        } //불 공격
        if (other.CompareTag("Attack(Penetrate)")) //관통공격
        {
            bool isCri = false;
            int ranDamage = Random.Range(0, 5);
            Skill penetrateAttack = other.GetComponent<Skill>();
           
            if (!IsCritical(isCri))
            {
                DamageText(penetrateAttack.skillDamage + ranDamage);
                _curHealth -= penetrateAttack.skillDamage + ranDamage;
            }
            else
            {
                DamageTextCri(penetrateAttack.skillDamage + 10 + ranDamage);
                _curHealth -= penetrateAttack.skillDamage + 10 + ranDamage;
            }
            MonsterHit();

        } //번개 공격
        if (other.CompareTag("Attack(Poison)")) //독 공격
        {
            bool isCri = false;
            int ranDamage = Random.Range(0, 5);
            Skill poisonAttack = other.GetComponent<Skill>();

            if (!IsCritical(isCri))
            {
                DamageText(poisonAttack.skillDamage + ranDamage);
                _curHealth -= poisonAttack.skillDamage + ranDamage;
                
            }
            else
            {
                DamageTextCri(poisonAttack.skillDamage + 10 + ranDamage);
                _curHealth -= poisonAttack.skillDamage + 10 + ranDamage;
            }

            MonsterHit();
        } //땅 공격
        
    }
    //크리티컬 판정 함수
    private bool IsCritical(bool result)
    {
        int ran = Random.Range(0, 100);
        if (ran > GameManager.instance.player.PlayerCriProbability) { result = false; }
        else { result = true; }
        return result;
    }
    //캐릭터와 부딫히면 박스콜라이더 껐다 켰다. //이거 박스콜라이더를 끄는방식이 아니라 bool값이 true/false 되는 방식으로 바꿔야함 (임시)
    private IEnumerator BoxOnOff()
    {
        meleeArea.enabled = false;
        yield return new WaitForSeconds(1f);
        meleeArea.enabled = true;
    }
    //몬스터 체력있을때랑 없을때 이벤트 함수
    public void MonsterHit()
    {
        if (_curHealth > 0) //몬스터의 체력이 0보다 클경우
        {
            anim.SetTrigger("Hit");
        }
        else if (_curHealth <= 0) //몬스터의 체력이 0보다 작을 경우
        {
            int ran2 = Random.Range(0, 21);
            if (ran2 < _healthCreatePercentage) { SpawnHealthItem(); }
            else { SpawnExp((ExpLevel)MonsterExpLevel); }

            if (_type == "NormalMonster" || _type == "Pattern1Monster" || _type == "Pattern2Monster" || _type == "Pattern3Monster") //몬스터가 노말몬스터이면 pool에 저장할거고 그게아니면 삭제시켜줄거야
            {
                Dead();
                isLook = true;
            }
            else { Destroy(gameObject); }
            GameManager.instance.MonsterCount++; //몬스터 잡은숫자 증가
        }
    }
    //경험치생성
    private Exp SpawnExp(ExpLevel level)
    {
        var newExp = Instantiate(expPrefab, transform.position+ new Vector3(0,0.4f,0), transform.rotation).GetComponent<Exp>();

        newExp.expData = expDatas[(int)level];
        newExp.name = newExp.expData.ExpLevel;
        return newExp;
    }
    //회복생성
    private HealthItem SpawnHealthItem()
    {
        var newHealth = Instantiate(healthPrefab, transform.position + new Vector3(0, 0.4f, 0), transform.rotation).GetComponent<HealthItem>();

        newHealth.healthItemData = healthItemDatas;
        newHealth.name = newHealth.healthItemData.HealthName;
        return newHealth;
    }
    //일반데미지 텍스트
    public void DamageText(int dmg) 
    {
        GameObject dmgtext1 = Instantiate(dmgText, dmgTextPos.position, Quaternion.Euler(70, 0, 0)); //Quaternion.identity 원래 가지고있는 각도로 생성
        dmgtext1.GetComponentInChildren<Text>().text = dmg.ToString();//자식텍스트로 들어가서 //dmg는 int니까 string형태로 바꿔주기
        Destroy(dmgtext1, 0.7f);
    }
    //크리티컬데미지 텍스트
    private void DamageTextCri(int dmg)
    {
        GameObject dmgTextCritical1 = Instantiate(dmgTextCritical, dmgTextPos.position, Quaternion.Euler(70, 0, 0));
        dmgTextCritical1.GetComponentInChildren<Text>().text = dmg.ToString();
        Destroy(dmgTextCritical1, 0.7f);
    }
    //생성 시
    private void OnEnable()
    {
        target = GameManager.instance.player.transform;
        manager = GameManager.instance;
        MonsterCurHealth = MonsterMaxHealth;
    }
    //풀로 넣기 함수
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
