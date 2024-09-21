using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetalliteTypeWeapon : MonoBehaviour
{
<<<<<<< HEAD
    public float distance;

    public float playerToObjAngle;
=======
    [SerializeField]
    private float playerToObjAngle;
    public float debugAngle;
>>>>>>> 0db05417940d2f531057a0f210b383f268a77edd

    public float PI;

    public float rotateAnglePerFrame; // 커맨더 패턴 구현시 타켓 돌아가는 속도 조정가능하게 수정
    public float rotateAngle;
    public Vector3 directionalVector;



    public Vector3 cashingVector3;
    private void Awake()
    {
        cashingVector3 = this.transform.position;
        PI = Mathf.PI;
        distance = Vector3.Distance(Vector3.zero, this.transform.position);
    }
    private void Start()
    {
        rotateAnglePerFrame = 0.01f;
        directionalVector = new Vector3(0, 1, 0);
        rotateAngle = 0.05f;
    }

    private void Update()
    {
        distance = Vector3.Distance(Vector3.zero, this.transform.position);
        //playerToTargetAngle = (Mathf.Atan2(target.y - guider.transform.position.y, target.x - guider.transform.position.x) + 2 * PI) % (2 * PI);
        playerToObjAngle = (Mathf.Atan2(baseParent.position.y - this.gameObject.transform.position.y, baseParent.position.x - this.gameObject.transform.position.x)  + 2 * PI) % (2 * PI);
        debugAngle = playerToObjAngle * 180 / PI;
        //방향 지시기가 이동해야할 최종 각도
    }

    private void FixedUpdate()
    {
        directionalVector.x = directionalVector.x * MathF.Cos(rotateAngle) - directionalVector.y * MathF.Sin(rotateAngle);
        directionalVector.y = directionalVector.x * MathF.Sin(rotateAngle) + directionalVector.y * MathF.Cos(rotateAngle);
        directionalVector.z = 0;
        directionalVector = directionalVector.normalized * 2f;

        this.transform.position = directionalVector;

        cashingVector3.x = 0; cashingVector3.y = 0; cashingVector3.z = (playerToObjAngle) * (180.0f) / PI - 45;

        this.transform.eulerAngles = cashingVector3;

    }
}
