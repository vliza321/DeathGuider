using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataList", menuName = "ScriptableObject/WeaponData")]
public class WeaponDataList : DataScriptableObjects
{
    public List<WeaponData> WeaponDatas = new List<WeaponData>();

    public Dictionary<(int, int, int), WeaponData> WeaponDataDic = new Dictionary<(int, int, int), WeaponData>();

    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in WeaponDatas)
        {
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
            data.EffectID = WeaponDataDic[(data.UserID, data.PrototypeWeaponID, data.InstanceID)].EffectID;
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
    public int EffectID;
}
