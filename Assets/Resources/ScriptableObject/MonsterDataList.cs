using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataList", menuName = "ScriptableObject/MonsterData")]
public class MonsterDataList : DataScriptableObjects
{
    public List<MonsterData> MonsterDatas = new List<MonsterData>();
}


[System.Serializable]
public class MonsterData
{
    public int id;
    public string Name;
    public int Level;
    public int HealthPoint;
}