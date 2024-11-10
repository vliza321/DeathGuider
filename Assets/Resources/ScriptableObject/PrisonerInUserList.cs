using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrisonerInUserDataList", menuName = "ScriptableObject/PrisonerInUserData")]

public class PrisonerInUserDataList : ScriptableObject
{
    public List<PrisonerInUserData> PrisonerInUserDatas = new List<PrisonerInUserData>();
}


[System.Serializable]
public class PrisonerInUserData
{
    public int id;
    public string Name;
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