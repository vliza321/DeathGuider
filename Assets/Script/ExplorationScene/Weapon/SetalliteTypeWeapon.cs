using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetalliteTypeWeapon : MonoBehaviour
{

    private float playerToObjAngle;

    private float PI;

    private float rotateAnglePerFrame; // 커맨더 패턴 구현시 타켓 돌아가는 속도 조정가능하게 수정
    private float rotateAngle;
    private Vector3 directionalVector;

    private Transform baseParent;

    private Vector3 cashingVector3;
    private void Awake()
    {
        baseParent = this.transform.parent;
        cashingVector3 = this.transform.position;
        PI = Mathf.PI;
    }
    private void Start()
    {
        rotateAnglePerFrame = 0.01f;
        directionalVector = new Vector3(0, 1, 0);
        rotateAngle = 0.05f;
        this.transform.parent = this.transform.parent.parent.parent.GetChild(this.transform.parent.parent.parent.childCount - 1);
    }

    private void Update()
    {
        //playerToTargetAngle = (Mathf.Atan2(target.y - guider.transform.position.y, target.x - guider.transform.position.x) + 2 * PI) % (2 * PI);
        playerToObjAngle = (Mathf.Atan2(this.gameObject.transform.position.y, this.gameObject.transform.position.x) + 2 * PI) % (2 * PI);
        //방향 지시기가 이동해야할 최종 각도
    }

    private void FixedUpdate()
    {
        directionalVector.x = directionalVector.x * MathF.Cos(rotateAngle) - directionalVector.y * MathF.Sin(rotateAngle);
        directionalVector.y = directionalVector.x * MathF.Sin(rotateAngle) + directionalVector.y * MathF.Cos(rotateAngle);
        directionalVector.z = 0;
        directionalVector = directionalVector.normalized * 2f;

        this.transform.position = directionalVector + baseParent.position;

        cashingVector3.x = 0; cashingVector3.y = 0; cashingVector3.z = (playerToObjAngle) * (180.0f) / PI ;

        this.transform.eulerAngles = cashingVector3;

    }
}
