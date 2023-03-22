using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFireLevel2 : SkillPattern
{
    GameObject skill;
    public GameObject hitPs;
    public override void PatternSkill()
    {
        SkillPattern();
    }
    void SkillPattern()
    {
        int euler = -90;
        for (int i = 0; i < 4; i++)
        {
            skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[0], GameManager.instance.weapon.skillAttackPos.position , GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[0].transform.rotation);
            Rigidbody skillRigid = skill.GetComponent<Rigidbody>();
            skillRigid.velocity = GameManager.instance.weapon.skillAttackPos.forward * 10;
            GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[0].transform.rotation = Quaternion.Euler(0, euler, 0);
            euler += 90;

        }

    }
    void Update()
    {
        transform.Rotate(transform.up, 200 * Time.deltaTime, Space.Self);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            SoundManager.Instance.FireLevelHitSound();
            GameObject hits = Instantiate(hitPs, transform.position, transform.rotation);
            Destroy(hits, 1);
        }

    }
}
