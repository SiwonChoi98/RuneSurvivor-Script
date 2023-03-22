using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFireLevel2 : SkillPattern
{
    [SerializeField] private GameObject hitPs;
    public override void PatternSkill()
    {
        SkillPattern();
    }
    public void SkillPattern()
    {
        GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[6], GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
        Destroy(skill, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //SoundManager.Instance.PlaySound(2, "");
            GameObject hits = Instantiate(hitPs, other.transform.position, transform.rotation);
            Destroy(hits, 1);
            
            Destroy(gameObject);
        }

    }
}
