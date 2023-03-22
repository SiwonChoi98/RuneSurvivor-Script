using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public enum ExpLevel { First, Second, Third }; //����ġ ����
   
    [Header("���� ������")]
    [SerializeField] private string _name; //���� �̸�
    [SerializeField] private string _resist; //���� �Ӽ�
    [SerializeField] private string _type; //������ �븻, ����, �������͸� �и�
    [SerializeField] private int _curHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage; //���� ������(���ݷ�)
    [SerializeField] private int _speed; //���� �̵��ӵ�
    [SerializeField] private int _expLevel; //���� ����ġ �ܰ�
    public string MonsterName { get => _name; set => _name = value; }
    public string MonsterResist { get => _resist; set => _resist = value; }
    public string MonsterType { get => _type; set => _type = value; }
    public int MonsterCurHealth { get => _curHealth; set => _curHealth = value; }
    public int MonsterMaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int MonsterDamage { get => _damage; set => _damage = value; }
    public int MonsterSpeed { get => _speed; set => _speed = value; }
    public int MonsterExpLevel { get => _expLevel; set => _expLevel = value; }

    
    [Header("���� �̵�")]
    public GameManager manager;
    public Transform target; // ���Ͱ� ������ Ÿ��
    protected Rigidbody rigid;
    protected Vector3 dirVec;
    protected Vector3 nextVec;
    [Header("���� ������")]
    [SerializeField] private GameObject dmgText; //�Ϲ� ������ �ؽ�Ʈ
    [SerializeField] private Transform dmgTextPos;
    [SerializeField] private GameObject dmgTextCritical; //ũ��Ƽ�� ������ �ؽ�Ʈ

    private BoxCollider meleeArea; // ���� �ݶ��̴�
    protected Animator anim;
    [Header("����ġ,ȸ��")]
    [SerializeField] private List<ExpData> expDatas;
    [SerializeField] private GameObject expPrefab;
    [SerializeField] private HealthItemData healthItemDatas;
    [SerializeField] private GameObject healthPrefab; //ȸ�������� 
    private int _healthCreatePercentage = 1; //ȸ�������� ���� Ȯ�� //50���� 1�� ���鿹��
    protected bool isLook = true;
    [Header("���� ����1����")]
    public bool _isLightningLevel1 = true; //�����Ӽ� 1���� ���ӵ����� ���� ����
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
    //����1���� �ǰ� ���� 
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
    } //����1���� �°� �ٽ� �����Ҽ� �ִ� ����
    #endregion
    //�̵� �Լ�
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
        if (other.CompareTag("Attack(Ice)")) //���̽�����
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


        }  //�� ����
        if (other.CompareTag("Attack(Fire)")) //���̾����
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

        } //�� ����
        if (other.CompareTag("Attack(Penetrate)")) //�������
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

        } //���� ����
        if (other.CompareTag("Attack(Poison)")) //�� ����
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
        } //�� ����
        
    }
    //ũ��Ƽ�� ���� �Լ�
    private bool IsCritical(bool result)
    {
        int ran = Random.Range(0, 100);
        if (ran > GameManager.instance.player.PlayerCriProbability) { result = false; }
        else { result = true; }
        return result;
    }
    //ĳ���Ϳ� �΋H���� �ڽ��ݶ��̴� ���� �״�. //�̰� �ڽ��ݶ��̴��� ���¹���� �ƴ϶� bool���� true/false �Ǵ� ������� �ٲ���� (�ӽ�)
    private IEnumerator BoxOnOff()
    {
        meleeArea.enabled = false;
        yield return new WaitForSeconds(1f);
        meleeArea.enabled = true;
    }
    //���� ü���������� ������ �̺�Ʈ �Լ�
    public void MonsterHit()
    {
        if (_curHealth > 0) //������ ü���� 0���� Ŭ���
        {
            anim.SetTrigger("Hit");
        }
        else if (_curHealth <= 0) //������ ü���� 0���� ���� ���
        {
            int ran2 = Random.Range(0, 21);
            if (ran2 < _healthCreatePercentage) { SpawnHealthItem(); }
            else { SpawnExp((ExpLevel)MonsterExpLevel); }

            if (_type == "NormalMonster" || _type == "Pattern1Monster" || _type == "Pattern2Monster" || _type == "Pattern3Monster") //���Ͱ� �븻�����̸� pool�� �����ҰŰ� �װԾƴϸ� ���������ٰž�
            {
                Dead();
                isLook = true;
            }
            else { Destroy(gameObject); }
            GameManager.instance.MonsterCount++; //���� �������� ����
        }
    }
    //����ġ����
    private Exp SpawnExp(ExpLevel level)
    {
        var newExp = Instantiate(expPrefab, transform.position+ new Vector3(0,0.4f,0), transform.rotation).GetComponent<Exp>();

        newExp.expData = expDatas[(int)level];
        newExp.name = newExp.expData.ExpLevel;
        return newExp;
    }
    //ȸ������
    private HealthItem SpawnHealthItem()
    {
        var newHealth = Instantiate(healthPrefab, transform.position + new Vector3(0, 0.4f, 0), transform.rotation).GetComponent<HealthItem>();

        newHealth.healthItemData = healthItemDatas;
        newHealth.name = newHealth.healthItemData.HealthName;
        return newHealth;
    }
    //�Ϲݵ����� �ؽ�Ʈ
    public void DamageText(int dmg) 
    {
        GameObject dmgtext1 = Instantiate(dmgText, dmgTextPos.position, Quaternion.Euler(70, 0, 0)); //Quaternion.identity ���� �������ִ� ������ ����
        dmgtext1.GetComponentInChildren<Text>().text = dmg.ToString();//�ڽ��ؽ�Ʈ�� ���� //dmg�� int�ϱ� string���·� �ٲ��ֱ�
        Destroy(dmgtext1, 0.7f);
    }
    //ũ��Ƽ�õ����� �ؽ�Ʈ
    private void DamageTextCri(int dmg)
    {
        GameObject dmgTextCritical1 = Instantiate(dmgTextCritical, dmgTextPos.position, Quaternion.Euler(70, 0, 0));
        dmgTextCritical1.GetComponentInChildren<Text>().text = dmg.ToString();
        Destroy(dmgTextCritical1, 0.7f);
    }
    //���� ��
    private void OnEnable()
    {
        target = GameManager.instance.player.transform;
        manager = GameManager.instance;
        MonsterCurHealth = MonsterMaxHealth;
    }
    //Ǯ�� �ֱ� �Լ�
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
