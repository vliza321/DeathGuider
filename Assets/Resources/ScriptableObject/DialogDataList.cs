using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogDataList",menuName = "ScriptableObject/DialogData")]
public class DialogDataList : ScriptableObject
{
    public List<DialogData> dialogDatas = new List<DialogData>();
}

[System.Serializable]
public class DialogData
{
    public int Number;
    public int CustomerID;
    public string State;
    public string Content;
}