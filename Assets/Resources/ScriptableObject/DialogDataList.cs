using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogDataList",menuName = "ScriptableObject/DialogData")]
//FIlENAME + "DataList"
public class DialogDataList : ScriptableObject
{
    //FIlENAME + "Datas"
    public List<DialogData> DialogDatas = new List<DialogData>();
}

//FIlENAME + "Data"
[System.Serializable]
public class DialogData
{
    public int Number;
    public int CustomerID;
    public string State;
    public string Content;

}