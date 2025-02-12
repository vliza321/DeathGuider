using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTypeWeapon : Weapon
{
    private int baseCoolTime;
    private int coolTimer;
    [SerializeField]
    private AttackDirectional playerAttackDirectional;
    [SerializeField]
    private ProjectileInWeapon[] projectile;
    private Transform weaponEffectPool;
    private Transform baseParent;
    public LaunchTypeWeapon(Transform baseObjectTransform, List<Transform> effectObject, AttackDirectional attackDirectional, Transform effectPool)
        : base(baseObjectTransform, effectObject, attackDirectional, effectPool)
    {
        weaponEffectPool = effectPool;
        baseParent = baseObjectTransform;
        playerAttackDirectional = attackDirectional;
        projectile = new ProjectileInWeapon[effectObject.Count];
        for (int eo = 0; eo < effectObject.Count; eo++)
        {
            projectile[eo] = effectObject[eo].GetComponent<ProjectileInWeapon>();
        }
        baseCoolTime = 200;
        coolTimer = baseCoolTime;
        foreach (var p in projectile)
        {
            p.gameObject.SetActive(false);
            p.AttactDirection = playerAttackDirectional.transform;
            p.Init();
        }
    }
    // Start is called before the first frame update
    public override void Init()
    {

    }

    // Update is called once per frame
    public override void Execute()
    {
        
        coolTimer--;
        if (coolTimer < 0)
        {
            foreach(var p in projectile)
            {
                if(p.transform.gameObject.activeSelf == false)
                {
                    p.gameObject.SetActive(true);
                    p.Execute(weaponEffectPool, baseParent, playerAttackDirectional.transform);
                    coolTimer = baseCoolTime;
                    break;
                }
            }
        }
    }

}
