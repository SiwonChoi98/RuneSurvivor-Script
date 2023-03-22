using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrefab : MonoBehaviour
{
    public GameObject[] skillLevel1Prefab;
    public GameObject[] skillLevel2Prefab;
    public GameObject[] skillLevel3Prefab;
    public GameObject[] skillLevel4Prefab;

    void Awake()
    {
        skillLevel1Prefab = Resources.LoadAll<GameObject>("SkillPrefab/Level1");
        skillLevel2Prefab = Resources.LoadAll<GameObject>("SkillPrefab/Level2");
        skillLevel3Prefab = Resources.LoadAll<GameObject>("SkillPrefab/Level3");
        skillLevel4Prefab = Resources.LoadAll<GameObject>("SkillPrefab/Level4");
    }
}
