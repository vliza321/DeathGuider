using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject weapon;

    private FollowerManager followerManager;
    private CameraManager cameraManager;
    private MonsterManager monsterManager;

    private GameObject guider;
    private GameObject swapedObject;
    private TreasureBoxEscapeStairManager treasureBoxEscapeStairManager;
    private AttackDirectional attackDirectional;
    public AttackDirectional AttackDirectional
    {
        get { return attackDirectional; }
        set { attackDirectional = value; }
    }
    [SerializeField]
    private int playerUnitCounter;

    private GameObject weaponEffectPool;

    public GameObject WeaponEffectPool
    {
        get { return weaponEffectPool; }
    }
    public GameObject Guider
    {
        get
        {
            return guider;
        }
    }

    public GameObject Weapon
    {
        get { return weapon; }
    }
    private FollowerMove guiderFollowerMove;

    private ResultManager resultManager;

    private PlayerState GuiderState;
    private void Awake()
    {
        weaponEffectPool = this.transform.GetChild(this.transform.childCount - 1).gameObject;
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
                monsterManager = manager.transform.gameObject.GetComponent<MonsterManager>();
            }
            if(manager.name == "CameraManager")
            {
                cameraManager = manager.transform.gameObject.GetComponent<CameraManager>();
            }
            if(manager.name == "FollowerManager")
            {
                followerManager = manager.transform.gameObject.GetComponent<FollowerManager>();
            }
            if(manager.name == "ResultManager")
            {
                resultManager = manager.GetComponent<ResultManager>();
            }
        }

        Manager = null;

        followerManager.PlayerManager = this;
        followerManager.MatchingAttactDirection(attackDirectional);
        guider.GetComponent<PlayerMove>().AttactDirectional = attackDirectional;
        swapedObject = followerManager.transform.GetChild(0).gameObject;
        cameraManager.Guider = guider;
        guider.GetComponent<PlayerMove>().Camera = cameraManager;
        guiderFollowerMove = guider.GetComponent<FollowerMove>();
        guider.GetComponent<FollowerMove>().enabled = false;
        playerUnitCounter = 1;
        GuiderState = guider.GetComponent<PlayerState>();
        
    }

    private void Start()
    {
        playerUnitCounter += followerManager.gameObject.transform.childCount;

        foreach (var UP in resultManager.DDOManager.UnitParticipateDatas.UnitParticipateDataDic.Values)
        {
            if (UP.PrototypeUnitID != 100)
            {
                GuiderState.Stat = resultManager.DDOManager.UnitDatas.UnitDataDic[(UP.UserID, UP.PrototypeUnitID, UP.InstanceID)];
                guider.GetComponent<PlayerMove>().HeadAnimation.runtimeAnimatorController
                    = resultManager.GameManager.PrototypeUnit[GuiderState.Stat.PrototypeUnitID].transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController;
                guider.GetComponent<PlayerMove>().BodyAnimation.runtimeAnimatorController
                    = resultManager.GameManager.PrototypeUnit[GuiderState.Stat.PrototypeUnitID].transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController;



                resultManager.Units.Add(GuiderState.Stat);
            }
        }
        float damage;
        foreach (var UW in resultManager.DDOManager.UseWeaponDatas.UseWeaponDatas)
        {
            if (UW.Position == 0)
            {
                weapon = Instantiate(resultManager.GameManager.PrototypeWeapon[UW.PrototypeWeaponID]);
                weapon.transform.SetParent(GuiderState.transform);
                weapon.GetComponent<Weapon>().WeaponData = resultManager.DDOManager.WeaponDatas.WeaponDataDic[(UW.UserID,UW.PrototypeWeaponID,UW.InstanceID)];
                damage = (GuiderState.Stat.Strength + resultManager.DDOManager.WeaponDatas.WeaponDataDic[(UW.UserID, UW.PrototypeWeaponID, UW.InstanceID)].AttackPoint) * GuiderState.Stat.Handicraft;
                if (GuiderState.Stat.Crime == resultManager.DDOManager.WeaponDatas.WeaponDataDic[(UW.UserID, UW.PrototypeWeaponID, UW.InstanceID)].Crime) damage = damage * 1.1f;
                weapon.transform.localScale = new Vector3(1, 1, 1);
                weapon.GetComponent<Weapon>().Initialize(monsterManager.WeaponDamage,damage,guider.transform);
                break;
            }
        }
        followerManager.AddUnitDataList(resultManager);
        followerManager.WeaponCreate(resultManager, monsterManager);
        guider.GetComponent<PlayerMove>().InitSprite(
            resultManager.GameManager.PrototypeUnit[GuiderState.Stat.PrototypeUnitID].transform.GetChild(0).GetComponent<SpriteRenderer>(),
            resultManager.GameManager.PrototypeUnit[GuiderState.Stat.PrototypeUnitID].transform.GetChild(1).GetComponent<SpriteRenderer>());
        
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
        swapedObject = followerManager.gameObject.transform.GetChild(0).gameObject;
        if (playerUnitCounter == 1) {
            guider.SetActive(false);
            resultManager.PlayerEscape();
            return;
        }

        //바꿀 대상이 되는 가장 앞에 있는 팔로워 바인딩

        //매니저들에서 가지고 있는 가이더 정보 변경
        monsterManager.PlayerSwap(swapedObject.GetComponent<PlayerMove>());
        guiderFollowerMove = followerManager.SwapGuider(guider, swapedObject, cameraManager, attackDirectional,guiderFollowerMove);
        cameraManager.Guider = swapedObject;
        treasureBoxEscapeStairManager.player = swapedObject;
        attackDirectional.Guider = swapedObject;

        //변경 후 처리 (죽은 가이더 끄기, 몬스터 넉백)
        guider.transform.parent = followerManager.transform; guider.transform.SetSiblingIndex(transform.childCount);
        guider.SetActive(false);
        guider = swapedObject;
        //MonsterKnockBack();
        if(playerUnitCounter == 0) attackDirectional.gameObject.SetActive(false);
    }
    /*
    void MonsterKnockBack()
    {
        MonsterManager ms = monsterManager.GetComponent<MonsterManager>();
        for (int i =0; i < ms.EnabledMonster;i++)
        {
            if (ms.Monster[i].GetComponent<MonsterMove>().Distance < 9.0f)
            { 
                ms.Monster[i].GetComponent<MonsterMove>().IsKnockBack = true;
                ms.Monster[i].GetComponent<MonsterMove>().KnockBackTimer = 300 - 300 * (int)(ms.Monster[i].GetComponent<MonsterMove>().Distance / 9.0f);
                ms.Monster[i].GetComponent<MonsterMove>().MonsterVelocityVector *= -2;
            }
        }
    }*/
}
