using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Player;
    public Vector2 PlayerPosition;
    public Vector2 PlayerVelocityVector;
    Vector3 PlayerLocalScale;
    public int canmove;

    public float MoveSpeed;
    public float MoveSpeeds;

    private void Awake()
    {
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
        PlayerVelocityVector.x = Input.GetAxisRaw("Horizontal");
        PlayerVelocityVector.y = Input.GetAxisRaw("Vertical");
        if(Input.GetAxisRaw("Horizontal")!=0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0) { Player.transform.localScale = new Vector3((-1)* PlayerLocalScale.x, PlayerLocalScale.y, PlayerLocalScale.z); } //µÚÁý±â
            else { Player.transform.localScale = new Vector3(PlayerLocalScale.x, PlayerLocalScale.y, PlayerLocalScale.z); }
        }
        
    }

    private void FixedUpdate()
    {
        
        PlayerVelocityVector = PlayerVelocityVector.normalized * MoveSpeed * Time.fixedDeltaTime;
        PlayerPosition.x = Player.GetComponent<Transform>().position.x;
        PlayerPosition.y = Player.GetComponent<Transform>().position.y;
        Player.transform.position = new Vector2(PlayerPosition.x + PlayerVelocityVector.x *canmove, PlayerPosition.y + PlayerVelocityVector.y*canmove);

        MoveSpeeds = Mathf.Sqrt(Mathf.Pow(PlayerVelocityVector.x, 2) + Mathf.Pow(PlayerVelocityVector.y, 2));
    }
}
