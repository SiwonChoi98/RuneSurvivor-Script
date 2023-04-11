using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //레벨별 필요 원소 갯수
    private int _level1 = 0;
    private int _level2 = 3;
    private int _level3 = 5;
    private int _level4 = 10;

    [Header("임시 저장공간 선택표시")]
    [SerializeField] private Transform checkSlotImage; //저장공간 선택되었다는 표시
    //---------------------임시---------------
    [SerializeField] private Transform slot1ChildRoot; //슬롯1 서브
    [SerializeField] private Transform slot2ChildRoot; //슬롯2 서브
    [SerializeField] private Transform slot3ChildRoot; //슬롯3 서브

    [Header("스킬(자식)저장 리스트")]
    public List<Slot> slots1Child;
    public List<Slot> slots2Child;
    public List<Slot> slots3Child;
    [Header("스킬(서브) 이미지 오브젝트")]
    [SerializeField] private GameObject[] slots1ChildGo; //게임오브젝트 setActive시 필요
    [SerializeField] private GameObject[] slots2ChildGo; //게임오브젝트
    [SerializeField] private GameObject[] slots3ChildGo; //게임오브젝트

    //서브슬롯 저장위치
    private int _slot1i;
    private int _slot2i;
    private int _slot3i;


    [SerializeField] private Transform slot1ChildRootBack; //슬롯1 서브빽업
    [SerializeField] private Transform slot2ChildRootBack; //슬롯2 서브빽업
    [SerializeField] private Transform slot3ChildRootBack; //슬롯3 서브빽업

    [Header("스킬(서브)빽업 저장 리스트")]
    [SerializeField] private List<Slot> slots1ChildBack;
    [SerializeField] private List<Slot> slots2ChildBack;
    [SerializeField] private List<Slot> slots3ChildBack;
    //--------------------------------

    [SerializeField] private Transform slotRoot; //슬롯 총담당 하는 그룹

    public Store store;

    [Header("스킬(메인)저장 리스트")]
    public List<Slot> slots;
    [Header("슬롯 별 메인,서브 속성")]
    private string _slot1MainElemental;
    private string _slot1SubElemental;
    private string _slot2MainElemental;
    private string _slot2SubElemental;
    private string _slot3MainElemental;
    private string _slot3SubElemental;
    //메인슬롯 저장위치
    private int _num;

    private int _mainRan;
    private int _subRan;
    //public System.Action<SkillProperty> onSlotClick;
    void Start()
    {
        slots1ChildBack = new List<Slot>();
        slots2ChildBack = new List<Slot>();
        slots3ChildBack = new List<Slot>();
        //---------------------------test
        slots1Child = new List<Slot>();
        slots2Child = new List<Slot>();
        slots3Child = new List<Slot>();
        int slot1ChildCnt = slot1ChildRoot.childCount;
        //--------------------------------

        slots = new List<Slot>();

        int slotCnt = slotRoot.childCount;

        for (int i = 0; i < slotCnt; i++) //메인슬롯
        {
            Slot slot = slotRoot.GetChild(i).GetComponent<Slot>(); //slot 스크립트가 붙어 있는걸 다 찾아주는거야
            slots.Add(slot); //슬롯의 개수를 (3개) 파악한 다음에 slots라는 리스트에 slot을 추가해줄거야
        }
        //---------------------------test
        for (int i = 0; i < slot1ChildCnt - 3; i++) //서브슬롯
        {
            Slot slot = slot1ChildRoot.GetChild(i).GetComponent<Slot>();
            slots1Child.Add(slot);
            Slot slot2 = slot2ChildRoot.GetChild(i).GetComponent<Slot>();
            slots2Child.Add(slot2);
            Slot slot3 = slot3ChildRoot.GetChild(i).GetComponent<Slot>();
            slots3Child.Add(slot3);
        }
        for (int i = 0; i < 4; i++) //슬롯 빽업 4개만
        {
            Slot slot = slot1ChildRootBack.GetChild(i).GetComponent<Slot>();
            slots1ChildBack.Add(slot);
            Slot slot2 = slot2ChildRootBack.GetChild(i).GetComponent<Slot>();
            slots2ChildBack.Add(slot2);
            Slot slot3 = slot3ChildRootBack.GetChild(i).GetComponent<Slot>();
            slots3ChildBack.Add(slot3);
        }
        //--------------------------------
        store.onSlotClick += BuyItem; //스토어에서 클릭하면 buyitem 함수까지 합쳐서 실행해줘라

    }
    void BuyItem(SkillProperty skill)
    {
        //클릭시 가장 낮은 인덱스 부터 인벤토리에 넣어줄거야
        var emptySlot2 = slots.Find(t => //find는 0번 인덱스부터 훑는다. 조건 만족되면 반환시킨다. 인덱스가 낮을수록 우선순위가 높은것
        {
            return t.skill == null || t.skill.skillName == string.Empty; //skill이 null 이거나 skull의 이름이 empty면 리턴
            //이거랑 같은의미
            /*
            if(t.skill == null || t.skill.skillName == string.Empty){
                return true;
            }
            */
        });
        Slot emptySlot = slots[_num];


        if (emptySlot.name == "slot") //1. 처음 메인 슬롯에 스킬 채워줌
        {
            GameManager.instance.uiManager.PanelFadeIn(GameManager.instance.uiManager.skillImageFadeGroup[_num]); //효과
            emptySlot.SetItem(skill);
            Debug.Log(skill.skillName + "이 저장되었습니다.");
        }
        else //2. 메인슬롯에 스킬이 있으면
        {
            if (_num == 0)
            {
                if (_slot1i >= 10) { return; } //슬롯에 위치가 10이상이면 리턴  

                slots1Child[_slot1i].SetItem(skill);//하위슬롯위치가 10보다 작으면 그위치에 넣어줌

                if (emptySlot.skillLevel == 1) //레벨 1때는 하위슬롯이 없는 상태
                {
                    if (BadCompatibilitySlot()) //상성체크 
                    {
                        if (slots[1].name == "slot" && slots[2].name == "slot") //0번 슬롯이 1레벨인데 2,3번 슬롯에 스킬이 없으면 상성이 안맞아도 다음꺼에 넣어줌
                        {
                            emptySlot2.SetItem(skill); //비어있는곳에 넣기
                        }
                        else
                        {
                            emptySlot.SetItem(null); //메인스킬 제거
                            slots1Child[_slot1i].SetItem(null); //하위슬롯 제거
                            Debug.Log("레벨1 스킬이 상성스킬으로 인해 삭제되었습니다.");
                            RockLevel1GameOver();
                        }
                    }
                    else
                    {
                        MainSubElementalCheck(); //메인/서브 속성 정해준다. //레벨업 전에
                        emptySlot.skillLevel++; //스킬 레벨업
                        Debug.Log("메인스킬 Level Up");
                        SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장
                        slots1Child[_slot1i].SetItem(null); //하위슬롯에 들어온거 제거
                        _slot1i = 0; //저장 위치초기화
                        SkillSlot1(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                    }
                }
                else //2레벨부터는 하위슬롯이 있냐 없냐를 먼저 판단해야할듯?
                {
                    if (slots1Child[_slot1i].enabled == true) //넣을공간이 있으면 실행
                    {
                        if (BadCompatibilitySlot()) //상성체크
                        {
                            if (_slot1i == 0) //슬롯에 위치가 0이면 삭제할 원소가 없다는뜻 그래서 메인스킬(2레벨인거) 레벨 다운
                            {
                                emptySlot.skillLevel--; //스킬레벨 다운 // 될때 전에 있는 스킬들 다시 가져와야해 마지막꺼 빼고
                                Debug.Log("메인스킬 Level Down");
                                SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장

                                slots1Child[_slot1i].SetItem(null); //하위슬롯에 들어온거 제거
                                SkillSlot1(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                                SkillBackUpOff();
                                RockLevel1GameOver();
                            }
                            else //그게아니다 위치가 0이 아니다 그러면 지금스킬과 전스킬 둘다 제거 
                            {
                                Debug.Log("상성스킬으로 인해 전에 저장했던 원소가 삭제되었습니다.");
                                slots1Child[_slot1i - 1].SetItem(null); //하위슬롯에 들어온 전꺼 제거
                                slots1Child[_slot1i].SetItem(null); //하위슬롯에 들어온거 제거
                                _slot1i--; //위치 전으로 옮김
                            }
                        }
                        else
                        {
                            _slot1i++; //저장위치 옮겨줌
                            Debug.Log(skill.skillName + "이 저장되었습니다.");
                        }
                    }
                    else //하위슬롯이 닫혀있으면(꽉 찼다는 말?)
                    {
                        if (BadCompatibilitySlot()) //상성체크 
                        {
                            Debug.Log("상성스킬으로 인해 전에 저장했던 원소가 삭제되었습니다.");
                            slots1Child[_slot1i - 1].SetItem(null); //하위슬롯에 들어온 전꺼 제거
                            slots1Child[_slot1i].SetItem(null); //하위슬롯에 들어온거 제거
                            _slot1i--; //위치 전으로 옮김
                        }
                        else
                        {
                            SkillBackUpOn(); //레벨업 전 빽업 슬롯을 null되기전인 현재슬롯을 저장해둔다.'
                            MainSubElementalCheck(); //메인/서브 속성 정해준다. //레벨업 전에
                            emptySlot.skillLevel++; //스킬 레벨업
                            Debug.Log("메인스킬 Level Up");

                            SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장
                            for (int i = 0; i < slots1Child.Count; i++) { slots1Child[i].SetItem(null); }//하위슬롯에 들어온거 제거
                            _slot1i = 0; //저장 위치초기화
                            SkillSlot1(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                            slots1Child[_slot1i].SetItem(skill); //레벨업 하면서 첫번째 원소에 스킬저장 
                            _slot1i++; //스킬저장후 위치 옮기기
                        }
                    }
                }
            }
            else if (_num == 1)
            {
                if (_slot2i >= 10) { return; } //슬롯에 위치가 10이상이면 리턴  

                slots2Child[_slot2i].SetItem(skill);//하위슬롯위치가 10보다 작으면 그위치에 넣어줌

                if (emptySlot.skillLevel == 1) //레벨 1때는 하위슬롯이 없는 상태
                {
                    if (BadCompatibilitySlot()) //상성체크 
                    {
                        if (slots[0].name == "slot" && slots[2].name == "slot") //2번 슬롯이 1레벨인데 1,3번 슬롯에 스킬이 없으면 상성이 안맞아도 다음꺼에 넣어줌
                        {
                            emptySlot2.SetItem(skill); //비어있는곳에 넣기
                        }
                        else
                        {
                            emptySlot.SetItem(null); //메인스킬 제거
                            slots2Child[_slot2i].SetItem(null); //하위슬롯 제거
                            Debug.Log("레벨1 스킬이 상성스킬으로 인해 삭제되었습니다.");
                            RockLevel1GameOver();
                        }
                    }
                    else
                    {
                        MainSubElementalCheck(); //메인/서브 속성 정해준다. //레벨업 전에
                        emptySlot.skillLevel++; //스킬 레벨업 
                        Debug.Log("메인스킬 Level Up");
                        SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장
                        slots2Child[_slot2i].SetItem(null); //하위슬롯에 들어온거 제거
                        _slot2i = 0; //저장 위치초기화
                        SkillSlot2(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                    }
                }
                else //2레벨부터는 하위슬롯이 있냐 없냐를 먼저 판단해야할듯?
                {
                    if (slots2Child[_slot2i].enabled == true) //넣을공간이 있으면 실행
                    {
                        if (BadCompatibilitySlot()) //상성체크
                        {
                            if (_slot2i == 0) //슬롯에 위치가 0이면 삭제할 원소가 없다는뜻 그래서 메인스킬(2레벨인거) 레벨 다운
                            {
                                emptySlot.skillLevel--; //스킬레벨 다운
                                Debug.Log("메인스킬 Level Down");
                                SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장
                                slots2Child[_slot2i].SetItem(null); //하위슬롯에 들어온거 제거
                                SkillSlot2(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                                SkillBackUpOff(); //레벨다운 시 빽업 스킬 불러온다.
                                RockLevel1GameOver();

                            }
                            else //그게아니다 위치가 0이 아니다 그러면 지금스킬과 전스킬 둘다 제거 
                            {
                                Debug.Log("상성스킬으로 인해 전에 저장했던 원소가 삭제되었습니다.");
                                slots2Child[_slot2i - 1].SetItem(null); //하위슬롯에 들어온 전꺼 제거
                                slots2Child[_slot2i].SetItem(null); //하위슬롯에 들어온거 제거
                                _slot2i--; //위치 전으로 옮김
                            }
                        }
                        else
                        {
                            _slot2i++; //저장위치 옮겨줌
                            Debug.Log(skill.skillName + "이 저장되었습니다.");
                        }
                    }
                    else //하위슬롯이 닫혀있으면(꽉 찼다는 말?)
                    {
                        if (BadCompatibilitySlot()) //상성체크 
                        {
                            Debug.Log("상성스킬으로 인해 전에 저장했던 원소가 삭제되었습니다.");
                            slots2Child[_slot2i - 1].SetItem(null); //하위슬롯에 들어온 전꺼 제거
                            slots2Child[_slot2i].SetItem(null); //하위슬롯에 들어온거 제거
                            _slot2i--; //위치 전으로 옮김
                        }
                        else
                        {
                            SkillBackUpOn(); //레벨업 전 빽업 슬롯을 null되기전인 현재슬롯을 저장해둔다.'
                            MainSubElementalCheck(); //메인/서브 속성 정해준다. //레벨업 전에
                            emptySlot.skillLevel++; //스킬 레벨업
                            Debug.Log("메인스킬 Level Up");
                            SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장
                            for (int i = 0; i < slots2Child.Count; i++) { slots2Child[i].SetItem(null); }//하위슬롯에 들어온거 제거
                            _slot2i = 0; //저장 위치초기화
                            SkillSlot2(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                            slots2Child[_slot2i].SetItem(skill); //레벨업 하면서 첫번째 원소에 스킬저장 
                            _slot2i++; //스킬저장후 위치 옮기기
                        }
                    }
                }
            }
            else if (_num == 2)
            {
                if (_slot3i >= 10) { return; } //슬롯에 위치가 10이상이면 리턴  

                slots3Child[_slot3i].SetItem(skill);//하위슬롯위치가 10보다 작으면 그위치에 넣어줌

                if (emptySlot.skillLevel == 1) //레벨 1때는 하위슬롯이 없는 상태
                {
                    if (BadCompatibilitySlot()) //상성체크 
                    {
                        if (slots[0].name == "slot" && slots[1].name == "slot") //3번 슬롯이 1레벨인데 1,2번 슬롯에 스킬이 없으면 상성이 안맞아도 다음꺼에 넣어줌
                        {
                            emptySlot2.SetItem(skill); //비어있는곳에 넣기
                        }
                        else
                        {
                            emptySlot.SetItem(null); //메인스킬 제거
                            slots3Child[_slot3i].SetItem(null); //하위슬롯 제거
                            Debug.Log("레벨1 스킬이 상성스킬으로 인해 삭제되었습니다.");
                            RockLevel1GameOver();
                        }
                    }
                    else
                    {
                        MainSubElementalCheck(); //메인/서브 속성 정해준다. //레벨업 전에
                        emptySlot.skillLevel++; //스킬 레벨업
                        Debug.Log("메인스킬 Level Up");
                        SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장
                        slots3Child[_slot3i].SetItem(null); //하위슬롯에 들어온거 제거
                        _slot3i = 0; //저장 위치초기화
                        SkillSlot3(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                    }
                }
                else //2레벨부터는 하위슬롯이 있냐 없냐를 먼저 판단해야할듯?
                {
                    if (slots3Child[_slot3i].enabled == true) //넣을공간이 있으면 실행
                    {
                        if (BadCompatibilitySlot()) //상성체크
                        {
                            if (_slot3i == 0) //슬롯에 위치가 0이면 삭제할 원소가 없다는뜻 그래서 메인스킬(2레벨인거) 레벨 다운
                            {
                                emptySlot.skillLevel--; //스킬레벨 다운 // 될때 전에 있는 스킬들 다시 가져와야해 마지막꺼 빼고
                                Debug.Log("메인스킬 Level Down");
                                SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장
                                slots3Child[_slot3i].SetItem(null); //하위슬롯에 들어온거 제거
                                SkillSlot3(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                                SkillBackUpOff();
                                RockLevel1GameOver();


                            }
                            else //그게아니다 위치가 0이 아니다 그러면 지금스킬과 전스킬 둘다 제거 
                            {
                                Debug.Log("상성스킬으로 인해 전에 저장했던 원소가 삭제되었습니다.");
                                slots3Child[_slot3i - 1].SetItem(null); //하위슬롯에 들어온 전꺼 제거
                                slots3Child[_slot3i].SetItem(null); //하위슬롯에 들어온거 제거
                                _slot3i--; //위치 전으로 옮김
                            }
                        }
                        else
                        {
                            _slot3i++; //저장위치 옮겨줌
                            Debug.Log(skill.skillName + "이 저장되었습니다.");
                        }
                    }
                    else //하위슬롯이 닫혀있으면(꽉 찼다는 말?)
                    {
                        if (BadCompatibilitySlot()) //상성체크 
                        {
                            Debug.Log("상성스킬으로 인해 전에 저장했던 원소가 삭제되었습니다.");
                            slots3Child[_slot3i - 1].SetItem(null); //하위슬롯에 들어온 전꺼 제거
                            slots3Child[_slot3i].SetItem(null); //하위슬롯에 들어온거 제거
                            _slot3i--; //위치 전으로 옮김
                        }
                        else
                        {
                            SkillBackUpOn(); //레벨업 전 빽업 슬롯을 null되기전인 현재슬롯을 저장해둔다.'
                            MainSubElementalCheck(); //메인/서브 속성 정해준다. //레벨업 전에
                            emptySlot.skillLevel++; //스킬 레벨업
                            Debug.Log("메인스킬 Level Up");
                            SlotLevelSkill(); //스킬 레벨에 따른 스크립트 저장
                            for (int i = 0; i < slots3Child.Count; i++) { slots3Child[i].SetItem(null); }//하위슬롯에 들어온거 제거
                            _slot3i = 0; //저장 위치초기화
                            SkillSlot3(); //스킬 레벨업을 하면서 하위슬롯 열고,닫고
                            slots3Child[_slot3i].SetItem(skill); //레벨업 하면서 첫번째 원소에 스킬저장 
                            _slot3i++; //스킬저장후 위치 옮기기
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        //-----------------------------------------------

    }
    private void Update() //fixed로 하면 timeScale = 0; 으로 상점 열릴때 안통한다
    {
        SkillChoice();
    }
    //스킬 저장 위치 함수
    void SkillChoice()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _num = 0;
            checkSlotImage.position = slots[0].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _num = 1;
            checkSlotImage.position = slots[1].transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _num = 2;
            checkSlotImage.position = slots[2].transform.position;
        }
    } 

    //레벨에 따른 하위슬롯들 열고/닫고 
    void SkillSlot1()
    {
        switch (slots[0].skillLevel)
        {
            case 0:
            case 1: //1레벨이 되었을때는 메인스킬이 들어왔을때랑 감소했을때
                for (int i = _level1; i < _level2; i++) //들어온거는 어차피 false인 상태라서 감소시에만 넣어줌
                {
                    slots1ChildGo[i].SetActive(false);
                    slots1Child[i].enabled = false;
                }
                break;
            case 2: //2레벨이 되었을때는 1레벨에서 스킬레벨업이나 아니면 3레벨에서 감소했을 때
                for (int i = _level1; i < _level2; i++)
                {
                    slots1ChildGo[i].SetActive(true);
                    slots1Child[i].enabled = true;
                }
                for (int i = _level2; i < _level3; i++) //레벨이 감소했을때 그만큼 하위슬롯들 없애줘야함
                {
                    slots1ChildGo[i].SetActive(false);
                    slots1Child[i].enabled = false;
                }
                break;
            case 3:
                for (int i = _level1; i < _level3; i++)
                {
                    slots1ChildGo[i].SetActive(true);
                    slots1Child[i].enabled = true;
                }
                for (int i = _level3; i < _level4; i++) //레벨이 감소했을때 그만큼 하위슬롯들 없애줘야함
                {
                    slots1ChildGo[i].SetActive(false);
                    slots1Child[i].enabled = false;
                }
                break;
            case 4:
                for (int i = _level1; i < _level4; i++)
                {
                    slots1ChildGo[i].SetActive(true);
                    slots1Child[i].enabled = true;
                }
                break;
        }
    }
    void SkillSlot2()
    {
        switch (slots[1].skillLevel)
        {
            case 0:
            case 1: //1레벨이 되었을때는 메인스킬이 들어왔을때랑 감소했을때
                for (int i = _level1; i < _level2; i++) //들어온거는 어차피 false인 상태라서 감소시에만 넣어줌
                {
                    slots2ChildGo[i].SetActive(false);
                    slots2Child[i].enabled = false;
                }
                break;
            case 2: //2레벨이 되었을때는 1레벨에서 스킬레벨업이나 아니면 3레벨에서 감소했을 때
                for (int i = _level1; i < _level2; i++)
                {
                    slots2ChildGo[i].SetActive(true);
                    slots2Child[i].enabled = true;
                }
                for (int i = _level2; i < _level3; i++) //레벨이 감소했을때 그만큼 하위슬롯들 없애줘야함
                {
                    slots2ChildGo[i].SetActive(false);
                    slots2Child[i].enabled = false;
                }
                break;
            case 3:
                for (int i = _level1; i < _level3; i++)
                {
                    slots2ChildGo[i].SetActive(true);
                    slots2Child[i].enabled = true;
                }
                for (int i = _level3; i < _level4; i++) //레벨이 감소했을때 그만큼 하위슬롯들 없애줘야함
                {
                    slots2ChildGo[i].SetActive(false);
                    slots2Child[i].enabled = false;
                }
                break;
            case 4:
                for (int i = _level1; i < _level4; i++)
                {
                    slots2ChildGo[i].SetActive(true);
                    slots2Child[i].enabled = true;
                }
                break;
        }
    }
    void SkillSlot3()
    {
        switch (slots[2].skillLevel)
        {
            case 0:
            case 1:
                for (int i = 0; i < slots3Child.Count; i++)
                {
                    slots3ChildGo[i].SetActive(false);
                    slots3Child[i].enabled = false;
                }
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    slots3ChildGo[i].SetActive(true);
                    slots3Child[i].enabled = true;
                }
                for (int i = 3; i < 5; i++) //레벨이 감소했을때 그만큼 하위슬롯들 없애줘야함
                {
                    slots3ChildGo[i].SetActive(false);
                    slots3Child[i].enabled = false;
                }
                break;
            case 3:
                for (int i = 0; i < 5; i++)
                {
                    slots3ChildGo[i].SetActive(true);
                    slots3Child[i].enabled = true;
                }
                for (int i = 5; i < 10; i++) //레벨이 감소했을때 그만큼 하위슬롯들 없애줘야함
                {
                    slots3ChildGo[i].SetActive(false);
                    slots3Child[i].enabled = false;
                }
                break;
            case 4:
                for (int i = 0; i < slots1Child.Count; i++)
                {
                    slots3ChildGo[i].SetActive(true);
                    slots3Child[i].enabled = true;
                }
                break;
        }
    }

    void SlotLevelSkill() //스킬레벨과 메인 슬롯 + 서브슬롯에 따라 스크립트가 바뀌는 로직
    {
        if (slots[_num].skillLevel == 1)
        {
            switch (slots[_num].skillElemental)
            {
                case "Fire":
                    slots[_num].SetItem(store.skillData.skillLevel1DataList[1]);
                    break;
                case "Water":
                    slots[_num].SetItem(store.skillData.skillLevel1DataList[0]);
                    break;
                case "Lightning":
                    slots[_num].SetItem(store.skillData.skillLevel1DataList[2]);
                    break;
                case "Rock":
                    slots[_num].SetItem(store.skillData.skillLevel1DataList[3]);
                    break;
            }
        }
        else if (slots[_num].skillLevel == 2) //2레벨 스킬저장
        {
            switch (_num)
            {
                case 0:
                    switch (_slot1MainElemental)
                    {
                        case "Fire":
                            if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[0]); } //불+불 저장
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[1]); }//불+번개 저장
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[2]); } //불+땅 저장
                            break;
                        case "Water":
                            if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[3]); }//물+물 저장
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[4]); }//물+불 저장
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[6]); }//번개+번개 저장
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[7]); }//번개+땅 저장
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[8]); }//번개+물 저장
                            break;
                        case "Rock":
                            if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[9]); }//땅+땅 저장
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[10]); }//땅+불 저장
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[11]); }//땅+물 저장
                            break;
                    }
                    break;
                case 1:
                    switch (_slot2MainElemental)
                    {
                        case "Fire":
                            if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[0]); }//불+불 저장
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[1]); }//불+번개 저장
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[2]); }//불+땅 저장
                            break;
                        case "Water":
                            if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[3]); }//물+물 저장
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[4]); }//물+불 저장
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[6]); }//번개+번개 저장
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[7]); }//번개+땅 저장
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[8]); }//번개+물 저장
                            break;
                        case "Rock":
                            if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[9]); }//땅+땅 저장
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[10]); }//땅+불 저장
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[11]); }//땅+물 저장
                            break;
                    }
                    break;
                case 2:
                    switch (_slot3MainElemental)
                    {
                        case "Fire":
                            if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[0]); }//불+불 저장
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[1]); } //불+번개 저장
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[2]); }//불+땅 저장
                            break;
                        case "Water":
                            if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[3]); } //물+물 저장
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[4]); } //물+불 저장
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[6]); }//번개+번개 저장
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[7]); }//번개+땅 저장
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[8]); }//번개+물 저장
                            break;
                        case "Rock":
                            if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[9]); }//땅+땅 저장
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[10]); }//땅+불 저장
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[11]); } //땅+물 저장
                            break;
                    }
                    break;
            }

        }
        else if (slots[_num].skillLevel == 3) //3레벨 스킬저장
        {
            switch (_num)
            {
                case 0:
                    switch (_slot1MainElemental)
                    {
                        case "Fire":
                            if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[0]); }//불+불 저장
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[1]); }//불+번개 저장
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[2]); }//불+땅 저장
                            break;
                        case "Water":
                            if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[3]); }//물+물 저장
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[4]); }//물+불 저장
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[6]); }//번개+번개 저장
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[7]); }//번개+땅 저장
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[8]); }//번개+물 저장
                            break;
                        case "Rock":
                            if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[9]); }//땅+땅 저장
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[10]); }//땅+불 저장
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[11]); }//땅+물 저장
                            break;
                    }
                    break;
                case 1:
                    switch (_slot2MainElemental)
                    {
                        case "Fire":
                            if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[0]); }//불+불 저장
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[1]); }//불+번개 저장
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[2]); }//불+땅 저장
                            break;
                        case "Water":
                            if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[3]); }//물+물 저장
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[4]); } //물+불 저장
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[6]); } //번개+번개 저장
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[7]); }//번개+땅 저장
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[8]); }//번개+물 저장
                            break;
                        case "Rock":
                            if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[9]); }//땅+땅 저장
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[10]); }//땅+불 저장
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[11]); }//땅+물 저장
                            break;
                    }
                    break;
                case 2:
                    switch (_slot3MainElemental)
                    {
                        case "Fire":
                            if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[0]); } //불+불 저장
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[1]); }//불+번개 저장
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[2]); } //불+땅 저장
                            break;
                        case "Water":
                            if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[3]); }//물+물 저장
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[4]); }//물+불 저장
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[6]); }//번개+번개 저장
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[7]); }//번개+땅 저장
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[8]); } //번개+물 저장
                            break;
                        case "Rock":
                            if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[9]); }//땅+땅 저장
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[10]); }//땅+불 저장
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[11]); }//땅+물 저장
                            break;
                    }
                    break;
            }

        }
        else if (slots[_num].skillLevel == 4) //4레벨 스킬저장
        {
            switch (_num)
            {
                case 0:
                    switch (_slot1MainElemental)
                    {
                        case "Fire":
                            if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[0]); } //불+불 저장
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[1]); }//불+번개 저장
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[2]); }//불+땅 저장
                            break;
                        case "Water":
                            if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[3]); }//물+물 저장
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[4]); }//물+불 저장
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[6]); } //번개+번개 저장
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[7]); }//번개+땅 저장
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[8]); }//번개+물 저장
                            break;
                        case "Rock":
                            if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[9]); }//땅+땅 저장
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[10]); }//땅+불 저장
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[11]); }//땅+물 저장
                            break;
                    }
                    break;
                case 1:
                    switch (_slot2MainElemental)
                    {
                        case "Fire":
                            if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[0]); }//불+불 저장
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[1]); } //불+번개 저장
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[2]); } //불+땅 저장
                            break;
                        case "Water":
                            if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[3]); }//물+물 저장
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[4]); } //물+불 저장
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[6]); }//번개+번개 저장
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[7]); }//번개+땅 저장
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[8]); }//번개+물 저장
                            break;
                        case "Rock":
                            if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[9]); }//땅+땅 저장
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[10]); }//땅+불 저장
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[11]); }//땅+물 저장
                            break;
                    }
                    break;
                case 2:
                    switch (_slot3MainElemental)
                    {
                        case "Fire":
                            if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[0]); }//불+불 저장
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[1]); }//불+번개 저장
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[2]); }//불+땅 저장
                            break;
                        case "Water":
                            if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[3]); }//물+물 저장
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[4]); }//물+불 저장
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[5]); }//물+번개 저장
                            break;
                        case "Lightning":
                            if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[6]); }//번개+번개 저장
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[7]); } //번개+땅 저장
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[8]); }//번개+물 저장
                            break;
                        case "Rock":
                            if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[9]); }//땅+땅 저장
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[10]); }//땅+불 저장
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[11]); }//땅+물 저장
                            break;
                    }
                    break;
            }

        }

    }
    bool BadCompatibilitySlot()//상성체크 로직
    {
        bool compatibility = false;
        switch (_num)
        {
            case 0:
                switch (slots[_num].skillElemental)
                {
                    case "Fire":
                        if (slots1Child[_slot1i].skillElemental == "Water") { compatibility = true; }
                        break;
                    case "Water":
                        if (slots1Child[_slot1i].skillElemental == "Rock") { compatibility = true; }
                        break;
                    case "Rock":
                        if (slots1Child[_slot1i].skillElemental == "Lightning") { compatibility = true; }
                        break;
                    case "Lightning":
                        if (slots1Child[_slot1i].skillElemental == "Fire") { compatibility = true; }
                        break;
                }
                break;
            case 1:
                switch (slots[_num].skillElemental)
                {
                    case "Fire":
                        if (slots2Child[_slot2i].skillElemental == "Water") { compatibility = true; }
                        break;
                    case "Water":
                        if (slots2Child[_slot2i].skillElemental == "Rock") { compatibility = true; }
                        break;
                    case "Rock":
                        if (slots2Child[_slot2i].skillElemental == "Lightning") { compatibility = true; }
                        break;
                    case "Lightning":
                        if (slots2Child[_slot2i].skillElemental == "Fire") { compatibility = true; }
                        break;
                }
                break;
            case 2:
                switch (slots[_num].skillElemental)
                {
                    case "Fire":
                        if (slots3Child[_slot3i].skillElemental == "Water") { compatibility = true; }
                        break;
                    case "Water":
                        if (slots3Child[_slot3i].skillElemental == "Rock") { compatibility = true; }
                        break;
                    case "Rock":
                        if (slots3Child[_slot3i].skillElemental == "Lightning") { compatibility = true; }
                        break;
                    case "Lightning":
                        if (slots3Child[_slot3i].skillElemental == "Fire") { compatibility = true; }
                        break;
                }
                break;
        }

        return compatibility;
    } //상성안맞으면 스킬제거
    void SkillBackUpOn() //스킬을 빽업슬롯에 저장해둔다. 
    {
        switch (_num)
        {
            case 0:
                if (slots[_num].skillLevel == 2)
                {
                    for (int i = 0; i < _level2 - 1; i++)
                    {
                        slots1ChildBack[i].gameObject.name = slots1Child[i].gameObject.name;
                        slots1ChildBack[i].image.sprite = slots1Child[i].image.sprite;
                        slots1ChildBack[i].skillLevel = slots1Child[i].skillLevel;
                        slots1ChildBack[i].skillElemental = slots1Child[i].skillElemental;
                        _slot1i++;
                    }
                }
                else if (slots[_num].skillLevel == 3)
                {
                    for (int i = 0; i < _level3 - 1; i++)
                    {
                        slots1ChildBack[i].gameObject.name = slots1Child[i].gameObject.name;
                        slots1ChildBack[i].image.sprite = slots1Child[i].image.sprite;
                        slots1ChildBack[i].skillLevel = slots1Child[i].skillLevel;
                        slots1ChildBack[i].skillElemental = slots1Child[i].skillElemental;
                        slots1Child[i].image.enabled = true;
                        _slot1i++;
                    }
                }
                break;
            case 1:
                if (slots[_num].skillLevel == 2)
                {
                    for (int i = 0; i < _level2 - 1; i++)
                    {
                        slots2ChildBack[i].gameObject.name = slots2Child[i].gameObject.name;
                        slots2ChildBack[i].image.sprite = slots2Child[i].image.sprite;
                        slots2ChildBack[i].skillLevel = slots2Child[i].skillLevel;
                        slots2ChildBack[i].skillElemental = slots2Child[i].skillElemental;
                        _slot2i++;
                    }
                }
                else if (slots[_num].skillLevel == 3)
                {
                    for (int i = 0; i < _level3 - 1; i++)
                    {
                        slots2ChildBack[i].gameObject.name = slots2Child[i].gameObject.name;
                        slots2ChildBack[i].image.sprite = slots2Child[i].image.sprite;
                        slots2ChildBack[i].skillLevel = slots2Child[i].skillLevel;
                        slots2ChildBack[i].skillElemental = slots2Child[i].skillElemental;
                        slots2Child[i].image.enabled = true;
                        _slot2i++;
                    }
                }
                break;
            case 2:
                if (slots[_num].skillLevel == 2)
                {
                    for (int i = 0; i < _level2 - 1; i++)
                    {
                        slots3ChildBack[i].gameObject.name = slots3Child[i].gameObject.name;
                        slots3ChildBack[i].image.sprite = slots3Child[i].image.sprite;
                        slots3ChildBack[i].skillLevel = slots3Child[i].skillLevel;
                        slots3ChildBack[i].skillElemental = slots3Child[i].skillElemental;
                        _slot3i++;
                    }
                }
                else if (slots[_num].skillLevel == 3)
                {
                    for (int i = 0; i < _level3 - 1; i++)
                    {
                        slots3ChildBack[i].gameObject.name = slots3Child[i].gameObject.name;
                        slots3ChildBack[i].image.sprite = slots3Child[i].image.sprite;
                        slots3ChildBack[i].skillLevel = slots3Child[i].skillLevel;
                        slots3ChildBack[i].skillElemental = slots3Child[i].skillElemental;
                        slots3Child[i].image.enabled = true;
                        _slot3i++;
                    }
                }
                break;
        }

    } //스킬레벨업 전에 빽업
    void SkillBackUpOff()
    {
        switch (_num)
        {
            case 0:
                if (slots[_num].skillLevel == 2)
                {
                    for (int i = 0; i < _level2 - 1; i++)
                    {
                        slots1Child[i].gameObject.name = slots1ChildBack[i].gameObject.name;
                        slots1Child[i].image.sprite = slots1ChildBack[i].image.sprite;
                        slots1Child[i].skillLevel = slots1ChildBack[i].skillLevel;
                        slots1Child[i].skillElemental = slots1ChildBack[i].skillElemental;
                        slots1Child[i].image.enabled = true;
                        _slot1i++;
                    }
                }
                else if (slots[_num].skillLevel == 3)
                {
                    for (int i = 0; i < _level3 - 1; i++)
                    {
                        slots1Child[i].gameObject.name = slots1ChildBack[i].gameObject.name;
                        slots1Child[i].image.sprite = slots1ChildBack[i].image.sprite;
                        slots1Child[i].skillLevel = slots1ChildBack[i].skillLevel;
                        slots1Child[i].skillElemental = slots1ChildBack[i].skillElemental;
                        slots1Child[i].image.enabled = true;
                        _slot1i++;
                    }
                }
                break;
            case 1:
                if (slots[_num].skillLevel == 2)
                {
                    for (int i = 0; i < _level2 - 1; i++)
                    {
                        slots2Child[i].gameObject.name = slots2ChildBack[i].gameObject.name;
                        slots2Child[i].image.sprite = slots2ChildBack[i].image.sprite;
                        slots2Child[i].skillLevel = slots2ChildBack[i].skillLevel;
                        slots2Child[i].skillElemental = slots2ChildBack[i].skillElemental;
                        slots2Child[i].image.enabled = true;
                        _slot2i++;
                    }
                }
                else if (slots[_num].skillLevel == 3)
                {
                    for (int i = 0; i < _level3 - 1; i++)
                    {
                        slots2Child[i].gameObject.name = slots2ChildBack[i].gameObject.name;
                        slots2Child[i].image.sprite = slots2ChildBack[i].image.sprite;
                        slots2Child[i].skillLevel = slots2ChildBack[i].skillLevel;
                        slots2Child[i].skillElemental = slots2ChildBack[i].skillElemental;
                        slots2Child[i].image.enabled = true;
                        _slot2i++;
                    }
                }
                break;
            case 2:
                if (slots[_num].skillLevel == 2)
                {
                    for (int i = 0; i < _level2 - 1; i++)
                    {
                        slots3Child[i].gameObject.name = slots3ChildBack[i].gameObject.name;
                        slots3Child[i].image.sprite = slots3ChildBack[i].image.sprite;
                        slots3Child[i].skillLevel = slots3ChildBack[i].skillLevel;
                        slots3Child[i].skillElemental = slots3ChildBack[i].skillElemental;
                        slots3Child[i].image.enabled = true;
                        _slot3i++;
                    }
                }
                else if (slots[_num].skillLevel == 3)
                {
                    for (int i = 0; i < _level3 - 1; i++)
                    {
                        slots3Child[i].gameObject.name = slots3ChildBack[i].gameObject.name;
                        slots3Child[i].image.sprite = slots3ChildBack[i].image.sprite;
                        slots3Child[i].skillLevel = slots3ChildBack[i].skillLevel;
                        slots3Child[i].skillElemental = slots3ChildBack[i].skillElemental;
                        slots3Child[i].image.enabled = true;
                        _slot3i++;
                    }
                }
                break;
        }

    } //빽업슬롯에 있는걸 현재 하위원소에 저장 //레벨 다운시 뺵업해둔 것 옮겨주기


    void MainSubElementalCheck()//메인/서브 속성 정해준다. //상성안맞는게 나오면 다시돌림.
    {

        _mainRan = Random.Range(0, 10);
        _subRan = Random.Range(0, 10);
        MainElementalPercentage(_mainRan); //메인 속성 정해주기
        SubElementalPercentage(_subRan); //서브 속성 정해주기
        for (int i = 0; i < 9999; i++) //(상성이 안맞으면)중복되면 계속 돌려줘라
        {
            if (BadElementalCheck())
            {
                _mainRan = Random.Range(0, 10);
                _subRan = Random.Range(0, 10);
                MainElementalPercentage(_mainRan); //메인 속성 정해주기
                SubElementalPercentage(_subRan); //서브 속성 정해주기
            }
            else
            {
                break;
            }
        }

    }
    bool BadElementalCheck() //메인/서브 속성이 안맞는게 있는지 체크
    {
        bool bad = false;
        if (_num == 0)
        {
            switch (_slot1MainElemental)
            {
                case "Fire":
                    if (_slot1SubElemental == "Water") { bad = true; }
                    break;
                case "Water":
                    if (_slot1SubElemental == "Rock") { bad = true; }
                    break;
                case "Lightning":
                    if (_slot1SubElemental == "Fire") { bad = true; }
                    break;
                case "Rock":
                    if (_slot1SubElemental == "Lightning") { bad = true; }
                    break;
            }
        }
        else if (_num == 1)
        {
            switch (_slot2MainElemental)
            {
                case "Fire":
                    if (_slot2SubElemental == "Water") { bad = true; }
                    break;
                case "Water":
                    if (_slot2SubElemental == "Rock") { bad = true; }
                    break;
                case "Lightning":
                    if (_slot2SubElemental == "Fire") { bad = true; }
                    break;
                case "Rock":
                    if (_slot2SubElemental == "Lightning") { bad = true; }
                    break;
            }
        }
        else if (_num == 2)
        {
            switch (_slot3MainElemental)
            {
                case "Fire":
                    if (_slot3SubElemental == "Water") { bad = true; }
                    break;
                case "Water":
                    if (_slot3SubElemental == "Rock") { bad = true; }
                    break;
                case "Lightning":
                    if (_slot3SubElemental == "Fire") { bad = true; }
                    break;
                case "Rock":
                    if (_slot3SubElemental == "Lightning") { bad = true; }
                    break;
            }
        }
        return bad;
    }
    void MainElementalPercentage(int ran)
    {
        switch (_num)
        {
            case 0:
                if (slots[0].skillLevel == 1)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot1MainElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            _slot1MainElemental = slots1Child[0].skillElemental;
                            break;
                    }
                }
                else if (slots[0].skillLevel == 2)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            _slot1MainElemental = slots[_num].skillElemental;
                            break;
                        case 4:
                        case 5:
                            _slot1MainElemental = slots1Child[0].skillElemental;
                            break;
                        case 6:
                        case 7:
                            _slot1MainElemental = slots1Child[1].skillElemental;
                            break;
                        case 8:
                        case 9:
                            _slot1MainElemental = slots1Child[2].skillElemental;
                            break;
                    }
                }
                else if (slots[0].skillLevel == 3)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot1MainElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                            _slot1MainElemental = slots1Child[0].skillElemental;
                            break;
                        case 6:
                            _slot1MainElemental = slots1Child[1].skillElemental;
                            break;
                        case 7:
                            _slot1MainElemental = slots1Child[2].skillElemental;
                            break;
                        case 8:
                            _slot1MainElemental = slots1Child[3].skillElemental;
                            break;
                        case 9:
                            _slot1MainElemental = slots1Child[4].skillElemental;
                            break;
                    }
                }
                break;
            case 1:
                if (slots[1].skillLevel == 1)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot2MainElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            _slot2MainElemental = slots2Child[0].skillElemental;
                            break;
                    }
                }
                else if (slots[1].skillLevel == 2)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            _slot2MainElemental = slots[_num].skillElemental;
                            break;
                        case 4:
                        case 5:
                            _slot2MainElemental = slots2Child[0].skillElemental;
                            break;
                        case 6:
                        case 7:
                            _slot2MainElemental = slots2Child[1].skillElemental;
                            break;
                        case 8:
                        case 9:
                            _slot2MainElemental = slots2Child[2].skillElemental;
                            break;
                    }
                }
                else if (slots[1].skillLevel == 3)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot2MainElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                            _slot2MainElemental = slots2Child[0].skillElemental;
                            break;
                        case 6:
                            _slot2MainElemental = slots2Child[1].skillElemental;
                            break;
                        case 7:
                            _slot2MainElemental = slots2Child[2].skillElemental;
                            break;
                        case 8:
                            _slot2MainElemental = slots2Child[3].skillElemental;
                            break;
                        case 9:
                            _slot2MainElemental = slots2Child[4].skillElemental;
                            break;
                    }
                }
                break;
            case 2:
                if (slots[2].skillLevel == 1)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot3MainElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            _slot3MainElemental = slots3Child[0].skillElemental;
                            break;
                    }
                }
                else if (slots[2].skillLevel == 2)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            _slot3MainElemental = slots[_num].skillElemental;
                            break;
                        case 4:
                        case 5:
                            _slot3MainElemental = slots3Child[0].skillElemental;
                            break;
                        case 6:
                        case 7:
                            _slot3MainElemental = slots3Child[1].skillElemental;
                            break;
                        case 8:
                        case 9:
                            _slot3MainElemental = slots3Child[2].skillElemental;
                            break;
                    }
                }
                else if (slots[2].skillLevel == 3)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot3MainElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                            _slot3MainElemental = slots3Child[0].skillElemental;
                            break;
                        case 6:
                            _slot3MainElemental = slots3Child[1].skillElemental;
                            break;
                        case 7:
                            _slot3MainElemental = slots3Child[2].skillElemental;
                            break;
                        case 8:
                            _slot3MainElemental = slots3Child[3].skillElemental;
                            break;
                        case 9:
                            _slot3MainElemental = slots3Child[4].skillElemental;
                            break;
                    }
                }
                break;
        }


    } //공용  //레벨이 몇일때 메인속성확률 
    void SubElementalPercentage(int ran)
    {
        switch (_num)
        {
            case 0:
                if (slots[0].skillLevel == 1)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot1SubElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            _slot1SubElemental = slots1Child[0].skillElemental;
                            break;
                    }
                }
                else if (slots[0].skillLevel == 2)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            _slot1SubElemental = slots[_num].skillElemental;
                            break;
                        case 4:
                        case 5:
                            _slot1SubElemental = slots1Child[0].skillElemental;
                            break;
                        case 6:
                        case 7:
                            _slot1SubElemental = slots1Child[1].skillElemental;
                            break;
                        case 8:
                        case 9:
                            _slot1SubElemental = slots1Child[2].skillElemental;
                            break;
                    }
                }
                else if (slots[0].skillLevel == 3)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot1SubElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                            _slot1SubElemental = slots1Child[0].skillElemental;
                            break;
                        case 6:
                            _slot1SubElemental = slots1Child[1].skillElemental;
                            break;
                        case 7:
                            _slot1SubElemental = slots1Child[2].skillElemental;
                            break;
                        case 8:
                            _slot1SubElemental = slots1Child[3].skillElemental;
                            break;
                        case 9:
                            _slot1SubElemental = slots1Child[4].skillElemental;
                            break;
                    }
                }
                break;
            case 1:
                if (slots[1].skillLevel == 1)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot2SubElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            _slot2SubElemental = slots2Child[0].skillElemental;
                            break;
                    }
                }
                else if (slots[1].skillLevel == 2)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            _slot2SubElemental = slots[_num].skillElemental;
                            break;
                        case 4:
                        case 5:
                            _slot2SubElemental = slots2Child[0].skillElemental;
                            break;
                        case 6:
                        case 7:
                            _slot2SubElemental = slots2Child[1].skillElemental;
                            break;
                        case 8:
                        case 9:
                            _slot2SubElemental = slots2Child[2].skillElemental;
                            break;
                    }
                }
                else if (slots[1].skillLevel == 3)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot2SubElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                            _slot2SubElemental = slots2Child[0].skillElemental;
                            break;
                        case 6:
                            _slot2SubElemental = slots2Child[1].skillElemental;
                            break;
                        case 7:
                            _slot2SubElemental = slots2Child[2].skillElemental;
                            break;
                        case 8:
                            _slot2SubElemental = slots2Child[3].skillElemental;
                            break;
                        case 9:
                            _slot2SubElemental = slots2Child[4].skillElemental;
                            break;
                    }
                }
                break;
            case 2:
                if (slots[2].skillLevel == 1)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot3SubElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            _slot3SubElemental = slots3Child[0].skillElemental;
                            break;
                    }
                }
                else if (slots[2].skillLevel == 2)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            _slot3SubElemental = slots[_num].skillElemental;
                            break;
                        case 4:
                        case 5:
                            _slot3SubElemental = slots3Child[0].skillElemental;
                            break;
                        case 6:
                        case 7:
                            _slot3SubElemental = slots3Child[1].skillElemental;
                            break;
                        case 8:
                        case 9:
                            _slot3SubElemental = slots3Child[2].skillElemental;
                            break;
                    }
                }
                else if (slots[2].skillLevel == 3)
                {
                    switch (ran)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            _slot3SubElemental = slots[_num].skillElemental;
                            break;
                        case 5:
                            _slot3SubElemental = slots3Child[0].skillElemental;
                            break;
                        case 6:
                            _slot3SubElemental = slots3Child[1].skillElemental;
                            break;
                        case 7:
                            _slot3SubElemental = slots3Child[2].skillElemental;
                            break;
                        case 8:
                            _slot3SubElemental = slots3Child[3].skillElemental;
                            break;
                        case 9:
                            _slot3SubElemental = slots3Child[4].skillElemental;
                            break;
                    }
                }
                break;
        }
    } //공용  //레벨이 몇일때 서브속성확률 
    void RockLevel1GameOver()
    {
        if (slots[0].name == "RockLevel1" && slots[1].name == "slot" && slots[2].name == "slot")
        {
            GameManager.instance.player.IsLive = false;

        }
        else if (slots[0].name == "slot" && slots[1].name == "RockLevel1" && slots[2].name == "slot")
        {
            GameManager.instance.player.IsLive = false;
        }
        else if (slots[0].name == "slot" && slots[1].name == "slot" && slots[2].name == "RockLevel1")
        {
            GameManager.instance.player.IsLive = false;
        }

    } //1레벨 땅만 있으면 게임오버
}