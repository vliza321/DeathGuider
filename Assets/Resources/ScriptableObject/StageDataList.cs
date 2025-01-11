using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataList", menuName = "ScriptableObject/StageData")]
public class StageDataList : DataScriptableObjects
{   
    //key ´Â int Çü, StageDataÀÇ ID
    public Dictionary<int, StageData> StageDataDic = new Dictionary<int, StageData>();
    
    
    public List<StageData> StageDatas = new List<StageData>();


    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in StageDatas)
        {
            StageDataDic.Add(data.ID, data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {
        foreach (var data in StageDatas)
        {
            data.IsOpen = StageDataDic[data.ID].IsOpen;
        }
    }
}


[System.Serializable]
public class StageData
{
    public int ID;
    public string Name;
    public string Dialog;
    public int IsOpen;
}