using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel1 : SkillPattern
{
    [SerializeField] private GameObject hitPs;
    private GameObject skillObject;
 
    public override void PatternSkill()
    {
        GameManager.instance.StartCoroutine(SkillPattern());
    }
    private IEnumerator SkillPattern()
    {
        if (GameManager.instance.player.Target != null) { GameManager.instance.waterLevel1PosTrans.transform.LookAt(GameManager.instance.player.Target.transform.position); }
        else { GameManager.instance.waterLevel1PosTrans.transform.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.forward); }
        for (int i = 0; i < 3; i++)
        { 
            skillObject = Instantiate(GameManager.instance.weapon.skillPrefab.skillLevel1Prefab[3], GameManager.instance.player.transform.position + new Vector3(0, 0.3f, 0), GameManager.instance.skillCreatePos.createWaterLevel1SkillPos[0].transform.rotation);
            
            yield return new WaitForSeconds(0.3f);
        }
        Debug.Log("waterLevelCreate");
       
    }
    Vector3[] point = new Vector3[4];

    [SerializeField] [Range(0, 1)] private float t = 0;
    [SerializeField] public float spd1 = 0.7f;

    private void FixedUpdate()
    {
        point[0] = transform.position + new Vector3(0, 0.4f, 0); // P0
        point[1] = GameManager.instance.skillCreatePos.createWaterLevel1SkillPos[0].transform.position;
        point[2] = GameManager.instance.skillCreatePos.createWaterLevel1SkillPos[1].transform.position;
        point[3] = GameManager.instance.skillCreatePos.createWaterLevel1SkillPos[2].transform.position;
       

        if (t > 1) return;
        t += Time.deltaTime * spd1;
        DrawTrajectory();
    }
    private void DrawTrajectory()
    {
        transform.position = new Vector3(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y),
            FourPointBezier(point[0].z, point[1].z, point[2].z, point[3].z)
        );
    }

    private float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall") || other.CompareTag("Floor"))
        {
            GameObject hits = Instantiate(hitPs, transform.position, transform.rotation);
            Destroy(hits, 1);
            SoundManager.Instance.WaterLevel1HitSound();
            Destroy(gameObject);
        }
    }
    
}


