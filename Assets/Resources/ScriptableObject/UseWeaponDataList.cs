using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UseWeaponDataList", menuName = "ScriptableObject/UseWeaponData")]
public class UseWeaponDataList : DataScriptableObjects
{   
    //key 는 (int,int,int,int), 순서대로 UseWeaponData의 UserID,PrototypeWeaponID, InstanceID, PartyID 
    public Dictionary<(int, int, int, int), UseWeaponData> UseWeaponDataDic = new Dictionary<(int, int, int, int), UseWeaponData>();
    
    
    public List<UseWeaponData> UseWeaponDatas = new List<UseWeaponData>();

    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in UseWeaponDatas)
        {
            UseWeaponDataDic.Add((data.UserID, data.PrototypeWeaponID, data.InstanceID, data.PartyID), data);
        }
        return result;
    }
    public void TranslateDicToListAtSaveDatas()
    {
        UseWeaponDatas.Clear();
    }
}


[System.Serializable]
public class UseWeaponData
{
    public int UserID;
    public int PrototypeWeaponID;
    public int InstanceID;
    public int PartyID;
    public int Position;
}