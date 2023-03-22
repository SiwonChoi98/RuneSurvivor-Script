using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWaterLevel2 : SkillPattern
{
    // Start is called before the first frame update
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }
    IEnumerator SkillPattern()
    {
        Vector3 vec3 = new Vector3(0, 0, 0);
        for (int i = 0; i < 5; i++)
        {
            GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[5], GameManager.instance.weapon.skillAttackPos.position + vec3, GameManager.instance.weapon.skillAttackPos.rotation);
            Rigidbody arrowRigid = skill.GetComponent<Rigidbody>();
            arrowRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;
            vec3.x += 1;
        }
        yield return null;

    }
}
