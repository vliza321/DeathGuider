using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetalliteTypeWeaponManager : Weapon
{
    private int setalliteCount;
    private GameObject baseSetallite;
    
    private Transform baseObject;
    private Transform effectPool;
    private WeaponData weaponData;

    private Queue<SetalliteTypeWeapon> effects = new Queue<SetalliteTypeWeapon>();
    SetalliteTypeWeapon cachingObject;
    
    public SetalliteTypeWeaponManager(Transform BaseObjectTransform, Transform EffectPool, WeaponData weaponData) 
        : base(BaseObjectTransform, EffectPool, weaponData)
    {
        baseSetallite = BaseObjectTransform.GetChild(0).gameObject;
        baseObject = BaseObjectTransform;
        effectPool = EffectPool;
        this.weaponData = weaponData;
    }

    public override void Init(Dictionary<GameObject, float> weaponDamage, float damage, Transform Unit)
    {
        Vector3 cashingVector3 = new Vector3(0, 1, 0);
        float AngleColculateFloat;
        float PI = Mathf.PI;

        setalliteCount = 1 + (weaponData.Rank) * (2 + (int)(weaponData.Enforce / 2.4f)) + (int)(weaponData.Enforce / 3);

        for (int i = 0; i < setalliteCount; i++)
        {
            AngleColculateFloat = (360.0f / setalliteCount) * i / 180.0f * PI;
            cashingVector3.x = -Mathf.Sin(AngleColculateFloat);
            cashingVector3.y = Mathf.Cos(AngleColculateFloat);

            GameObject newEffect = Instantiate(baseSetallite, new Vector3(0, 0, 0), Quaternion.identity);
            newEffect.transform.localScale =  baseSetallite.transform.parent.parent.localScale;
            newEffect.transform.parent = effectPool;
            cachingObject = newEffect.GetComponent<SetalliteTypeWeapon>();
            cachingObject.Initialized(baseObject,cashingVector3);
            effects.Enqueue(cachingObject);
            weaponDamage.Add(newEffect, damage);
        }
        baseSetallite.SetActive(false);
    }
    public override void Execute()
    {
        foreach(var st in effects)
        {
            st.Execute();
        }
    }
}
