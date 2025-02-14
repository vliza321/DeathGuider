using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponDataList : DataScriptableObjects
{   
    //key 는 (int,int,int), 순서대로 WeaponData의 UserID,PrototypeWeaponID, InstanceID 
    public Dictionary<(int, int, int), WeaponData> WeaponDataDic = new Dictionary<(int, int, int), WeaponData>();
    
    
    public List<WeaponData> WeaponDatas = new List<WeaponData>();

    public bool TranslateListToDic()
    {
        bool result = true;
        int zero = 0;
        foreach (var data in WeaponDatas)
        {
            zero = data.PrototypeWeaponID % 1000;
            data.Rank = zero;
           WeaponDataDic.Add((data.UserID,data.PrototypeWeaponID,data.InstanceID), data);
        }

        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {
        foreach (var data in WeaponDatas)
        {
            data.PrototypeWeaponID = WeaponDataDic[(data.UserID,data.PrototypeWeaponID,data.InstanceID)].PrototypeWeaponID;
            data.Name = WeaponDataDic[(data.UserID,data.PrototypeWeaponID,data.InstanceID)].Name;
            data.AttackPoint = WeaponDataDic[(data.UserID, data.PrototypeWeaponID, data.InstanceID)].AttackPoint;
            data.Durability = WeaponDataDic[(data.UserID, data.PrototypeWeaponID, data.InstanceID)].Durability;
            data.Enforce= WeaponDataDic[(data.UserID, data.PrototypeWeaponID, data.InstanceID)].Enforce;
        }
    }
}

[System.Serializable]
public class WeaponData
{
    public int UserID;
    public int PrototypeWeaponID;
    public int InstanceID;
    public string Name;
    public int AttackPoint;
    public int Durability;
    public int Type;
    public int Enforce;
    public int Crime;
    public int Rank; //등급
    public int EffectID;
    public int ActivityStatus;//CSV 추가 필요
}
