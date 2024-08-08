using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwap : MonoBehaviour
{
    private GameObject followerManager;
    private GameObject cameraManager;
    private GameObject monsterSpawnManager;

    private GameObject guider;
    private GameObject swapedObject;
    private TreasureBoxEscapeStairManager treasureBoxEscapeStairManager;
    private AttackDirectional attackDirectional;
    public GameObject Guider
    {
        get
        {
            return guider;
        }
    }
    private void Awake()
    {
        attackDirectional = this.transform.GetChild(1).gameObject.GetComponent<AttackDirectional>();
        guider = this.transform.GetChild(0).gameObject;
        GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
        foreach (GameObject manager in Manager)
        {
            if (manager.name == "TreasureBoxEscapeStairManager")
            {
                treasureBoxEscapeStairManager = manager.GetComponent<TreasureBoxEscapeStairManager>();
            }
            if(manager.name == "MonsterSpawnManager")
            {
                monsterSpawnManager = manager.transform.gameObject;
            }
            if(manager.name == "CameraManager")
            {
                cameraManager = manager.transform.gameObject;
            }
            if(manager.name == "FollowerManager")
            {
                followerManager = manager.transform.gameObject;
            }
        }

        Manager = null;

        followerManager.GetComponent<Follower>().PlayerManager = this.gameObject;
        guider.GetComponent<FollowerMove>().enabled = false;
        swapedObject = followerManager.transform.GetChild(0).gameObject;
        cameraManager.GetComponent<CameraMove>().Guider = guider;
    }

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
        swapedObject = followerManager.transform.GetChild(0).gameObject;
        monsterSpawnManager.GetComponent<MonsterSpawn>().PlayerSwap(swapedObject);
        swapedObject.transform.GetChild(0).tag = "Player";

        swapedObject.transform.parent = this.gameObject.transform;
        swapedObject.GetComponent<FollowerMove>().enabled = false;
        swapedObject.GetComponent<PlayerMove>().enabled = true;
        swapedObject.GetComponent<PlayerInRegion>().enabled = true;
        swapedObject.layer = 8;
        swapedObject.tag = "Player";
        swapedObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        swapedObject.transform.position = guider.transform.position;
        swapedObject.transform.SetAsFirstSibling();

        guider.transform.GetChild(0).tag = "Untagged";
        guider.GetComponent<FollowerMove>().enabled = true;
        guider.GetComponent<PlayerMove>().enabled = false;
        guider.GetComponent<PlayerInRegion>().enabled = false;
        guider.transform.parent = followerManager.transform;
        guider.layer = 10;
        guider.tag = "follower";
        guider.GetComponent<SpriteRenderer>().sortingOrder = 0;

        guider = this.gameObject.transform.GetChild(0).gameObject;
        cameraManager.GetComponent<CameraMove>().Guider = guider;
        treasureBoxEscapeStairManager.player = swapedObject;
        //attackDirectional.Guider = swapedObject;
        MonsterKnockBack();
    }

    void MonsterKnockBack()
    {
        MonsterSpawn ms = monsterSpawnManager.GetComponent<MonsterSpawn>();
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
