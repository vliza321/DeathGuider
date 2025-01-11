using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProgressDataList", menuName = "ScriptableObject/ProgressData")]
public class ProgressDataList : DataScriptableObjects
{
    public List<ProgressData> ProgressDatas = new List<ProgressData>();

    public Dictionary<(int, int), ProgressData> ProgressDataDic = new Dictionary<(int, int), ProgressData>();

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


[System.Serializable]
public class ProgressData
{
    public int UserID;
    public int StageID;
    public int Progress;
}