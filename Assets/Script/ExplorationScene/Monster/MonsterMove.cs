using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterActionState
{
    Spawning,
    Moving,
    KnockBack,
    FarAway,
    Dying
}

public class MonsterMove : MonoBehaviour
{
    [SerializeField]
    private MonsterActionState actionState;
    private CapsuleCollider2D capsuleCollider;
    private MonsterState monsterState;
    private GameObject player;
    
    [SerializeField]
    private Transform monsterObject;
    private Vector2 monsterLocalScale;
    private Vector2 monsterVelocityVector;
    
    private float signX;
    private float signY;
    private SpriteRenderer monsterSpriteRender;
    private Vector3 cashingVector;
    [SerializeField]
    private float spawnTimer;

    private float moveSpeeds;
    private bool canMove;

    [SerializeField]
    private float distance;

    [SerializeField]
    private GameObject guider;


    private float knockBackTimer;

    private float guiderMoveSpeed;
    public MonsterActionState ActionState
    {
        get { return actionState; }
        set { actionState = value; }
    }
    public Vector2 MonsterVelocityVector
    {
        get { return monsterVelocityVector; } set { monsterVelocityVector = value; }
    }

    public float Distance
    {
        get { return distance; }
        set { distance = value; }
    }
    private Vector3 playerPos;

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }
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
    public float KnockBackTimer
    {
        get { return knockBackTimer; }
        set { knockBackTimer = value; }
    }

    // Start is called before the first frame update
    public void Init()
    {
        capsuleCollider = this.GetComponent<CapsuleCollider2D>();
        monsterState = this.gameObject.GetComponent<MonsterState>();
        monsterSpriteRender = this.gameObject.GetComponent<SpriteRenderer>();
        player = this.transform.parent.GetComponent<MonsterManager>().Player;

        actionState = MonsterActionState.Dying;
        cashingVector = new Vector3(0, 0, 0);
        signX = 0;
        signY = 0;
        monsterObject = this.transform;
        monsterVelocityVector = new Vector2(0, 0);
        spawnTimer = 2;
        knockBackTimer = 1;
        canMove = false;

        guider = player.transform.GetChild(0).gameObject;
        moveSpeeds = guider.GetComponent<PlayerMove>().MoveSpeed / 5.0f;
        playerPos = guider.transform.position;
        isKnockBack = false;
        guiderMoveSpeed = guider.GetComponent<PlayerMove>().MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(guider.transform.position, this.transform.position);
        switch (actionState)
        {
            case MonsterActionState.Spawning:
                spawnTimer -= Time.deltaTime;
                if(spawnTimer <0)
                {
                    actionState = MonsterActionState.Moving;
                    capsuleCollider.enabled = true;
                    canMove = true;
                    spawnTimer = 2;
                }
                break;

            case MonsterActionState.Moving:
                playerPos = guider.transform.position;
                monsterLocalScale = monsterObject.position;
                monsterVelocityVector.x = monsterObject.position.x - guider.transform.position.x;
                monsterVelocityVector.y = monsterObject.position.y - guider.transform.position.y;

                if (distance < 0.5f) { moveSpeeds = 0.05f; }
                else if ((distance < 12.0f) && (distance >= 0.5f))
                {
                    moveSpeeds = guiderMoveSpeed * 0.5f + 0.5f;
                }
                else if (distance > 12.0f) { moveSpeeds = guiderMoveSpeed * 0.25f + 0.1f; }

                if (distance > 30.0f)
                {
                    actionState = MonsterActionState.FarAway;
                }
                else
                {
                    if(canMove)
                    {
                        monsterVelocityVector = monsterVelocityVector.normalized * moveSpeeds * Time.fixedDeltaTime;
                    }
                    else
                    {
                        monsterVelocityVector = Vector2.zero;
                    }
                    this.transform.Translate(-monsterVelocityVector.x, -monsterVelocityVector.y, 0);
                }
                break;

            case MonsterActionState.KnockBack:
                
                knockBackTimer -= Time.deltaTime;
                if (knockBackTimer < 0)
                {
                    actionState = MonsterActionState.Moving;
                    knockBackTimer = 1;
                }
                break;

            case MonsterActionState.FarAway:
                if (Random.Range(0, 2) == 1) signX = 1;
                else signX = -1;
                if (Random.Range(0, 2) == 1) signY = 1;
                else signY = -1;
                cashingVector.x = playerPos.x + signX * 12.8f * (Random.Range(6, 10) * 0.1f);
                cashingVector.y = playerPos.y + signY * 12.8f * (Random.Range(4, 8) * 0.1f);
                cashingVector.z = playerPos.z;
                this.transform.position = cashingVector;
                actionState = MonsterActionState.Moving;
                break;

            case MonsterActionState.Dying:
                spawnTimer -= Time.deltaTime;
                if (spawnTimer < 0)
                {
                    this.gameObject.SetActive(false);
                    capsuleCollider.enabled = false;
                    spawnTimer = 2;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (monsterVelocityVector.x > 0) monsterSpriteRender.flipX = false;
        else monsterSpriteRender.flipX = true;
    }
}
