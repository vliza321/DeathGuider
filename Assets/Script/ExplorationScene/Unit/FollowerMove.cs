using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMove : MonoBehaviour
{
    private GameObject follower;
    private Vector2 followerLocalscale;
    private Vector2 followerVelocityVector;

    private float guiderMoveSpeed;
    private float moveSpeeds;
    private float distance;

    private Vector3 frontguiderLocalscale;
    [SerializeField]
    private GameObject frontGuider;
    [SerializeField]
    private GameObject guider;
    private Animator bodyAnimation;
    private Animator headAnimation;

    private Vector3 cashingVector;
    // Start is called before the first frame update
    void Start()
    {
        cashingVector = Vector3.zero;
        headAnimation = this.transform.GetChild(0).GetComponent<Animator>();
        bodyAnimation = this.transform.GetChild(1).GetComponent<Animator>();
        guider = this.transform.parent.GetComponent<FollowerManager>().PlayerManager.transform.GetChild(0).gameObject;
        follower = this.gameObject;
        followerVelocityVector = new Vector2(0, 0);
       
        moveSpeeds = guider.GetComponent<PlayerMove>().MoveSpeed;
        guiderMoveSpeed = guider.GetComponent<PlayerMove>().MoveSpeed;

        frontguiderLocalscale = frontGuider.transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        frontguiderLocalscale = frontGuider.transform.localScale;
        followerLocalscale = follower.transform.position;
        distance = Vector3.Distance(frontGuider.transform.position, followerLocalscale);
        followerVelocityVector.x = follower.transform.position.x - frontGuider.transform.position.x;
        followerVelocityVector.y = follower.transform.position.y - frontGuider.transform.position.y;
        if (distance < 00.1f) { 
            moveSpeeds = 0.0f;
            headAnimation.SetBool("isMove", false);
            bodyAnimation.SetBool("isMove", false);
        }
        else
        {
            headAnimation.SetBool("isMove", true);
            bodyAnimation.SetBool("isMove", true);
            if (distance < 0.940f)
            {
                moveSpeeds = guiderMoveSpeed * 0.66f;
            }
            else if (distance < 4.8f)
            {
                moveSpeeds = guiderMoveSpeed;
            }
            else if (distance < 12.8f)
            {
                moveSpeeds = guiderMoveSpeed + 0.50f;
            }
            else moveSpeeds = guiderMoveSpeed + 3.0f;
        }
        cashingVector.x = frontguiderLocalscale.x;
        cashingVector.y = frontguiderLocalscale.y;
        cashingVector.z = frontguiderLocalscale.z;
        this.gameObject.transform.localScale = cashingVector;

    }

    private void FixedUpdate()
    {
        followerVelocityVector = followerVelocityVector.normalized * moveSpeeds * Time.fixedDeltaTime;
        cashingVector.x = follower.transform.position.x - followerVelocityVector.x;
        cashingVector.y = follower.transform.position.y - followerVelocityVector.y;
        cashingVector.z = 0;
        follower.transform.position = cashingVector;
    }
}
