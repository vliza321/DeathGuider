using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInWeapon : MonoBehaviour
{
    private float baseRuntime;
    [SerializeField]
    private float runtime;
    [SerializeField]
    private Vector3 moveDirection;
    private Transform attactDirection;
    [SerializeField]
    private Vector3 worldPosition;
    [SerializeField]
    private float playerToObjAngle;
    private float radian;
    public Transform AttactDirection
    {
        get { return attactDirection;}
        set { attactDirection = value; }
    }

    private Vector3 grandParentPos;
    private Vector3 cashingVector3;
    private GameObject baseParent;
    // Start is called before the first frame update
    public void Init()
    {
        baseParent = this.transform.parent.gameObject;
        radian = 180 / MathF.PI;
        playerToObjAngle = 0;
        moveDirection = new Vector3(0, 0, 0);
        baseRuntime = 2;
        runtime = baseRuntime;
        grandParentPos = this.transform.parent.parent.transform.position;
        cashingVector3 = new Vector3(0, 0, 0);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        runtime -= Time.deltaTime;
        if(runtime <0)
        {
            DestoryProjectile();
        }
    }

    private void FixedUpdate()
    {
        
        this.gameObject.transform.position += moveDirection;
    }


    public void Execute(Transform EffectPool, Transform WeaponUserUnit, Transform AttactDirection)
    {

        this.transform.parent = EffectPool;

        cashingVector3 = this.transform.localScale;
        cashingVector3.x = (cashingVector3.x >= 0) ? cashingVector3.x : -cashingVector3.x;
        cashingVector3.y = (cashingVector3.y >= 0) ? cashingVector3.y : -cashingVector3.y;
        cashingVector3.z = (cashingVector3.z >= 0) ? cashingVector3.z : -cashingVector3.z;

        this.transform.localScale = cashingVector3;

        grandParentPos = WeaponUserUnit.position;
        attactDirection = AttactDirection;

        moveDirection.x = attactDirection.transform.position.x - grandParentPos.x;
        moveDirection.y = attactDirection.transform.position.y - grandParentPos.y;
        moveDirection = moveDirection.normalized * 0.2f;

        playerToObjAngle = (Mathf.Atan2(moveDirection.y, moveDirection.x)) * radian;
        cashingVector3.x = 0;
        cashingVector3.y = 0;
        cashingVector3.z = playerToObjAngle;
        this.transform.eulerAngles = cashingVector3;

        runtime = baseRuntime;

        this.gameObject.SetActive(true);
    }

    public void DestoryProjectile()
    {
        this.transform.parent = this.baseParent.transform;
        this.gameObject.SetActive(false);
        this.gameObject.transform.localPosition = Vector3.zero;
    }
}
