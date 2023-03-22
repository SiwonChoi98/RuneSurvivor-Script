using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMonster : Monster
{
    public override void Move()
    {
        if (!GameManager.instance.player.IsLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001"))
            return; //플레이어 죽으면 움직임 제한 or hit상태이면 제한
        dirVec = target.position - rigid.position;
        nextVec = dirVec.normalized * MonsterSpeed * Time.fixedDeltaTime;
        rigid.velocity = Vector3.zero;

        rigid.MovePosition(rigid.position + nextVec);
        transform.LookAt(target.position);
    }
}
