using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuiderDataList", menuName = "ScriptableObject/GuiderData")]
public class GuiderDataList : ScriptableObject
{
    public List<GuiderData> GuiderDatas = new List<GuiderData>();
}

[System.Serializable]
public class GuiderData
{
    public int id;
    public string Name;
    public int Head;
    public int Body;
    public int Shadow;

}