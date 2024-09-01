using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private FollowerManager followerManager;
    private CameraManager cameraManager;
    private GameObject monsterSpawnManager;

    private GameObject guider;
    private GameObject swapedObject;
    private TreasureBoxEscapeStairManager treasureBoxEscapeStairManager;
    private AttackDirectional attackDirectional;
    [SerializeField]
    private int playerUnitCounter;
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
                cameraManager = manager.transform.gameObject.GetComponent<CameraManager>();
            }
            if(manager.name == "FollowerManager")
            {
                followerManager = manager.transform.gameObject.GetComponent<FollowerManager>();
            }
        }

        Manager = null;

        followerManager.PlayerManager = this.gameObject;
        guider.GetComponent<FollowerMove>().enabled = false;
        swapedObject = followerManager.transform.GetChild(0).gameObject;
        cameraManager.Guider = guider;
        guider.GetComponent<PlayerMove>().Camera = cameraManager;
        playerUnitCounter = 1;
    }

    private void Start()
    {
        playerUnitCounter += followerManager.gameObject.transform.childCount;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapPlayer();
        }
    }

    public void SwapPlayer() 
    {
        playerUnitCounter--;
        if (playerUnitCounter < 0) return;

        //바꿀 대상이 되는 가장 앞에 있는 팔로워 바인딩
        swapedObject = followerManager.gameObject.transform.GetChild(0).gameObject;

        //매니저들에서 가지고 있는 가이더 정보 변경
        monsterSpawnManager.GetComponent<MonsterManager>().PlayerSwap(swapedObject);
        followerManager.SwapGuider(guider, swapedObject, cameraManager, attackDirectional);
        cameraManager.Guider = swapedObject;
        treasureBoxEscapeStairManager.player = swapedObject;
        attackDirectional.Guider = swapedObject;

        //변경 후 처리 (죽은 가이더 끄기, 몬스터 넉백)
        guider.transform.parent = followerManager.transform;
        guider.SetActive(false);
        guider = swapedObject;
        //MonsterKnockBack();
        if(playerUnitCounter == 0) attackDirectional.gameObject.SetActive(false);
    }

    void MonsterKnockBack()
    {
        MonsterManager ms = monsterSpawnManager.GetComponent<MonsterManager>();
        for (int i =0; i < ms.EnabledMonster;i++)
        {
            if (ms.Monster[i].GetComponent<MonsterMove>().Distance < 9.0f)
            { 
                ms.Monster[i].GetComponent<MonsterMove>().IsKnockBack = true;
                ms.Monster[i].GetComponent<MonsterMove>().KnockBackTimer = 300 - 300 * (int)(ms.Monster[i].GetComponent<MonsterMove>().Distance / 9.0f);
                ms.Monster[i].GetComponent<MonsterMove>().MonsterVelocityVector *= -2;
            }
        }
    }
}
