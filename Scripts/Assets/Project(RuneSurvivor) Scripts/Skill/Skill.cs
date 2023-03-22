using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Skill : MonoBehaviour
{
    public string skillName; //��ų�̸�
    public Sprite skillImage; //��ų�̹���
    public double skillRate; //��ų���ݼӵ�
    public int skillDamage; //��ų���ݷ�
    public int skillLevel; //��ų����
    public string skillElemental; //��ų�Ӽ�
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

