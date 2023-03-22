using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    [HideInInspector]
    public SkillProperty skill;

    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Button sellButton;

    public double skillRate;
    public string skillElemental;
    public int skillLevel;
    public Skill skillPrefab;
    private void Awake()
    {
        SetSellBtnInteractable(false);
    }
    void SetSellBtnInteractable(bool b)
    {
        if(sellButton != null)
        {
            sellButton.interactable = b;
        }
    }
    public void SetItem(SkillProperty skill)
    {
        this.skill = skill;

        if(skill == null)
        {
            image.enabled = false;
            SetSellBtnInteractable(false); //�������� ������� x��ư ��������
            gameObject.name = "slot";
            skillPrefab = null;
            skillLevel = 0;
            skillElemental = null;
        }
        else
        {
            image.enabled = true;
            gameObject.name = skill.skillName;
            image.sprite = skill.skillImage;
            skillRate = skill.skillRate;
            skillElemental = skill.skillElemental;
            skillLevel = skill.skillLevel;
            skillPrefab = skill.skillData;
            SetSellBtnInteractable(true); //�������� ������ x�������ְ�
        }
    }
    public void OnClickSellBtn() //x��ư ������ ������ �������
    {
        SetItem(null);
    }
   
}
