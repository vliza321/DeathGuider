using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DontDestroyObjectManager : MonoBehaviour
{
    private static DontDestroyObjectManager instance;

    [SerializeField]
    private AppearDataList appearDatas;
    [SerializeField]
    private DialogDataList dialogDatas;
    [SerializeField]
    private HavePartyDataList havePartyDatas;
    [SerializeField]
    private LocalUserDataList localUserDatas;
    [SerializeField]
    private MonsterDataList monsterDatas;
    [SerializeField]
    private PartyDataList partyDatas;
    [SerializeField]
    private ProgressDataList progressDatas;
    [SerializeField]
    private PrototypeUnitDataList prototypeUnitDatas;
    [SerializeField]
    private PrototypeWeaponDataList prototypeWeaponDatas;
    [SerializeField]
    private StageDataList stageDatas;
    [SerializeField]
    private UnitDataList unitDatas;
    [SerializeField]
    private UnitParticipateDataList unitParticipateDatas;
    [SerializeField]
    private UseWeaponDataList useWeaponDatas;
    [SerializeField]
    private WeaponDataList weaponDatas;
    private GameManager gameManager;

    public GameManager GameManager
    {
        get { return gameManager; }
    }
    public AppearDataList AppearDatas
    {
        get { return appearDatas; }
    }
        
    public DialogDataList DialogDatas
    {
        get { return dialogDatas; }
    }
    public HavePartyDataList HavePartyDatas
    {
        get { return havePartyDatas; }
    }
    public LocalUserDataList LocalUserDatas
    {
        get { return localUserDatas; }
    }
    public MonsterDataList MonsterDatas
    {
        get { return monsterDatas; }
    }
    public PartyDataList PartyDatas
    {
        get { return partyDatas; }
    }
    public ProgressDataList ProgressDatas
    {
        get { return progressDatas; }
    }
    public PrototypeUnitDataList PrototypeUnitDatas
    {
        get { return prototypeUnitDatas; }
    }
    public PrototypeWeaponDataList PrototypeWeaponDatas
    {
        get { return prototypeWeaponDatas; }
    }
    public StageDataList StageDatas
    {
        get { return stageDatas; }
    }

    public UnitDataList UnitDatas
    {
        get { return unitDatas; }
    }
    public UnitParticipateDataList UnitParticipateDatas
    {
        get { return unitParticipateDatas; }
    }

    public UseWeaponDataList UseWeaponDatas
    {
        get { return useWeaponDatas; }
    }

    public WeaponDataList WeaponDatas
    {
        get { return weaponDatas; }
    }

    private CSVManager csvManager;

    private Dictionary<string, DataScriptableObjects> dataBaseDic;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }

    private void Initialize()
    {

        dataBaseDic = new Dictionary<string, DataScriptableObjects>();

        appearDatas = new AppearDataList();
        dialogDatas = new DialogDataList();
        havePartyDatas = new HavePartyDataList();
        localUserDatas = new LocalUserDataList();
        monsterDatas = new MonsterDataList();
        partyDatas = new PartyDataList();
        progressDatas = new ProgressDataList();
        prototypeUnitDatas = new PrototypeUnitDataList();
        prototypeWeaponDatas = new PrototypeWeaponDataList();
        stageDatas = new StageDataList();
        unitDatas = new UnitDataList();
        unitParticipateDatas = new UnitParticipateDataList();
        useWeaponDatas = new UseWeaponDataList();
        weaponDatas = new WeaponDataList();

        dataBaseDic.Add("Appear", appearDatas);
        dataBaseDic.Add("Dialog", dialogDatas);
        dataBaseDic.Add("HaveParty", havePartyDatas);
        dataBaseDic.Add("LocalUser", localUserDatas);
        dataBaseDic.Add("Monster", monsterDatas);
        dataBaseDic.Add("Party", partyDatas);
        dataBaseDic.Add("Progress", progressDatas);
        dataBaseDic.Add("PrototypeUnit", prototypeUnitDatas);
        dataBaseDic.Add("PrototypeWeapon", prototypeWeaponDatas);
        dataBaseDic.Add("Stage", stageDatas);
        dataBaseDic.Add("Unit", unitDatas);
        dataBaseDic.Add("UnitParticipate", unitParticipateDatas);
        dataBaseDic.Add("UseWeapon", useWeaponDatas);
        dataBaseDic.Add("Weapon", weaponDatas);


        //각 데이터 클래스의 list 초기화
        appearDatas.AppearDatas.Clear();
        dialogDatas.DialogDatas.Clear();
        havePartyDatas.HavePartyDatas.Clear();
        localUserDatas.LocalUserDatas.Clear();
        monsterDatas.MonsterDatas.Clear();
        partyDatas.PartyDatas.Clear();
        progressDatas.ProgressDatas.Clear();
        prototypeUnitDatas.PrototypeUnitDatas.Clear();
        prototypeWeaponDatas.PrototypeWeaponDatas.Clear();
        stageDatas.StageDatas.Clear();
        unitDatas.UnitDatas.Clear();
        unitParticipateDatas.UnitParticipateDatas.Clear();
        useWeaponDatas.UseWeaponDatas.Clear();
        weaponDatas.WeaponDatas.Clear();

        //각 데이터 클래스의 dictionary 초기화
        appearDatas.AppearDataDic.Clear();
        dialogDatas.DialogDataDic.Clear();
        havePartyDatas.HavePartyDataDic.Clear();
        localUserDatas.LocalUserDataDic.Clear();
        monsterDatas.MonsterDataDic.Clear();
        partyDatas.PartyDataDic.Clear();
        progressDatas.ProgressDataDic.Clear();
        prototypeUnitDatas.PrototypeUnitDataDic.Clear();
        prototypeWeaponDatas.PrototypeWeaponDataDic.Clear();
        stageDatas.StageDataDic.Clear();
        unitDatas.UnitDataDic.Clear();
        unitParticipateDatas.UnitParticipateDataDic.Clear();
        useWeaponDatas.UseWeaponDataDic.Clear();
        weaponDatas.WeaponDataDic.Clear();

        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if(ddo.name == "CSVManager")
            {
                Debug.Log("asdf");
            }
            if (ddo.CompareTag("DDO") && ddo.name == "CSVManager")// && SceneManager.GetActiveScene() != ddo.scene)
            {
                csvManager = ddo.GetComponent<CSVManager>();
            }
            if (ddo.CompareTag("DDO") && ddo.name == "GameManager")// && SceneManager.GetActiveScene() != ddo.scene)
            {
                gameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;
        csvManager.Initialize(dataBaseDic);
        GameManager.Initialized();

        //list타입 데이터 dictionary로 변환
        appearDatas.TranslateListToDic();
        dialogDatas.TranslateListToDic();
        havePartyDatas.TranslateListToDic();
        localUserDatas.TranslateListToDic();
        monsterDatas.TranslateListToDic();
        partyDatas.TranslateListToDic();
        progressDatas.TranslateListToDic();
        prototypeUnitDatas.TranslateListToDic();
        prototypeWeaponDatas.TranslateListToDic();
        stageDatas.TranslateListToDic();
        unitDatas.TranslateListToDic();
        unitParticipateDatas.TranslateListToDic();
        useWeaponDatas.TranslateListToDic();
        weaponDatas.TranslateListToDic();
        
    }
    public void Init()
    {

    }

    public bool SaveData()
    {
        appearDatas.TranslateDicToListAtSaveDatas();
        dialogDatas.TranslateDicToListAtSaveDatas();
        havePartyDatas.TranslateDicToListAtSaveDatas();
        localUserDatas.TranslateDicToListAtSaveDatas();
        monsterDatas.TranslateDicToListAtSaveDatas();
        partyDatas.TranslateDicToListAtSaveDatas();
        progressDatas.TranslateDicToListAtSaveDatas();
        prototypeUnitDatas.TranslateDicToListAtSaveDatas();
        prototypeWeaponDatas.TranslateDicToListAtSaveDatas();
        stageDatas.TranslateDicToListAtSaveDatas();
        unitDatas.TranslateDicToListAtSaveDatas();
        unitParticipateDatas.TranslateDicToListAtSaveDatas();
        useWeaponDatas.TranslateDicToListAtSaveDatas();
        weaponDatas.TranslateDicToListAtSaveDatas();
        return csvManager.SaveToCSVAllFile(dataBaseDic);
    }

    public void Update()
    {
        if(UnitDatas.UnitDatas.Count == 0)
        {
            Debug.Log("초기화초기화");
        }
    }
}
