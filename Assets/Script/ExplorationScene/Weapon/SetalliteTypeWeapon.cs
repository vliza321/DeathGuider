using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetalliteTypeWeapon : MonoBehaviour
{
    private Weapon setalliteTypeWeapon;
    [SerializeField]
    private float playerToObjAngle;
    [SerializeField]
    private float debugAngle;

    private float PI;
    private float radian;
    private float rotateAnglePerFrame; // 커맨더 패턴 구현시 타켓 돌아가는 속도 조정가능하게 수정
    private float rotateAngle;
    private Vector3 directionalVector;
    [SerializeField]
    private Transform baseParent;

    private Vector3 cashingVector3;

    public void Init(Transform BaseParentTransform, Vector3 InitDirection, Transform WeaponEffectPool)
    {
        baseParent = BaseParentTransform;
        cashingVector3 = this.transform.position;
        PI = Mathf.PI;
        directionalVector = InitDirection;
        rotateAnglePerFrame = 0.01f;
        rotateAngle = 0.02f;
        this.transform.parent = WeaponEffectPool;
        radian = 180 / MathF.PI;
    }

    public void Execute(Transform baseObject, Transform effectPool)
    {
        if (baseParent.gameObject.activeSelf == false) this.gameObject.SetActive(false);
        playerToObjAngle = (Mathf.Atan2(baseParent.position.y - this.gameObject.transform.position.y, baseParent.position.x - this.gameObject.transform.position.x) + 2 * PI) % (2 * PI);
        directionalVector.x = directionalVector.x * MathF.Cos(rotateAngle) - directionalVector.y * MathF.Sin(rotateAngle);
        directionalVector.y = directionalVector.x * MathF.Sin(rotateAngle) + directionalVector.y * MathF.Cos(rotateAngle);
        directionalVector.z = 0;
        directionalVector = directionalVector.normalized *2.5f;

        this.transform.position = directionalVector + baseParent.position;

        cashingVector3.x = 0; cashingVector3.y = 0; cashingVector3.z = (playerToObjAngle) * radian - 45;

        this.transform.eulerAngles = cashingVector3;
    }
}
