using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
  
    private GameObject player;
    [SerializeField]
    private Transform monsterObject;
    private Vector2 monsterLocalScale;
    private Vector2 monsterVelocityVector;
    private float signX;
    private float signY;
    public Vector2 MonsterVelocityVector
    {
        get { return monsterVelocityVector; } set { monsterVelocityVector = value; }
    }

    private float moveSpeeds;

    private float distance;
    public float Distance
    {
        get { return distance; }
        set { distance = value; }
    }
    private Vector3 playerPos;

    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }
    [SerializeField]
    private GameObject guider;
    public GameObject Guider
    {
        get { return guider; }
        set { guider = value; } 
    }

    private bool isKnockBack;
    public bool IsKnockBack
    {
        get { return isKnockBack; }
        set { isKnockBack = value; }
    }

    private int knockBackTimer;
    public int KnockBackTimer
    {
        get { return knockBackTimer; }
        set { knockBackTimer = value; }
    }

    private float guiderMoveSpeed;
    // Start is called before the first frame update
    private void Awake()
    {
        signX = 0;
        signY = 0;
        monsterObject = this.transform;
        monsterVelocityVector = new Vector2(0, 0);
    }
    void Start()
    {
        Debug.Log("monsterStart");
        player = this.transform.parent.GetComponent<MonsterManager>().Player;
        guider = player.transform.GetChild(0).gameObject;
        moveSpeeds = guider.GetComponent<PlayerMove>().MoveSpeed / 5.0f;
        playerPos = guider.transform.position;
        isKnockBack = false;
        knockBackTimer = 10;
        guiderMoveSpeed = guider.GetComponent<PlayerMove>().MoveSpeed;
        if (Random.Range(0, 2) == 1) signX = 1;
        else signX = -1;
        if (Random.Range(0, 2) == 1) signY = 1;
        else signY = -1;
        this.transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(6, 10) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(4, 8) / 10.0f), playerPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        switch (isKnockBack)
        {
            case true: 
                knockBackTimer--;
                if(knockBackTimer < 1)
                {
                    isKnockBack = false;
                    knockBackTimer = 10;
                }

                break;
            case false:
                //guider = Player.GetComponent<PlayerSwap>().Guider;
                playerPos = guider.transform.position;
                monsterLocalScale = monsterObject.position;
                distance = Vector3.Distance(guider.transform.position, monsterLocalScale);
                monsterVelocityVector.x = monsterObject.position.x - guider.transform.position.x;
                monsterVelocityVector.y = monsterObject.position.y - guider.transform.position.y;

                if (distance < 0.5f) { moveSpeeds = 0.10f; }
                if ((distance < 12.0f) && (distance >= 0.5f))
                {
                    moveSpeeds = guiderMoveSpeed / 2.0f + 0.5f;
                }
                if (distance >= 12.0f) { moveSpeeds = guiderMoveSpeed / 5.0f + 0.1f; }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (distance > 30.0f) {
            if (Random.Range(0, 2) == 1) signX = 1;
            else signX = -1;
            if (Random.Range(0, 2) == 1) signY = 1;
            else signY = -1;
            this.transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(6, 10) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(4, 8) / 10.0f), playerPos.z); 
        }
        
        monsterVelocityVector = monsterVelocityVector.normalized * moveSpeeds * Time.fixedDeltaTime * (Mathf.Log10(knockBackTimer));

        //MonsterObject.transform.position = new Vector2(MonsterObject.transform.position.x - monsterVelocityVector.x, MonsterObject.transform.position.y - monsterVelocityVector.y);
        this.transform.Translate(-monsterVelocityVector.x, -monsterVelocityVector.y, 0);
    }
}
