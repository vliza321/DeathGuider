using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackDirectional : MonoBehaviour
{
    private GameObject guider;
    private Vector3 target;
    private PlayerMove guiderMove;
    private Vector2 attackDirectionalVelocity;
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

    public GameObject Guider
    {
        get { return guider; }
        set { guider = value; }
    }
    private void Awake()
    {
        attackDirectionalVelocity = new Vector2(0, 0);
    }
    private void Start()
    {
        guider = this.transform.parent.GetComponent<PlayerSwap>().Guider;
        guiderMove = guider.GetComponent<PlayerMove>();
        this.transform.position = guiderMove.AttackTargetPoint;
    }

    private void Update()
    {

        target = guiderMove.AttackTargetPoint;
    }

    private void FixedUpdate()
    {
        this.transform.Translate(attackDirectionalVelocity.x, attackDirectionalVelocity.y, 0);
        /*
        playerVelocityVector = playerVelocityVector.normalized * MoveSpeed * Time.fixedDeltaTime;
        PlayerPosition.x = Player.GetComponent<Transform>().position.x;
        PlayerPosition.y = Player.GetComponent<Transform>().position.y;
        this.transform.position = new Vector2(PlayerPosition.x + playerVelocityVector.x * canmove, PlayerPosition.y + playerVelocityVector.y * canmove);
    */
    }
}
