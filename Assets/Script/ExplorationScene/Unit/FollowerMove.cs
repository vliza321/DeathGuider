using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMove : MonoBehaviour
{
    private GameObject Follower;
    private Vector2 FollowerLocalscale;
    private Vector2 FollowerVelocityVector;

    private float guiderMoveSpeed;
    private float moveSpeeds;
    private float distance;

    private Vector3 FrontguiderLocalscale;
    [SerializeField]
    private GameObject FrontGuider;
    [SerializeField]
    private GameObject Guider;

    // Start is called before the first frame update
    void Start()
    {
        Guider = this.transform.parent.GetComponent<FollowerManager>().PlayerManager.transform.GetChild(0).gameObject;
        Follower = this.gameObject;
        FollowerVelocityVector = new Vector2(0, 0);
       
        moveSpeeds = Guider.GetComponent<PlayerMove>().MoveSpeed;
        guiderMoveSpeed = Guider.GetComponent<PlayerMove>().MoveSpeed;

        FrontguiderLocalscale = FrontGuider.transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        FrontguiderLocalscale = FrontGuider.transform.localScale;
        FollowerLocalscale = Follower.transform.position;
        distance = Vector3.Distance(FrontGuider.transform.position, FollowerLocalscale);
        FollowerVelocityVector.x = Follower.transform.position.x - FrontGuider.transform.position.x;
        FollowerVelocityVector.y = Follower.transform.position.y - FrontGuider.transform.position.y;
        if (distance < 00.1f) { moveSpeeds = 0.0f; }
        else if (distance < 01.940f)
        {
            moveSpeeds = guiderMoveSpeed / 1.5f ;
        }
        else if (distance < 12.8f)
        {
            moveSpeeds = guiderMoveSpeed + 0.50f;
        }
        else moveSpeeds = guiderMoveSpeed + 3.0f;
        this.gameObject.transform.localScale = new Vector3(FrontguiderLocalscale.x, FrontguiderLocalscale.y, FrontguiderLocalscale.z);

    }

    private void FixedUpdate()
    {
        FollowerVelocityVector = FollowerVelocityVector.normalized * moveSpeeds * Time.fixedDeltaTime;

        Follower.transform.position = new Vector2(Follower.transform.position.x - FollowerVelocityVector.x, Follower.transform.position.y - FollowerVelocityVector.y);
    }
}
