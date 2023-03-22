using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightningLevel3 : SkillPattern
{
    int euler;
    public GameObject hitPs;
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }
    IEnumerator SkillPattern()
    {
        GameManager.instance.weapon.skillAttackPos.rotation = Quaternion.Euler(0, 0, 0);
        for (int i = 0; i < 8; i++)
        {
            GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel3Prefab[1], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
            Rigidbody skillRigid = skill.GetComponent<Rigidbody>();
            skillRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;
            euler += 45;
            GameManager.instance.weapon.skillAttackPos.rotation = Quaternion.Euler(0, euler, 0);
        }
        yield return new WaitForSeconds(0.5f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundManager.Instance.FireLevelHitSound();
            GameObject hits = Instantiate(hitPs, other.transform.position, transform.rotation);
            Destroy(hits, 1);   
        }
    }
}
