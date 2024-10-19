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
    [SerializeField]
    private List<Transform> childList;

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
    public void ChangePos(Vector3 vector)
    {
        this.transform.position = vector;
    }

    public Weapon(Transform baseObjectTransform, Transform effectPool)
    {

    }
    public Weapon()
    { 

    }
    public Weapon(Transform baseObjectTransform, GameObject effectObject, AttackDirectional attackDirectional, Transform effectPool) 
    {

    }
    public Weapon(Transform baseObjectTransform, List<Transform> effectObject, AttackDirectional attackDirectional, Transform effectPool)
    {

    }


    public virtual void Init()
    {

    }
    public virtual void Init(Transform baseObjectTransform, GameObject effectObject, AttackDirectional attackDirectional, Transform effectPool)
    {

    }
    public virtual void Init(Transform baseObjectTransform, List<Transform> effectObject, AttackDirectional attackDirectional, Transform effectPool)
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

    public virtual void Execute()
    {

    }

    private void Awake()
    {
        if (this.transform.childCount == 0) return;

        Transform[] temtchild = new Transform[this.transform.childCount];
        temtchild = GetComponentsInChildren<Transform>();
        for(int i = 1; i<temtchild.Length;i++)
        {
            childList.Add(temtchild[i]);
        }
        temtchild = null;
    }

    private void Start()
    {
        if (weaponType == WeaponType.Close)
        {
            Debug.Log("create Close");
            weapon = new CloseTypeWeapon(this.transform,
            this.transform.GetChild(0).gameObject,
            this.transform.parent.GetComponent<PlayerMove>().AttactDirectional,
            this.transform.parent.parent.GetChild(this.transform.parent.parent.childCount - 1).transform);
        }
        if (weaponType == WeaponType.Launch)
        {
            Debug.Log("create Launch");
            weapon = new LaunchTypeWeapon(
            this.transform,
            childList,
            this.transform.parent.GetComponent<PlayerMove>().AttactDirectional,
            this.transform.parent.parent.GetChild(this.transform.parent.parent.childCount - 1).transform);
        }
        if (weaponType == WeaponType.Setallite)
        {
            Debug.Log("create Setallite");
            weapon = new SetalliteTypeWeaponManager(
            this.transform,
            this.transform.parent.parent.GetChild(this.transform.parent.parent.childCount - 1).transform);
        }
        weapon.Init();
        this.Init();
    }

    private void Update()
    {
        weapon.Execute();
    }

}
