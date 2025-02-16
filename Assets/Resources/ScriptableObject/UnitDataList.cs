using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDataList : DataScriptableObjects
{
    //key 는 (int,int,int), 순서대로 UnitData의 UserID,PrototypeUnitID, InstanceID 
    public Dictionary<(int, int, int), UnitData> UnitDataDic = new Dictionary<(int, int, int), UnitData>();

    
    public List<UnitData> UnitDatas = new List<UnitData>();

    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in UnitDatas)
        {
            UnitDataDic.Add((data.UserID, data.PrototypeUnitID, data.InstanceID), data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {
        foreach (var data in UnitDatas)
        {
            data.Name = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].Name;
            data.Level = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].Level;
            data.EXP = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].EXP;
            data.MaxHealthPoint = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].MaxHealthPoint;
            data.HealthPoint = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].HealthPoint;
            data.Strength = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].Strength;
            data.Defense = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].Defense;
            data.Handicraft = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].Handicraft;
            data.DeathErosion = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].DeathErosion;
            data.Enforce = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].Enforce;
            data.HealthEnforce = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].HealthEnforce;
            data.StrengthEnforce = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].StrengthEnforce;
            data.DefenseEnforce = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].DefenseEnforce;
            data.HandicraftEnforce = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].HandicraftEnforce;
            data.ActivityStatus = UnitDataDic[(data.UserID, data.PrototypeUnitID, data.InstanceID)].ActivityStatus;
        }
    }
}


public class UnitData
{
    public int UserID;
    public int PrototypeUnitID;
    public int InstanceID;
    public string Name;
    public int Level;
    public int EXP;
    public int MaxHealthPoint;
    public int HealthPoint;
    public int Strength;
    public int Defense;
    public float Handicraft;
    public int DeathErosion;
    public int Enforce;
    public int HealthEnforce;
    public int StrengthEnforce;
    public int DefenseEnforce;
    public int HandicraftEnforce;
    public int Crime;
    public int ActivityStatus;
    public int HeadID;
    public int BodyID;
}