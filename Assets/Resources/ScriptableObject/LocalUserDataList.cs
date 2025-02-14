using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            data.BusEnhance = LocalUserDataDic[data.ID].BusEnhance;
            data.PrisonEnhance = LocalUserDataDic[data.ID].PrisonEnhance;
            data.HealthEnhance = LocalUserDataDic[data.ID].HealthEnhance;
            data.ErosionEnhance = LocalUserDataDic[data.ID].ErosionEnhance;
            data.GYMEnhance = LocalUserDataDic[data.ID].GYMEnhance;
            data.SmithEnhance = LocalUserDataDic[data.ID].SmithEnhance;
            data.BattleEfficiency = LocalUserDataDic[data.ID].BattleEfficiency;
            data.BattleReward = LocalUserDataDic[data.ID].BattleReward;
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
    public int BusEnhance;
    public int PrisonEnhance;
    public int HealthEnhance;
    public int ErosionEnhance;
    public int GYMEnhance;
    public int SmithEnhance;
    public int BattleEfficiency;
    public int BattleReward;
}
