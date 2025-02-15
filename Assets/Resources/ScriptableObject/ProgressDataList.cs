using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDataList : DataScriptableObjects
{    
    //key 는 (int,int), 순서대로 ProgressData의 UserID, StageID
    public Dictionary<(int, int), ProgressData> ProgressDataDic = new Dictionary<(int, int), ProgressData>();



    public List<ProgressData> ProgressDatas = new List<ProgressData>();
    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in ProgressDatas)
        {
            ProgressDataDic.Add((data.UserID, data.StageID), data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {
        foreach (var data in ProgressDatas)
        {
            data.Progress = ProgressDataDic[(data.UserID, data.StageID)].Progress;
        }
    }
}


public class ProgressData
{
    public int UserID;
    public int StageID;
    public int Progress;
}