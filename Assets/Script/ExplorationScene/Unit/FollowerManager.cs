using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] follower;
    
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
        Debug.Log(attackDirectional.gameObject.name);
        AttackDirectional = attackDirectional;
        followerCounter = this.transform.childCount-1;
        follower = new GameObject[followerCounter];
        for (int i = 0; i < followerCounter; i++)
        {
            follower[i] = this.transform.GetChild(i).gameObject;
            follower[i].GetComponent<PlayerMove>().AttactDirectional = attackDirectional;
            follower[i].GetComponent<PlayerMove>().enabled = false;
            follower[i].GetComponent<FollowerMove>().enabled = true;
            follower[i].gameObject.tag = "follower";
            follower[i].gameObject.layer = this.gameObject.layer;
            follower[i].GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
    public void SwapGuider(GameObject guider, GameObject firstFollower, CameraManager cameraObj, AttackDirectional attackDirectional)
    {
        firstFollower.GetComponent<FollowerMove>().enabled = false;
        firstFollower.GetComponent<PlayerMove>().enabled = true;
        firstFollower.GetComponent<PlayerMove>().MoveSpeed = guider.GetComponent<PlayerMove>().MoveSpeed;
        firstFollower.GetComponent<PlayerMove>().Camera = cameraObj;
        firstFollower.GetComponent<PlayerMove>().AttactDirectional = attackDirectional;
        firstFollower.GetComponent<CapsuleCollider2D>().enabled = true;
        firstFollower.layer = 10;
        firstFollower.tag = "Player";
        //follower[0].GetComponent<SpriteRenderer>().sortingOrder = 1;
        firstFollower.transform.position = guider.transform.position;
        firstFollower.transform.parent = guider.transform.parent;
        firstFollower.transform.SetAsFirstSibling();
    }

}
