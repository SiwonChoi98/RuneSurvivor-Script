using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Monster _monster;
    void Awake()
    {
        _monster = gameObject.GetComponent<Monster>();
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Area"))
        {
            return;
        }
       
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position; 
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffZ = Mathf.Abs(playerPos.z - myPos.z);

        Vector3 playerDir = GameManager.instance.player.inputVec; 

        float dirX = playerPos.x < myPos.x ? -1 : 1;
        float dirZ = playerPos.z < myPos.z ? -1 : 1;
        switch (gameObject.transform.tag)
        {
            case "Floor":
                if (diffX > diffZ) { transform.position += Vector3.right * dirX * 140; }
                else if (diffX < diffZ) { transform.position += Vector3.forward * dirZ * 140; }
                break;
            case "Enemy":
                if (_monster.MonsterCurHealth > ((_monster.MonsterMaxHealth / 10) * 9)) { transform.Translate(playerDir * 70 + new Vector3(Random.Range(-3, 3f), 0f, Random.Range(-3f, 3f)), Space.World); } //체력이 90퍼 이하면 이동x 
                else { return; }
                break;
            case "Wall":
                if (diffX > diffZ) { transform.position += Vector3.right * dirX * Random.Range(69, 70); }
                else if (diffX < diffZ) { transform.position += Vector3.forward * dirZ * Random.Range(69, 70); }
                break;
            case "Grass":
                if (diffX > diffZ) { transform.position += Vector3.right * dirX * Random.Range(69, 70); }
                else if (diffX < diffZ) { transform.position += Vector3.forward * dirZ * Random.Range(69, 70); }
                break;
        }
        
    }
}
