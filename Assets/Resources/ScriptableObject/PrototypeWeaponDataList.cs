using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrototypeWeaponDataList", menuName = "ScriptableObject/PrototypeWeaponData")]
public class PrototypeWeaponDataList : DataScriptableObjects
{
    public List<PrototypeWeaponData> PrototypeWeaponDatas = new List<PrototypeWeaponData>();

    public Dictionary<int, PrototypeWeaponData> PrototypeWeaponDataDic = new Dictionary<int, PrototypeWeaponData>();
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
    public int EffectID;
}