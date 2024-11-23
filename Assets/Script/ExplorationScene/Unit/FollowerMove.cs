using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMove : MonoBehaviour
{
    private GameObject follower;
    private Vector2 followerPosition;
    private Vector2 followerVelocityVector;

    private float guiderMoveSpeed;
    private float baseSpeeds;
    private float moveSpeed;
    private float distance;

    private Vector3 frontguiderLocalscale;
    [SerializeField]
    private GameObject frontGuider;
    [SerializeField]
    private GameObject guider;
    private Animator bodyAnimation;
    private Animator headAnimation;

    private Vector3 cashingVector;

    private PlayerMove thisPlayerMove;
    private FollowerMove thisFollowerMove;
    private CapsuleCollider2D thisCollider;
    // Start is called before the first frame update
    void Start()
    {
        cashingVector = Vector3.zero;
        headAnimation = this.transform.GetChild(0).GetComponent<Animator>();
        bodyAnimation = this.transform.GetChild(1).GetComponent<Animator>();
        guider = this.transform.parent.GetComponent<FollowerManager>().PlayerManager.transform.GetChild(0).gameObject;
        follower = this.gameObject;
        followerVelocityVector = new Vector2(0, 0);
       
        moveSpeed = guider.GetComponent<PlayerMove>().MoveSpeed;
        baseSpeeds = guider.GetComponent<PlayerMove>().MoveSpeed;
        guiderMoveSpeed = guider.GetComponent<PlayerMove>().MoveSpeed;

        frontguiderLocalscale = frontGuider.transform.localScale;
        thisPlayerMove = this.gameObject.GetComponent<PlayerMove>();
        thisFollowerMove = this.gameObject.GetComponent<FollowerMove>();
        thisCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        frontguiderLocalscale = frontGuider.transform.localScale;
        followerPosition = follower.transform.position;
        distance = Vector3.Distance(frontGuider.transform.position, followerPosition);
        followerVelocityVector.x = follower.transform.position.x - frontGuider.transform.position.x;
        followerVelocityVector.y = follower.transform.position.y - frontGuider.transform.position.y;
        if (distance < 00.1f) { 
            moveSpeed = 0.0f;
            headAnimation.SetBool("isMove", false);
            bodyAnimation.SetBool("isMove", false);
        }
        else
        {
            headAnimation.SetBool("isMove", true);
            bodyAnimation.SetBool("isMove", true);
            if (distance < 0.940f)
            {
                moveSpeed = guiderMoveSpeed * 0.66f;
            }
            else if (distance < 4.8f)
            {
                moveSpeed = guiderMoveSpeed;
            }
            else if (distance < 12.8f)
            {
                moveSpeed = guiderMoveSpeed + 0.50f;
            }
            else moveSpeed = guiderMoveSpeed + 3.0f;
        }
        cashingVector.x = frontguiderLocalscale.x;
        cashingVector.y = frontguiderLocalscale.y;
        cashingVector.z = frontguiderLocalscale.z;
        this.gameObject.transform.localScale = cashingVector;

    }

    private void FixedUpdate()
    {
        followerVelocityVector = followerVelocityVector.normalized * moveSpeed * Time.fixedDeltaTime;
        cashingVector.x = follower.transform.position.x - followerVelocityVector.x;
        cashingVector.y = follower.transform.position.y - followerVelocityVector.y;
        cashingVector.z = 0;
        follower.transform.position = cashingVector;
    }

    public void SwapGuider(CameraManager cameraObj)
    {
        thisPlayerMove.MoveSpeed = baseSpeeds;
        this.thisPlayerMove.Camera = cameraObj;
        thisPlayerMove.enabled = true;
        thisFollowerMove.enabled = false;
        thisCollider.enabled = true;
        this.gameObject.layer = 10;
        this.gameObject.tag = "Player";
        this.gameObject.transform.position = frontGuider.transform.position;
        this.gameObject.transform.parent = frontGuider.transform.parent;
        this.gameObject.transform.SetAsFirstSibling();
    }
}
