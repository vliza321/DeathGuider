using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrototypeUnitDataList", menuName = "ScriptableObject/PrototypeUnitData")]
public class PrototypeUnitDataList : DataScriptableObjects
{
    public List<PrototypeUnitData> PrototypeUnitDatas = new List<PrototypeUnitData>();

    public Dictionary<int, PrototypeUnitData> PrototypeUnitDataDic = new Dictionary<int, PrototypeUnitData>();
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