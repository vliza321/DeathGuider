using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float hp;
    private bool alive; // 살았는지 죽었는지만
    private bool canMove; // 움직이는지 멈췄는지만
    private int monsterNum;
    private bool inGame;
    private int attackPoint;

    private MonsterMove monsterMove;
    public int AttactPoint
    { 
        get { return attackPoint; } 
        set { attackPoint = value; } 
    }

    // delete public 
    //public int spawnCounter;

    [SerializeField]
    private int experiencePoints = 10; // 몬스터 처치 시 플레이어에게 줄 경험치

    private ObjectPool monsterPool;
    private PlayerState playerState;

    public ObjectPool MonsterPool
    {
        get { return monsterPool; }
        set { monsterPool = value; }
    }
   
    public bool InGame
    {
        get { return inGame; }
        set { inGame = value; }
    }

    public int MonsterNum
    {
        get { return monsterNum; }
        set { monsterNum = value; }
    }

    public float Hp
    {
        get { return hp; }
        set { hp += value; }
    }

    public bool Alive
    {
        get { return alive; }
        set { alive = value; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private void Awake()
    {
        hp = 100;
        alive = false;
        canMove = false; 
        playerState = FindObjectOfType<PlayerState>();
        monsterMove = this.gameObject.GetComponent<MonsterMove>();
    }
    void Start()
    {

        alive = true;
        canMove = true;
        //spawnCounter = 3000;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void monsterRespawn(GameObject Player)
    {
        
        hp = 100;
        alive = true;
        canMove = true;
        monsterMove.Player = Player;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            hp--;
            if (hp <= 0)
            {
                canMove = false;
                alive = false;

                // 경험치 부여
                if (playerState != null)
                {
                    playerState.AddExperience(experiencePoints);
                }

                // 몬스터 비활성화 및 풀에 반환
                monsterPool.ReturnObject(this.gameObject);
            }
        }
    }
}