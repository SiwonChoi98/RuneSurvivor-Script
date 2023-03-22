using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaterLevel2 : SkillPattern
{
    Vector3 offSet;
    public GameObject hitPs;
    public override void PatternSkill()
    {
        SkillPattern();
    }
    void Awake()
    {
        offSet = transform.position - GameManager.instance.player.transform.position;
    }
    void SkillPattern()
    {
        for (int i = 0; i < 3; i++) //11
        {
            GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel2Prefab[11], GameManager.instance.skillCreatePos.createWaterWaterLevel2SkillPos[i].transform.position, GameManager.instance.skillCreatePos.createWaterWaterLevel2SkillPos[i].transform.rotation);

            Destroy(skill, ((int)GameManager.instance.weapon.skillData.skillLevel2Datas[3].skillRate-1));
        }

    }
    void Update()
    {
        transform.position = GameManager.instance.player.transform.position + offSet;
        transform.RotateAround(GameManager.instance.player.transform.position, Vector3.up, 200 * Time.deltaTime);
        offSet = transform.position - GameManager.instance.player.transform.position;
       

        //GameManager.instance.player.transform.position
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundManager.Instance.WaterLevel1HitSound();
            GameObject hits = Instantiate(hitPs, transform.position, transform.rotation);
            Destroy(hits, 1);
            Destroy(gameObject);
        }
    }
}
