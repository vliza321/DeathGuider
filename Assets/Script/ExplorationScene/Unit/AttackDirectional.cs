using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class AttackDirectional : MonoBehaviour
{
    [SerializeField]
    GameObject targetObj;
    [SerializeField]
    private float distanceWithPlayer;
    public GameObject guider;
    public Vector3 target;
    private PlayerMove guiderMove;
    private bool isMove;
    public bool IsMove
    {
        get { return isMove; }
        set 
        {
            if (isMove)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
        }
    }
    public Vector2 playerToObjVector;
    public float playerToTargetAngle;
    public float playerToObjAngle;
    private float PI;
    public float rotateAnglePerFrame;
    public float rotateAngle;
    public Vector2 directionalVector2;
    public Vector2 directionalVelocity;
    public GameObject Guider
    {
        get { return guider; }
        set { guider = value; }
    }
    private void Awake()
    {
        isMove = true;
        distanceWithPlayer = 3.0f;
        PI = Mathf.PI;  
    }
    private void Start()
    {
        guider = this.transform.parent.GetComponent<PlayerSwap>().Guider;
        guiderMove = guider.GetComponent<PlayerMove>();
        this.transform.position = guiderMove.AttackTargetPoint;
        rotateAnglePerFrame = 0;

        target = guiderMove.AttackTargetPoint;
        this.transform.position = target;
        directionalVector2 = target;
        playerToTargetAngle = Mathf.Atan2(target.y - guider.transform.position.y, target.x - guider.transform.position.x);
        playerToObjAngle = Mathf.Atan2(this.gameObject.transform.position.y - guider.transform.position.y, this.gameObject.transform.position.x - guider.transform.position.x);

        rotateAnglePerFrame = 0.01f ;
    }

    private void Update()
    {
        distanceWithPlayer = Vector3.Distance(this.transform.position, guider.transform.position);
        targetObj.transform.position = target;
        // 공격 방향 지시기의 벡터 연산
        playerToObjVector.x = target.x - guider.transform.position.x;
        playerToObjVector.y = target.y - guider.transform.position.y;
        // 방향 지시기 이동할 최종 각도 계산
        target = guiderMove.AttackTargetPoint;
        playerToTargetAngle = Mathf.Atan2(target.y - guider.transform.position.y, target.x - guider.transform.position.x);
        playerToObjAngle = Mathf.Atan2(this.gameObject.transform.position.y - guider.transform.position.y, this.gameObject.transform.position.x - guider.transform.position.x);
        //방향 지시기가 이동해야할 최종 각도
        rotateAngle = (playerToTargetAngle - playerToObjAngle);
    }

    private void FixedUpdate()
    {
        directionalVector2.x = directionalVector2.x * MathF.Cos(rotateAnglePerFrame) - directionalVector2.y * MathF.Sin(rotateAnglePerFrame);
        directionalVector2.y = directionalVector2.x * MathF.Sin(rotateAnglePerFrame) + directionalVector2.y * MathF.Cos(rotateAnglePerFrame);

        directionalVelocity = directionalVector2.normalized * Time.deltaTime * rotateAnglePerFrame;
        //directionalVelocity = directionalVelocity.normalized * Time.fixedDeltaTime;
        if (isMove)
        {
            rotateAnglePerFrame = 0.01f;
            this.transform.eulerAngles = new Vector3(0, 0, playerToObjAngle * 180 / PI - 90);

        }
        else
        {
            rotateAnglePerFrame = 0.0f;
        }

        this.transform.position = new Vector3(directionalVector2.x + guider.transform.position.x, directionalVector2.y + guider.transform.position.y,0);

        /*
        if (rotateAngle > 0.0111f)
        {
            this.transform.eulerAngles = new Vector3(rotateAngle, 0, 0);
            attackDirectionalVelocity.x = this.transform.position.x * MathF.Cos(rotateAngle) - this.transform.position.y * MathF.Sin(rotateAngle);
            attackDirectionalVelocity.y = this.transform.position.x * MathF.Sin(rotateAngle) + this.transform.position.y * MathF.Cos(rotateAngle);
            this.transform.position = new Vector3(attackDirectionalVelocity.x, attackDirectionalVelocity.y, 0) + guider.transform.position;
        }
        //this.transform.position = target;
        //this.transform.Translate(this.transform.position.x * MathF.Cos(rotateAngle) - this.transform.position.y * MathF.Sin(rotateAngle), 
        //  this.transform.position.x * MathF.Sin(rotateAngle) + this.transform.position.y * MathF.Cos(rotateAngle), 0);
    */
    }
}
