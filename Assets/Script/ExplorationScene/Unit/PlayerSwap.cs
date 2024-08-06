using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwap : MonoBehaviour
{
    private GameObject FollowerManager;
    private GameObject CameraManager;
    private GameObject MonsterSpawnManager;

    private GameObject Guider;
    private GameObject swapedobject;
    private TreasureBoxEscapeStairManager treasureBoxEscapeStairManager;

    public GameObject guider
    {
        get
        {
            return Guider;
        }
    }
    private void Awake()
    {
        Guider = this.transform.GetChild(0).gameObject;
        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {
            if (manager.name == "TreasureBoxEscapeStairManager")
            {
                treasureBoxEscapeStairManager = manager.GetComponent<TreasureBoxEscapeStairManager>();
            }
            if(manager.name == "MonsterSpawnManager")
            {
                MonsterSpawnManager = manager.transform.gameObject;
            }
            if(manager.name == "CameraManager")
            {
                CameraManager = manager.transform.gameObject;
            }
            if(manager.name == "FollowerManager")
            {
                FollowerManager = manager.transform.gameObject;
            }
        }

        Manager = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        Guider.GetComponent<FollowerMove>().enabled = false;
        swapedobject = FollowerManager.transform.GetChild(0).gameObject;
        CameraManager.GetComponent<CameraMove>().Guider = Guider;
    }
    //this.gameObject.GetComponent<PlayerState>().enabled = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MonsterKnockBack();
        }
    }

    public void SwapPlayer(int num) // 죽는거 구현 전 임시, 임의로 서로 스왑
    {
        swapedobject = FollowerManager.transform.GetChild(0).gameObject;
        MonsterSpawnManager.GetComponent<MonsterSpawn>().PlayerSwap(swapedobject);
        swapedobject.transform.GetChild(0).tag = "Player";

        swapedobject.transform.parent = this.gameObject.transform;
        swapedobject.GetComponent<FollowerMove>().enabled = false;
        swapedobject.GetComponent<PlayerMove>().enabled = true;
        swapedobject.GetComponent<PlayerInRegion>().enabled = true;
        swapedobject.layer = 8;
        swapedobject.tag = "Player";
        swapedobject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        swapedobject.transform.position = Guider.transform.position;
        swapedobject.transform.SetAsFirstSibling();

        Guider.transform.GetChild(0).tag = "Untagged";
        Guider.GetComponent<FollowerMove>().enabled = true;
        Guider.GetComponent<PlayerMove>().enabled = false;
        Guider.GetComponent<PlayerInRegion>().enabled = false;
        Guider.transform.parent = FollowerManager.transform;
        Guider.layer = 10;
        Guider.tag = "follower";
        Guider.GetComponent<SpriteRenderer>().sortingOrder = 0;

        Guider = this.gameObject.transform.GetChild(0).gameObject;
        CameraManager.GetComponent<CameraMove>().Guider = Guider;
        treasureBoxEscapeStairManager.player = swapedobject;
        MonsterKnockBack();
    }

    void MonsterKnockBack()
    {
        MonsterSpawn ms = MonsterSpawnManager.GetComponent<MonsterSpawn>();
        for (int i =0; i < ms.EnabledMonster;i++)
        {
            if (ms.Monster[i].GetComponent<MonsterMove>().distance < 9.0f)
            { 
                ms.Monster[i].GetComponent<MonsterMove>().isKnockBack = true;
                ms.Monster[i].GetComponent<MonsterMove>().knockBackTimer = 300 - 300 * (int)(ms.Monster[i].GetComponent<MonsterMove>().distance / 9.0f);
                ms.Monster[i].GetComponent<MonsterMove>().monsterVelocityVector *= -2;

            }
        }
    }
}
