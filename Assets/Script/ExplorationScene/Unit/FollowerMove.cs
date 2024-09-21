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

    private Vector3 FrontguiderLocalscale;
    [SerializeField]
    private GameObject FrontGuider;
    [SerializeField]
<<<<<<< HEAD
    private GameObject Guider;
=======
    private GameObject guider;
    private Animator bodyAnimation;
    private Animator headAnimation;
>>>>>>> 0db05417940d2f531057a0f210b383f268a77edd

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        Guider = this.transform.parent.GetComponent<FollowerManager>().PlayerManager.transform.GetChild(0).gameObject;
        Follower = this.gameObject;
        FollowerVelocityVector = new Vector2(0, 0);
=======
        headAnimation = this.transform.GetChild(0).GetComponent<Animator>();
        bodyAnimation = this.transform.GetChild(1).GetComponent<Animator>();
        guider = this.transform.parent.GetComponent<FollowerManager>().PlayerManager.transform.GetChild(0).gameObject;
        follower = this.gameObject;
        followerVelocityVector = new Vector2(0, 0);
>>>>>>> 0db05417940d2f531057a0f210b383f268a77edd
       
        moveSpeeds = Guider.GetComponent<PlayerMove>().MoveSpeed;
        guiderMoveSpeed = Guider.GetComponent<PlayerMove>().MoveSpeed;

        FrontguiderLocalscale = FrontGuider.transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        FrontguiderLocalscale = FrontGuider.transform.localScale;
        FollowerLocalscale = Follower.transform.position;
        distance = Vector3.Distance(FrontGuider.transform.position, FollowerLocalscale);
        FollowerVelocityVector.x = Follower.transform.position.x - FrontGuider.transform.position.x;
        FollowerVelocityVector.y = Follower.transform.position.y - FrontGuider.transform.position.y;
        if (distance < 00.1f) { moveSpeeds = 0.0f; }
        else if (distance < 0.940f)
        {
            moveSpeeds = guiderMoveSpeed / 1.5f ;
=======
        frontguiderLocalscale = frontGuider.transform.localScale;
        followerLocalscale = follower.transform.position;
        distance = Vector3.Distance(frontGuider.transform.position, followerLocalscale);
        followerVelocityVector.x = follower.transform.position.x - frontGuider.transform.position.x;
        followerVelocityVector.y = follower.transform.position.y - frontGuider.transform.position.y;
        if (distance < 00.1f) { 
            moveSpeeds = 0.0f;
            headAnimation.SetBool("isMove", false);
            bodyAnimation.SetBool("isMove", false);
>>>>>>> 0db05417940d2f531057a0f210b383f268a77edd
        }
        else
        {
            headAnimation.SetBool("isMove", true);
            bodyAnimation.SetBool("isMove", true);
            if (distance < 0.940f)
            {
                moveSpeeds = guiderMoveSpeed / 1.5f;
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
<<<<<<< HEAD
        else if (distance < 12.8f)
        {
            moveSpeeds = guiderMoveSpeed + 0.50f;
        }
        else moveSpeeds = guiderMoveSpeed + 3.0f;
        this.gameObject.transform.localScale = new Vector3(FrontguiderLocalscale.x, FrontguiderLocalscale.y, FrontguiderLocalscale.z);
=======
        
        this.gameObject.transform.localScale = new Vector3(frontguiderLocalscale.x, frontguiderLocalscale.y, frontguiderLocalscale.z);
>>>>>>> 0db05417940d2f531057a0f210b383f268a77edd

    }

    private void FixedUpdate()
    {
        followerVelocityVector = followerVelocityVector.normalized * moveSpeeds * Time.fixedDeltaTime;
        follower.transform.position = new Vector2(follower.transform.position.x - followerVelocityVector.x, follower.transform.position.y - followerVelocityVector.y);
    }
}
