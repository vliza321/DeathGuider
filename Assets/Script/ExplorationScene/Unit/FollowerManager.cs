using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    [SerializeField]
    private ListQueue<FollowerMove> followerList;
    [SerializeField]
    private FollowerMove[] follower;
    
    private int followerCounter;
    public int FollowerCounter
    {
        get { return followerCounter; }
        set { followerCounter = value; }
    }

    private PlayerManager playerManager;
    public PlayerManager PlayerManager
    {
        get { return playerManager; }
        set { playerManager = value; }
    }
    [SerializeField]
    private AttackDirectional attackDirectional;

    public AttackDirectional AttackDirectional
    {
        get { return AttackDirectional; }
        set { attackDirectional = value; }
    }

    private GameObject weaponEffectPool;

    public GameObject WeaponEffectPool
    {
        get { return weaponEffectPool; }
    }
    private void Awake()
    {
        weaponEffectPool = this.transform.GetChild(this.transform.childCount - 1).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void MatchingAttactDirection(AttackDirectional attackDirectional)
    {
        followerList = new ListQueue<FollowerMove>();
        AttackDirectional = attackDirectional;
        followerCounter = this.transform.childCount-1;
        follower = new FollowerMove[followerCounter];
        for (int i = 0; i < followerCounter; i++)
        {
            followerList.Enqueue(this.transform.GetChild(i).gameObject.GetComponent<FollowerMove>());
            follower[i] = this.transform.GetChild(i).gameObject.GetComponent<FollowerMove>();
            follower[i].gameObject.GetComponent<PlayerMove>().AttactDirectional = attackDirectional;
            follower[i].gameObject.GetComponent<PlayerMove>().enabled = false;
            follower[i].gameObject.GetComponent<FollowerMove>().enabled = true;
            follower[i].gameObject.tag = "follower";
            follower[i].gameObject.layer = this.gameObject.layer;
            follower[i].gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            
        }
    }
    public FollowerMove SwapGuider(GameObject guider, GameObject firstFollower, CameraManager cameraObj, AttackDirectional attackDirectional, FollowerMove guiderFollwerMove)
    {
        var target = followerList.Dequeue();
        target.SwapGuider(cameraObj);
        followerList.Enqueue(guiderFollwerMove);
        return target;
    }

}
