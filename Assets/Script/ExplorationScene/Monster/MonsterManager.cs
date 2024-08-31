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

    private int[] monsterRespawnTimer;
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

        monsterRespawnTimer = new int[MaxMonster];
        monsterSpawnTimer = new int[(int)(MaxMonster/16)];
        spawnTimerCanDoWork = new bool[(int)(MaxMonster/16)];

        monstercounter = MaxMonster;
        enabledMonster = 1;
        spawnTimerCanDoWork[0] = true;

        for(int i = 0; i < MaxMonster; i++)
        {
            monsterRespawnTimer[i] = 1000;
        }

        for (int i = 0; i < 10; i++)
        {
            monsterSpawnTimer[i] = 1500;
        }
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
        /*
        // 몬스터 리스폰 처리
        for (int i = 0; i < monstercounter; i++)
        {
            GameObject currentMonster = monsterPool.GetObject();
            MonsterState monsterState = currentMonster.GetComponent<MonsterState>();

            if (!currentMonster.activeSelf && monsterState.getInGame())
            {
                monsterRespawnTimer[i]--;
            }

            if (monsterRespawnTimer[i] <= 0)
            {
                if (Random.Range(0, 2) == 1) signX = 1;
                else signX = -1;
                if (Random.Range(0, 2) == 1) signY = 1;
                else signY = -1;
                playerPos = guider.transform.position;
                monsterRespawnTimer[i] = 1000;
                
                //몬스터 스폰을 할건데 1. 플레이어 이동 방향 바로 앞에 2. 플레이어 멈춰있을때 3. 완전 랜덤
                switch (Random.Range(0, 3))
                {
                    case 0:
                    case 1:
                    case 2:
                        currentMonster.transform.position = new Vector3(
                            playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f),
                            playerPos.y + signY * 12.8f * (Random.Range(13, 17) / 10.0f),
                            playerPos.z);
                        break;
                }

                monsterState.monsterRespawn();
            }
        }*/
        
        // 몬스터 생성 처리
        for (int i = 0; i < 10; i++)
        {
            if (spawnTimerCanDoWork[i] == false) break;

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
                GameObject newMonster = monsterSpawnPool.GetObject();

                switch (Random.Range(0, 3))
                {
                    case 0:
                    case 1:
                    case 2:
                        newMonster.transform.position = new Vector3(
                            playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f),
                            playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f),
                            playerPos.z);
                        break;
                }

                newMonster.GetComponent<MonsterState>().monsterRespawn(player);
            }
        }
    }
}
