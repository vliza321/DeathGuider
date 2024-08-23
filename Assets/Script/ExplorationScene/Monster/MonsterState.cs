using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float hp;
    private bool alive; // 살았는지 죽었는지만
    private bool canmove; // 움직이는지 멈췄는지만
    private int monsterNum;
    private bool inGame;
    private int attackPoint;
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


    public bool getInGame()
    {
        return inGame;
    }

    public void setInGame()
    {
        inGame = true;
    }
    public int getMonsterNum()
    {
        return monsterNum;
    }

    public void setMonsterNum(int a)
    {
        monsterNum = a;
    }
    public float getHp()
    {
        return hp;
    }
    public void setHp(float a)
    {
        hp +=a;
    }
    public bool getAlive()
    {
        return alive;
    }
    public void setAlive(bool a)
    {
        alive = a;
    }

    public bool getCanMove()
    {
        return canmove;
    }
    public void setCanMove(bool a)
    {
        canmove = a;
    }


    private void Awake()
    {
        hp = 100;
        alive = false;
        canmove = false;
        monsterPool = FindObjectOfType<ObjectPool>(); 
        playerState = FindObjectOfType<PlayerState>(); 
    }
    void Start()
    {

        alive = true;
        canmove = true;
        //spawnCounter = 3000;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void monsterRespawn()
    {
        hp = 100;
        alive = true;
        canmove = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            hp--;
            if (hp <= 0)
            {
                canmove = false;
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