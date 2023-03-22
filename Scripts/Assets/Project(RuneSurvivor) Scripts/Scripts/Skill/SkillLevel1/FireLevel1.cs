using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLevel1 : SkillPattern
{
    public GameObject hitPs;
    public override void PatternSkill()
    {
        SkillPattern();
    }
    void SkillPattern()
    {
        GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel1Prefab[0], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
        Rigidbody arrowRigid = skill.GetComponent<Rigidbody>();
        arrowRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            SoundManager.Instance.FireLevelHitSound();
            GameObject hits = Instantiate(hitPs, transform.position, transform.rotation);
            Destroy(hits, 1);
            Destroy(gameObject);
        }
    }
}
