using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FIlENAME + "DataList"
public class DialogDataList : DataScriptableObjects
{
    //FIlENAME + "Datas"
    public List<DialogData> DialogDatas = new List<DialogData>();

    public Dictionary<int, DialogData> DialogDataDic = new Dictionary<int, DialogData>();
    public bool TranslateListToDic()
    {
        bool result = true;
        foreach(var data in DialogDatas)
        {
            DialogDataDic.Add(data.Number, data);        
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {

    }
}

public class DialogData
{
    public int Number;
    public int CustomerID;
    public string State;
    public string Content;
}

public class DataScriptableObjects
{

}