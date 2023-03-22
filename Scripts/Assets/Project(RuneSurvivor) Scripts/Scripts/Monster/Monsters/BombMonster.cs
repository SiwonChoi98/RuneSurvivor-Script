using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMonster : Monster
{
    public override void Move()
    {
        if (!GameManager.instance.player.IsLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001"))
            return;
        rigid.MovePosition(rigid.position + nextVec);
        transform.LookAt(target.position);
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
