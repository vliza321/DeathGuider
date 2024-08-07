using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private GameObject[] follower;
    
    private int followerCounter;
    public int FollowerCounter
    {
        get { return followerCounter; }
        set { followerCounter = value; }
    }
    [SerializeField]
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

    public void printdebug()
    {
        Debug.Log("test");
    }
}
