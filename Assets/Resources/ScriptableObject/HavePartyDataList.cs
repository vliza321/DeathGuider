using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HavePartyDataList", menuName = "ScriptableObject/HavePartyData")]
public class HavePartyDataList : DataScriptableObjects
{
    public List<HavePartyData> HavePartyDatas = new List<HavePartyData>();

    public Dictionary<(int,int), HavePartyData> HavePartyDataDic = new Dictionary<(int,int), HavePartyData>();
    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in HavePartyDatas)
        {
            HavePartyDataDic.Add((data.UserID, data.PartyID), data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {

    }
}


[System.Serializable]
public class HavePartyData
{
    public int UserID;
    public int PartyID;
}