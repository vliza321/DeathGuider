using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : autorizedObject
{
    private MonsterMove monsterMove;

    [SerializeField]
    private MonsterData status;
    [SerializeField]
    private float healthPoint;

    
    public float HealthPoint
    {
        get { return healthPoint; }
        set { healthPoint = value; }
    }

    // delete public 
    //public int spawnCounter;

    private ObjectPool<MonsterState> monsterPool;

    private string weaponTagName = "Weapon";
    public ObjectPool<MonsterState> MonsterPool
    {
        get { return monsterPool; }
        set { monsterPool = value; }
    }
   
    public void Init(MonsterData prototypeData, ObjectPool<MonsterState> respawnPool)
    {
        healthPoint = prototypeData.MaxHealthPoint;
        status = prototypeData;
        monsterMove = this.gameObject.GetComponent<MonsterMove>();
        monsterPool = respawnPool;
    }

    // Update is called once per frame
    public void monsterSpawn()
    {
        healthPoint = status.MaxHealthPoint;
        monsterMove.ActionState = MonsterActionState.Spawning;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(weaponTagName))
        {
            //hp -= collision.GetComponent<WeaponState>().Damage;
            healthPoint-= 5;
            if (healthPoint <= 0)
            {
                // 몬스터 비활성화 및 풀에 반환
                monsterPool.ReleaseObject(this);
            }
        }
    }
}