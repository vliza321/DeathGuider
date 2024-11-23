using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrisonerDataList", menuName = "ScriptableObject/PrisonerData")]
public class PrisonerDataList : DataScriptableObjects
{
    public List<PrisonerData> PrisonerDatas = new List<PrisonerData>();
}


[System.Serializable]
public class PrisonerData
{
    public int id;
    public string Name;
    public int Head;
    public int Body;
    public int Shadow;
}