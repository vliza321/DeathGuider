using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class ResultManager : MonoBehaviour
{

    private int aliveUnitCount;
    public int AliveUnitCount
    {
        get { return aliveUnitCount; }

    }

    private static Dictionary<int, float> posToHp;
    private static ResultManager instance;

    private float timer;
    private bool isVictory;
    private bool playerEscape;
    private FadeInOut fadeInOutUI;
    private GameManager gameManager;
    private DontDestroyObjectManager ddoManager;

    [SerializeField]
    private List<UnitData> units;
    private List<WeaponData> newWeapons;

    [SerializeField] private float gold;
    [SerializeField] private float darkEssense;
    [SerializeField] private int deathEssense;
    [SerializeField] private float exp;

    private Dictionary<int, PlayerHp> posToUnitData;

    private float explorationProgress;
    private float currentExplorationProgress;

    public float Timer
    {
        get { return timer; }
    }

    public GameManager GameManager
    {
        get { return gameManager; }
    }
    public DontDestroyObjectManager DDOManager
    {
        get { return ddoManager; }
    }

    public float Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    public float DarkEssense
    {
        get { return darkEssense; }
        set { darkEssense = value; }
    }

    public int DeathEssense
    {
        get { return deathEssense; }
        set { deathEssense = value; }
    }
    public float Exp
    {
        get { return exp; }
        set { exp = value; }
    }

    public List<UnitData> Units
    {
        get { return units; }
        set { units = value; }
    }

    public Dictionary<int, PlayerHp> PosToUnitData
    {
        get { return posToUnitData; }
        set { posToUnitData = value; }
    }

    public bool IsVictory
    {
        get { return isVictory; }
        set { isVictory = value; }
    }

    public FadeInOut FadeInOutUI
    {
        get { return fadeInOutUI; }
        set { fadeInOutUI = value; }
    }

    void Awake()
    {
        if (posToUnitData != null)
        {
            posToUnitData.Clear();
        }
        if (units != null) units.Clear();
        if (newWeapons != null) newWeapons.Clear();
        if (posToHp != null) posToHp.Clear();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }


        isVictory = false;


        timer = 0;
        playerEscape = false;
        if (SceneManager.GetActiveScene() != this.gameObject.scene) InitializeManagers();
    }

    void Update()
    {
        if (!playerEscape) timer += Time.deltaTime;
    }

    private void InitializeManagers()
    {

        posToUnitData = new Dictionary<int, PlayerHp>();
        units = new List<UnitData>();
        newWeapons = new List<WeaponData>();
        posToHp = new Dictionary<int, float>();
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                ddoManager = ddo.GetComponent<DontDestroyObjectManager>();
                ddoManager.setResultManager(this);
            }
            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                gameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;

        GameObject[] UI = GameObject.FindGameObjectsWithTag("UI");
        foreach (var u in UI)
        {
            if (u.name == "FadeInOutEffect")
                fadeInOutUI = u.GetComponent<FadeInOut>();
        }

    }

    public void PlayerEscape()
    {
        playerEscape = true;
        foreach (var u in posToUnitData)
        {
            int a = u.Key;
            posToHp.Add(u.Key, posToUnitData[a].DDOResist());
        }

        fadeInOutUI.StartFadeOut();
        isVictory = true;
        //필요 작업 : 유닛 정보 복사해서 가지고 오기
        //CopyUnitData();
        // SaveBattleResult(true);
        //Invoke("LoadMainScene", 3f);
    }

    private void CopyUnitData()
    {
        units = new List<UnitData>();
        int userID = gameManager.SelectUserID;

        foreach (var unit in ddoManager.UnitParticipateDatas.UnitParticipateDataDic.Values)
        {
            if (unit.UserID == userID)
            {
                var key = (unit.UserID, unit.PrototypeUnitID, unit.InstanceID);

                if (ddoManager.UnitDatas.UnitDataDic.ContainsKey(key))
                {
                    UnitData originalUnit = ddoManager.UnitDatas.UnitDataDic[key];

                    UnitData copiedUnit = new UnitData
                    {
                        UserID = originalUnit.UserID,
                        PrototypeUnitID = originalUnit.PrototypeUnitID,
                        InstanceID = originalUnit.InstanceID,
                        Name = originalUnit.Name,
                        Level = originalUnit.Level,
                        EXP = originalUnit.EXP,
                        MaxHealthPoint = originalUnit.MaxHealthPoint,
                        HealthPoint = originalUnit.HealthPoint,
                        Strength = originalUnit.Strength,
                        Defense = originalUnit.Defense,
                        Handicraft = originalUnit.Handicraft,
                        DeathErosion = originalUnit.DeathErosion,
                        Enforce = originalUnit.Enforce,
                        HealthEnforce = originalUnit.HealthEnforce,
                        StrengthEnforce = originalUnit.StrengthEnforce,
                        DefenseEnforce = originalUnit.DefenseEnforce,
                        HandicraftEnforce = originalUnit.HandicraftEnforce,
                        Crime = originalUnit.Crime,
                        ActivityStatus = originalUnit.ActivityStatus,
                        HeadID = originalUnit.HeadID,
                        BodyID = originalUnit.BodyID
                    };

                    units.Add(copiedUnit);
                }
            }
        }
    }

    public void PlayerDefeated()
    {
        playerEscape = true;
        foreach (var u in posToUnitData)
        {
            int a = u.Key;
            posToHp.Add(u.Key, posToUnitData[a].DDOResist());
        }
        fadeInOutUI.StartFadeOut();
        isVictory = false;
        SceneManager.LoadScene("Result");
        //SaveBattleResult(false);
        //Invoke("LoadMainScene", 3f);
    }

    public void SaveBattleResult(bool isVictory)
    {
        int userID = gameManager.SelectUserID;
        int stageID = gameManager.SelectStageID;

        timer = (timer >= (300 + stageID * 10) ? timer = (300 + stageID * 10) : timer);

        ddoManager.LocalUserDatas.LocalUserDataDic[userID].Day++;

        UpdateBattleProgress(userID, stageID, isVictory);
        UpdateResources(userID, isVictory);
        //health 오류
        UpdateUnitHealth(userID);
        UpdateWeaponDurability(userID, stageID);
        UpdateUnitErosion(userID, stageID);
        AddNewWeapons(userID);
        ddoManager.SaveData();
    }

    private void UpdateBattleProgress(int userID, int stageID, bool isVictory)
    {
        // 진척도 누적 예시 코드
        if (ddoManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress != 100)
        {
            ddoManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress += 10 + (int)(timer / (15 + stageID * 3));// + 시간 비례식 필요 

            if(ddoManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress >= 100)
            {
                ddoManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress = 100;
                if (stageID == ddoManager.StageDatas.StageDatas.Count - 1) return;
                ddoManager.StageDatas.StageDataDic[stageID++].IsOpen = 1;
            }
        }
    }

    private void UpdateResources(int userID, bool isVictory)
    {
        float multiplier = isVictory ? 1f : 0.5f;
        multiplier += multiplier * (ddoManager.LocalUserDatas.LocalUserDataDic[userID].BattleEfficiency) / 10;
        ddoManager.LocalUserDatas.LocalUserDataDic[userID].Gold += (int)(gold * multiplier);
        ddoManager.LocalUserDatas.LocalUserDataDic[userID].DarkEssence += (int)(darkEssense * multiplier);
        ddoManager.LocalUserDatas.LocalUserDataDic[userID].DeathEssence += deathEssense;
    }



    private void UpdateUnitHealth(int userID)
    {
        aliveUnitCount = 0;
        foreach (var u in ddoManager.UnitParticipateDatas.UnitParticipateDataDic.Values)
        {
            var key = (userID, u.PrototypeUnitID, u.InstanceID);

            if (ddoManager.UnitDatas.UnitDataDic[key].HealthPoint == 0)
            {
                ddoManager.UnitDatas.UnitDataDic.Remove(key);
                ddoManager.UnitDatas.UnitDatas.RemoveAll(unit => unit.UserID == userID && unit.PrototypeUnitID == u.PrototypeUnitID && unit.InstanceID == u.InstanceID);
                continue;
            }

            ddoManager.UnitDatas.UnitDataDic[key].HealthPoint = 
                (posToUnitData[u.Position].HeartPoint % 1 > 0 ? (int)(posToUnitData[u.Position].HeartPoint + 1) : (int)(posToUnitData[u.Position].HeartPoint));
            aliveUnitCount++;
        }

        foreach (var u in ddoManager.UnitParticipateDatas.UnitParticipateDataDic.Values)
        {
            var key = (userID, u.PrototypeUnitID, u.InstanceID);
            float gainExp = exp / (aliveUnitCount * MathF.Log(
                ddoManager.UnitDatas.UnitDataDic[key].Level + 1
                , 2));
            ddoManager.UnitDatas.UnitDataDic[key].EXP = (int)(gainExp);

            while (ddoManager.UnitDatas.UnitDataDic[key].EXP > 100)
            {
                ddoManager.UnitDatas.UnitDataDic[key].Level++;
                ddoManager.UnitDatas.UnitDataDic[key].EXP -= 100;

                ddoManager.UnitDatas.UnitDataDic[key].MaxHealthPoint += 6;
                ddoManager.UnitDatas.UnitDataDic[key].HealthPoint += 6;
                ddoManager.UnitDatas.UnitDataDic[key].Strength += 2;
                ddoManager.UnitDatas.UnitDataDic[key].Defense += 2;
                ddoManager.UnitDatas.UnitDataDic[key].Handicraft += 2;
            }
        }
    }

      

    private void UpdateWeaponDurability(int userID, int stageID)
    {
        foreach (var useWeapon in ddoManager.UseWeaponDatas.UseWeaponDataDic.Values)
        {
            var key = (userID, useWeapon.PrototypeWeaponID, useWeapon.InstanceID);
            ddoManager.WeaponDatas.WeaponDataDic[key].Durability -= 5 + (int)(timer / (60 - stageID * 1));// + 시간 비례식 필요 

            if (ddoManager.WeaponDatas.WeaponDataDic[key].Durability < 0)
            {
                ddoManager.WeaponDatas.WeaponDataDic.Remove(key);
                ddoManager.WeaponDatas.WeaponDatas.RemoveAll(w => w.UserID == userID && w.PrototypeWeaponID == useWeapon.PrototypeWeaponID && w.InstanceID == useWeapon.InstanceID);
            }
        }


    }


    private void UpdateUnitErosion(int userID, int stageID)
    {
        foreach (var u in ddoManager.UnitParticipateDatas.UnitParticipateDataDic.Values)
        {
            var key = (userID, u.PrototypeUnitID, u.InstanceID);
            ddoManager.UnitDatas.UnitDataDic[key].DeathErosion += 2 + (int)(timer / (60 - stageID * 3));// + 시간 비례식 필요 
        }
    }

    private void AddNewWeapons(int userID)
    {
        foreach (var newWeapon in newWeapons)
        {

            ddoManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter++;

            //새로 획득한 무기의 InstanceID를 변경
            newWeapon.InstanceID = ddoManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter;

            DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter++;
            newWeapon.InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter;

            var key = (userID, newWeapon.PrototypeWeaponID, newWeapon.InstanceID);

            //새로운 WeaponData 생성 -> 없을시 함수 종료시 nullreference
            WeaponData addWeapon = new WeaponData();
            addWeapon = newWeapon;

            //데이터 베이스에 중복된 무기가 있는지 검사
            if (!ddoManager.WeaponDatas.WeaponDataDic.ContainsKey(key))
            {
                //데이터 베이스에 list 및 dictionary에 추가
                ddoManager.WeaponDatas.WeaponDataDic.Add(key, newWeapon);
                ddoManager.WeaponDatas.WeaponDatas.Add(addWeapon);
            }
        }
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("LobbyTest");
    }

    public void AddNewStat(int pos, PlayerHp unitHp)
    {
        units.Add(unitHp.Stat);
        posToUnitData.Add(pos, unitHp);
    }
}