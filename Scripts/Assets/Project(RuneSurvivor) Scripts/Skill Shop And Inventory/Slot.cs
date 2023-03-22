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
            SetSellBtnInteractable(false); //아이템이 비었으면 x버튼 못누르게
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
            SetSellBtnInteractable(true); //아이템이 있으면 x누를수있게
        }
    }
    public void OnClickSellBtn() //x버튼 누르면 아이템 비어지게
    {
        SetItem(null);
    }
   
}
