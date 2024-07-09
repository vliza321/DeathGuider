using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject player;

    public int monstercounter;
    public int EnabledMonster;
    public int MaxMonster; 
    public GameObject[] monster;
    public int[] monsterRespawnTimer;
    public int[] monsterSpawnTimer;
    public bool[] spawnTimerCanDoWork;

    public GameObject guider;
    public MonsterState monsterState;
    // Start is called before the first frame update
    void Awake()
    {
        monstercounter = this.transform.childCount;
        MaxMonster = this.transform.childCount;
        EnabledMonster = 1;
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
                            monster[i].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(10, 15) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(10, 13) / 10.0f), playerPos.z);
                        }
                        else monster[i].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(10, 15) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(10, 13) / 10.0f), playerPos.z);
                        break;
                    case 1:
                        monster[i].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(8, 15) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(3, 5) / 10.0f), playerPos.z);
                        break;
                    case 2:
                        monster[i].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(8, 15) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(7, 10) / 10.0f), playerPos.z);
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
                    if (EnabledMonster > 1) spawnTimerCanDoWork[i] = true;
                    break;
                case 1:
                    if (EnabledMonster > 3) spawnTimerCanDoWork[i] = true;
                    break;
                case 2:
                    if (EnabledMonster > 5) spawnTimerCanDoWork[i] = true;
                    break;
                case 3:
                    if (EnabledMonster > 8) spawnTimerCanDoWork[i] = true;
                    break;
                case 4:
                    if (EnabledMonster > 13) spawnTimerCanDoWork[i] = true;
                    break;
                case 5:
                    if (EnabledMonster > 21) spawnTimerCanDoWork[i] = true;
                    break;
                case 6:
                    if (EnabledMonster >34) spawnTimerCanDoWork[i] = true;
                    break;
                case 7:
                    if (EnabledMonster > 55) spawnTimerCanDoWork[i] = true;
                    break;
                case 8:
                    if (EnabledMonster > 89) spawnTimerCanDoWork[i] = true;
                    break;
                case 9:
                    if (EnabledMonster > 144) spawnTimerCanDoWork[i] = true;
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
                if(EnabledMonster < MaxMonster)
                {

                    EnabledMonster++;
                    monster[EnabledMonster-1].SetActive(true);
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            if (guider.GetComponent<PlayerMove>().PlayerVelocityVector.x != 0 && guider.GetComponent<PlayerMove>().PlayerVelocityVector.y != 0)
                            {
                                signX = guider.GetComponent<PlayerMove>().PlayerVelocityVector.x;
                                signY = guider.GetComponent<PlayerMove>().PlayerVelocityVector.y;
                                monster[EnabledMonster - 1].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(8, 15) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(5, 13) / 10.0f), playerPos.z);
                            }
                            else monster[EnabledMonster - 1].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(9, 15) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(7, 15) / 10.0f), playerPos.z);

                            break;
                        case 1:
                            monster[EnabledMonster-1].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(9, 15) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(7, 15) / 10.0f), playerPos.z);
                            break;
                        case 2:
                            monster[EnabledMonster-1].transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(9, 15) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(7, 15) / 10.0f), playerPos.z);
                            break;
                    }
                    monster[EnabledMonster-1].GetComponent<MonsterState>().setInGame();
                }
            }
           
        }

    }
}
