using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterPool : MonoBehaviour
{
    [Header("일반 몬스터 스폰(풀)")]
    public GameObject[] prefabs;
    public List<GameObject>[] pools;
    [Header("패턴 몬스터 스폰(풀)")]
    public GameObject[] prefabsPattern;
    public List<GameObject>[] poolsPattern;
   

    
    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; 
        poolsPattern = new List<GameObject>[prefabsPattern.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
        for (int i = 0; i < poolsPattern.Length; i++)
        {
            poolsPattern[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
    
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    } //노말 몬스터 풀
    public GameObject GetPattern(int index)
    {
        GameObject select = null;
        foreach (GameObject item in poolsPattern[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabsPattern[index], transform);
            poolsPattern[index].Add(select);
        }
        return select;
    } //패턴 몬스터 풀
    
   
}
