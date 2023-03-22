using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Store : MonoBehaviour
{
    public SkillData skillData;
    public Transform slotRoot;
    public List<Slot> slots;
    public System.Action<SkillProperty> onSlotClick;

    public List<SkillProperty> createSlots; //������ ����Ʈ ���� 
    private void OnEnable()
    {
        slots = new List<Slot>();
        int slotCnt = slotRoot.childCount;
        int dataCnt;
        if (GameManager.instance.player.PlayerLevel != 1) { dataCnt = skillData.skillLevel1DataList.Count; }
        else { dataCnt = skillData.skillLevel1DataList.Count-1; }

        for(int i=0; i< dataCnt; i++) //���� �ִ� ��ų ä������ �κ�
        {
            createSlots.Add(skillData.skillLevel1DataList[i]);
        }

        for (int i = 0; i < slotCnt; i++) //ä���� ����Ʈ���� ��ų�� ���ش�.. �̷��� �ߺ��� �Ȼ�����
        {
            var slot = slotRoot.GetChild(i).GetComponent<Slot>();

            int ran = Random.Range(0, createSlots.Count);
            SkillProperty skill = createSlots[ran];
            createSlots.RemoveAt(ran);
            slot.SetItem(skill); //�������� �������� �κ�
            slots.Add(slot); //������ ������ (3��) �ľ��� ������ slots��� ����Ʈ�� slot�� �߰����ٰž�
            if (GameManager.instance.player.PlayerLevel == 1) { break; } //1���������� �ٷ� 1�����ְ� �����.
        }
    }
    //������ Ŭ�� �Լ�
    public void OnClickSlot(Slot slot) 
    {
        if(onSlotClick != null) //�������� �������� Ŭ���� (null�� �ƴ� ����)  
        {
            onSlotClick(slot.skill); //slot.skill�� �־����
            StoreExit();
            createSlots.RemoveRange(0, createSlots.Count);
            
        }
    }
    //Ŭ�� ���� �̺�Ʈ
    private void StoreExit()
    {
        SoundManager.Instance.ButtonClickSound();
        GameManager.instance.isStore = false;
        GameManager.instance.store.SetActive(false);
        if (GameManager.instance.player.PlayerLevel == 1) { GameManager.instance.isGameStart = true; }
    } 
}
