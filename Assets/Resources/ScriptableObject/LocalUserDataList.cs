using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalUserDataList", menuName = "ScriptableObject/LocalUserData")]
public class LocalUserDataList : ScriptableObject
{
    public List<LocalUserData> LocalUserDatas = new List<LocalUserData>();
}


[System.Serializable]
public class LocalUserData
{
    public int id;
    public int clearStage;
    public int Gold;
    public int DeathEssence;
    public int DarkStrength;
    public int date;
    public int Gid;
    public int Pid1;
    public int Pid2;
    public int Pid3;
    public int Pid4;
    public int Wid1;
    public int Wid2;
    public int Wid3;
    public int Wid4;
    public int Wid5;
}
