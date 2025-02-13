using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInWeapon : MonoBehaviour
{
    private float baseRuntime;
    private float runtime;
    private Vector3 moveDirection;
    private Transform attactDirection;
    private Vector3 worldPosition;
    private float playerToObjAngle;
    private float radian;
    public Transform AttactDirection
    {
        get { return attactDirection;}
        set { attactDirection = value; }
    }
    private LaunchTypeWeapon baseClass = new LaunchTypeWeapon();
    private Transform grandParentPos;
    private Vector3 cashingVector3;
    private Transform baseParent;
    private ProjectileInWeapon projectile;
    private Transform weaponEffectPool;
    private float rank;
    // Start is called before the first frame update
    public void Initialized(LaunchTypeWeapon baseClass, Transform baseParent, Transform Unit, Transform EffectPool, float rank)
    {
        weaponEffectPool = EffectPool;
        this.baseClass = baseClass;
        this.baseParent = baseParent;
        this.gameObject.transform.SetParent(baseParent);
        grandParentPos = Unit;
        projectile = this;
        radian = 180 / MathF.PI;
        playerToObjAngle = 0;
        baseRuntime = 2;
        runtime = baseRuntime;
        cashingVector3 = new Vector3(0, 0, 0);
        moveDirection = new Vector3(0, 0, 0);
        this.rank = rank;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        runtime -= Time.deltaTime;
        if (runtime < 0)
        {
            runtime = baseRuntime;
            DestoryProjectile();
        }
    }

    private void FixedUpdate()
    {
        
        this.gameObject.transform.position += moveDirection;
    }


    public void Execute(Transform AttactDirection)
    {
        this.transform.SetParent(weaponEffectPool);
        cashingVector3 = this.transform.localScale;
        cashingVector3.x = (cashingVector3.x >= 0) ? cashingVector3.x : -cashingVector3.x;
        cashingVector3.y = (cashingVector3.y >= 0) ? cashingVector3.y : -cashingVector3.y;
        cashingVector3.z = (cashingVector3.z >= 0) ? cashingVector3.z : -cashingVector3.z;

        this.transform.localScale = cashingVector3;

        attactDirection = AttactDirection;

        moveDirection.x = attactDirection.transform.position.x - grandParentPos.position.x;
        moveDirection.y = attactDirection.transform.position.y - grandParentPos.position.y;
        moveDirection = moveDirection.normalized * 0.05f * (rank + 4);

        playerToObjAngle = (Mathf.Atan2(moveDirection.y, moveDirection.x)) * radian;
        cashingVector3.x = 0;
        cashingVector3.y = 0;
        cashingVector3.z = playerToObjAngle;
        this.transform.eulerAngles = cashingVector3;

        this.gameObject.SetActive(true);
    }

    public void DestoryProjectile()
    {
        this.transform.parent = this.baseParent;
        this.gameObject.SetActive(false);
        this.gameObject.transform.localPosition = Vector3.zero;
        baseClass.Effects.Enqueue(projectile);
    }
}
