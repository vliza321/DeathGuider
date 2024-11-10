using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInUserDataList", menuName = "ScriptableObject/WeaponInUserData")]
public class WeaponInUserDataList : ScriptableObject
{
    public List<WeaponInUserData> WeaponInUserDatas = new List<WeaponInUserData>();
}

[System.Serializable]
public class WeaponInUserData
{
    public int id;
    public int Type;
    public string Name;
    public int Level;
    public int AttackPoint;
    public int AttackPointLevel;
    public int Crime;
}
