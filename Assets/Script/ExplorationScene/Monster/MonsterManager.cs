using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �߻����
public class MonsterManager : MonoBehaviour
{
    
    [SerializeField]
    private ObjectPool monsterSpawnPool;
    [SerializeField]
    private ObjectPool monsterRespawnPool;


    private GameObject player;
    [SerializeField]
    private int monstercounter;
    [SerializeField]
    private int MaxMonster;

    public int maxMonster
    {
        get { return MaxMonster; }
    }


    private int enabledMonster;
    private Vector3 playerPos;

    private float signX;
    private float signY;
    public int EnabledMonster
    {
        get { return enabledMonster; }
    }
    private GameObject[] monster;
    public GameObject[] Monster
    {
        get { return monster; }
        set { monster = value; }
    }

    private int monsterRespawnTimer;
    private int[] monsterSpawnTimer;
    private bool[] spawnTimerCanDoWork;

    private GameObject guider;

    [SerializeField]
    private GameObject[] monsterPrefab;
    public GameObject Player
    { 
        get {  return player; } 
        set { player = value; }
    }
    private Vector3 currentPos;
    private int spawnLevel;
    // Start is called before the first frame update
    void Awake()
    {
        monsterSpawnPool = new ObjectPool(monsterPrefab,maxMonster);
        monsterRespawnPool = new ObjectPool(maxMonster);

        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {
            if (manager.name == "PlayerManager")
            {
                Player = manager.transform.gameObject;
            }
        }
        Manager = null;

        signX = 0;
        signY = 0;

        monsterSpawnTimer = new int[(int)(MaxMonster/16)];
        spawnTimerCanDoWork = new bool[(int)(MaxMonster/16)];

        monstercounter = MaxMonster;
        enabledMonster = 1;
        spawnTimerCanDoWork[0] = true;
        monsterRespawnTimer = 1000;
        spawnLevel = 1;
        for (int i = 0; i < 10; i++)
        {
            monsterSpawnTimer[i] = 1500;
        }
        monsterSpawnTimer[0] = 10;
    }
    
    void Start()
    {
        guider = player.GetComponent<PlayerManager>().Guider;
        playerPos = guider.transform.position;

        for (int i = 0; i < MaxMonster; i++)
        {
            monsterSpawnPool.ReturnObject(this.transform.GetChild(i).gameObject);
        }
    }

    public void PlayerSwap(GameObject Guider)
    {
        guider = Guider;
    }
    // Update is called once per frame

    private void Update()
    {
        GameObject newMonster;
        // 몬스터 리스폰 처리
        if (monsterRespawnTimer > 0)
        {
            monsterRespawnTimer--;
        }
        else
        {
            if (Random.Range(0, 2) == 1) signX = 1;
            else signX = -1;
            if (Random.Range(0, 2) == 1) signY = 1;
            else signY = -1;
            playerPos = guider.transform.position;
            monsterRespawnTimer = 1000;

            while(monsterRespawnPool.PoolQueue.Count != 0)
            {
                newMonster = monsterRespawnPool.GetObject();
                newMonster.GetComponent<MonsterState>().monsterSpawn(guider,monsterRespawnPool);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        if (guider.GetComponent<PlayerMove>().PlayerVelocityVector.x != 0 && guider.GetComponent<PlayerMove>().PlayerVelocityVector.y != 0)
                        {
                            signX *= guider.GetComponent<PlayerMove>().PlayerVelocityVector.x;
                            signY *= guider.GetComponent<PlayerMove>().PlayerVelocityVector.y;
                            currentPos.x = playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f);
                            currentPos.y = playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }
                        else
                        {
                            currentPos.x = playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f);
                            currentPos.y = playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }
                        break;
                    case 1:
                        currentPos.x = playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f);
                        currentPos.y = playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                    case 2:
                        currentPos.x = playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f);
                        currentPos.y = playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                }
            }
            //몬스터 스폰을 할건데 1. 플레이어 이동 방향 바로 앞에 2. 플레이어 멈춰있을때 3. 완전 랜덤
        }

        // Do Spawn Monster
        for (int i = 0; i < 10; i++)
        {
            switch (i)
            {
                case 0:
                    if (enabledMonster > 1)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 1:
                    if (enabledMonster > 3)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 2:
                    if (enabledMonster > 5)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 3:
                    if (enabledMonster > 8)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 4:
                    if (enabledMonster > 13)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 5:
                    if (enabledMonster > 21)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 6:
                    if (enabledMonster > 34)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 7:
                    if (enabledMonster > 55)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 8:
                    if (enabledMonster > 89)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
                case 9:
                    if (enabledMonster > 144)
                    {
                        spawnTimerCanDoWork[i] = true;
                        spawnLevel++;
                    }
                    break;
            }
        }
        
        // 몬스터 생성 처리
        for (int i = 0; i < 10; i++)
        {
            if (spawnTimerCanDoWork[i] == false || monsterSpawnPool.PoolQueue.Count == 0) break;

            if (monsterSpawnTimer[i] > 0) monsterSpawnTimer[i]--;

            if (monsterSpawnTimer[i] <= 0 && enabledMonster < MaxMonster)
            {
                if (Random.Range(0, 2) == 1) signX = 1;
                else signX = -1;
                if (Random.Range(0, 2) == 1) signY = 1;
                else signY = -1;
                playerPos = guider.transform.position;
                monsterSpawnTimer[i] = 1500;

                enabledMonster++;

                if (monsterSpawnPool.PoolQueue.Count == 0) break;
                newMonster = monsterSpawnPool.GetObject();
                newMonster.GetComponent<MonsterState>().monsterSpawn(player,monsterRespawnPool);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        if (guider.GetComponent<PlayerMove>().PlayerVelocityVector.x != 0 && guider.GetComponent<PlayerMove>().PlayerVelocityVector.y != 0)
                        {
                            signX *= guider.GetComponent<PlayerMove>().PlayerVelocityVector.x;
                            signY *= guider.GetComponent<PlayerMove>().PlayerVelocityVector.y;
                            newMonster.transform.position = new Vector3(
                                playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f),
                                playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f),
                                playerPos.z);
                        }
                        else
                        {
                            currentPos.x = playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f);
                            currentPos.y = playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }
                        break;
                    case 1:
                        currentPos.x = playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f);
                        currentPos.y = playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                    case 2:
                        currentPos.x = playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f);
                        currentPos.y = playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                }



            }
        }
    }
}
