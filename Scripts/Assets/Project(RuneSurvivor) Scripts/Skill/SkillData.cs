using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
[System.Serializable]
public class SkillProperty
{
    public string skillName; //스킬이름
    public double skillRate; //스킬공격속도
    public int skillDamage; //스킬공격력
    public int skillLevel; //스킬레벨
    public string skillElemental; //스킬속성
    public Sprite skillImage; //스킬이미지
    public Skill skillData; //스킬 프리팹

    public SkillProperty(string skillName,  float skillRate, int skillDamage, int skillLevel, string skillElemental, Sprite skillImage, Skill skillData)//  , Sprite skillImage, Skill skillPrefab
    {
        this.skillName = skillName;
        this.skillImage = skillImage;
        this.skillRate = skillRate;
        this.skillDamage = skillDamage;
        this.skillLevel = skillLevel;
        this.skillElemental = skillElemental;
        this.skillData = skillData;
    }
    //스킬패턴 (나중에 추가)
}

[System.Serializable]
public class SkillData : MonoBehaviour
{
    public List<SkillProperty> skillLevel1DataList;
    public List<SkillProperty> skillLevel2DataList;
    public List<SkillProperty> skillLevel3DataList;
    public List<SkillProperty> skillLevel4DataList;
    public Skill[] skillLevel1Datas;
    public Skill[] skillLevel2Datas;
    public Skill[] skillLevel3Datas;
    public Skill[] skillLevel4Datas;
    void Awake()
    {
        Sprite[] spritesAll = Resources.LoadAll<Sprite>("Sprites/Skill/"); //스킬 이미지 가져옴

        //----------------------------------------------------------------------------스킬레벨 1
        Skill[] skillDatasLevel1 = Resources.LoadAll<Skill>("SkillPrefab/Level1");

        skillLevel1DataList.Add(new SkillProperty("WaterLevel1",  2, 10, 1, "Water", spritesAll[3], skillDatasLevel1[3])); //spritesAll[3], skillPrefabLevel1[1]
        skillLevel1DataList.Add(new SkillProperty("FireLevel1", 2, 10, 1, "Fire", spritesAll[0], skillDatasLevel1[0]));//  spritesAll[0], skillPrefabLevel1[0]
        skillLevel1DataList.Add(new SkillProperty("LightningLevel1",  2, 10, 1, "Lightning", spritesAll[1], skillDatasLevel1[1])); // spritesAll[1], skillPrefabLevel1[2]
        skillLevel1DataList.Add(new SkillProperty("RockLevel1", 2, 10, 1, "Rock", spritesAll[2], skillDatasLevel1[2])); //spritesAll[2],skillPrefabLevel1[3]

        //데이터 추가 시 사용
        //JsonData jsonSkillLevel1Data = JsonMapper.ToJson(skillLevel1DataList); //json으로 변환해서 저장 //json파일은 string으로만 저장됨
        //File.WriteAllText(Application.streamingAssetsPath + "/SkillData.json", jsonSkillLevel1Data.ToString());

        //json파일에서 바꿔줄수있게 사용
        string jsonSkillDataString = File.ReadAllText(Application.streamingAssetsPath + "/SkillData.json");
        JsonData skillData = JsonMapper.ToObject(jsonSkillDataString); //json형태의 string이였던 문자열이 다시 PlayerData 형에 맞게 변환됨.

        for (int i = 0; i < skillLevel1DataList.Count; i++) //스킬레벨1 리스트 데이터 저장
        {
            skillLevel1DataList[i].skillName = skillData[i]["skillName"].ToString();
            skillLevel1DataList[i].skillRate = (double)skillData[i]["skillRate"];
            skillLevel1DataList[i].skillDamage = (int)skillData[i]["skillDamage"];
            skillLevel1DataList[i].skillLevel = (int)skillData[i]["skillLevel"];
            skillLevel1DataList[i].skillElemental = skillData[i]["skillElemental"].ToString();

        }
        for (int i = 0; i < skillLevel1Datas.Length; i++) //각 1레벨 스킬에 데이터를 저장
        {
            skillLevel1Datas[i].skillName = skillLevel1DataList[i].skillName;
            skillLevel1Datas[i].skillRate = skillLevel1DataList[i].skillRate;
            skillLevel1Datas[i].skillDamage = skillLevel1DataList[i].skillDamage;
            skillLevel1Datas[i].skillLevel = skillLevel1DataList[i].skillLevel;
            skillLevel1Datas[i].skillElemental = skillLevel1DataList[i].skillElemental;

        }

        //----------------------------------------------------------------------------스킬레벨 2
        Skill[] skillDatasLevel2 = Resources.LoadAll<Skill>("SkillPrefab/Level2");
        
        skillLevel2DataList.Add(new SkillProperty("FireFireLevel2", 1.8f, 30, 2, "Fire", spritesAll[0], skillDatasLevel2[0])); // spritesAll[0],skillPrefabLevel2[0]
        skillLevel2DataList.Add(new SkillProperty("FireLightningLevel2", 1.8f, 30, 2, "Fire", spritesAll[0], skillDatasLevel2[1])); // spritesAll[0],skillPrefabLevel2[1]
        skillLevel2DataList.Add(new SkillProperty("FireRockLevel2", 1.8f, 30, 2, "Fire", spritesAll[0], skillDatasLevel2[2])); // spritesAll[0],skillPrefabLevel2[0]
        skillLevel2DataList.Add(new SkillProperty("WaterWaterLevel2", 1.8f, 20, 2, "Water", spritesAll[3], skillDatasLevel2[11])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel2DataList.Add(new SkillProperty("WaterFireLevel2", 1.8f, 20, 2, "Water", spritesAll[3], skillDatasLevel2[9])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel2DataList.Add(new SkillProperty("WaterLightningLevel2", 1.8f, 20, 2, "Water", spritesAll[3], skillDatasLevel2[10])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel2DataList.Add(new SkillProperty("LightningLightningLevel2", 1.8f, 20, 2, "Lightning", spritesAll[1], skillDatasLevel2[3])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel2DataList.Add(new SkillProperty("LightningRockLevel2", 1.8f, 20, 2, "Lightning", spritesAll[1], skillDatasLevel2[4])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel2DataList.Add(new SkillProperty("LightningWaterLevel2", 1.8f, 20, 2, "Lightning", spritesAll[1], skillDatasLevel2[5])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel2DataList.Add(new SkillProperty("RockRockLevel2", 1.8f, 30, 2, "Rock", spritesAll[2], skillDatasLevel2[7])); //spritesAll[2],skillPrefabLevel2[3]
        skillLevel2DataList.Add(new SkillProperty("RockFireLevel2", 1.8f, 30, 2, "Rock",spritesAll[2],skillDatasLevel2[6])); //spritesAll[2],skillPrefabLevel2[3]
        skillLevel2DataList.Add(new SkillProperty("RockWaterLevel2", 1.8f, 30, 2, "Rock", spritesAll[2], skillDatasLevel2[8])); //spritesAll[2],skillPrefabLevel2[3]

        //데이터 추가 시 사용
        //JsonData jsonSkillLevel2Data = JsonMapper.ToJson(skillLevel2DataList); //json으로 변환해서 저장 //json파일은 string으로만 저장됨
       // File.WriteAllText(Application.streamingAssetsPath + "/Skill2Data.json", jsonSkillLevel2Data.ToString());

        //json파일에서 바꿔줄수있게 사용
        string jsonSkill2DataString = File.ReadAllText(Application.streamingAssetsPath + "/Skill2Data.json");
        JsonData skillData2 = JsonMapper.ToObject(jsonSkill2DataString); //json형태의 string이였던 문자열이 다시 PlayerData 형에 맞게 변환됨.

        for (int i = 0; i < skillLevel2DataList.Count; i++) //스킬레벨2 리스트 데이터 저장
        {
            skillLevel2DataList[i].skillName = skillData2[i]["skillName"].ToString();
            skillLevel2DataList[i].skillRate = (double)skillData2[i]["skillRate"];
            skillLevel2DataList[i].skillDamage = (int)skillData2[i]["skillDamage"];
            skillLevel2DataList[i].skillLevel = (int)skillData2[i]["skillLevel"];
            skillLevel2DataList[i].skillElemental = skillData2[i]["skillElemental"].ToString();

        }
        for (int i = 0; i < skillLevel2Datas.Length; i++) //각 2레벨 스킬에 데이터를 저장
        {
            skillLevel2Datas[i].skillName = skillLevel2DataList[i].skillName;
            skillLevel2Datas[i].skillRate = skillLevel2DataList[i].skillRate;
            skillLevel2Datas[i].skillDamage = skillLevel2DataList[i].skillDamage;
            skillLevel2Datas[i].skillLevel = skillLevel2DataList[i].skillLevel;
            skillLevel2Datas[i].skillElemental = skillLevel2DataList[i].skillElemental;

        }
        //----------------------------------------------------------------------------스킬레벨 3
        Skill[] skillDatasLevel3 = Resources.LoadAll<Skill>("SkillPrefab/Level3");

        skillLevel3DataList.Add(new SkillProperty("FireFireLevel3", 1.4f, 100, 3, "Fire", spritesAll[0], skillDatasLevel3[0])); // spritesAll[0],skillPrefabLevel2[0]
        skillLevel3DataList.Add(new SkillProperty("FireLightningLevel3", 1.4f, 100, 3, "Fire", spritesAll[0], skillDatasLevel3[1])); // spritesAll[0],skillPrefabLevel2[0]
        skillLevel3DataList.Add(new SkillProperty("FireRockLevel3", 1.4f, 100, 3, "Fire", spritesAll[0], skillDatasLevel3[2])); // spritesAll[0],skillPrefabLevel2[0]
        skillLevel3DataList.Add(new SkillProperty("WaterWaterLevel3", 1.4f, 100, 3, "Water", spritesAll[3], skillDatasLevel3[11])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel3DataList.Add(new SkillProperty("WaterFireLevel3", 1.4f, 100, 3, "Water", spritesAll[3], skillDatasLevel3[9])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel3DataList.Add(new SkillProperty("WaterLightningLevel3", 1.4f, 100, 3, "Water", spritesAll[3], skillDatasLevel3[10])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel3DataList.Add(new SkillProperty("LightningLightningLevel3", 1.4f, 100, 3, "Lightning", spritesAll[1], skillDatasLevel3[3])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel3DataList.Add(new SkillProperty("LightningRockLevel3", 1.4f, 100, 3, "Lightning", spritesAll[1], skillDatasLevel3[4])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel3DataList.Add(new SkillProperty("LightningWaterLevel3", 1.4f, 100, 3, "Lightning", spritesAll[1], skillDatasLevel3[5])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel3DataList.Add(new SkillProperty("RockRockLevel3", 1.4f, 100, 3, "Rock", spritesAll[2], skillDatasLevel3[7])); //spritesAll[2],skillPrefabLevel2[3]
        skillLevel3DataList.Add(new SkillProperty("RockFireLevel3", 1.4f, 100, 3, "Rock", spritesAll[2], skillDatasLevel3[6])); //spritesAll[2],skillPrefabLevel2[3]
        skillLevel3DataList.Add(new SkillProperty("RockWaterLevel3", 1.4f, 100, 3, "Rock", spritesAll[2], skillDatasLevel3[8])); //spritesAll[2],skillPrefabLevel2[3]

        //데이터 추가 시 사용
        //JsonData jsonSkillLevel3Data = JsonMapper.ToJson(skillLevel3DataList); //json으로 변환해서 저장 //json파일은 string으로만 저장됨
        //File.WriteAllText(Application.streamingAssetsPath + "/Skill3Data.json", jsonSkillLevel3Data.ToString());

        //json파일에서 바꿔줄수있게 사용
        string jsonSkill3DataString = File.ReadAllText(Application.streamingAssetsPath + "/Skill3Data.json");
        JsonData skillData3 = JsonMapper.ToObject(jsonSkill3DataString); //json형태의 string이였던 문자열이 다시 PlayerData 형에 맞게 변환됨.

        for (int i = 0; i < skillLevel3DataList.Count; i++) //스킬레벨3 리스트 데이터 저장
        {
            skillLevel3DataList[i].skillName = skillData3[i]["skillName"].ToString();
            skillLevel3DataList[i].skillRate = (double)skillData3[i]["skillRate"];
            skillLevel3DataList[i].skillDamage = (int)skillData3[i]["skillDamage"];
            skillLevel3DataList[i].skillLevel = (int)skillData3[i]["skillLevel"];
            skillLevel3DataList[i].skillElemental = skillData3[i]["skillElemental"].ToString();

        }
        for (int i = 0; i < skillLevel3Datas.Length; i++) //각 3레벨 스킬에 데이터를 저장
        {
            skillLevel3Datas[i].skillName = skillLevel3DataList[i].skillName;
            skillLevel3Datas[i].skillRate = skillLevel3DataList[i].skillRate;
            skillLevel3Datas[i].skillDamage = skillLevel3DataList[i].skillDamage;
            skillLevel3Datas[i].skillLevel = skillLevel3DataList[i].skillLevel;
            skillLevel3Datas[i].skillElemental = skillLevel3DataList[i].skillElemental;

        }
        //----------------------------------------------------------------------------스킬레벨 3
        Skill[] skillDatasLevel4 = Resources.LoadAll<Skill>("SkillPrefab/Level4");

        skillLevel4DataList.Add(new SkillProperty("FireFireLevel4", 1.0f, 200, 4, "Fire", spritesAll[0], skillDatasLevel4[0])); // spritesAll[0],skillPrefabLevel2[0]
        skillLevel4DataList.Add(new SkillProperty("FireLightningLevel4", 1.0f, 200, 4, "Fire", spritesAll[0], skillDatasLevel4[1])); // spritesAll[0],skillPrefabLevel2[0]
        skillLevel4DataList.Add(new SkillProperty("FireRockLevel4", 1.0f, 200, 4, "Fire", spritesAll[0], skillDatasLevel4[2])); // spritesAll[0],skillPrefabLevel2[0]
        skillLevel4DataList.Add(new SkillProperty("WaterWaterLevel4", 1.0f, 200, 4, "Water", spritesAll[3], skillDatasLevel4[11])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel4DataList.Add(new SkillProperty("WaterFireLevel4", 1.0f, 200, 4, "Water", spritesAll[3], skillDatasLevel4[9])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel4DataList.Add(new SkillProperty("WaterLightningLevel4", 1.0f, 200, 4, "Water", spritesAll[3], skillDatasLevel4[10])); // spritesAll[3],skillPrefabLevel2[1]
        skillLevel4DataList.Add(new SkillProperty("LightningLightningLevel4", 1.0f, 200, 4, "Lightning", spritesAll[1], skillDatasLevel4[3])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel4DataList.Add(new SkillProperty("LightningRockLevel4", 1.0f, 200, 4, "Lightning", spritesAll[1], skillDatasLevel4[4])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel4DataList.Add(new SkillProperty("LightningWaterLevel4", 1.0f, 200, 4, "Lightning", spritesAll[1], skillDatasLevel4[5])); //spritesAll[1],skillPrefabLevel2[2]
        skillLevel4DataList.Add(new SkillProperty("RockRockLevel4", 1.0f, 200, 4, "Rock", spritesAll[2], skillDatasLevel4[7])); //spritesAll[2],skillPrefabLevel2[3]
        skillLevel4DataList.Add(new SkillProperty("RockFireLevel4", 1.0f, 200, 4, "Rock", spritesAll[2], skillDatasLevel4[6])); //spritesAll[2],skillPrefabLevel2[3]
        skillLevel4DataList.Add(new SkillProperty("RockWaterLevel4", 1.0f, 200, 4, "Rock", spritesAll[2], skillDatasLevel4[8])); //spritesAll[2],skillPrefabLevel2[3]

        //데이터 추가 시 사용
        //JsonData jsonSkillLevel4Data = JsonMapper.ToJson(skillLevel4DataList); //json으로 변환해서 저장 //json파일은 string으로만 저장됨
        //File.WriteAllText(Application.streamingAssetsPath + "/Skill4Data.json", jsonSkillLevel4Data.ToString());

        //json파일에서 바꿔줄수있게 사용
        string jsonSkill4DataString = File.ReadAllText(Application.streamingAssetsPath + "/Skill4Data.json");
        JsonData skillData4 = JsonMapper.ToObject(jsonSkill4DataString); //json형태의 string이였던 문자열이 다시 PlayerData 형에 맞게 변환됨.

        for (int i = 0; i < skillLevel4DataList.Count; i++) //스킬레벨4 리스트 데이터 저장
        {
            skillLevel4DataList[i].skillName = skillData4[i]["skillName"].ToString();
            skillLevel4DataList[i].skillRate = (double)skillData4[i]["skillRate"];
            skillLevel4DataList[i].skillDamage = (int)skillData4[i]["skillDamage"];
            skillLevel4DataList[i].skillLevel = (int)skillData4[i]["skillLevel"];
            skillLevel4DataList[i].skillElemental = skillData4[i]["skillElemental"].ToString();

        }
        for (int i = 0; i < skillLevel4Datas.Length; i++) //각 4레벨 스킬에 데이터를 저장
        {
            skillLevel4Datas[i].skillName = skillLevel4DataList[i].skillName;
            skillLevel4Datas[i].skillRate = skillLevel4DataList[i].skillRate;
            skillLevel4Datas[i].skillDamage = skillLevel4DataList[i].skillDamage;
            skillLevel4Datas[i].skillLevel = skillLevel4DataList[i].skillLevel;
            skillLevel4Datas[i].skillElemental = skillLevel4DataList[i].skillElemental;

        }
    }
}

