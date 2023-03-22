using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Skill : MonoBehaviour
{
    public string skillName; //스킬이름
    public Sprite skillImage; //스킬이미지
    public double skillRate; //스킬공격속도
    public int skillDamage; //스킬공격력
    public int skillLevel; //스킬레벨
    public string skillElemental; //스킬속성
    public SkillPattern skillPattern;

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Area")) { Destroy(gameObject); }
    }
   
}
public abstract class SkillPattern : Skill
{
    public abstract void PatternSkill();
}

