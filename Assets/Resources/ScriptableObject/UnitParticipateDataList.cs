using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitParticipateDataList", menuName = "ScriptableObject/UnitParticipateData")]
public class UnitParticipateDataList : DataScriptableObjects
{
    public List<UnitParticipateData> UnitParticipateDatas = new List<UnitParticipateData>();

    public Dictionary<(int, int, int, int), UnitParticipateData> UnitParticipateDataDic = new Dictionary<(int, int, int, int), UnitParticipateData>();

    public bool TranslateListToDic()
    {
        bool result = true;
        foreach (var data in UnitParticipateDatas)
        {
            UnitParticipateDataDic.Add((data.UserID, data.PrototypeUnitID, data.InstanceID, data.PartyID), data);
        }
        return result;
    }

    public void TranslateDicToListAtSaveDatas()
    {
        UnitParticipateDatas.Clear();
    }
}


[System.Serializable]
public class UnitParticipateData
{
    public int UserID;
    public int PrototypeUnitID;
    public int InstanceID;
    public int PartyID;
    public int Position;

}