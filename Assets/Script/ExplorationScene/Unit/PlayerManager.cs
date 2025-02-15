using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private GameObject weapon;

    private FollowerManager followerManager;
    private CameraManager cameraManager;
    private MonsterManager monsterManager;

    private GameObject guider;
    private GameObject swapedObject;
    [SerializeField]
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

    private PlayerHp guiderHp;
    private GameManager gameManager;
    private DontDestroyObjectManager ddoManager;

    private void Awake()
    {
        //GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        //foreach (var ddo in DDO)
        //{
        //    if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
        //    {
        //        ddoManager = ddo.GetComponent<DontDestroyObjectManager>();
        //    }
        //    if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
        //    {
        //        gameManager = ddo.transform.gameObject.GetComponent<GameManager>();
        //    }
        //}
        //DDO = null;

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
            if (manager.name == "MonsterSpawnManager")
            {
                monsterManager = manager.transform.gameObject.GetComponent<MonsterManager>();
            }
            if (manager.name == "CameraManager")
            {
                cameraManager = manager.transform.gameObject.GetComponent<CameraManager>();
            }
            if (manager.name == "FollowerManager")
            {
                followerManager = manager.transform.gameObject.GetComponent<FollowerManager>();
            }
            if (manager.name == "ResultManager")
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
        guiderHp = guider.GetComponent<PlayerHp>();

    }

    private void Start()
    {
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                ddoManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                gameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;

       
        bool guiderInParty = false;
        float damage;

        playerUnitCounter = ddoManager.UnitParticipateDatas.UnitParticipateDatas.Count;
        guiderHp = guider.GetComponentInChildren<PlayerHp>();

        foreach (var UP in ddoManager.UnitParticipateDatas.UnitParticipateDataDic.Values)
        {
            if (UP.Position == 0)
            {
                guiderInParty = true;
                damage = ddoManager.MonsterDatas.MonsterDataDic[gameManager.SelectStageID].Strength + gameManager.SelectStageID;
                guiderHp.Init(ddoManager.UnitDatas.UnitDataDic[(UP.UserID, UP.PrototypeUnitID, UP.InstanceID)], damage);
                
                guider.GetComponent<PlayerMove>().HeadAnimation.runtimeAnimatorController
                    = gameManager.PrototypeUnit[guiderHp.Stat.PrototypeUnitID].transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController;
                guider.GetComponent<PlayerMove>().BodyAnimation.runtimeAnimatorController
                    = gameManager.PrototypeUnit[guiderHp.Stat.PrototypeUnitID].transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController;

                resultManager.Units.Add(guiderHp.Stat);
                resultManager.PosToUnitData.Add(0, guiderHp);
            }
        }
        foreach (var UW in ddoManager.UseWeaponDatas.UseWeaponDatas)
        {
            if (UW.Position == 0)
            {
                weapon = Instantiate(gameManager.PrototypeWeapon[UW.PrototypeWeaponID]);
                weapon.transform.SetParent(guiderHp.transform.parent.transform);
                weapon.GetComponent<Weapon>().WeaponData = ddoManager.WeaponDatas.WeaponDataDic[(UW.UserID,UW.PrototypeWeaponID,UW.InstanceID)];
                damage = (guiderHp.Stat.Strength + ddoManager.WeaponDatas.WeaponDataDic[(UW.UserID, UW.PrototypeWeaponID, UW.InstanceID)].AttackPoint) * guiderHp.Stat.Handicraft;
                if (guiderHp.Stat.Crime == ddoManager.WeaponDatas.WeaponDataDic[(UW.UserID, UW.PrototypeWeaponID, UW.InstanceID)].Crime) damage = damage * 1.1f;
                weapon.transform.localScale = new Vector3(1, 1, 1);
                weapon.GetComponent<Weapon>().Initialize(monsterManager.WeaponDamage,damage,guider.transform,ddoManager.WeaponDatas.WeaponDataDic[(UW.UserID, UW.PrototypeWeaponID, UW.InstanceID)]);
                break;
            }
        }
        followerManager.AddUnitDataList(ddoManager,resultManager);
        followerManager.WeaponCreate(ddoManager, gameManager, monsterManager, resultManager);
        guider.GetComponent<PlayerMove>().InitSprite(
            gameManager.PrototypeUnit[guiderHp.Stat.PrototypeUnitID].transform.GetChild(0).GetComponent<SpriteRenderer>(),
            gameManager.PrototypeUnit[guiderHp.Stat.PrototypeUnitID].transform.GetChild(1).GetComponent<SpriteRenderer>());

        if (!guiderInParty) SwapPlayer();
        
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
        if (playerUnitCounter == 0) {
            guider.SetActive(false);
            resultManager.PlayerEscape();
            return;
        }

        //바꿀 대상이 되는 가장 앞에 있는 팔로워 바인딩

        //매니저들에서 가지고 있는 가이더 정보 변경
        monsterManager.PlayerSwap(swapedObject.GetComponent<PlayerMove>());
        guiderFollowerMove = followerManager.SwapGuider(guider, swapedObject, cameraManager, attackDirectional,guiderFollowerMove);
        cameraManager.Guider = swapedObject;
        treasureBoxEscapeStairManager.Player = swapedObject;
        attackDirectional.Guider = swapedObject;

        //변경 후 처리 (죽은 가이더 끄기, 몬스터 넉백)
        guider.transform.parent = followerManager.transform; guider.transform.SetSiblingIndex(transform.childCount);
        guider.SetActive(false);
        guider = swapedObject;
        //MonsterKnockBack();
        if(playerUnitCounter == 0) attackDirectional.gameObject.SetActive(false);
    }
}
