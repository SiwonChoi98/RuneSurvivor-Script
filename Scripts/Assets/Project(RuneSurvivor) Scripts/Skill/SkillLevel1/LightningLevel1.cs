using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningLevel1 : SkillPattern
{
    public GameObject hitPs;
    
    public override void PatternSkill()
    {
       SkillPattern();
    }
    void SkillPattern()
    {
        transform.position = GameManager.instance.player.transform.position;

        GameObject skillObject = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel1Prefab[1]);
        Debug.Log("번개1레벨이 생성되었습니다.");
        Destroy(skillObject, (int)GameManager.instance.weapon.skillData.skillLevel1Datas[2].skillRate);

    }
    void Update()
    {
        transform.position = GameManager.instance.player.transform.position + new Vector3(0, 1, 0); //플레이어를 계속 따라다니게 만든다.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundManager.Instance.LightningLevel1HitSound();
            GameObject hits = Instantiate(hitPs, other.transform.position, transform.rotation);
            Destroy(hits, 1);
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Monster monster = other.gameObject.GetComponent<Monster>();
            if (monster._isLightningLevel1)
            {
                SoundManager.Instance.LightningLevel1HitSound();
                int ran = Random.Range(0, 5);
                monster.MonsterCurHealth -= skillDamage + ran;
                monster.DamageText(skillDamage + ran);
                monster.MonsterHit();
                monster._isLightningLevel1 = false;

            }
        }
    }
}
