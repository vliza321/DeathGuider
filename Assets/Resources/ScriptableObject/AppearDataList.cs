using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AppearDataList", menuName = "ScriptableObject/AppearData")]
public class AppearDataList : DataScriptableObjects
{
    public List<AppearData> AppearDatas = new List<AppearData>();

    public Dictionary<(int,int), AppearData> AppearDataDic = new Dictionary<(int,int),AppearData>();
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

[System.Serializable]
public class AppearData
{
    public int StageID;
    public int MonsterID;
}