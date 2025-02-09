using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods :MonoBehaviour
{
    private float amount;
    [SerializeField]
    private int nesting;

    private int key;
    private GoodsSpawnManager spawnManager;
    private int timer;
    private Goods caching;
    public int Key
    {
        get { return key; }
    }


    public void init(GoodsSpawnManager GSM, int key,int stageID)
    {
        timer = 3000;
        nesting = 1;
        spawnManager = GSM;
        this.key = key;
        amount = 10 + stageID * 10;

    }

    public float Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public int Nesting
    {
        get { return nesting; }
        set { nesting = value; }
    }

    public float getAmount()
    {
        float result = amount * nesting;
        return result;
    }

    public void releaseGoods(Vector3 position)
    {
        timer = 2000;
        this.gameObject.SetActive(true);
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player")) // ÇÃ·¹ÀÌ¾î È¹µæ
        {
            spawnManager.GetGoods(this, getAmount());
        }
        else // ÀçÈ­³¢¸® Ãæµ¹
        {
            caching = collision.GetComponent<Goods>();
            if (collision.isTrigger)
            {
                int a = nesting + caching.nesting;
                if(collision.gameObject.GetInstanceID() < this.gameObject.GetInstanceID())
                {
                    nesting = a;
                    caching.nesting = a;
                    spawnManager.ReturnGoods(caching);
                }
            }
        }
    }
    

    public void Update()
    {
        timer--;
        if(timer < 0)
        {
            nesting = 1;
            spawnManager.ReturnGoods(this);
        }
    }
}
