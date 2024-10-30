using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    private delegate void AnimGet();
    private AnimGet animGet;

    private GameObject player;
    private Vector3 playerVelocityVector;
    private Vector3 playerLocalScale;
    private bool canMove;
    private float moveSpeed; // 추후 관련 스탯 처리 스크립트 만든 후 수정
    private Vector3 attackTargetPoint;
    private Vector3 attackTargetVector;
    private AttackDirectional attackDirectional;

    private Animator bodyAnimation;
    private Animator headAnimation;
    private CameraManager camera;
    private PlayerState playerState;
    public CameraManager Camera
    {
        set { camera = value; }
    }
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
    private void PlayHeadAnim()
    {
        headAnimation.SetBool("isMove", true);
    }
    private void PlayBodyAnim()
    {
        bodyAnimation.SetBool("isMove", true);
    }

    private void PauseHeadAnim()
    {
        headAnimation.SetBool("isMove", false);
    }
    private void PauseBodyAnim()
    {
        bodyAnimation.SetBool("isMove", false);
    }

    private AnimGet playHeadAnim;
    private AnimGet playBodyAnim;
    private AnimGet pauseHeadAnim;
    private AnimGet pauseBodyAnim;

    private Vector3 cashingVector;

    private void Awake()
    {
        playerState = this.gameObject.GetComponent<PlayerState>();
        playHeadAnim = new AnimGet(PlayHeadAnim);
        playBodyAnim = new AnimGet(PlayBodyAnim); 
        pauseHeadAnim = new AnimGet(PauseHeadAnim); 
        pauseBodyAnim = new AnimGet(PauseBodyAnim);

        moveSpeed = 5.8f;
        player = this.gameObject;
        player.transform.position = new Vector2(0,0);
        playerLocalScale = player.transform.localScale;
        canMove = true;
        playerVelocityVector.x = -1;
        playerVelocityVector.y = 0;
        attackTargetVector.x = -1;
        attackTargetVector.y = 0;
        attackTargetPoint = player.transform.position + playerVelocityVector.normalized * 2.0f;

        cashingVector = Vector3.zero;
    }



    void Start()
    {
        headAnimation = this.transform.GetChild(0).GetComponent<Animator>();
        bodyAnimation = this.transform.GetChild(1).GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerState.CanMove) canMove = true;
        else canMove = false;
        playerVelocityVector.x = Input.GetAxisRaw("Horizontal");
        playerVelocityVector.y = Input.GetAxisRaw("Vertical");
        if(Input.GetAxisRaw("Horizontal")!=0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0) {
                cashingVector.x = (-1) * playerLocalScale.x;
                cashingVector.y = playerLocalScale.y;
                cashingVector.z = playerLocalScale.z;
                player.transform.localScale = cashingVector; 
            } //뒤집기
            else {
                cashingVector.x = playerLocalScale.x;
                cashingVector.y = playerLocalScale.y;
                cashingVector.z = playerLocalScale.z;
                player.transform.localScale = cashingVector;
            }
        }
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            attackTargetVector = playerVelocityVector.normalized;
            animGet += playHeadAnim;
            animGet += playBodyAnim;
        }
        else
        {
            animGet += pauseHeadAnim;
            animGet += pauseBodyAnim;
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
        animGet();
    }

    private void FixedUpdate()
    {
        playerVelocityVector = playerVelocityVector.normalized * moveSpeed * Time.fixedDeltaTime;
        player.transform.Translate(playerVelocityVector.x, playerVelocityVector.y, 0);
    }
}
