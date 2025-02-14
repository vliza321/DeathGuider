using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavePartyDataList : DataScriptableObjects
{   
    //key 는 (int,int), 순서대로 HavePartyData의 UserID, PartyID
    public Dictionary<(int,int), HavePartyData> HavePartyDataDic = new Dictionary<(int,int), HavePartyData>();
    
    
    
    public List<HavePartyData> HavePartyDatas = new List<HavePartyData>();
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