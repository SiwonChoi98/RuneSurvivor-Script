using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //������ �ʿ� ���� ����
    private int _level1 = 0;
    private int _level2 = 3;
    private int _level3 = 5;
    private int _level4 = 10;

    [Header("�ӽ� ������� ����ǥ��")]
    [SerializeField] private Transform checkSlotImage; //������� ���õǾ��ٴ� ǥ��
    //---------------------�ӽ�---------------
    [SerializeField] private Transform slot1ChildRoot; //����1 ����
    [SerializeField] private Transform slot2ChildRoot; //����2 ����
    [SerializeField] private Transform slot3ChildRoot; //����3 ����

    [Header("��ų(�ڽ�)���� ����Ʈ")]
    public List<Slot> slots1Child;
    public List<Slot> slots2Child;
    public List<Slot> slots3Child;
    [Header("��ų(����) �̹��� ������Ʈ")]
    [SerializeField] private GameObject[] slots1ChildGo; //���ӿ�����Ʈ setActive�� �ʿ�
    [SerializeField] private GameObject[] slots2ChildGo; //���ӿ�����Ʈ
    [SerializeField] private GameObject[] slots3ChildGo; //���ӿ�����Ʈ

    //���꽽�� ������ġ
    private int _slot1i;
    private int _slot2i;
    private int _slot3i;


    [SerializeField] private Transform slot1ChildRootBack; //����1 ���껪��
    [SerializeField] private Transform slot2ChildRootBack; //����2 ���껪��
    [SerializeField] private Transform slot3ChildRootBack; //����3 ���껪��

    [Header("��ų(����)���� ���� ����Ʈ")]
    [SerializeField] private List<Slot> slots1ChildBack;
    [SerializeField] private List<Slot> slots2ChildBack;
    [SerializeField] private List<Slot> slots3ChildBack;
    //--------------------------------

    [SerializeField] private Transform slotRoot; //���� �Ѵ�� �ϴ� �׷�

    public Store store;

    [Header("��ų(����)���� ����Ʈ")]
    public List<Slot> slots;
    [Header("���� �� ����,���� �Ӽ�")]
    private string _slot1MainElemental;
    private string _slot1SubElemental;
    private string _slot2MainElemental;
    private string _slot2SubElemental;
    private string _slot3MainElemental;
    private string _slot3SubElemental;
    //���ν��� ������ġ
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

        for (int i = 0; i < slotCnt; i++) //���ν���
        {
            Slot slot = slotRoot.GetChild(i).GetComponent<Slot>(); //slot ��ũ��Ʈ�� �پ� �ִ°� �� ã���ִ°ž�
            slots.Add(slot); //������ ������ (3��) �ľ��� ������ slots��� ����Ʈ�� slot�� �߰����ٰž�
        }
        //---------------------------test
        for (int i = 0; i < slot1ChildCnt - 3; i++) //���꽽��
        {
            Slot slot = slot1ChildRoot.GetChild(i).GetComponent<Slot>();
            slots1Child.Add(slot);
            Slot slot2 = slot2ChildRoot.GetChild(i).GetComponent<Slot>();
            slots2Child.Add(slot2);
            Slot slot3 = slot3ChildRoot.GetChild(i).GetComponent<Slot>();
            slots3Child.Add(slot3);
        }
        for (int i = 0; i < 4; i++) //���� ���� 4����
        {
            Slot slot = slot1ChildRootBack.GetChild(i).GetComponent<Slot>();
            slots1ChildBack.Add(slot);
            Slot slot2 = slot2ChildRootBack.GetChild(i).GetComponent<Slot>();
            slots2ChildBack.Add(slot2);
            Slot slot3 = slot3ChildRootBack.GetChild(i).GetComponent<Slot>();
            slots3ChildBack.Add(slot3);
        }
        //--------------------------------
        store.onSlotClick += BuyItem; //������ Ŭ���ϸ� buyitem �Լ����� ���ļ� ���������

    }
    void BuyItem(SkillProperty skill)
    {
        //Ŭ���� ���� ���� �ε��� ���� �κ��丮�� �־��ٰž�
        var emptySlot2 = slots.Find(t => //find�� 0�� �ε������� �ȴ´�. ���� �����Ǹ� ��ȯ��Ų��. �ε����� �������� �켱������ ������
        {
            return t.skill == null || t.skill.skillName == string.Empty; //skill�� null �̰ų� skull�� �̸��� empty�� ����
            //�̰Ŷ� �����ǹ�
            /*
            if(t.skill == null || t.skill.skillName == string.Empty){
                return true;
            }
            */
        });
        Slot emptySlot = slots[_num];


        if (emptySlot.name == "slot") //1. ó�� ���� ���Կ� ��ų ä����
        {
            GameManager.instance.uiManager.PanelFadeIn(GameManager.instance.uiManager.skillImageFadeGroup[_num]); //ȿ��
            emptySlot.SetItem(skill);
            Debug.Log(skill.skillName + "�� ����Ǿ����ϴ�.");
        }
        else //2. ���ν��Կ� ��ų�� ������
        {
            if (_num == 0)
            {
                if (_slot1i >= 10) { return; } //���Կ� ��ġ�� 10�̻��̸� ����  

                slots1Child[_slot1i].SetItem(skill);//����������ġ�� 10���� ������ ����ġ�� �־���

                if (emptySlot.skillLevel == 1) //���� 1���� ���������� ���� ����
                {
                    if (BadCompatibilitySlot()) //��üũ 
                    {
                        if (slots[1].name == "slot" && slots[2].name == "slot") //0�� ������ 1�����ε� 2,3�� ���Կ� ��ų�� ������ ���� �ȸ¾Ƶ� �������� �־���
                        {
                            emptySlot2.SetItem(skill); //����ִ°��� �ֱ�
                        }
                        else
                        {
                            emptySlot.SetItem(null); //���ν�ų ����
                            slots1Child[_slot1i].SetItem(null); //�������� ����
                            Debug.Log("����1 ��ų�� �󼺽�ų���� ���� �����Ǿ����ϴ�.");
                            RockLevel1GameOver();
                        }
                    }
                    else
                    {
                        MainSubElementalCheck(); //����/���� �Ӽ� �����ش�. //������ ����
                        emptySlot.skillLevel++; //��ų ������
                        Debug.Log("���ν�ų Level Up");
                        SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����
                        slots1Child[_slot1i].SetItem(null); //�������Կ� ���°� ����
                        _slot1i = 0; //���� ��ġ�ʱ�ȭ
                        SkillSlot1(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                    }
                }
                else //2�������ʹ� ���������� �ֳ� ���ĸ� ���� �Ǵ��ؾ��ҵ�?
                {
                    if (slots1Child[_slot1i].enabled == true) //���������� ������ ����
                    {
                        if (BadCompatibilitySlot()) //��üũ
                        {
                            if (_slot1i == 0) //���Կ� ��ġ�� 0�̸� ������ ���Ұ� ���ٴ¶� �׷��� ���ν�ų(2�����ΰ�) ���� �ٿ�
                            {
                                emptySlot.skillLevel--; //��ų���� �ٿ� // �ɶ� ���� �ִ� ��ų�� �ٽ� �����;��� �������� ����
                                Debug.Log("���ν�ų Level Down");
                                SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����

                                slots1Child[_slot1i].SetItem(null); //�������Կ� ���°� ����
                                SkillSlot1(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                                SkillBackUpOff();
                                RockLevel1GameOver();
                            }
                            else //�װԾƴϴ� ��ġ�� 0�� �ƴϴ� �׷��� ���ݽ�ų�� ����ų �Ѵ� ���� 
                            {
                                Debug.Log("�󼺽�ų���� ���� ���� �����ߴ� ���Ұ� �����Ǿ����ϴ�.");
                                slots1Child[_slot1i - 1].SetItem(null); //�������Կ� ���� ���� ����
                                slots1Child[_slot1i].SetItem(null); //�������Կ� ���°� ����
                                _slot1i--; //��ġ ������ �ű�
                            }
                        }
                        else
                        {
                            _slot1i++; //������ġ �Ű���
                            Debug.Log(skill.skillName + "�� ����Ǿ����ϴ�.");
                        }
                    }
                    else //���������� ����������(�� á�ٴ� ��?)
                    {
                        if (BadCompatibilitySlot()) //��üũ 
                        {
                            Debug.Log("�󼺽�ų���� ���� ���� �����ߴ� ���Ұ� �����Ǿ����ϴ�.");
                            slots1Child[_slot1i - 1].SetItem(null); //�������Կ� ���� ���� ����
                            slots1Child[_slot1i].SetItem(null); //�������Կ� ���°� ����
                            _slot1i--; //��ġ ������ �ű�
                        }
                        else
                        {
                            SkillBackUpOn(); //������ �� ���� ������ null�Ǳ����� ���罽���� �����صд�.'
                            MainSubElementalCheck(); //����/���� �Ӽ� �����ش�. //������ ����
                            emptySlot.skillLevel++; //��ų ������
                            Debug.Log("���ν�ų Level Up");

                            SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����
                            for (int i = 0; i < slots1Child.Count; i++) { slots1Child[i].SetItem(null); }//�������Կ� ���°� ����
                            _slot1i = 0; //���� ��ġ�ʱ�ȭ
                            SkillSlot1(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                            slots1Child[_slot1i].SetItem(skill); //������ �ϸ鼭 ù��° ���ҿ� ��ų���� 
                            _slot1i++; //��ų������ ��ġ �ű��
                        }
                    }
                }
            }
            else if (_num == 1)
            {
                if (_slot2i >= 10) { return; } //���Կ� ��ġ�� 10�̻��̸� ����  

                slots2Child[_slot2i].SetItem(skill);//����������ġ�� 10���� ������ ����ġ�� �־���

                if (emptySlot.skillLevel == 1) //���� 1���� ���������� ���� ����
                {
                    if (BadCompatibilitySlot()) //��üũ 
                    {
                        if (slots[0].name == "slot" && slots[2].name == "slot") //2�� ������ 1�����ε� 1,3�� ���Կ� ��ų�� ������ ���� �ȸ¾Ƶ� �������� �־���
                        {
                            emptySlot2.SetItem(skill); //����ִ°��� �ֱ�
                        }
                        else
                        {
                            emptySlot.SetItem(null); //���ν�ų ����
                            slots2Child[_slot2i].SetItem(null); //�������� ����
                            Debug.Log("����1 ��ų�� �󼺽�ų���� ���� �����Ǿ����ϴ�.");
                            RockLevel1GameOver();
                        }
                    }
                    else
                    {
                        MainSubElementalCheck(); //����/���� �Ӽ� �����ش�. //������ ����
                        emptySlot.skillLevel++; //��ų ������ 
                        Debug.Log("���ν�ų Level Up");
                        SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����
                        slots2Child[_slot2i].SetItem(null); //�������Կ� ���°� ����
                        _slot2i = 0; //���� ��ġ�ʱ�ȭ
                        SkillSlot2(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                    }
                }
                else //2�������ʹ� ���������� �ֳ� ���ĸ� ���� �Ǵ��ؾ��ҵ�?
                {
                    if (slots2Child[_slot2i].enabled == true) //���������� ������ ����
                    {
                        if (BadCompatibilitySlot()) //��üũ
                        {
                            if (_slot2i == 0) //���Կ� ��ġ�� 0�̸� ������ ���Ұ� ���ٴ¶� �׷��� ���ν�ų(2�����ΰ�) ���� �ٿ�
                            {
                                emptySlot.skillLevel--; //��ų���� �ٿ�
                                Debug.Log("���ν�ų Level Down");
                                SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����
                                slots2Child[_slot2i].SetItem(null); //�������Կ� ���°� ����
                                SkillSlot2(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                                SkillBackUpOff(); //�����ٿ� �� ���� ��ų �ҷ��´�.
                                RockLevel1GameOver();

                            }
                            else //�װԾƴϴ� ��ġ�� 0�� �ƴϴ� �׷��� ���ݽ�ų�� ����ų �Ѵ� ���� 
                            {
                                Debug.Log("�󼺽�ų���� ���� ���� �����ߴ� ���Ұ� �����Ǿ����ϴ�.");
                                slots2Child[_slot2i - 1].SetItem(null); //�������Կ� ���� ���� ����
                                slots2Child[_slot2i].SetItem(null); //�������Կ� ���°� ����
                                _slot2i--; //��ġ ������ �ű�
                            }
                        }
                        else
                        {
                            _slot2i++; //������ġ �Ű���
                            Debug.Log(skill.skillName + "�� ����Ǿ����ϴ�.");
                        }
                    }
                    else //���������� ����������(�� á�ٴ� ��?)
                    {
                        if (BadCompatibilitySlot()) //��üũ 
                        {
                            Debug.Log("�󼺽�ų���� ���� ���� �����ߴ� ���Ұ� �����Ǿ����ϴ�.");
                            slots2Child[_slot2i - 1].SetItem(null); //�������Կ� ���� ���� ����
                            slots2Child[_slot2i].SetItem(null); //�������Կ� ���°� ����
                            _slot2i--; //��ġ ������ �ű�
                        }
                        else
                        {
                            SkillBackUpOn(); //������ �� ���� ������ null�Ǳ����� ���罽���� �����صд�.'
                            MainSubElementalCheck(); //����/���� �Ӽ� �����ش�. //������ ����
                            emptySlot.skillLevel++; //��ų ������
                            Debug.Log("���ν�ų Level Up");
                            SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����
                            for (int i = 0; i < slots2Child.Count; i++) { slots2Child[i].SetItem(null); }//�������Կ� ���°� ����
                            _slot2i = 0; //���� ��ġ�ʱ�ȭ
                            SkillSlot2(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                            slots2Child[_slot2i].SetItem(skill); //������ �ϸ鼭 ù��° ���ҿ� ��ų���� 
                            _slot2i++; //��ų������ ��ġ �ű��
                        }
                    }
                }
            }
            else if (_num == 2)
            {
                if (_slot3i >= 10) { return; } //���Կ� ��ġ�� 10�̻��̸� ����  

                slots3Child[_slot3i].SetItem(skill);//����������ġ�� 10���� ������ ����ġ�� �־���

                if (emptySlot.skillLevel == 1) //���� 1���� ���������� ���� ����
                {
                    if (BadCompatibilitySlot()) //��üũ 
                    {
                        if (slots[0].name == "slot" && slots[1].name == "slot") //3�� ������ 1�����ε� 1,2�� ���Կ� ��ų�� ������ ���� �ȸ¾Ƶ� �������� �־���
                        {
                            emptySlot2.SetItem(skill); //����ִ°��� �ֱ�
                        }
                        else
                        {
                            emptySlot.SetItem(null); //���ν�ų ����
                            slots3Child[_slot3i].SetItem(null); //�������� ����
                            Debug.Log("����1 ��ų�� �󼺽�ų���� ���� �����Ǿ����ϴ�.");
                            RockLevel1GameOver();
                        }
                    }
                    else
                    {
                        MainSubElementalCheck(); //����/���� �Ӽ� �����ش�. //������ ����
                        emptySlot.skillLevel++; //��ų ������
                        Debug.Log("���ν�ų Level Up");
                        SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����
                        slots3Child[_slot3i].SetItem(null); //�������Կ� ���°� ����
                        _slot3i = 0; //���� ��ġ�ʱ�ȭ
                        SkillSlot3(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                    }
                }
                else //2�������ʹ� ���������� �ֳ� ���ĸ� ���� �Ǵ��ؾ��ҵ�?
                {
                    if (slots3Child[_slot3i].enabled == true) //���������� ������ ����
                    {
                        if (BadCompatibilitySlot()) //��üũ
                        {
                            if (_slot3i == 0) //���Կ� ��ġ�� 0�̸� ������ ���Ұ� ���ٴ¶� �׷��� ���ν�ų(2�����ΰ�) ���� �ٿ�
                            {
                                emptySlot.skillLevel--; //��ų���� �ٿ� // �ɶ� ���� �ִ� ��ų�� �ٽ� �����;��� �������� ����
                                Debug.Log("���ν�ų Level Down");
                                SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����
                                slots3Child[_slot3i].SetItem(null); //�������Կ� ���°� ����
                                SkillSlot3(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                                SkillBackUpOff();
                                RockLevel1GameOver();


                            }
                            else //�װԾƴϴ� ��ġ�� 0�� �ƴϴ� �׷��� ���ݽ�ų�� ����ų �Ѵ� ���� 
                            {
                                Debug.Log("�󼺽�ų���� ���� ���� �����ߴ� ���Ұ� �����Ǿ����ϴ�.");
                                slots3Child[_slot3i - 1].SetItem(null); //�������Կ� ���� ���� ����
                                slots3Child[_slot3i].SetItem(null); //�������Կ� ���°� ����
                                _slot3i--; //��ġ ������ �ű�
                            }
                        }
                        else
                        {
                            _slot3i++; //������ġ �Ű���
                            Debug.Log(skill.skillName + "�� ����Ǿ����ϴ�.");
                        }
                    }
                    else //���������� ����������(�� á�ٴ� ��?)
                    {
                        if (BadCompatibilitySlot()) //��üũ 
                        {
                            Debug.Log("�󼺽�ų���� ���� ���� �����ߴ� ���Ұ� �����Ǿ����ϴ�.");
                            slots3Child[_slot3i - 1].SetItem(null); //�������Կ� ���� ���� ����
                            slots3Child[_slot3i].SetItem(null); //�������Կ� ���°� ����
                            _slot3i--; //��ġ ������ �ű�
                        }
                        else
                        {
                            SkillBackUpOn(); //������ �� ���� ������ null�Ǳ����� ���罽���� �����صд�.'
                            MainSubElementalCheck(); //����/���� �Ӽ� �����ش�. //������ ����
                            emptySlot.skillLevel++; //��ų ������
                            Debug.Log("���ν�ų Level Up");
                            SlotLevelSkill(); //��ų ������ ���� ��ũ��Ʈ ����
                            for (int i = 0; i < slots3Child.Count; i++) { slots3Child[i].SetItem(null); }//�������Կ� ���°� ����
                            _slot3i = 0; //���� ��ġ�ʱ�ȭ
                            SkillSlot3(); //��ų �������� �ϸ鼭 �������� ����,�ݰ�
                            slots3Child[_slot3i].SetItem(skill); //������ �ϸ鼭 ù��° ���ҿ� ��ų���� 
                            _slot3i++; //��ų������ ��ġ �ű��
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
    private void Update() //fixed�� �ϸ� timeScale = 0; ���� ���� ������ �����Ѵ�
    {
        SkillChoice();
    }
    //��ų ���� ��ġ �Լ�
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

    //������ ���� �������Ե� ����/�ݰ� 
    void SkillSlot1()
    {
        switch (slots[0].skillLevel)
        {
            case 0:
            case 1: //1������ �Ǿ������� ���ν�ų�� ���������� ����������
                for (int i = _level1; i < _level2; i++) //���°Ŵ� ������ false�� ���¶� ���ҽÿ��� �־���
                {
                    slots1ChildGo[i].SetActive(false);
                    slots1Child[i].enabled = false;
                }
                break;
            case 2: //2������ �Ǿ������� 1�������� ��ų�������̳� �ƴϸ� 3�������� �������� ��
                for (int i = _level1; i < _level2; i++)
                {
                    slots1ChildGo[i].SetActive(true);
                    slots1Child[i].enabled = true;
                }
                for (int i = _level2; i < _level3; i++) //������ ���������� �׸�ŭ �������Ե� ���������
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
                for (int i = _level3; i < _level4; i++) //������ ���������� �׸�ŭ �������Ե� ���������
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
            case 1: //1������ �Ǿ������� ���ν�ų�� ���������� ����������
                for (int i = _level1; i < _level2; i++) //���°Ŵ� ������ false�� ���¶� ���ҽÿ��� �־���
                {
                    slots2ChildGo[i].SetActive(false);
                    slots2Child[i].enabled = false;
                }
                break;
            case 2: //2������ �Ǿ������� 1�������� ��ų�������̳� �ƴϸ� 3�������� �������� ��
                for (int i = _level1; i < _level2; i++)
                {
                    slots2ChildGo[i].SetActive(true);
                    slots2Child[i].enabled = true;
                }
                for (int i = _level2; i < _level3; i++) //������ ���������� �׸�ŭ �������Ե� ���������
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
                for (int i = _level3; i < _level4; i++) //������ ���������� �׸�ŭ �������Ե� ���������
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
                for (int i = 3; i < 5; i++) //������ ���������� �׸�ŭ �������Ե� ���������
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
                for (int i = 5; i < 10; i++) //������ ���������� �׸�ŭ �������Ե� ���������
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

    void SlotLevelSkill() //��ų������ ���� ���� + ���꽽�Կ� ���� ��ũ��Ʈ�� �ٲ�� ����
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
        else if (slots[_num].skillLevel == 2) //2���� ��ų����
        {
            switch (_num)
            {
                case 0:
                    switch (_slot1MainElemental)
                    {
                        case "Fire":
                            if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[0]); } //��+�� ����
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[1]); }//��+���� ����
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[2]); } //��+�� ����
                            break;
                        case "Water":
                            if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[3]); }//��+�� ����
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[4]); }//��+�� ����
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[6]); }//����+���� ����
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[7]); }//����+�� ����
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[8]); }//����+�� ����
                            break;
                        case "Rock":
                            if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[9]); }//��+�� ����
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[10]); }//��+�� ����
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[11]); }//��+�� ����
                            break;
                    }
                    break;
                case 1:
                    switch (_slot2MainElemental)
                    {
                        case "Fire":
                            if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[0]); }//��+�� ����
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[1]); }//��+���� ����
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[2]); }//��+�� ����
                            break;
                        case "Water":
                            if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[3]); }//��+�� ����
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[4]); }//��+�� ����
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[6]); }//����+���� ����
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[7]); }//����+�� ����
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[8]); }//����+�� ����
                            break;
                        case "Rock":
                            if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[9]); }//��+�� ����
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[10]); }//��+�� ����
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[11]); }//��+�� ����
                            break;
                    }
                    break;
                case 2:
                    switch (_slot3MainElemental)
                    {
                        case "Fire":
                            if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[0]); }//��+�� ����
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[1]); } //��+���� ����
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[2]); }//��+�� ����
                            break;
                        case "Water":
                            if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[3]); } //��+�� ����
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[4]); } //��+�� ����
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel2DataList[6]); }//����+���� ����
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[7]); }//����+�� ����
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[8]); }//����+�� ����
                            break;
                        case "Rock":
                            if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel2DataList[9]); }//��+�� ����
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel2DataList[10]); }//��+�� ����
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel2DataList[11]); } //��+�� ����
                            break;
                    }
                    break;
            }

        }
        else if (slots[_num].skillLevel == 3) //3���� ��ų����
        {
            switch (_num)
            {
                case 0:
                    switch (_slot1MainElemental)
                    {
                        case "Fire":
                            if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[0]); }//��+�� ����
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[1]); }//��+���� ����
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[2]); }//��+�� ����
                            break;
                        case "Water":
                            if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[3]); }//��+�� ����
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[4]); }//��+�� ����
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[6]); }//����+���� ����
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[7]); }//����+�� ����
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[8]); }//����+�� ����
                            break;
                        case "Rock":
                            if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[9]); }//��+�� ����
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[10]); }//��+�� ����
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[11]); }//��+�� ����
                            break;
                    }
                    break;
                case 1:
                    switch (_slot2MainElemental)
                    {
                        case "Fire":
                            if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[0]); }//��+�� ����
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[1]); }//��+���� ����
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[2]); }//��+�� ����
                            break;
                        case "Water":
                            if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[3]); }//��+�� ����
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[4]); } //��+�� ����
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[6]); } //����+���� ����
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[7]); }//����+�� ����
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[8]); }//����+�� ����
                            break;
                        case "Rock":
                            if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[9]); }//��+�� ����
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[10]); }//��+�� ����
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[11]); }//��+�� ����
                            break;
                    }
                    break;
                case 2:
                    switch (_slot3MainElemental)
                    {
                        case "Fire":
                            if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[0]); } //��+�� ����
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[1]); }//��+���� ����
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[2]); } //��+�� ����
                            break;
                        case "Water":
                            if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[3]); }//��+�� ����
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[4]); }//��+�� ����
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel3DataList[6]); }//����+���� ����
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[7]); }//����+�� ����
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[8]); } //����+�� ����
                            break;
                        case "Rock":
                            if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel3DataList[9]); }//��+�� ����
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel3DataList[10]); }//��+�� ����
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel3DataList[11]); }//��+�� ����
                            break;
                    }
                    break;
            }

        }
        else if (slots[_num].skillLevel == 4) //4���� ��ų����
        {
            switch (_num)
            {
                case 0:
                    switch (_slot1MainElemental)
                    {
                        case "Fire":
                            if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[0]); } //��+�� ����
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[1]); }//��+���� ����
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[2]); }//��+�� ����
                            break;
                        case "Water":
                            if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[3]); }//��+�� ����
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[4]); }//��+�� ����
                            else if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot1SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[6]); } //����+���� ����
                            else if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[7]); }//����+�� ����
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[8]); }//����+�� ����
                            break;
                        case "Rock":
                            if (_slot1SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[9]); }//��+�� ����
                            else if (_slot1SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[10]); }//��+�� ����
                            else if (_slot1SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[11]); }//��+�� ����
                            break;
                    }
                    break;
                case 1:
                    switch (_slot2MainElemental)
                    {
                        case "Fire":
                            if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[0]); }//��+�� ����
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[1]); } //��+���� ����
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[2]); } //��+�� ����
                            break;
                        case "Water":
                            if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[3]); }//��+�� ����
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[4]); } //��+�� ����
                            else if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot2SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[6]); }//����+���� ����
                            else if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[7]); }//����+�� ����
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[8]); }//����+�� ����
                            break;
                        case "Rock":
                            if (_slot2SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[9]); }//��+�� ����
                            else if (_slot2SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[10]); }//��+�� ����
                            else if (_slot2SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[11]); }//��+�� ����
                            break;
                    }
                    break;
                case 2:
                    switch (_slot3MainElemental)
                    {
                        case "Fire":
                            if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[0]); }//��+�� ����
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[1]); }//��+���� ����
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[2]); }//��+�� ����
                            break;
                        case "Water":
                            if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[3]); }//��+�� ����
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[4]); }//��+�� ����
                            else if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[5]); }//��+���� ����
                            break;
                        case "Lightning":
                            if (_slot3SubElemental == "Lightning") { slots[_num].SetItem(store.skillData.skillLevel4DataList[6]); }//����+���� ����
                            else if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[7]); } //����+�� ����
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[8]); }//����+�� ����
                            break;
                        case "Rock":
                            if (_slot3SubElemental == "Rock") { slots[_num].SetItem(store.skillData.skillLevel4DataList[9]); }//��+�� ����
                            else if (_slot3SubElemental == "Fire") { slots[_num].SetItem(store.skillData.skillLevel4DataList[10]); }//��+�� ����
                            else if (_slot3SubElemental == "Water") { slots[_num].SetItem(store.skillData.skillLevel4DataList[11]); }//��+�� ����
                            break;
                    }
                    break;
            }

        }

    }
    bool BadCompatibilitySlot()//��üũ ����
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
    } //�󼺾ȸ����� ��ų����
    void SkillBackUpOn() //��ų�� �������Կ� �����صд�. 
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

    } //��ų������ ���� ����
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

    } //�������Կ� �ִ°� ���� �������ҿ� ���� //���� �ٿ�� �����ص� �� �Ű��ֱ�


    void MainSubElementalCheck()//����/���� �Ӽ� �����ش�. //�󼺾ȸ´°� ������ �ٽõ���.
    {

        _mainRan = Random.Range(0, 10);
        _subRan = Random.Range(0, 10);
        MainElementalPercentage(_mainRan); //���� �Ӽ� �����ֱ�
        SubElementalPercentage(_subRan); //���� �Ӽ� �����ֱ�
        for (int i = 0; i < 9999; i++) //(���� �ȸ�����)�ߺ��Ǹ� ��� �������
        {
            if (BadElementalCheck())
            {
                _mainRan = Random.Range(0, 10);
                _subRan = Random.Range(0, 10);
                MainElementalPercentage(_mainRan); //���� �Ӽ� �����ֱ�
                SubElementalPercentage(_subRan); //���� �Ӽ� �����ֱ�
            }
            else
            {
                break;
            }
        }

    }
    bool BadElementalCheck() //����/���� �Ӽ��� �ȸ´°� �ִ��� üũ
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


    } //����  //������ ���϶� ���μӼ�Ȯ�� 
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
    } //����  //������ ���϶� ����Ӽ�Ȯ�� 
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

    } //1���� ���� ������ ���ӿ���
}