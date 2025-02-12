using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterState : autorizedObject
{
    private MonsterData status;
    private float healthPoint;
    private TestMonsterMove monsterMove;

    private ObjectPool<TestMonsterState> monsterPool;
    private MonsterSpawnManager monsterManager;
    int thisMonsterNum;

    private string weaponTagName = "Weapon";


    // Start is called before the first frame update
    public void Init(MonsterData status, ObjectPool<TestMonsterState> respawnPool, MonsterSpawnManager monsterManager, int num)
    {
        this.status = status;

        healthPoint = status.MaxHealthPoint;
        monsterMove = this.gameObject.GetComponent<TestMonsterMove>();
        monsterPool = respawnPool;
        this.monsterManager = monsterManager;
        thisMonsterNum = num;
    }

    public void monsterSpawn()
    {
        healthPoint = status.MaxHealthPoint;
        monsterMove.ActionState = MonsterActionState.Spawning;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(weaponTagName))
        {
            monsterMove.ActionState = MonsterActionState.KnockBack;
            healthPoint -= 5;
            if (healthPoint <= 0)
            {
                monsterMove.ActionState = MonsterActionState.Dying;
                // 몬스터 비활성화 및 풀에 반환
                /*
                monsterManager.GoldSpawn.ReleaseGoods(thisMonsterNum, this.transform.position);
                monsterManager.DarkEssenseSpawn.ReleaseGoods(thisMonsterNum, this.transform.position);
                monsterManager.ExpSpawn.ReleaseGoods(thisMonsterNum, this.transform.position);*/
            }
        }
    }
}
