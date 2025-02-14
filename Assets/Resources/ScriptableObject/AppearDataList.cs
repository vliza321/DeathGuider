using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearDataList : DataScriptableObjects
{
    //key 는 (int,int), 순서대로 AppearData의 StageID, MonsterID
    public Dictionary<(int,int), AppearData> AppearDataDic = new Dictionary<(int,int),AppearData>();


    public List<AppearData> AppearDatas = new List<AppearData>();
    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in AppearDatas)
        {
            AppearDataDic.Add((data.StageID, data.MonsterID),data);
        }
        if(AppearDatas.Count != AppearDataDic.Count)
        {
            result = false;
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {

    }
}

public class AppearData
{
    public int StageID;
    public int MonsterID;
}