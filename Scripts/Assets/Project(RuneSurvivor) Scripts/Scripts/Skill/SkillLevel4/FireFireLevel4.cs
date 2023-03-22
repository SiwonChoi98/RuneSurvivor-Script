using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFireLevel4 : SkillPattern
{
    int euler = -45;
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }
    IEnumerator SkillPattern()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel4Prefab[0], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
            Rigidbody arrowRigid = skill.GetComponent<Rigidbody>();
            arrowRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;
            GameManager.instance.weapon.skillAttackPos.rotation = Quaternion.Euler(0, euler, 0);
            euler += 15;
        }
        yield return null;

    }
}
