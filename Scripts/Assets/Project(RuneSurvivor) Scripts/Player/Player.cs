using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    [Header("플레이어 데이터")]
    [SerializeField] private int _curHealth;
    [SerializeField] private int _maxHealth; 
    [SerializeField] private int _curExp;
    [SerializeField] private int _maxExp;
    [SerializeField] private int _speed;
    [SerializeField] private int _playerLevel;
    [SerializeField] private int _criProbability;
    public int PlayerCurHealth { get { return _curHealth; } set { _curHealth = value; } } //set,get앞에다가 private쓰면 타클래스에서 접근할수없다. 
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
    [Header("게임오버")]
    private bool _isLive = true;
    [Header("자동타격")]
    [SerializeField] private float _range; //캐릭터 공격 사정거리
    [SerializeField] private LayerMask _LayerMask; //enemy 마스크만 공격할수 있게
    public Transform Target = null; //공격할 대상
    [Header("체력 감소 시 카메라")]
    [SerializeField] private HpDownCamera hpDownCamera;

    [Header("상태 파티클")]
    public List<ParticleSystem> particleSystems;
    
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();
        InvokeRepeating("SearchEnemy", 0f, 1f); // 1초 마다 가까이있는 새로운 적 찾기
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.isGameStart) 
        {
            Move();
            FreezeRotation(); 
        }
    }
    //이동 <키 입력을 받는건 update MovePosition은 fixedupdate에서 처리하는게 안정적이다.>
    private void Move() 
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.z = Input.GetAxisRaw("Vertical");
        _nextVec.x = 0; //임시
        _rigid.velocity = Vector3.zero;
        if (_isLive)
        {
            _nextVec = inputVec.normalized * _speed * Time.fixedDeltaTime;

            _rigid.MovePosition(_rigid.position + _nextVec);    
            transform.LookAt(_rigid.position + _nextVec); 
            anim.SetBool("IsRun", _nextVec != Vector3.zero);
        }

    }
    //가까운 몬스터 검색
    private void SearchEnemy()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _range, _LayerMask); //OverlapSphere : 객체 주변 콜라이더를 검출 //cols 라는 배열에는 range거리에 있는 LayerMask 다 검출
        Transform t_shortestTarget = null; //터렛과 가장 가까운 거 찾기 //위치는 현재 널이야

        if (cols.Length > 0) //cols 배열에 1개이상 들어가면 실행
        {
            float t_shortestDistance = Mathf.Infinity; //Infinity : 무한 계속커진다.
            foreach (Collider t_colTarget in cols) //주변콜라이더를 t_colTarget으로 넘겨줌
            {
                float t_disfance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position); //SqrMagnitude : 오브젝트간의 거리를 체크할 때 사용 
                if (t_shortestDistance > t_disfance) //t_disfance : 플레이어와 몬스터 사이의 거리
                {
                    t_shortestDistance = t_disfance; //무한대 거리가 더 크면 무한대 거리는 t_disfance가 된다.
                    t_shortestTarget = t_colTarget.transform; //주변콜라이더는 현재 null인 t_shortestTarget으로 집어넣고
                }
            }
        }
        Target = t_shortestTarget; //타켓을 public으로 확인 할 수 있는 Target으로 넣어준다.

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (_curHealth > 0) //체력이 0이상일때만 감소카메라 발동 
            { 
                StartCoroutine("HpDownCamera");
                Monster monster = other.GetComponent<Monster>();
                GameObject rockLevel1GO = GameObject.Find("RockLevel1(Clone)"); //몬스터랑 충돌시 보호막이 필드에 있는지 검사후 없으면 false로 돌려줘서 데미지반감 사라지게
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
                _isLive = false; // 0이하로 내려가면 
                GameManager.instance.StopGame();
            }
        }
        //경험치
        #region 
        if (other.gameObject.name == "Exp 1" || other.gameObject.name == "Exp 2" ||other.gameObject.name == "Exp 3") //경험치에 따라서 다르게 올려준다.
        {
            SoundManager.Instance.CoinSound();
            _curExp += other.GetComponent<Exp>().expData.Exp; //curExp += 1;
        }
        if (other.gameObject.TryGetComponent<Exp>(out Exp exp))
        {
            exp.target = gameObject;
        } //자석(경험치)
        #endregion
        //회복
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
        } //자석(회복아이템)
        #endregion
    }

    //(OnTrigger) 쉐이더 카메라 on/off 함수
    private IEnumerator HpDownCamera()
    {
        hpDownCamera.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hpDownCamera.enabled = false;
    }
    //회전 제한 함수
    private void FreezeRotation()
    {
        _rigid.angularVelocity = Vector3.zero;
    }
   
}
