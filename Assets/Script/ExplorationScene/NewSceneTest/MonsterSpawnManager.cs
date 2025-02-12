using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : ManagerBase
{
    GameObject monsterSpawnManager;

    private ObjectPool<TestMonsterState> monsterSpawnPool;
    private ObjectPool<TestMonsterState> monsterRespawnPool;

    private TestMonsterMove[] monster;

    private int MaxMonster = 160;
    
    private int signX, signY;

    private int[] monsterSpawnTimer;

    private int enabledMonster;

    private int monsterRespawnTimer;

    private int spawnLevel;

    private Vector2 screenSize;

    private bool playerEscape;

    Vector3 playerPos;
    Vector3 currentPos;

    TestMonsterState newMonster;

    public MonsterSpawnManager(MasterManager master, GameObject thisObject) : base(master)
    {
        monsterSpawnManager = thisObject;
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;

        playerEscape = false;


        monsterSpawnPool = new ObjectPool<TestMonsterState>(MaxMonster);
        monsterRespawnPool = new ObjectPool<TestMonsterState>(MaxMonster);

        monster = new TestMonsterMove[MaxMonster];

        for (int a = 0; a < MaxMonster; a++)
        {
            TestMonsterState newMonster = Instantiate(masterManager.GameManager.Monster[11].gameObject).GetComponent<TestMonsterState>();
            newMonster.transform.SetParent(monsterSpawnManager.transform);
            monsterSpawnPool.ReleaseObject(newMonster);
            monster[a] = newMonster.GetComponent<TestMonsterMove>();
            newMonster.GetComponent<TestMonsterState>().Init(masterManager.DDOManager.MonsterDatas.MonsterDataDic[masterManager.GameManager.SelectStageID], monsterRespawnPool, this, a);
        }

        signX = 0;
        signY = 0;

        monsterSpawnTimer = new int[(int)(MaxMonster / 10)];

        enabledMonster = 1;
        monsterRespawnTimer = 1000;
        spawnLevel = 1;
        for (int i = 0; i < monsterSpawnTimer.Length; i++)
        {
            monsterSpawnTimer[i] = 1200;
        }
        monsterSpawnTimer[0] = 100;
    }

    public override void OnNotify()
    {
        //이벤트 함수 실행

    }


    public override void Awake()
    {
        playerPos = new Vector3(0, 0, 0);
        currentPos = new Vector3(0, 0, 0);
    }
    public override void Start()
    {

    }

    public override void Update()
    {
        // 몬스터 리스폰 처리
        if (monsterRespawnTimer > 0 && !playerEscape)
        {
            monsterRespawnTimer--;
        }
        else
        {
            if (Random.Range(0, 2) == 1) signX = 1;
            else signX = -1;
            if (Random.Range(0, 2) == 1) signY = 1;
            else signY = -1;
            //playerPos = guider.transform.position;
            monsterRespawnTimer = 1000;

            while (monsterRespawnPool.PoolQueue.Count != 0)
            {
                newMonster = monsterRespawnPool.GetObject();
                newMonster.GetComponent<MonsterState>().monsterSpawn();
                newMonster.GetComponent<MonsterMove>().ActionState = MonsterActionState.Spawning;
                switch (Random.Range(0, 4))
                {
                    default:
                        currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(12, 20) * 0.1f);
                        currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(12, 20) * 0.1f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                    case 0:
                        /*
                        if (guider.PlayerVelocityVector.x != 0 && guider.PlayerVelocityVector.y != 0)
                        {
                            signX *= guider.PlayerVelocityVector.x;
                            signY *= guider.PlayerVelocityVector.y;
                            currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(12, 20) * 0.1f);
                            currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(12, 20) * 0.1f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }
                        else
                        {
                            currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(12, 20) * 0.1f);
                            currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(12, 20) * 0.1f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }*/
                        newMonster.transform.position = currentPos;
                        break;
                    case 1:
                        currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(3, 10) * 0.1f);
                        currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(15, 20) * 0.1f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                    case 2:
                        currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(15, 20) * 0.1f);
                        currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(3, 7) * 0.1f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                    case 3:
                        currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(12, 20) * 0.1f);
                        currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(12, 20) * 0.1f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                }
            }
            //몬스터 스폰을 할건데 1. 플레이어 이동 방향 바로 앞에 2. 플레이어 멈춰있을때 3. 완전 랜덤
        }

        // Do Spawn Monster
        switch (spawnLevel)
        {
            case 0:
                if (enabledMonster > 1)// level 1
                {
                    spawnLevel++;
                }
                break;
            case 1:
                if (enabledMonster > 2)// level 2
                {
                    spawnLevel++;
                }
                break;
            case 2:
                if (enabledMonster > 4)// level 3
                {
                    spawnLevel++;
                }
                break;
            case 3:
                if (enabledMonster > 7)// level 4
                {
                    spawnLevel++;
                }
                break;
            case 4:
                if (enabledMonster > 11)// level 5
                {
                    spawnLevel++;
                }
                break;
            case 5:
                if (enabledMonster > 16)// level 6
                {
                    spawnLevel++;
                }
                break;
            case 6:
                if (enabledMonster > 22)// level 7
                {
                    spawnLevel++;
                }
                break;
            case 7:
                if (enabledMonster > 29)// level 8
                {
                    spawnLevel++;
                }
                break;
            case 8:
                if (enabledMonster > 37)// level 9
                {
                    spawnLevel++;
                }
                break;
            case 9:
                if (enabledMonster > 46)// level 10
                {
                    spawnLevel++;
                }
                break;
            case 10:
                if (enabledMonster > 56)// level 11
                {
                    spawnLevel++;
                }
                break;
            case 11:
                if (enabledMonster > 67)// level 12
                {
                    spawnLevel++;
                }
                break;
            case 12:
                if (enabledMonster > 79)// level 13
                {
                    spawnLevel++;
                }
                break;
            case 13:
                if (enabledMonster > 82)// level 14
                {
                    spawnLevel++;
                }
                break;
            case 14:
                if (enabledMonster > 96)// level 15
                {
                    spawnLevel++;
                }
                break;
            case 15:
                if (enabledMonster > 111)// level 16
                {
                    spawnLevel++;
                }
                break;
        }

        // 몬스터 생성 처리
        for (int i = 0; i < spawnLevel; i++)
        {
            if (monsterSpawnPool.PoolQueue.Count == 0) break;

            if (monsterSpawnTimer[i] > 0 && !playerEscape)
            {
                monsterSpawnTimer[i]--;
            }

            if (monsterSpawnTimer[i] <= 0 && enabledMonster < MaxMonster)
            {
                if (Random.Range(0, 2) == 1) signX = 1;
                else signX = -1;
                if (Random.Range(0, 2) == 1) signY = 1;
                else signY = -1;
                //playerPos = guider.transform.position;
                monsterSpawnTimer[i] = 1500;

                enabledMonster++;

                if (monsterSpawnPool.PoolQueue.Count == 0) break;
                newMonster = monsterSpawnPool.GetObject();
                newMonster.monsterSpawn();
                switch (Random.Range(0, 2))
                {
                    default:
                        currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(10, 20) * 0.0005f);
                        currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(10, 20) * 0.0005f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;

                    case 0:
                        //플레이어가 보고 있는 방향으로 리스폰
                        /*
                        if (guider.PlayerVelocityVector.x != 0 && guider.PlayerVelocityVector.y != 0)
                        {
                            signX *= guider.PlayerVelocityVector.x;
                            signY *= guider.PlayerVelocityVector.y;
                            currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(10, 20) * 0.0005f);
                            currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(10, 20) * 0.0005f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }
                        //일반 리스폰
                        else
                        {
                            currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(10, 20) * 0.0005f);
                            currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(10, 20) * 0.0005f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }*/
                        newMonster.transform.position = currentPos;
                        break;
                    case 1:
                        //일반 리스폰
                        currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(10, 20) * 0.0005f);
                        currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(10, 20) * 0.0005f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                }
            }
        }
    }
}
