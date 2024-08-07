using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject Player;
    private Vector2 PlayerPosition;
    private Vector3 playerVelocityVector;
    Vector3 PlayerLocalScale;
    private int canmove;
    [SerializeField]
    private float MoveSpeed; // 추후 관련 스탯 처리 스크립트 만든 후 수정
    [SerializeField]
    private Vector3 attackTargetPoint;
    public Vector3 AttackTargetPoint
    {
        get { return attackTargetPoint; }
    }
    public Vector3 PlayerVelocityVector
    {
        get
        {
            return playerVelocityVector;
        }
        set
        {
            playerVelocityVector = value;
        }
    }

    public float moveSpeed
    {
        get
        {
            return MoveSpeed;
        }
        set
        {
            MoveSpeed = value;
        }
    }

    private void Awake()
    {
        Player = this.gameObject;
        Player.transform.position = new Vector2(0,0);

    }

    void Start()
    {
        Player = this.gameObject;
        PlayerLocalScale = Player.transform.localScale;
        canmove = 1;
        playerVelocityVector.x = 1;
        playerVelocityVector.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<PlayerState>().CanMove) canmove = 1;
        else canmove = 0;
        playerVelocityVector.x = Input.GetAxisRaw("Horizontal");
        playerVelocityVector.y = Input.GetAxisRaw("Vertical");
        if(Input.GetAxisRaw("Horizontal")!=0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0) { Player.transform.localScale = new Vector3((-1)* PlayerLocalScale.x, PlayerLocalScale.y, PlayerLocalScale.z); } //뒤집기
            else { Player.transform.localScale = new Vector3(PlayerLocalScale.x, PlayerLocalScale.y, PlayerLocalScale.z); }
        }
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
           attackTargetPoint = Player.transform.position + playerVelocityVector.normalized * 3;
        }
    }

    private void FixedUpdate()
    {
        playerVelocityVector = playerVelocityVector.normalized * MoveSpeed * Time.fixedDeltaTime;
        //PlayerPosition.x = Player.GetComponent<Transform>().position.x;
        //PlayerPosition.y = Player.GetComponent<Transform>().position.y;
        //Player.transform.position = new Vector2(PlayerPosition.x + playerVelocityVector.x *canmove, PlayerPosition.y + playerVelocityVector.y*canmove);
        Player.transform.Translate(playerVelocityVector.x, playerVelocityVector.y, 0);
    }
}
