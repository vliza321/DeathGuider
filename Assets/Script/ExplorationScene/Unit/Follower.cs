using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject Player;
    public GameObject guider;
    public GameObject[] follower;
    
    public int followercounter;
    private void Awake()
    {
        for(int i =0; i<this.transform.childCount;i++)
        {
            follower[i] = this.transform.GetChild(i).gameObject;
        }
        followercounter = follower.Length;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void printdebug()
    {
        Debug.Log("test");
    }
}
