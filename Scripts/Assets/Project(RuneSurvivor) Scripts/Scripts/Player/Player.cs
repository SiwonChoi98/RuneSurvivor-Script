using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    [Header("�÷��̾� ������")]
    [SerializeField] private int _curHealth;
    [SerializeField] private int _maxHealth; 
    [SerializeField] private int _curExp;
    [SerializeField] private int _maxExp;
    [SerializeField] private int _speed;
    [SerializeField] private int _playerLevel;
    [SerializeField] private int _criProbability;
    public int PlayerCurHealth { get { return _curHealth; } set { _curHealth = value; } } //set,get�տ��ٰ� private���� ŸŬ�������� �����Ҽ�����. 
    public int PlayerMaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int PlayerCurExp { get => _curExp; set => _curExp = value; }
    public int PlayerMaxExp { get => _maxExp; set => _maxExp = value; }
    public int PlayerMoveSpeed { get => _speed; set => _speed = value; }
    public int PlayerCriProbability { get => _criProbability; set => _criProbability = value; }
    public int PlayerLevel { get => _playerLevel; set => _playerLevel = value; }
    public float PlayerRange { get => _range; set => _range = value; }
    public bool IsLive { get => _isLive; set => _isLive = false; }

    public Vector3 inputVec;
    private Vector3 _nextVec;
    private Rigidbody _rigid;
    [SerializeField] private Weapon weapon;
    public Animator anim;
    [Header("���ӿ���")]
    private bool _isLive = true;
    [Header("�ڵ�Ÿ��")]
    [SerializeField] private float _range; //ĳ���� ���� �����Ÿ�
    [SerializeField] private LayerMask _LayerMask; //enemy ����ũ�� �����Ҽ� �ְ�
    public Transform Target = null; //������ ���
    [Header("ü�� ���� �� ī�޶�")]
    [SerializeField] private HpDownCamera hpDownCamera;

    [Header("���� ��ƼŬ")]
    public List<ParticleSystem> particleSystems;
    
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();
        InvokeRepeating("SearchEnemy", 0f, 1f); // 1�� ���� �������ִ� ���ο� �� ã��
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.isGameStart) 
        {
            Move();
            FreezeRotation(); 
        }
    }
    //�̵� <Ű �Է��� �޴°� update MovePosition�� fixedupdate���� ó���ϴ°� �������̴�.>
    private void Move() 
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.z = Input.GetAxisRaw("Vertical");
        _nextVec.x = 0; //�ӽ�
        _rigid.velocity = Vector3.zero;
        if (_isLive)
        {
            _nextVec = inputVec.normalized * _speed * Time.fixedDeltaTime;

            _rigid.MovePosition(_rigid.position + _nextVec);    
            transform.LookAt(_rigid.position + _nextVec); 
            anim.SetBool("IsRun", _nextVec != Vector3.zero);
        }

    }
    //����� ���� �˻�
    private void SearchEnemy()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _range, _LayerMask); //OverlapSphere : ��ü �ֺ� �ݶ��̴��� ���� //cols ��� �迭���� range�Ÿ��� �ִ� LayerMask �� ����
        Transform t_shortestTarget = null; //�ͷ��� ���� ����� �� ã�� //��ġ�� ���� ���̾�

        if (cols.Length > 0) //cols �迭�� 1���̻� ���� ����
        {
            float t_shortestDistance = Mathf.Infinity; //Infinity : ���� ���Ŀ����.
            foreach (Collider t_colTarget in cols) //�ֺ��ݶ��̴��� t_colTarget���� �Ѱ���
            {
                float t_disfance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position); //SqrMagnitude : ������Ʈ���� �Ÿ��� üũ�� �� ��� 
                if (t_shortestDistance > t_disfance) //t_disfance : �÷��̾�� ���� ������ �Ÿ�
                {
                    t_shortestDistance = t_disfance; //���Ѵ� �Ÿ��� �� ũ�� ���Ѵ� �Ÿ��� t_disfance�� �ȴ�.
                    t_shortestTarget = t_colTarget.transform; //�ֺ��ݶ��̴��� ���� null�� t_shortestTarget���� ����ְ�
                }
            }
        }
        Target = t_shortestTarget; //Ÿ���� public���� Ȯ�� �� �� �ִ� Target���� �־��ش�.

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (_curHealth > 0) //ü���� 0�̻��϶��� ����ī�޶� �ߵ� 
            { 
                StartCoroutine("HpDownCamera");
                Monster monster = other.GetComponent<Monster>();
                GameObject rockLevel1GO = GameObject.Find("RockLevel1(Clone)"); //���Ͷ� �浹�� ��ȣ���� �ʵ忡 �ִ��� �˻��� ������ false�� �����༭ �������ݰ� �������
                Debug.Log("monsterDamage");
                int monDamage = monster.MonsterDamage;

                if (rockLevel1GO != null) { monDamage = monster.MonsterDamage / 2; }
                else { monDamage = monster.MonsterDamage; }

                _curHealth -= monDamage;
                Debug.Log(monDamage);

            }
            else 
            {
                _curHealth = 0;
                _isLive = false; // 0���Ϸ� �������� 
                GameManager.instance.StopGame();
            }
        }
        //����ġ
        #region 
        if (other.gameObject.name == "Exp 1" || other.gameObject.name == "Exp 2" ||other.gameObject.name == "Exp 3") //����ġ�� ���� �ٸ��� �÷��ش�.
        {
            SoundManager.Instance.CoinSound();
            _curExp += other.GetComponent<Exp>().expData.Exp; //curExp += 1;
        }
        if (other.gameObject.TryGetComponent<Exp>(out Exp exp))
        {
            exp.target = gameObject;
        } //�ڼ�(����ġ)
        #endregion
        //ȸ��
        #region
        if (other.gameObject.name == "HealthItem1") 
        {
            SoundManager.Instance.HealSound();
            particleSystems[1].Play();
            _curHealth += other.GetComponent<HealthItem>().healthItemData.Health;
            if(_curHealth >= _maxHealth) { _curHealth = _maxHealth; }
        }
        if (other.gameObject.TryGetComponent<HealthItem>(out HealthItem healthItem))
        {
            healthItem.target = gameObject;
        } //�ڼ�(ȸ��������)
        #endregion
    }

    //(OnTrigger) ���̴� ī�޶� on/off �Լ�
    private IEnumerator HpDownCamera()
    {
        hpDownCamera.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hpDownCamera.enabled = false;
    }
    //ȸ�� ���� �Լ�
    private void FreezeRotation()
    {
        _rigid.angularVelocity = Vector3.zero;
    }
   
}
