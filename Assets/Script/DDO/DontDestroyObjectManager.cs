using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DontDestroyObjectManager : MonoBehaviour
{
    [SerializeField]
    private BossMonsterDataList bossMonsterDatas;
    [SerializeField]
    private DialogDataList dialogDatas;
    [SerializeField]
    private GuiderDataList guiderDatas;
    [SerializeField]
    private GuiderInUserDataList guiderInUserDatas;
    [SerializeField]
    private LocalUserDataList localUserDatas;
    [SerializeField]
    private MonsterDataList monsterDatas;
    [SerializeField]
    private PrisonerDataList prisonerDatas;
    [SerializeField]
    private PrisonerInUserDataList prisonerInUserDatas;
    [SerializeField]
    private StageDataList stageDatas;
    [SerializeField]
    private WeaponInUserDataList weaponInUserDatas;
    private CSVManager csvManager;
    private void Awake()
    {
        // DataList 타입의 모든 ScriptableObject 로드
        List<DataScriptableObjects> scriptableObjects = ScriptableObjectLoader.LoadAllScriptableObjects();

        // 모든 ScriptableObject 분리
        foreach (var SO in scriptableObjects)
        {
            if (SO is BossMonsterDataList)
            {
                bossMonsterDatas = (BossMonsterDataList)SO;
            }

            if (SO is DialogDataList)
            {
                dialogDatas = (DialogDataList)SO;
            }
            if (SO is GuiderDataList)
            {
                guiderDatas = (GuiderDataList)SO;
            }
            if (SO is GuiderInUserDataList)
            {
                guiderInUserDatas = (GuiderInUserDataList)SO;
            }

            if (SO is LocalUserDataList)
            {
                localUserDatas = (LocalUserDataList)SO;
            }
            if (SO is MonsterDataList)
            {
                monsterDatas = (MonsterDataList)SO;

            }
            if (SO is PrisonerDataList)
            {
                prisonerDatas = (PrisonerDataList)SO;
            }
            if (SO is PrisonerInUserDataList)
            {
                prisonerInUserDatas = (PrisonerInUserDataList)SO;
            }
            if (SO is StageDataList)
            {
                stageDatas = (StageDataList)SO;
            }
            if (SO is WeaponInUserDataList)
            {
                weaponInUserDatas = (WeaponInUserDataList)SO;
            }

        }
        scriptableObjects = null;

        bossMonsterDatas.BossMonsterDatas.Clear();
        dialogDatas.DialogDatas.Clear();
        guiderDatas.GuiderDatas.Clear();
        guiderInUserDatas.GuiderInUserDatas.Clear();
        localUserDatas.LocalUserDatas.Clear();
        monsterDatas.MonsterDatas.Clear();
        prisonerDatas.PrisonerDatas.Clear();
        prisonerInUserDatas.PrisonerInUserDatas.Clear();
        stageDatas.StageDatas.Clear();
        weaponInUserDatas.WeaponInUserDatas.Clear();

    }
    public void Start()
    {       
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach(var ddo in DDO)
        {
            if(ddo.name == "CSVManager")
            {
                csvManager = ddo.GetComponent<CSVManager>();
            }
        }
        DDO = null;
        csvManager.Initialize();

        //localUserDatas.LocalUserDatas[0].Gold++;
        csvManager.SaveToCSVAllFile();
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