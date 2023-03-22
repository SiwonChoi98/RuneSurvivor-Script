using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaterLevel3 : SkillPattern
{
    float ga = 1;
    public GameObject hitPs;
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }

    void Awake()
    {
        GetComponent<SphereCollider>().radius = ga;
    }
    IEnumerator SkillPattern()
    {
        GameManager.instance.weapon.skillAttackPos.rotation = Quaternion.Euler(0, 0, 0);

        ga = 1;
        GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel3Prefab[11], GameManager.instance.player.transform.position + new Vector3(0, 0.4f, 0), GameManager.instance.player.transform.rotation);
        Destroy(skill, 1.5f);
        yield return new WaitForSeconds(3f);

    }
    void Update()
    {
        transform.position = GameManager.instance.player.transform.position;
        GetComponent<SphereCollider>().radius = ga;
        if (ga > 2.5f) { ga = 1; }
        else { ga += 0.15f; }
   
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundManager.Instance.WaterLevel1HitSound();
            GameObject hits = Instantiate(hitPs, other.transform.position, transform.rotation);
            Destroy(hits, 1);
        }
    }
}
