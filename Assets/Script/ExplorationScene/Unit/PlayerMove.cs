using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject Player;
    private Vector2 PlayerPosition;
    private Vector2 playerVelocityVector;
    Vector3 PlayerLocalScale;
    private int canmove;
    [SerializeField]
    private float MoveSpeed;
   
    public Vector2 PlayerVelocityVector
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
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<PlayerState>().canmove) canmove = 1;
        else canmove = 0;
        playerVelocityVector.x = Input.GetAxisRaw("Horizontal");
        playerVelocityVector.y = Input.GetAxisRaw("Vertical");
        if(Input.GetAxisRaw("Horizontal")!=0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0) { Player.transform.localScale = new Vector3((-1)* PlayerLocalScale.x, PlayerLocalScale.y, PlayerLocalScale.z); } //µÚÁý±â
            else { Player.transform.localScale = new Vector3(PlayerLocalScale.x, PlayerLocalScale.y, PlayerLocalScale.z); }
        }
    }

    private void FixedUpdate()
    {
        playerVelocityVector = playerVelocityVector.normalized * MoveSpeed * Time.fixedDeltaTime;
        PlayerPosition.x = Player.GetComponent<Transform>().position.x;
        PlayerPosition.y = Player.GetComponent<Transform>().position.y;
        Player.transform.position = new Vector2(PlayerPosition.x + playerVelocityVector.x *canmove, PlayerPosition.y + playerVelocityVector.y*canmove);
    }
}
