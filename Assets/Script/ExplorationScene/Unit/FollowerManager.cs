using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    GameObject[] weapon = new GameObject[4];

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

    public void AddUnitDataList(ResultManager resultManager)
    {
        int position = 0;
        foreach (var UP in resultManager.DDOManager.UnitParticipateDatas.UnitParticipateDatas)
        {
            if (UP.Position == 0)
            {
                continue;
            }
            position = UP.Position;
            follower[position - 1].gameObject.GetComponent<PlayerState>().Stat = resultManager.DDOManager.UnitDatas.UnitDataDic[(UP.UserID, UP.PrototypeUnitID, UP.InstanceID)];

            var temt = resultManager.DDOManager.UnitDatas.UnitDataDic[(UP.UserID, UP.PrototypeUnitID, UP.InstanceID)];
            follower[position - 1].InitAnimator(resultManager.GameManager.PrisonerHeadAnim[temt.HeadID], resultManager.GameManager.PrisonerBodyAnim[temt.BodyID]);
            follower[position - 1].InitSprite(resultManager.GameManager.PrisonerHeadImg[temt.HeadID], resultManager.GameManager.PrisonerBodyImg[temt.BodyID]);
        }

        for (int i = 0; i < followerCounter; i++)
        {
            resultManager.Units.Add(follower[i].gameObject.GetComponent<PlayerState>().Stat);
        }
    }
    public void WeaponCreate(ResultManager resultManager, MonsterManager monsterManager)
    {
        int position = 0;
        float damage = 0;
        foreach (var UW in resultManager.DDOManager.UseWeaponDatas.UseWeaponDatas)
        {
            position = UW.Position;
            if (UW.Position == 0)
            {
                continue;
            }
            weapon[position - 1] = Instantiate(resultManager.GameManager.PrototypeWeapon[UW.PrototypeWeaponID]);
            weapon[position - 1].GetComponent<Weapon>().WeaponData = resultManager.DDOManager.WeaponDatas.WeaponDataDic[(UW.UserID,UW.PrototypeWeaponID,UW.InstanceID)];
            weapon[position - 1].transform.SetParent(follower[position - 1].transform);
            weapon[position - 1].transform.localScale = new Vector3(1, 1, 1);
            weapon[position - 1].GetComponent<Weapon>().WeaponData = resultManager.DDOManager.WeaponDatas.WeaponDataDic[(UW.UserID, UW.PrototypeWeaponID, UW.InstanceID)];
            UnitData temtData = follower[position - 1].gameObject.GetComponent<PlayerState>().Stat;
            position = UW.Position;
            damage = (temtData.Strength + resultManager.DDOManager.WeaponDatas.WeaponDataDic[(UW.UserID, UW.PrototypeWeaponID, UW.InstanceID)].AttackPoint) * temtData.Handicraft;
            if (temtData.Crime == resultManager.DDOManager.WeaponDatas.WeaponDataDic[(UW.UserID, UW.PrototypeWeaponID, UW.InstanceID)].Crime) damage = damage * 1.1f;
            weapon[position - 1].GetComponent<Weapon>().Initialize(monsterManager.WeaponDamage,damage,follower[position - 1].transform);
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
