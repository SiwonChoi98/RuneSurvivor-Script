using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaterLevel4 : SkillPattern
{
    private int euler;
    private int count;
    [SerializeField] private GameObject WaterWaterLevel4nullObject;
    [SerializeField] private GameObject hitPs;
    [SerializeField] private GameObject nullObject;
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }
    private IEnumerator SkillPattern()
    { 
        nullObject = Instantiate(WaterWaterLevel4nullObject, GameManager.instance.weapon.skillAttackPos.position, GameManager.instance.weapon.skillAttackPos.rotation);
        Rigidbody rigid = nullObject.GetComponent<Rigidbody>();
        rigid.velocity = GameManager.instance.player.transform.forward * 8;
        Destroy(nullObject, 5);
        count = 0;
        for (int j=0; j<5; j++) //nullObject °¹¼ö
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject skill = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel4Prefab[11], nullObject.transform.position + new Vector3(0, 0.3f, 0), nullObject.transform.rotation);
                euler += 45;
                nullObject.transform.rotation = Quaternion.Euler(0, euler + count, 0);
                Rigidbody rigid2 = skill.GetComponent<Rigidbody>();
                rigid2.velocity = (nullObject.transform.forward * 2) * 10;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.3f);
            count += 10;
        }
    }
    private void Update()
    {
        transform.forward = GetComponent<Rigidbody>().velocity;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            //SoundManager.Instance.PlaySound(2, "");
            GameObject hits = Instantiate(hitPs, other.transform.position, transform.rotation);
            Destroy(hits, 1);
            Destroy(gameObject);
        }
    }
}
