using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossMonsterDataList", menuName = "ScriptableObject/BossMonsterData")]
public class BossMonsterDataList : ScriptableObject
{
    public List<BossMonsterData> BossMonsterDatas = new List<BossMonsterData>();
}

[System.Serializable]
public class BossMonsterData
{
    public int id;
    public string Name;
    public int Level;
    public int HealthPoint;
    public int AttactPoint;
}