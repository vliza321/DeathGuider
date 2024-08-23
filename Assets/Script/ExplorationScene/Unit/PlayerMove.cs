using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject player;
    [SerializeField]
    private Vector3 playerVelocityVector;
    private Vector3 playerLocalScale;
    private bool canmove;
    [SerializeField]
    private float moveSpeed; // 추후 관련 스탯 처리 스크립트 만든 후 수정
    [SerializeField]
    private Vector3 attackTargetPoint;
    [SerializeField]
    private Vector3 attackTargetVector;
    [SerializeField]
    private AttackDirectional attackDirectional;
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

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }

    public AttackDirectional AttactDirectional
    {
        get { return attackDirectional; }
        set { attackDirectional = value; }
    }
    private void Awake()
    {
        attackDirectional = this.transform.parent.GetChild(1).gameObject.GetComponent<AttackDirectional>();
        player = this.gameObject;
        player.transform.position = new Vector2(0,0);
        playerLocalScale = player.transform.localScale;
        canmove = true;
        playerVelocityVector.x = -1;
        playerVelocityVector.y = 0;
        attackTargetVector.x = -1;
        attackTargetVector.y = 0;
        attackTargetPoint = player.transform.position + playerVelocityVector.normalized * 2.0f;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<PlayerState>().CanMove) canmove = true;
        else canmove = false;
        playerVelocityVector.x = Input.GetAxisRaw("Horizontal");
        playerVelocityVector.y = Input.GetAxisRaw("Vertical");
        if(Input.GetAxisRaw("Horizontal")!=0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0) { player.transform.localScale = new Vector3((-1)* playerLocalScale.x, playerLocalScale.y, playerLocalScale.z); } //뒤집기
            else { player.transform.localScale = new Vector3(playerLocalScale.x, playerLocalScale.y, playerLocalScale.z); }
        }
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            attackTargetVector = playerVelocityVector.normalized;
        }
        attackTargetPoint = player.transform.position + attackTargetVector.normalized * 2.0f;
        if(Input.GetAxisRaw("AttackDirectionalBind") > 0)
        {
            attackDirectional.IsMove = false;
        }
        else
        {
            attackDirectional.IsMove = true;
        }
    }

    private void FixedUpdate()
    {
        playerVelocityVector = playerVelocityVector.normalized * moveSpeed * Time.fixedDeltaTime;
        player.transform.Translate(playerVelocityVector.x, playerVelocityVector.y, 0);
    }
}
