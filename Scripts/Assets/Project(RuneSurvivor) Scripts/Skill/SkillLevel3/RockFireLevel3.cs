using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFireLevel3 : SkillPattern
{
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }
    IEnumerator SkillPattern()
    {

        GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel3Prefab[6], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
        yield return null;

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

