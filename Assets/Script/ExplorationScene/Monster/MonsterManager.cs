using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private MonsterMove[] monster;
    public MonsterMove[] Monster
    {
        get { return monster; }
        set { monster = value; }
    }

    private int monsterRespawnTimer;
    [SerializeField]
    private int[] monsterSpawnTimer;

    private PlayerMove guider;

    [SerializeField]
    private GameObject[] monsterPrefab;
    public GameObject Player
    { 
        get {  return player; } 
        set { player = value; }
    }
    private Vector3 currentPos;
    [SerializeField]
    private int spawnLevel;

    private bool playerEscape;

    private Vector2 screenSize;
    // Start is called before the first frame update
    void Awake()
    {
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
        playerEscape = false;
        monster = new MonsterMove[this.transform.childCount];
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

        monsterSpawnTimer = new int[(int)(MaxMonster/10)];

        monstercounter = MaxMonster;
        enabledMonster = 1;
        monsterRespawnTimer = 1000;
        spawnLevel = 1;
        for (int i = 0; i < monsterSpawnTimer.Length; i++)
        {
            monsterSpawnTimer[i] = 1200;
        }
        monsterSpawnTimer[0] = 100;
    }
    
    void Start()
    {
        guider = player.GetComponent<PlayerManager>().Guider.GetComponent<PlayerMove>();
        playerPos = guider.transform.position;

        for (int i = 0; i < MaxMonster; i++)
        {
            monsterSpawnPool.ReturnObject(this.transform.GetChild(i).gameObject);
        }
        for(int i = 0;i<this.transform.childCount;i++)
        {
            monster[i] = this.transform.GetChild(i).GetComponent<MonsterMove>() ;
        }
    }

    public void PlayerSwap(PlayerMove Guider)
    {
        guider = Guider;
        foreach(var m in monster)
        {
            if (m.gameObject.activeSelf == true)
            {
                m.Guider = Guider.gameObject;
            }
        }
        
    }
    // Update is called once per frame
    public void PlayerEscape()
    {
        playerEscape = true;
        foreach(var m in monster)
        {
            m.GetComponent<MonsterState>().CanMove = false;
        }
    }
    private void Update()
    {
        GameObject newMonster;
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
            playerPos = guider.transform.position;
            monsterRespawnTimer = 1000;

            while(monsterRespawnPool.PoolQueue.Count != 0)
            {
                newMonster = monsterRespawnPool.GetObject();
                newMonster.GetComponent<MonsterState>().monsterSpawn(guider.gameObject,monsterRespawnPool);
                switch (Random.Range(0, 4))
                {
                    default:
                        currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(12, 20) * 0.1f);
                        currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(12, 20) * 0.1f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                    case 0:
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
                        }
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

            if (monsterSpawnTimer[i] > 0 && !playerEscape) { 
                monsterSpawnTimer[i]--; 
            }

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
                switch (Random.Range(0, 4))
                {
                    default:
                        currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(12, 20) * 0.1f);
                        currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(12, 20) * 0.1f);
                        currentPos.z = playerPos.z;
                        newMonster.transform.position = currentPos;
                        break;
                    case 0:
                        if (guider.PlayerVelocityVector.x != 0 && guider.PlayerVelocityVector.y != 0)
                        {
                            signX *= guider.PlayerVelocityVector.x;
                            signY *= guider.PlayerVelocityVector.y;
                            currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(15, 20) * 0.1f);
                            currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(15, 20) * 0.1f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }
                        else
                        {
                            currentPos.x = playerPos.x + signX * screenSize.x * (Random.Range(15, 20) * 0.1f);
                            currentPos.y = playerPos.y + signY * screenSize.y * (Random.Range(15, 20) * 0.1f);
                            currentPos.z = playerPos.z;
                            newMonster.transform.position = currentPos;
                        }
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
        }
    }
}
