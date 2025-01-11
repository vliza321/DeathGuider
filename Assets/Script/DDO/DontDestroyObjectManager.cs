using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DontDestroyObjectManager : MonoBehaviour
{
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
    private void Awake()
    {
        // DataList 타입의 모든 ScriptableObject 로드
        List<DataScriptableObjects> scriptableObjects = ScriptableObjectLoader.LoadAllScriptableObjects();

        // 모든 ScriptableObject 분리
        foreach (var SO in scriptableObjects)
        {
            if (SO is AppearDataList)
            {
                appearDatas = (AppearDataList)SO;
            }

            if (SO is DialogDataList)
            {
                dialogDatas = (DialogDataList)SO;
            }

            if (SO is HavePartyDataList)
            {
                havePartyDatas = (HavePartyDataList)SO;
            }

            if (SO is LocalUserDataList)
            {
                localUserDatas = (LocalUserDataList)SO;
            }

            if (SO is MonsterDataList)
            {
                monsterDatas = (MonsterDataList)SO;

            }

            if(SO is PartyDataList)
            {
                partyDatas = (PartyDataList)SO;
            }

            if(SO is ProgressDataList)
            {
                progressDatas = (ProgressDataList)SO;
            }

            if (SO is PrototypeUnitDataList)
            {
                prototypeUnitDatas = (PrototypeUnitDataList)SO;
            }

            if (SO is PrototypeWeaponDataList)
            {
                prototypeWeaponDatas = (PrototypeWeaponDataList)SO;
            }

            if (SO is StageDataList)
            {
                stageDatas = (StageDataList)SO;
            }

            if (SO is UnitDataList)
            {
                unitDatas = (UnitDataList)SO;
            }

            if(SO is UnitParticipateDataList)
            {
                unitParticipateDatas = (UnitParticipateDataList)SO;
            }

            if (SO is UseWeaponDataList)
            {
                useWeaponDatas = (UseWeaponDataList)SO;
            }

            if (SO is WeaponDataList)
            {
                weaponDatas = (WeaponDataList)SO;
            }

        }
        scriptableObjects = null;
        
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

        //csvmanager불러오기
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "CSVManager")
            {
                csvManager = ddo.GetComponent<CSVManager>();
            }
        }
        DDO = null;
        csvManager.Initialize();

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

        //테스트용
        localUserDatas.LocalUserDatas[0].Gold += 100;
        if (!SaveData())
        {
            Debug.LogError("Fail Save ScriptalbeObject To CSVFile");
        }

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
        return csvManager.SaveToCSVAllFile();
    }
}

public static class ScriptableObjectLoader
{
    public static List<DataScriptableObjects> LoadAllScriptableObjects()
    {
        // Resources 폴더에서 특정 타입의 모든 객체 로드
        Object[] objects = Resources.LoadAll("", typeof(DataScriptableObjects));
        List<DataScriptableObjects> scriptableObjects = new List<DataScriptableObjects>();
        foreach(var obj in objects)
        {
            if(obj is DataScriptableObjects so)
            {
                scriptableObjects.Add(so);
            }
        }
        return scriptableObjects;
    }
}