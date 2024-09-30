using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Setallite,
    Close,
    Launch
}

public class WeaponState
{
    private int level;
    private int attackPoint;
    private string crime;
    private WeaponType weaponType;
    private int growthRate;
    public int Level
    {
        get { return level; }
    }
    public int AttackPoint
    {
        get { return attackPoint; }
    }

    public string Crime
    {
        get { return crime; }
    }
    
    public WeaponType WeaponType
    {
        get { return weaponType; }
    }

    public void LevelUp()
    {
        level += 1;
        attackPoint += growthRate;
    }

    public WeaponState(int baseLevelIsOne,int attackPointIsInTable, string crimeTypeIsInTable, WeaponType weaponTypeIsInTable, int growthRateIsInTable) 
    {
        level = baseLevelIsOne;
        attackPoint = attackPointIsInTable;
        crime = crimeTypeIsInTable;
        weaponType = weaponTypeIsInTable;
        growthRate = growthRateIsInTable;
        Debug.Log(level);
        if( level != 1)
        {
            attackPoint = attackPointIsInTable + growthRate * (level-1);
        }
    }
}
