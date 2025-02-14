using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeUnitDataList : DataScriptableObjects
{   
    //key ´Â int Çü, PrototypeUnitDataÀÇ ID
    public Dictionary<int, PrototypeUnitData> PrototypeUnitDataDic = new Dictionary<int, PrototypeUnitData>();

    public List<PrototypeUnitData> PrototypeUnitDatas = new List<PrototypeUnitData>();


    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in PrototypeUnitDatas)
        {
            PrototypeUnitDataDic.Add(data.ID, data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {

    }
}

[System.Serializable]
public class PrototypeUnitData
{
    public int ID;
    public string Name;
    public int MaxHealthPoint;
    public int Strength;
    public int Defense;
    public int Handicraft;
    public int Crime;
    public int HeadID;
    public int BodyID;
}