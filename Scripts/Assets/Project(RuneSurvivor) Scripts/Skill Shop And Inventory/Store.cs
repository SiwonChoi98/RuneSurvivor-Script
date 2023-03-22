using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Store : MonoBehaviour
{
    public SkillData skillData;
    public Transform slotRoot;
    public List<Slot> slots;
    public System.Action<SkillProperty> onSlotClick;

    public List<SkillProperty> createSlots; //아이템 리스트 저장 
    private void OnEnable()
    {
        slots = new List<Slot>();
        int slotCnt = slotRoot.childCount;
        int dataCnt;
        if (GameManager.instance.player.PlayerLevel != 1) { dataCnt = skillData.skillLevel1DataList.Count; }
        else { dataCnt = skillData.skillLevel1DataList.Count-1; }

        for(int i=0; i< dataCnt; i++) //고를수 있는 스킬 채워지는 부분
        {
            createSlots.Add(skillData.skillLevel1DataList[i]);
        }

        for (int i = 0; i < slotCnt; i++) //채워진 리스트에서 스킬을 빼준다.. 이러면 중복이 안생기지
        {
            var slot = slotRoot.GetChild(i).GetComponent<Slot>();

            int ran = Random.Range(0, createSlots.Count);
            SkillProperty skill = createSlots[ran];
            createSlots.RemoveAt(ran);
            slot.SetItem(skill); //아이템이 보여지는 부분
            slots.Add(slot); //슬롯의 개수를 (3개) 파악한 다음에 slots라는 리스트에 slot을 추가해줄거야
            if (GameManager.instance.player.PlayerLevel == 1) { break; } //1레벨에서는 바로 1번해주고 멈춘다.
        }
    }
    //아이템 클릭 함수
    public void OnClickSlot(Slot slot) 
    {
        if(onSlotClick != null) //상점에서 아이템을 클릭시 (null이 아닌 곳에)  
        {
            onSlotClick(slot.skill); //slot.skill을 넣어줘라
            StoreExit();
            createSlots.RemoveRange(0, createSlots.Count);
            
        }
    }
    //클릭 이후 이벤트
    private void StoreExit()
    {
        SoundManager.Instance.ButtonClickSound();
        GameManager.instance.isStore = false;
        GameManager.instance.store.SetActive(false);
        if (GameManager.instance.player.PlayerLevel == 1) { GameManager.instance.isGameStart = true; }
    } 
}
