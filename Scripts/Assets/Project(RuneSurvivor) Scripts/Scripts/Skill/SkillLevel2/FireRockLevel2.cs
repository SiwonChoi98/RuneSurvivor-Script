using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRockLevel2 : SkillPattern
{
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }
    IEnumerator SkillPattern()
    {

        GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[2], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
        Rigidbody arrowRigid = skill.GetComponent<Rigidbody>();
        arrowRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;


        yield return null;

    }
}
