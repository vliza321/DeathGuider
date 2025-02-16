using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using Unity;

public class PlayerHp : MonoBehaviour
{
    private UnitData stat;
    private float HitDelay;
    private float MaxHP;
    [SerializeField]
    private float heartPoint;

    private string MonsterTagName = "Monster";
    private Vector3 cashingVector;
    private float damage;
    private float hitConstant;

    private FollowerManager followerManager;
    public UnitData Stat
    {
        get { return stat; }
        set { stat = value; }
    }

    public float HeartPoint
    {
        get { return heartPoint; }
        set { heartPoint = value; }
    }

    // Start is called before the first frame update
    public void Init(UnitData stat, float damage, FollowerManager manager)
    {
        this.stat = stat;
        cashingVector = Vector3.zero;
        HitDelay = 0.5f;
        MaxHP = stat.MaxHealthPoint;
        heartPoint = MaxHP;
        this.damage = damage / 2.0f;
        hitConstant = this.damage / (stat.Defense + 10.0f);
        followerManager = manager;
    }

    public float DDOResist()
    {
        return heartPoint;
    }

    public void DestroySelf()
    {
        Destroy(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (HitDelay > 0) { HitDelay -= Time.deltaTime; }
        cashingVector.x = transform.parent.position.x;
        cashingVector.y = transform.parent.position.y;
        cashingVector.z = transform.parent.position.z;
        this.transform.position = cashingVector;
    }

    float GetHp()
    {
        return heartPoint;
    }

    void SetHp(float hp)
    {
        heartPoint += hp;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(MonsterTagName) && HitDelay == 0)
        {
            Debug.Log("피격 :" + hitConstant);
            HitDelay -= 0.5f;
            heartPoint -= hitConstant;
            if (heartPoint <= 0)
            {
                followerManager.PlayerManager.SwapPlayer();
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag(MonsterTagName) && HitDelay == 0)
        {
            Debug.Log("피격 :" + hitConstant);
            HitDelay -= 0.5f;
            heartPoint -= hitConstant;
            if (heartPoint <= 0)
            {
                followerManager.PlayerManager.SwapPlayer();
            }
        }
    }
}
