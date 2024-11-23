using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataList", menuName = "ScriptableObject/StageData")]
public class StageDataList : DataScriptableObjects
{ 
    public List<StageData> StageDatas = new List<StageData>();
}


[System.Serializable]
public class StageData
{
    public int id;
    public int IsClear;
    public int Progress;
    public int AppearingMonsterId1;
    public int AppearingMonsterId2;
    public int AppearingMonsterId3;
    public int BossMonsterId;
    public int BossMonsterIsClear;
    public int BossMonsterHealthPoint;
}