using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMove : MonoBehaviour
{

    public GameObject Player;
    public GameObject Follower;
    public Vector2 FollowerLocalscale;
    public Vector2 FollowerVelocityVector;

    public float MoveSpeeds;
    public float MoveSpeedss;
    public float distance;

    public Vector3 playerPos;
    public Vector3 FrontguiderLocalscale;
    [SerializeField]
    private GameObject FrontGuider;
    private GameObject Guider;

    private float guiderMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Follower = this.gameObject;
        FollowerVelocityVector = new Vector2(0, 0);

        Guider = Player.GetComponent<PlayerSwap>().guider;
        MoveSpeeds = Guider.GetComponent<PlayerMove>().moveSpeed;

        FrontguiderLocalscale = FrontGuider.transform.localScale;

        guiderMoveSpeed = Guider.GetComponent<PlayerMove>().moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        FrontguiderLocalscale = FrontGuider.transform.localScale;
        FollowerLocalscale = Follower.transform.position;
        distance = Vector3.Distance(FrontGuider.transform.position, FollowerLocalscale);
        FollowerVelocityVector.x = Follower.transform.position.x - FrontGuider.transform.position.x;
        FollowerVelocityVector.y = Follower.transform.position.y - FrontGuider.transform.position.y;
        if (distance < 00.18f) { MoveSpeeds = 0.0f; }
        if ((distance < 0.940f) && (distance >= 00.18f))
        {
            MoveSpeeds = guiderMoveSpeed / 1.75f;
        }
        if ((distance < 12.8f) && (distance >= 00.94f))
        {
            MoveSpeeds = guiderMoveSpeed+1.0f;
        }

        if (distance >= 12.8f) { MoveSpeeds = Guider.GetComponent<PlayerMove>().moveSpeed + 3.0f; }
        this.gameObject.transform.localScale = new Vector3(FrontguiderLocalscale.x, FrontguiderLocalscale.y, FrontguiderLocalscale.z);

    }

    private void FixedUpdate()
    {
        FollowerVelocityVector = FollowerVelocityVector.normalized * MoveSpeeds * Time.fixedDeltaTime;

        Follower.transform.position = new Vector2(Follower.transform.position.x - FollowerVelocityVector.x, Follower.transform.position.y - FollowerVelocityVector.y);

        MoveSpeedss = Mathf.Sqrt(Mathf.Pow(FollowerVelocityVector.x, 2) + Mathf.Pow(FollowerVelocityVector.y, 2));
    }
}
