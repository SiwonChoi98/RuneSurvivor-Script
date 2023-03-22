using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightningLevel2 : SkillPattern
{
    public GameObject hitPs;
    int euler;
    public override void PatternSkill()
    {
        SkillPattern();
    }
    void SkillPattern()
    {
        GameManager.instance.weapon.skillAttackPos.rotation = Quaternion.Euler(0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[1], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
            Rigidbody skillRigid = skill.GetComponent<Rigidbody>();
            skillRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;
            euler += 90;
            GameManager.instance.weapon.skillAttackPos.rotation = Quaternion.Euler(0, euler, 0);

        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundManager.Instance.FireLevelHitSound();
            GameObject hits = Instantiate(hitPs, transform.position, transform.rotation);
            Destroy(hits, 1);
        }
    }
}
