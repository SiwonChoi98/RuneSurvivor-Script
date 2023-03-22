using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningLightningLevel2 : SkillPattern
{
    public GameObject hitPs;   
    public override void PatternSkill()
    {
        SkillPattern();
    }
    void SkillPattern()
    {
        int ran = Random.Range(-5, 5);
        GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[3], GameManager.instance.player.transform.position + new Vector3(ran, 0, ran), GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[3].transform.rotation);
        SoundManager.Instance.LightningLevel1HitSound();
        Destroy(skill, 3f);
        Debug.Log("lightning2level attack!!");

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            SoundManager.Instance.LightningLevel1HitSound();
            GameObject hits = Instantiate(hitPs, transform.position, transform.rotation);
            Destroy(hits, 1);
        }
    }
}
