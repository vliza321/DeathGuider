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

    private MonsterManager monsterManager;

    public float HealthPoint
    {
        get { return healthPoint; }
        set { healthPoint = value; }
    }

    private int thisMonsterNum;

    // delete public 
    //public int spawnCounter;

    private ObjectPool<MonsterState> monsterPool;

    private string weaponTagName = "Weapon";

    private float hitConstant;

    public ObjectPool<MonsterState> MonsterPool
    {
        get { return monsterPool; }
        set { monsterPool = value; }
    }
    public void ReleaseObject()
    {
        monsterPool.ReleaseObject(this);
    }


    public void Init(MonsterData prototypeData, ObjectPool<MonsterState> respawnPool, MonsterManager monsterManager, int num)
    {
        healthPoint = prototypeData.MaxHealthPoint;
        Debug.Log(healthPoint);
        status = prototypeData;
        hitConstant = 1.0f / (status.Defense + 10.0f);
        monsterMove = this.gameObject.GetComponent<MonsterMove>();
        monsterPool = respawnPool;
        this.monsterManager = monsterManager;
        thisMonsterNum = num;
    }
    // Update is called once per frame
    public void monsterSpawn()
    {
        healthPoint = status.MaxHealthPoint;
        monsterMove.ActionState = MonsterActionState.Spawning;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(weaponTagName) && monsterMove.ActionState == MonsterActionState.Moving)
        {
            Debug.Log("타격 :" + (1 + monsterManager.WeaponDamage[collision.gameObject] * hitConstant));
            monsterMove.ActionState = MonsterActionState.KnockBack;
            healthPoint -= 1 + monsterManager.WeaponDamage[collision.gameObject] * hitConstant;
            
            if (healthPoint <= 0)
            {
                monsterMove.ActionState = MonsterActionState.Dying;
                // 몬스터 비활성화 및 풀에 반환
                monsterManager.GoldSpawn.ReleaseGoods(thisMonsterNum,this.transform.position);
                monsterManager.DarkEssenseSpawn.ReleaseGoods(thisMonsterNum,this.transform.position);
                monsterManager.ExpSpawn.ReleaseGoods(thisMonsterNum,this.transform.position);
            }
        }
    }

}