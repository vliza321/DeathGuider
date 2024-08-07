using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �߻����
public class MonsterSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private int monstercounter;
    [SerializeField]
    private int MaxMonster;
    private int enabledMonster;
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
    private MonsterState monsterState;

    public GameObject Player
    { 
        get {  return player; } 
        set { player = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {
            if (manager.name == "PlayerManager")
            {
                Player = manager.transform.gameObject;
            }
        }
        Manager = null;

        monster = new GameObject[MaxMonster];
        monsterRespawnTimer = new int[MaxMonster];
        monsterSpawnTimer = new int[(int)(MaxMonster/16)];
        spawnTimerCanDoWork = new bool[(int)(MaxMonster/16)];

        monstercounter = this.transform.childCount;
        MaxMonster = this.transform.childCount;
        enabledMonster = 1;
        spawnTimerCanDoWork[0] = true;
        for(int i = 0; i < MaxMonster; i++)
        {
            monster[i] = this.transform.GetChild(i).gameObject;
            monsterRespawnTimer[i] = 1000;

        }
        for (int i = 0; i < 10; i++)
        {
            monsterSpawnTimer[i] = 1500;
        }


    }
    void Start()
    {
        monster[0].GetComponent<MonsterState>().setInGame();
        guider = player.GetComponent<PlayerSwap>().Guider;
        //monsterState = monster
    }
    public void PlayerSwap(GameObject Guider)
    {
        guider = Guider;
        for (int i = 0; i < MaxMonster; i++)
        {
            monster[i].GetComponent<MonsterMove>().guider = Guider;
        }
    }
    // Update is called once per frame

    private void Update()
    {
        float signX;
        float signY;
        Vector3 playerPos = guider.transform.position;
        for (int i = 0; i < monstercounter; i++)
        {
            //Do Respawn Monster
            if (monster[i].activeSelf == false && monster[i].GetComponent<MonsterState>().getInGame()) monsterRespawnTimer[i]--;
            if(monsterRespawnTimer[i] <= 0)
            {
                if (Random.Range(0, 2) == 1) signX = 1;
                else signX = -1;
                if (Random.Range(0, 2) == 1) signY = 1;
                else signY = -1;
                monsterRespawnTimer[i] = 1000;
                monster[i].SetActive(true);
                switch(Random.Range(0,3))
                {
                    case 0:
                        if (guider.GetComponent<PlayerMove>().PlayerVelocityVector.x != 0 && guider.GetComponent<PlayerMove>().PlayerVelocityVector.y != 0)
                        {
                            signX *= guider.GetComponent<PlayerMove>().PlayerVelocityVector.x;
                            signY *= guider.GetComponent<PlayerMove>().PlayerVelocityVector.y;
                            monster[i].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.z);
                        }
                        else monster[i].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.z);
                        break;
                    case 1:
                        monster[i].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.z);
                        break;
                    case 2:
                        monster[i].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.z);
                        break;
                }
                monster[i].GetComponent<MonsterState>().monsterRespawn();
            }
        }

        // Do Spawn Monster
        for (int i = 0; i <10; i++)
        {
            switch (i)
            {
                case 0:
                    if (enabledMonster > 1) spawnTimerCanDoWork[i] = true;
                    break;
                case 1:
                    if (enabledMonster > 3) spawnTimerCanDoWork[i] = true;
                    break;
                case 2:
                    if (enabledMonster > 5) spawnTimerCanDoWork[i] = true;
                    break;
                case 3:
                    if (enabledMonster > 8) spawnTimerCanDoWork[i] = true;
                    break;
                case 4:
                    if (enabledMonster > 13) spawnTimerCanDoWork[i] = true;
                    break;
                case 5:
                    if (enabledMonster > 21) spawnTimerCanDoWork[i] = true;
                    break;
                case 6:
                    if (enabledMonster >34) spawnTimerCanDoWork[i] = true;
                    break;
                case 7:
                    if (enabledMonster > 55) spawnTimerCanDoWork[i] = true;
                    break;
                case 8:
                    if (enabledMonster > 89) spawnTimerCanDoWork[i] = true;
                    break;
                case 9:
                    if (enabledMonster > 144) spawnTimerCanDoWork[i] = true;
                    break;

            }
            if (spawnTimerCanDoWork[i] == false) break;
            if(monsterSpawnTimer[i]>0)monsterSpawnTimer[i]--;
            if (monsterSpawnTimer[i] <= 0)
            {
                if (Random.Range(0, 2) == 1) signX = 1;
                else signX = -1;
                if (Random.Range(0, 2) == 1) signY = 1;
                else signY = -1;
                monsterSpawnTimer[i] = 1500;
                if(enabledMonster < MaxMonster)
                {

                    enabledMonster++;
                    monster[enabledMonster-1].SetActive(true);
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            if (guider.GetComponent<PlayerMove>().PlayerVelocityVector.x != 0 && guider.GetComponent<PlayerMove>().PlayerVelocityVector.y != 0)
                            {
                                signX *= guider.GetComponent<PlayerMove>().PlayerVelocityVector.x;
                                signY *= guider.GetComponent<PlayerMove>().PlayerVelocityVector.y;
                                monster[enabledMonster - 1].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f), playerPos.z);
                            }
                            else monster[enabledMonster - 1].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f), playerPos.z);

                            break;
                        case 1:
                            monster[enabledMonster-1].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f), playerPos.z);
                            break;
                        case 2:
                            monster[enabledMonster-1].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(13, 17) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(10, 15) / 10.0f), playerPos.z);
                            break;
                    }
                    monster[enabledMonster-1].GetComponent<MonsterState>().setInGame();
                }
            }
        }

    }
} 
