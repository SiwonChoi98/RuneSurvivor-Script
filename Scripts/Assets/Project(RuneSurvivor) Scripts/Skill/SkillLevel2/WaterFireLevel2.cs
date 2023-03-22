using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFireLevel2 : SkillPattern
{
    int euler;
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }
    IEnumerator SkillPattern()
    {
        GameManager.instance.weapon.skillAttackPos.rotation = Quaternion.Euler(0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[9], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
            Rigidbody skillRigid = skill.GetComponent<Rigidbody>();
            skillRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;
            euler += 90;
            GameManager.instance.weapon.skillAttackPos.rotation = Quaternion.Euler(0, euler, 0);
            
        }
        yield return null;

    }
}
