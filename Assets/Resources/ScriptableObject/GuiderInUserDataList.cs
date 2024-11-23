using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuiderInUserDataList", menuName = "ScriptableObject/GuiderInUserData")]
public class GuiderInUserDataList : DataScriptableObjects
{
    public List<GuiderInUserData> GuiderInUserDatas = new List<GuiderInUserData>();
}


[System.Serializable]
public class GuiderInUserData
{
    public int id;
    public string Name;
    public int Release;
    public int Level;
    public int HealthPoint;
    public int NowHealthPoint;
    public int HealthPointLevel;
    public int AttackPoint;
    public int AttackPointLevel;
    public int Crime1;
    public int Crime2;
    public int Crime3;
    public int Head;
    public int Body;
    public int Shadow;
}