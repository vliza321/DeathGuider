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
    private WeaponData weaponData;
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

    public WeaponData WeaponData
    {
        get { return weaponData; }
        set { weaponData = value; }
    }

    public List<Transform> ChildList
    {
        get { return childList; }
        set { childList = value; }
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

    public Weapon(Transform baseObjectTransform, Transform effectPool, WeaponData weaponData)
    {

    }
    public Weapon()
    { 

    }
    public Weapon(Transform baseObjectTransform, GameObject effectObject, AttackDirectional attackDirectional, Transform effectPool, WeaponData weaponData) 
    {

    }
    public Weapon(Transform baseObjectTransform, List<Transform> effectObject, AttackDirectional attackDirectional, Transform effectPool, WeaponData weaponData)
    {

    }
    public virtual void Init()
    {

    }

    public virtual void Init(Dictionary<GameObject, float> weaponDamage, float damage, Transform Unit)
    {

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

    public void Initialize(Dictionary<GameObject, float> weaponDamage,float damage,Transform baseParent,WeaponData weaponData)
    {
        this.weaponData = weaponData;
        if (weaponType == WeaponType.Close)
        {
            weapon = new CloseTypeWeapon(
                this.transform,
                this.transform.GetChild(0).gameObject,
                this.transform.parent.GetComponent<PlayerMove>().AttactDirectional,
                baseParent.parent.GetChild(baseParent.parent.childCount -1),
                weaponData);
        }
        if (weaponType == WeaponType.Launch)
        {
            weapon = new LaunchTypeWeapon(
                this.transform,
                this.transform.GetChild(0).gameObject,
                this.transform.parent.GetComponent<PlayerMove>().AttactDirectional,
                baseParent.parent.GetChild(baseParent.parent.childCount - 1),
                weaponData);
        }
        if (weaponType == WeaponType.Setallite)
        {
            weapon = new SetalliteTypeWeaponManager(
                this.transform,
                baseParent.parent.GetChild(baseParent.parent.childCount - 1),
                weaponData);
        }
        weapon.Init(weaponDamage,damage, baseParent);
        //this.Init();
    }

    private void Update()
    {
        weapon.Execute();
    }

}
