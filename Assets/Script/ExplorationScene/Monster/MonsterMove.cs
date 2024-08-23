using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
  
    private GameObject Player;
    [SerializeField]
    private Transform MonsterObject;
    private Vector2 MonsterLocalScale;
    private Vector2 MonsterVelocityVector;
    private float signX;
    private float signY;
    public Vector2 monsterVelocityVector
    {
        get { return MonsterVelocityVector; } set { MonsterVelocityVector = value; }
    }

    private float MoveSpeeds;

    private float Distance;
    public float distance
    {
        get { return Distance; }
        set { Distance = value; }
    }
    private Vector3 playerPos;
    [SerializeField]
    private GameObject Guider;
    public GameObject guider
    {
        get { return Guider; }
        set { Guider = value; } 
    }

    private bool IsKnockBack;
    public bool isKnockBack
    {
        get { return IsKnockBack; }
        set { IsKnockBack = value; }
    }

    private int KnockBackTimer;
    public int knockBackTimer
    {
        get { return KnockBackTimer; }
        set { KnockBackTimer = value; }
    }

    private float guiderMoveSpeed;
    // Start is called before the first frame update
    private void Awake()
    {
        signX = 0;
        signY = 0;
        MonsterObject = this.transform;
        MonsterVelocityVector = new Vector2(0, 0);
    }
    void Start()
    {
        Player = this.transform.parent.GetComponent<MonsterSpawn>().Player;
        Guider = Player.transform.GetChild(0).gameObject;
        MoveSpeeds = Guider.GetComponent<PlayerMove>().MoveSpeed / 5.0f;
        playerPos = Guider.transform.position;
        IsKnockBack = false;
        KnockBackTimer = 10;
        guiderMoveSpeed = Guider.GetComponent<PlayerMove>().MoveSpeed;
        if (Random.Range(0, 2) == 1) signX = 1;
        else signX = -1;
        if (Random.Range(0, 2) == 1) signY = 1;
        else signY = -1;
        this.transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(6, 10) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(4, 8) / 10.0f), playerPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        switch (IsKnockBack)
        {
            case true: 
                KnockBackTimer--;
                if(KnockBackTimer < 1)
                {
                    IsKnockBack = false;
                    KnockBackTimer = 10;
                }

                break;
            case false:
                //guider = Player.GetComponent<PlayerSwap>().Guider;
                playerPos = Guider.transform.position;
                MonsterLocalScale = MonsterObject.position;
                Distance = Vector3.Distance(Guider.transform.position, MonsterLocalScale);
                MonsterVelocityVector.x = MonsterObject.position.x - Guider.transform.position.x;
                MonsterVelocityVector.y = MonsterObject.position.y - Guider.transform.position.y;

                if (Distance < 0.5f) { MoveSpeeds = 0.10f; }
                if ((Distance < 12.0f) && (Distance >= 0.5f))
                {
                    MoveSpeeds = guiderMoveSpeed / 2.0f + 0.5f;
                }
                if (Distance >= 12.0f) { MoveSpeeds = guiderMoveSpeed / 5.0f + 0.1f; }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (Distance > 30.0f) {
            if (Random.Range(0, 2) == 1) signX = 1;
            else signX = -1;
            if (Random.Range(0, 2) == 1) signY = 1;
            else signY = -1;
            this.transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(6, 10) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(4, 8) / 10.0f), playerPos.z); 
        }
        
        MonsterVelocityVector = MonsterVelocityVector.normalized * MoveSpeeds * Time.fixedDeltaTime * (Mathf.Log10(KnockBackTimer));

        //MonsterObject.transform.position = new Vector2(MonsterObject.transform.position.x - MonsterVelocityVector.x, MonsterObject.transform.position.y - MonsterVelocityVector.y);
        this.transform.Translate(-MonsterVelocityVector.x, -MonsterVelocityVector.y, 0);
    }
}
