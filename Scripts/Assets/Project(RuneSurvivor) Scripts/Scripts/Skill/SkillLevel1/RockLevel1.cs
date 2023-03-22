using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLevel1 : SkillPattern
{
    public override void PatternSkill()
    {
       skillpaturn();
    }
    void Awake()
    {
        transform.position = GameManager.instance.player.transform.position;
    }
    void skillpaturn()
    {
        GameObject skillObject = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel1Prefab[2], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
        Destroy(skillObject, (int)GameManager.instance.weapon.skillData.skillLevel1Datas[3].skillRate);

    }
    void FixedUpdate()
    {
        transform.position = GameManager.instance.player.transform.position; //플레이어를 계속 따라다니게 만든다.
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundManager.Instance.RockLevel1HitSound();
        }
    }
}
