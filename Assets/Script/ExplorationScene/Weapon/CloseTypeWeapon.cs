using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTypeWeapon : Weapon
{
    
    private GameObject effectObject;
    private int baseCoolTime;
    private int coolTimer;
    private Animator effectAnim;
    private PolygonCollider2D effectCollider;
    [SerializeField]
    private AttackDirectional playerDirectional;
    private Transform weaponEffectPool;

    private Vector3 cashingVector;
    private Transform baseParent;
    public int BaseCoolTime
    {
        get { return baseCoolTime; }
        set { baseCoolTime = value; }
    }

    public CloseTypeWeapon(Transform baseObjectTransform, GameObject effectObject, AttackDirectional attackDirectional, Transform effectPool) 
        : base(baseObjectTransform, effectObject, attackDirectional, effectPool)
    {
        weaponEffectPool = effectPool;
        baseParent = baseObjectTransform;
        cashingVector = new Vector3(0, 0, 0);

        this.effectObject = effectObject;
        effectAnim = effectObject.GetComponent<Animator>();
        effectCollider = effectObject.GetComponent<PolygonCollider2D>();

        if (baseObjectTransform.parent.CompareTag("follower"))
        {
            playerDirectional = attackDirectional;
        }
        else
        {
            playerDirectional = attackDirectional;
        }

        baseCoolTime = 300; // -> 기본 쿨타임 set함수로 변경
        coolTimer = baseCoolTime;
    }

    public override void Init()
    {
        
    }

    public override void Execute()
    {
        
        coolTimer--;
        if (coolTimer < 0)
        {
            cashingVector = playerDirectional.transform.position;
            coolTimer = baseCoolTime;
            effectAnim.SetBool("isActive", true);
            effectCollider.enabled = true;
            effectObject.transform.parent = weaponEffectPool;
            effectObject.transform.position = cashingVector;
        }
    }
}
