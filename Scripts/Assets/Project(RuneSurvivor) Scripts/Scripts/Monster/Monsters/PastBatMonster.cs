using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastBatMonster : Monster
{
    public override void Move()
    {
        if (!GameManager.instance.player.IsLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001"))
            return; //플레이어 죽으면 움직임 제한 or hit상태이면 제한
        rigid.position += transform.forward.normalized * MonsterSpeed * Time.fixedDeltaTime;
        if (isLook) { StartCoroutine(IsLook()); }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            isLook = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            gameObject.SetActive(false);
            isLook = true;

        }

    }
}
