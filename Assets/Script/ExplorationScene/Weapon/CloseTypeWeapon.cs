using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTypeWeapon : Weapon
{
    
    private GameObject effectObject;
    private float baseCoolTime;
    private float coolTimer;
    private Animator effectAnim;
    [SerializeField]
    private AttackDirectional playerDirectional;
    private Transform weaponEffectPool;
    private WeaponData weaponData;
    private Vector3 cachingVector;
    private Transform baseParent;
    private Queue<(GameObject,Animator)> effects = new Queue<(GameObject, Animator)>();
    (GameObject, Animator) cachingTuple;
    public float BaseCoolTime
    {
        get { return baseCoolTime; }
        set { baseCoolTime = value; }
    }

    public Queue<(GameObject, Animator)> Effects
    {
        get { return effects; }
        set { effects = value; }
    }

    public CloseTypeWeapon()
    {

    }
    public CloseTypeWeapon(Transform baseObjectTransform, GameObject effectObject, AttackDirectional attackDirectional, Transform effectPool, WeaponData weaponData) 
        : base(baseObjectTransform, effectObject, attackDirectional, effectPool, weaponData)
    
    {
        this.weaponData = weaponData;
        weaponEffectPool = effectPool;
        baseParent = baseObjectTransform;
        cachingVector = new Vector3(0, 0, 0);

        this.effectObject = effectObject;
        effectAnim = effectObject.GetComponent<Animator>();
        
        playerDirectional = attackDirectional;
        
        cachingTuple = (this.effectObject, effectAnim);
        this.effectObject.SetActive(false);
    }

    public override void Init(Dictionary<GameObject, float> weaponDamage, float damage, Transform Unit)
    {
        baseCoolTime = 40 / (Mathf.Pow((4 + weaponData.Rank), (2.1f)) + (int)(weaponData.Enforce / 2.4f));
        coolTimer = baseCoolTime;

        for (int i =0; i<2/baseCoolTime+1;i++)
        {
            GameObject newEffect = Instantiate(effectObject);
            newEffect.GetComponent<QuitAnim>().Initialized(this,baseParent);
            cachingTuple = (newEffect, newEffect.GetComponent<Animator>());
            effects.Enqueue(cachingTuple);
            weaponDamage.Add(newEffect, damage);
        }

    }

    public override void Execute()
    {
        coolTimer -= Time.deltaTime;

        if (coolTimer < 0)
        {

            if (effects.Count == 0)
            {
                return;
            }
            else
            {
                cachingTuple = effects.Dequeue();
                cachingTuple.Item1.SetActive(true);
                cachingTuple.Item1.transform.position = cachingVector;
                cachingTuple.Item2.SetBool("isActive", true);

                cachingVector = playerDirectional.transform.position;
                coolTimer = baseCoolTime;
                cachingTuple.Item1.transform.position = cachingVector;
                cachingTuple.Item1.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
