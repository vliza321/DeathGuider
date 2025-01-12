using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalUserDataList", menuName = "ScriptableObject/LocalUserData")]
public class LocalUserDataList : DataScriptableObjects
{
    //key ´Â int Çü, LocalUserDataÀÇ ID
    public Dictionary<int, LocalUserData> LocalUserDataDic = new Dictionary<int, LocalUserData>();


    public List<LocalUserData> LocalUserDatas = new List<LocalUserData>();
    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in LocalUserDatas)
        {
            LocalUserDataDic.Add(data.ID, data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {
        foreach (var data in LocalUserDatas)
        {
            data.Day = LocalUserDataDic[data.ID].Day;
            data.Gold = LocalUserDataDic[data.ID].Gold;
            data.DeathEssence = LocalUserDataDic[data.ID].DeathEssence;
            data.DarkEssence = LocalUserDataDic[data.ID].DarkEssence;
            data.UnitInstanceCounter = LocalUserDataDic[data.ID].UnitInstanceCounter;
            data.Floor = LocalUserDataDic[data.ID].Floor;
        }
    }
}


[System.Serializable]
public class LocalUserData
{
    public int ID;
    public int Day;
    public int Gold;
    public int DeathEssence;
    public int DarkEssence;
    public int UnitInstanceCounter;
    public int Floor;
}
