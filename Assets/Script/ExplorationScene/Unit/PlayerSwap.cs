using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwap : MonoBehaviour
{
    public GameObject Follower;
    public GameObject Guider;
    public GameObject CameraManager;
    public GameObject MonsterSpawner;
    // Start is called before the first frame update
    void Start()
    {
        Guider.GetComponent<FollowerMove>().enabled = false;
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

    void SwapPlayer(int num) // 죽는거 구현 전 임시, 임의로 서로 스왑
    {
        GameObject swapedobject = Follower.transform.GetChild(0).gameObject;
        MonsterSpawner.GetComponent<MonsterSpawn>().guider = swapedobject;
        swapedobject.transform.GetChild(0).tag = "Player";

        swapedobject.transform.parent = this.gameObject.transform;
        swapedobject.GetComponent<FollowerMove>().enabled = false;
        swapedobject.GetComponent<PlayerMove>().enabled = true;
        swapedobject.GetComponent<PlayerInRegion>().enabled = true;
        swapedobject.layer = 8;
        swapedobject.tag = "Player";
        swapedobject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        swapedobject.transform.position = Guider.transform.position;

        Guider.transform.GetChild(0).tag = "Untagged";
        Guider.GetComponent<FollowerMove>().enabled = true;
        Guider.GetComponent<PlayerMove>().enabled = false;
        Guider.GetComponent<PlayerInRegion>().enabled = false;
        Guider.transform.parent = Follower.transform;
        Guider.layer = 10;
        Guider.tag = "follower";
        Guider.GetComponent<SpriteRenderer>().sortingOrder = 0;

        Guider = this.gameObject.transform.GetChild(0).gameObject;
        CameraManager.GetComponent<CameraMove>().guider = Guider;

        MonsterKnockBack();
    }

    void MonsterKnockBack()
    {
        MonsterSpawn ms = MonsterSpawner.GetComponent<MonsterSpawn>();
        for (int i =0; i < ms.EnabledMonster;i++)
        {
            if (ms.monster[i].GetComponent<MonsterMove>().distance < 9.0f)
            { 
                ms.monster[i].GetComponent<MonsterMove>().isKnockBack = true;
                ms.monster[i].GetComponent<MonsterMove>().knockBackTimer = 300 - 300 * (int)(ms.monster[i].GetComponent<MonsterMove>().distance / 9.0f);
                ms.monster[i].GetComponent<MonsterMove>().MonsterVelocityVector *= -2;

            }
        }
    }
}
