using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Setallite,
    Close,
    Launch
}

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponType weaponType;

    private Weapon weapon;
    private int level;
    private int attackPoint;
    private string crime;

    private int growthRate;
    private GameObject parentUnit;



    public GameObject ParentUnit
    {
        get { return parentUnit; }
        set { parentUnit = value; }
    }
       
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



    
    public Weapon()
    { 

    }

    
    public Weapon(Transform baseObjectTransform, GameObject effectObject, AttackDirectional attackDirectional, Transform effectPool) 
    {

    }

    public void ChangePos(Vector3 vector)
    {
        this.transform.position = vector;
    }
    public virtual void Execute()
    {

    }
    public virtual void Init()
    {

    }
    public virtual void Init(Transform baseObjectTransform, GameObject effectObject, AttackDirectional attackDirectional, Transform effectPool)
    {

    }

    public virtual void Init(int baseLevelIsOne, int attackPointIsInTable, string crimeTypeIsInTable, WeaponType weaponTypeIsInTable, int growthRateIsInTable, GameObject parent)
    {
        level = baseLevelIsOne;
        attackPoint = attackPointIsInTable;
        crime = crimeTypeIsInTable;
        weaponType = weaponTypeIsInTable;
        growthRate = growthRateIsInTable;

        Debug.Log(level);
        if (level != 1)
        {
            attackPoint = attackPointIsInTable + growthRate * (level - 1);
        }
    }

    private void Awake()
    {
        if (weaponType == WeaponType.Close)
        {
            Debug.Log("create Close");
            weapon = new CloseTypeWeapon(this.transform,
            this.transform.GetChild(0).gameObject,
            this.transform.parent.GetComponent<PlayerMove>().AttactDirectional,
            this.transform.parent.parent.GetChild(this.transform.parent.parent.childCount - 1).transform);
        }
    }

    private void Start()
    { 
        weapon.Init();
        this.Init();
    }

    private void Update()
    {
        weapon.Execute();
    }

}
