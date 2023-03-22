using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] private Player player;
    [Header("여러 공격 위치")]
    public Transform skillAttackPos; //스킬나가는 위치
    [Header("스킬 슬롯")] //스킬슬롯에 들어와야 그 스킬이 나간다.
    [SerializeField] private Skill[] skillSlot;
    public Inventory inventory;
    public SkillPrefab skillPrefab; //스킬 오브젝트들
    public SkillData skillData; //스킬 프리팹을 생성해서 내보낼 곳
    [Header("각 스킬 쿨타임을 slot 123에 저장")]
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

        ////인벤토리에 있는 slot에 붙어 있는 Prefab을 여기다가 다시 붙여준다
        if (inventory.slots[0].skillPrefab != true) { return; }
        else
        { 
            skillSlot[0] = inventory.slots[0].skillPrefab;
            _slot1Rate -= Time.deltaTime;
            if (_slot1Rate <= 0)
            {
                _slot1Rate = skillSlot[0].skillRate; //쿨타임 초기화
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
