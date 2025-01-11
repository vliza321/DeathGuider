using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrototypeWeaponDataList", menuName = "ScriptableObject/PrototypeWeaponData")]
public class PrototypeWeaponDataList : DataScriptableObjects
{    
    //key ´Â int Çü, PrototypeWeaponDataÀÇ ID
    public Dictionary<int, PrototypeWeaponData> PrototypeWeaponDataDic = new Dictionary<int, PrototypeWeaponData>();


    public List<PrototypeWeaponData> PrototypeWeaponDatas = new List<PrototypeWeaponData>();
    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in PrototypeWeaponDatas)
        {
            PrototypeWeaponDataDic.Add(data.ID, data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {
        foreach (var data in PrototypeWeaponDatas)
        {
            data.InstanceCounter = PrototypeWeaponDataDic[data.ID].InstanceCounter;
        }
    }
}

[System.Serializable]
public class PrototypeWeaponData
{
    public int ID;
    public string Name;
    public int AttackPoint;
    public int Type;
    public int Crime;
    public int InstanceCounter;
}