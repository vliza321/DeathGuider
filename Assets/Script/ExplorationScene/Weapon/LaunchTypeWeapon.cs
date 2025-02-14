using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTypeWeapon : Weapon
{
    private GameObject effectObject;
    private float baseCoolTime;
    private float coolTimer;
    [SerializeField]
    private AttackDirectional playerAttackDirectional;

    private Transform weaponEffectPool;
    private Transform baseParent;
    private Queue<ProjectileInWeapon> effects = new Queue<ProjectileInWeapon>();
    ProjectileInWeapon cachingObject;
    private WeaponData weaponData;
    public Queue<ProjectileInWeapon> Effects
    {
        get { return effects; }
        set { effects = value; }
    }

    public LaunchTypeWeapon()
    {

    }
    public LaunchTypeWeapon(Transform baseObjectTransform, GameObject effectObject, AttackDirectional attackDirectional, Transform effectPool, WeaponData weaponData)
        : base(baseObjectTransform, effectObject, attackDirectional, effectPool, weaponData)
    {
        this.weaponData = weaponData;
        weaponEffectPool = effectPool;
        baseParent = baseObjectTransform;

        this.effectObject = effectObject;

        playerAttackDirectional = attackDirectional;
        this.effectObject.SetActive(false);
    }
    // Start is called before the first frame update
    public override void Init(Dictionary<GameObject, float> weaponDamage, float damage, Transform Unit)
    {

        baseCoolTime = 30 / (Mathf.Pow((4 + weaponData.Rank), (2.1f)) + (int)(weaponData.Enforce / 2.4f));
        coolTimer = baseCoolTime;

        for (int i = 0; i < 3 / baseCoolTime + 1; i++)
        {
            GameObject newEffect = Instantiate(effectObject);
            cachingObject = newEffect.GetComponent<ProjectileInWeapon>();
            cachingObject.Initialized(this, baseParent, Unit, weaponEffectPool, weaponData.Rank);
            effects.Enqueue(cachingObject);
            weaponDamage.Add(newEffect, damage);
        }
    }

    // Update is called once per frame
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
                cachingObject = effects.Dequeue();
                cachingObject.gameObject.SetActive(true);
                cachingObject.transform.localScale = new Vector3(1, 1, 1);
                cachingObject.Execute(playerAttackDirectional.transform);
                coolTimer = baseCoolTime;
            }
        }

        /*
        foreach(var e in effects)
        {
            if(e.gameObject.activeSelf == true)
                e.Execute(playerAttackDirectional.transform);
        }*/
    }

}
