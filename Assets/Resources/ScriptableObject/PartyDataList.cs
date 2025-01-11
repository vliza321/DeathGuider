using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartyDataList", menuName = "ScriptableObject/PartyData")]
public class PartyDataList : DataScriptableObjects
{
    public List<PartyData> PartyDatas = new List<PartyData>();

    public Dictionary<int, PartyData> PartyDataDic = new Dictionary<int, PartyData>();
    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in PartyDatas)
        {
            PartyDataDic.Add(data.ID, data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {
        
    }
}


[System.Serializable]
public class PartyData
{
    public int ID;
}