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
    }
    void Start()
    {
        int temt1;
        int temt2;
        if (Random.Range(0, 2) == 1) temt1 = 1;
        else temt1 = -1;
        if (Random.Range(0, 2) == 1) temt2 = 1;
        else temt2 = -1;
        alive = true;
        canmove = true;
        //spawnCounter = 3000;
        this.transform.position = new Vector3(temt1 * 12.8f * (Random.Range(5, 15) / 10.0f),  temt2 * 12.8f * (Random.Range(5, 15) / 10.0f),0);

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
        if(collision.gameObject.CompareTag("Weapon"))
        {
            hp--;
            if (hp <= 0)
            {
                this.gameObject.SetActive(false);
                canmove = false;
                alive = false;
            }
        }
    }
}
