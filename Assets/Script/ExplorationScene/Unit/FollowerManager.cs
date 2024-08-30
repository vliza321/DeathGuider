using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    private GameObject[] follower;
    
    private int followerCounter;
    public int FollowerCounter
    {
        get { return followerCounter; }
        set { followerCounter = value; }
    }
    private GameObject playerManager;
    public GameObject PlayerManager
    {
        get { return playerManager; }
        set { playerManager = value; }
    }

    private void Awake()
    {

        followerCounter = this.transform.childCount;
        follower = new GameObject[followerCounter];
        for (int i =0; i< followerCounter; i++)
        {
            follower[i] = this.transform.GetChild(i).gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            follower[i].GetComponent<PlayerMove>().enabled = false;
            follower[i].GetComponent<FollowerMove>().enabled = true;
            follower[i].gameObject.tag = "follower";
            follower[i].gameObject.layer = this.gameObject.layer;
        }
    }

    public void SwapGuider(GameObject guider, GameObject firstFollower, CameraMove cameraObj, AttackDirectional attackDirectional)
    {
        firstFollower.GetComponent<FollowerMove>().enabled = false;
        firstFollower.GetComponent<PlayerMove>().enabled = true;
        firstFollower.GetComponent<PlayerMove>().MoveSpeed = guider.GetComponent<PlayerMove>().MoveSpeed;
        firstFollower.GetComponent<PlayerInRegion>().enabled = true;
        firstFollower.GetComponent<PlayerMove>().Camera = cameraObj;
        firstFollower.GetComponent<PlayerMove>().AttactDirectional = attackDirectional;
        firstFollower.layer = 8;
        firstFollower.tag = "Player";
        //follower[0].GetComponent<SpriteRenderer>().sortingOrder = 1;
        firstFollower.transform.position = guider.transform.position;
        firstFollower.transform.parent = guider.transform.parent;
        firstFollower.transform.SetAsFirstSibling();
    }

}
