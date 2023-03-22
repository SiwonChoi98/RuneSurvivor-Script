using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningLightningLevel3 : SkillPattern
{
    [SerializeField] private GameObject hitPs;
    private int count = 3;
    public override void PatternSkill()
    {
        SkillPattern();
    }
    private void SkillPattern()
    {
        for (int i = 0; i < count; i++)
        {
            int ran = Random.Range(-10, 10);
            int ran2 = Random.Range(-10, 10);
            GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel3Prefab[3], GameManager.instance.player.transform.position + new Vector3(ran, 0, ran2), GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[3].transform.rotation);
            Destroy(skill, 3f);
            SoundManager.Instance.LightninglightningLevel3HitSound();
            Debug.Log("lightning3level attack!!");
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundManager.Instance.LightninglightningLevel3HitSound();
            GameObject hits = Instantiate(hitPs, transform.position, transform.rotation);
            Destroy(hits, 1);
        }
    }
}
