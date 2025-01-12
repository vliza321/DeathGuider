using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour
{
    private MonsterMove monsterMove;

    private MonsterData status;

    private bool canMove;
    private float healthPoint;

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public float HealthPoint
    {
        get { return healthPoint; }
        set { healthPoint = value; }
    }
    public MonsterData Status
    {
        get { return status; }
    }

    // delete public 
    //public int spawnCounter;

    [SerializeField]
    private int experiencePoints = 10; // 몬스터 처치 시 플레이어에게 줄 경험치

    private ObjectPool monsterPool;
    private PlayerState playerState;

    private string weaponTagName = "Weapon";
    public ObjectPool MonsterPool
    {
        get { return monsterPool; }
        set { monsterPool = value; }
    }
   
    private void Awake()
    {
        canMove = false;
        healthPoint = 100;
        playerState = FindObjectOfType<PlayerState>();
        monsterMove = this.gameObject.GetComponent<MonsterMove>();
    }

    // Update is called once per frame
    public void monsterSpawn(GameObject Player,ObjectPool respawnPool)
    {
        canMove = false;
        healthPoint = 100;
        monsterMove.Player = Player;
        monsterPool = respawnPool;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(weaponTagName))
        {
            //hp -= collision.GetComponent<WeaponState>().Damage;
            healthPoint-= 5;
            if (healthPoint <= 0)
            {
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