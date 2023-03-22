using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] private Player player;
    [Header("���� ���� ��ġ")]
    public Transform skillAttackPos; //��ų������ ��ġ
    [Header("��ų ����")] //��ų���Կ� ���;� �� ��ų�� ������.
    [SerializeField] private Skill[] skillSlot;
    public Inventory inventory;
    public SkillPrefab skillPrefab; //��ų ������Ʈ��
    public SkillData skillData; //��ų �������� �����ؼ� ������ ��
    [Header("�� ��ų ��Ÿ���� slot 123�� ����")]
    private double _slot1Rate;
    private double _slot2Rate;
    private double _slot3Rate;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Update()
    {
        if (!player.IsLive)
            return;

        ////�κ��丮�� �ִ� slot�� �پ� �ִ� Prefab�� ����ٰ� �ٽ� �ٿ��ش�
        if (inventory.slots[0].skillPrefab != true) { return; }
        else
        { 
            skillSlot[0] = inventory.slots[0].skillPrefab;
            _slot1Rate -= Time.deltaTime;
            if (_slot1Rate <= 0)
            {
                _slot1Rate = skillSlot[0].skillRate; //��Ÿ�� �ʱ�ȭ
                skillSlot[0].skillPattern.PatternSkill();
                player.anim.SetTrigger("DoAttack01");
            }

        }
        if (inventory.slots[1].skillPrefab != true) { return; }
        else
        { 
            skillSlot[1] = inventory.slots[1].skillPrefab;
            _slot2Rate -= Time.deltaTime;
            if (_slot2Rate <= 0)
            {
                _slot2Rate = skillSlot[1].skillRate;
                skillSlot[1].skillPattern.PatternSkill();
                player.anim.SetTrigger("DoAttack02");

            }

        }
        if (inventory.slots[2].skillPrefab != true) { return; }
        else
        { 
            skillSlot[2] = inventory.slots[2].skillPrefab;
            _slot3Rate -= Time.deltaTime;
            if (_slot3Rate <= 0)
            {
                _slot3Rate = skillSlot[2].skillRate;
                skillSlot[2].skillPattern.PatternSkill();
                player.anim.SetTrigger("DoAttack01");

            }

        }
    }
}
