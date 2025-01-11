using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataList", menuName = "ScriptableObject/MonsterData")]
public class MonsterDataList : DataScriptableObjects
{
    //key ´Â int Çü, MonsterDataÀÇ ID
    public Dictionary<int, MonsterData> MonsterDataDic = new Dictionary<int, MonsterData>();



    public List<MonsterData> MonsterDatas = new List<MonsterData>();
    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in MonsterDatas)
        {
            MonsterDataDic.Add(data.ID, data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {

    }
}


[System.Serializable]
public class MonsterData
{
    public int ID;
    public string Name;
    public int MaxHealthPoint;
    public int Strength;
    public int Defense;
}