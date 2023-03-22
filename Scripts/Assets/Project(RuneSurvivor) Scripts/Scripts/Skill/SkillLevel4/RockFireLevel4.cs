using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFireLevel4 : SkillPattern
{
    public override void PatternSkill()
    {
        SkillPattern();
    }
    private void SkillPattern()
    {
        Vector3 vec3 = new Vector3(0, 0, 0);
        for (int i = 0; i < 2; i++)
        {
            GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel4Prefab[6], GameManager.instance.weapon.skillAttackPos.position + vec3, GameManager.instance.weapon.skillAttackPos.rotation);
            Rigidbody arrowRigid = skill.GetComponent<Rigidbody>();
            arrowRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;
            vec3.x += 1;
        }

    }
}
