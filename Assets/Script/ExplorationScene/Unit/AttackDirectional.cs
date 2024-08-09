using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class AttackDirectional : MonoBehaviour
{
    private GameObject guider;
    private Vector3 target;
    private PlayerMove guiderMove;
    private bool isMove;
    private float playerToTargetAngle;
    private float playerToObjAngle;

    private float PI;

    private float rotateAnglePerFrame; // 커맨더 패턴 구현시 타켓 돌아가는 속도 조정가능하게 수정
    private float rotateAngle;
    private Vector2 directionalVector2;
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
    public PlayerMove GuiderMove
    {
        get { return guiderMove; }
        set { guiderMove = value; }
    }
    public GameObject Guider
    {
        get { return guider; }
        set { guider = value; }
    }
    private void Awake()
    {
        isMove = true;
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

        rotateAnglePerFrame = 0.05f ;
    }

    private void Update()
    {
        // 방향 지시기 이동할 최종 각도 계산
        target = guiderMove.AttackTargetPoint;
        playerToTargetAngle = (Mathf.Atan2(target.y - guider.transform.position.y, target.x - guider.transform.position.x) + 2 * PI) % (2*PI);
        playerToObjAngle = (Mathf.Atan2(this.gameObject.transform.position.y - guider.transform.position.y, this.gameObject.transform.position.x - guider.transform.position.x) + 2 * PI) %(2*PI);
        //방향 지시기가 이동해야할 최종 각도
        rotateAngle = (playerToTargetAngle - playerToObjAngle);
    }

    private void FixedUpdate()
    {
        if (rotateAngle < 0.0111f && rotateAngle > -0.0111f)
        {
            isMove = false;
        }
        else
        {
            isMove = true;
        }

        if (isMove == false)
        {
            rotateAnglePerFrame = 0.0f;
            directionalVector2.x = target.x - guider.transform.position.x;
            directionalVector2.y = target.y - guider.transform.position.y;
        }
        else
        {
            if ((rotateAngle > 0 && rotateAngle < PI)|| rotateAngle < -PI)
            {
                rotateAnglePerFrame = 0.05f;
            }
            else
            {
                rotateAnglePerFrame = -0.05f;
            }
            directionalVector2.x = directionalVector2.x * MathF.Cos(rotateAnglePerFrame) - directionalVector2.y * MathF.Sin(rotateAnglePerFrame);
            directionalVector2.y = directionalVector2.x * MathF.Sin(rotateAnglePerFrame) + directionalVector2.y * MathF.Cos(rotateAnglePerFrame);

            this.transform.eulerAngles = new Vector3(0, 0, playerToObjAngle * 180 / PI - 90);
        }
        this.transform.position = new Vector3(directionalVector2.x + guider.transform.position.x, directionalVector2.y + guider.transform.position.y, 0);
        this.transform.eulerAngles = new Vector3(0, 0, -90 + (playerToObjAngle) * (180.0f) / MathF.PI);
    }
}
