using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class autorizedObject : MonoBehaviour
{

}


public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    Dictionary<GameObject, float> weaponDamage = new Dictionary<GameObject, float>();
    [SerializeField]    
    private ObjectPool<MonsterState> monsterSpawnPool;
    [SerializeField]
    private ObjectPool<MonsterState> monsterRespawnPool;


    private GameObject player;
    [SerializeField]
    private int monstercounter;
    [SerializeField]
    private int maxMonster;

    public int MaxMonster
    {
        get { return maxMonster; }
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

    private DontDestroyObjectManager DDOManager;
    private GameManager GameManager;
    MonsterState newMonster;

    private GoodsSpawnManager goldSpawn;
    private GoodsSpawnManager darkEssenseSpawn;
    private GoodsSpawnManager expSpawn;
    public GoodsSpawnManager GoldSpawn
    {
        get { return goldSpawn; }
        set { goldSpawn = value; }
    }
    public GoodsSpawnManager DarkEssenseSpawn
    {
        get { return darkEssenseSpawn; }
        set { darkEssenseSpawn = value; }
    }
    public GoodsSpawnManager ExpSpawn
    {
        get { return expSpawn; }
        set { expSpawn = value; }
    }

    public Dictionary<GameObject, float> WeaponDamage
    {
        get { return weaponDamage; }
        set { weaponDamage = value; }
    }

    private List<BoxCollision> boxes;

    public List<BoxCollision> Boxes
    {
        get { return boxes; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
        playerEscape = false;
        monster = new MonsterMove[MaxMonster];
        monsterSpawnPool = new ObjectPool<MonsterState>(MaxMonster);
        monsterRespawnPool = new ObjectPool<MonsterState>(MaxMonster);

        boxes = new List<BoxCollision>();

        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {
            if (manager.name == "PlayerManager")
            {
                Player = manager.transform.gameObject;
            }
        }
        Manager = null;

        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                GameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;

        for (int a = 0; a < MaxMonster; a++)
        {
            MonsterState newMonster = Instantiate(GameManager.Monster[GameManager.SelectStageID].gameObject).GetComponent<MonsterState>();
            newMonster.transform.SetParent(this.transform);
            newMonster.gameObject.SetActive(false);
            monsterSpawnPool.ReleaseObject(newMonster);
            monster[a] = newMonster.GetComponent<MonsterMove>();
            newMonster.GetComponent<MonsterState>().Init(DDOManager.MonsterDatas.MonsterDataDic[GameManager.SelectStageID],monsterRespawnPool,this,a);
        }

       

        signX = 0;
        signY = 0;

        monsterSpawnTimer = new int[(int)(maxMonster/10)];

        monstercounter = maxMonster;
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
        foreach (var m in monster)
        {
            m.Init();
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
            m.CanMove = false;
        }
    }
    private void Update()
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
            playerPos = guider.transform.position;
            monsterRespawnTimer = 1000;

            while(monsterRespawnPool.PoolQueue.Count != 0)
            {
                newMonster = monsterRespawnPool.GetObject();
                newMonster.monsterSpawn();
                
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

            if (monsterSpawnTimer[i] <= 0 && enabledMonster < maxMonster)
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
                newMonster.GetComponent<MonsterState>().monsterSpawn();
                newMonster.GetComponent<MonsterMove>().ActionState = MonsterActionState.Spawning;
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
                        }
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
