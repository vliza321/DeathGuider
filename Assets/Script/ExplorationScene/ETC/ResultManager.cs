using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
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

    

    private float explorationProgress;
    private float currentExplorationProgress;

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

    void Awake()
    {
        units = new List<UnitData>(5);
        isVictory = false;
<<<<<<< HEAD
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "GameManager")
            {
                gameManager = ddo.GetComponent<GameManager>();
            }
            if (ddo.name == "DDOManager")
            {
                ddoManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
        }

        GameObject[] UI = GameObject.FindGameObjectsWithTag("UI");
        foreach (var u in UI)
        {
            if (u.name == "FadeInOutEffect")
            {
                fadeInOutUI = u.GetComponent<FadeInOut>();
            }
        }

=======
>>>>>>> 52df2dc3c80151fe82418f1e602f263b5cc1ea1e
        timer = 0;
        playerEscape = false;

        InitializeManagers();
    }

    void Update()
    {
        if (!playerEscape) timer += Time.deltaTime;
    }

    private void InitializeManagers()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "GameManager")
                gameManager = ddo.GetComponent<GameManager>();
            if (ddo.name == "DDOManager")
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
        }

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
        fadeInOutUI.StartFadeOut();
        //필요 작업 : 유닛 정보 복사해서 가지고 오기
        CopyUnitData();
        SaveBattleResult(true);
        Invoke("LoadMainScene", 3f);
    }

    private void CopyUnitData()
    {
        units = new List<UnitData>();
        int userID = gameManager.SelectUserID;

        foreach (var unit in DDOManager.UnitParticipateDatas.UnitParticipateDataDic.Values)
        {
            if (unit.UserID == userID)
            {
                var key = (unit.UserID, unit.PrototypeUnitID, unit.InstanceID);

                if (DDOManager.UnitDatas.UnitDataDic.ContainsKey(key))
                {
                    UnitData originalUnit = DDOManager.UnitDatas.UnitDataDic[key];

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
        SaveBattleResult(false);
        Invoke("LoadMainScene", 3f);
    }

    private void SaveBattleResult(bool isVictory)
    {
        int userID = gameManager.SelectUserID;
        int stageID = gameManager.SelectStageID;

<<<<<<< HEAD
        // **?? 1. 전투 결과 데이터 저장 **
        // + 결과 데이터 저장 중 진척도 관련 처리
        // 기존 진척도 + 기본 10 + 시간 비례 추가(최소 5 / 최대 20)
        //DDOManager.ProgressDatas.ProgressDataDic[(userID,stageID)].IsVictory = isVictory;
        //DDOManager.ProgressDatas.ProgressDataDic[userID].BattleTime = timer;
        //DDOManager.ProgressDatas.ProgressDataDic[userID].ExplorationProgress = explorationProgress;
        // 진척도 누적 예시 코드
        if (ddoManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress != 100)
            ddoManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress += 10;// + 시간 비례식 필요 

        // **?? 2. 획득한 재화 업데이트**
        //+추가사항 : 전투 실패시 gold, darkEssense 획득량 절반
        ddoManager.LocalUserDatas.LocalUserDataDic[userID].Gold += (int)gold;
        ddoManager.LocalUserDatas.LocalUserDataDic[userID].DarkEssence += (int)darkEssense;
        ddoManager.LocalUserDatas.LocalUserDataDic[userID].DeathEssence += deathEssense;
        
        // **?? 3. 유닛의 체력 정보 업데이트 (HealthData 사용)**
        // 이 스크립트의 List<UnitData> Unit 에 있는 데이터를 가져와서 작업해야함
        // 예시 코드
        var key = (1,1,1); // dictionary 타입의 key를 정의 및 임시 초기화
        // 전투에 참여한 units의 순회
=======
        UpdateBattleProgress(userID, stageID, isVictory);
        UpdateResources(userID, isVictory);
        UpdateUnitHealth(userID);
        UpdateWeaponDurability(userID);
        AddNewWeapons(userID);
        DDOManager.SaveData();
    }

    // **1. 전투 결과 데이터 저장 **
    // + 결과 데이터 저장 중 진척도 관련 처리
    // 기존 진척도 + 기본 10 + 시간 비례 추가(최소 5 / 최대 20)

    // 진척도 누적 예시 코드
    //if (DDOManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress != 100)
    //    DDOManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress += 10;// + 시간 비례식 필요 
    private void UpdateBattleProgress(int userID, int stageID, bool isVictory)
    {
        if (DDOManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress != 100)
            DDOManager.ProgressDatas.ProgressDataDic[(userID, stageID)].Progress += 10; // + 시간 비례식 필요
    }

    // **?? 2. 획득한 재화 업데이트**
    //+추가사항 : 전투 실패시 gold, darkEssense 획득량 절반
    //DDOManager.LocalUserDatas.LocalUserDataDic[userID].Gold += (int)gold;
    //DDOManager.LocalUserDatas.LocalUserDataDic[userID].DarkEssence += (int)darkEssense;
    //DDOManager.LocalUserDatas.LocalUserDataDic[userID].DeathEssence += deathEssense;
    private void UpdateResources(int userID, bool isVictory)
    {
        float multiplier = isVictory ? 1f : 0.5f;
        DDOManager.LocalUserDatas.LocalUserDataDic[userID].Gold += (int)(gold * multiplier);
        DDOManager.LocalUserDatas.LocalUserDataDic[userID].DarkEssence += (int)(darkEssense * multiplier);
        DDOManager.LocalUserDatas.LocalUserDataDic[userID].DeathEssence += deathEssense;
    }

    // **?? 3. 유닛의 체력 정보 업데이트 (HealthData 사용)**
    // 이 스크립트의 List<UnitData> Unit 에 있는 데이터를 가져와서 작업해야함
    // 예시 코드
    //var key = (1,1,1); // dictionary 타입의 key를 정의 및 임시 초기화
    // 전투에 참여한 units의 순회
    //    foreach (var unit in units)
    //    {
    //        // unit의 key 값 초기화
    //        key = (unit.UserID, unit.PrototypeUnitID, unit.InstanceID);
    //        // unit의 체력에 따른 처리 + 유닛이 수감자 인지 id 감사
    //        if(unit.HealthPoint <= 0 && unit.PrototypeUnitID == 100)
    //        {
    //           // 삭제시 데이터 베이스에서 list와 dictionary타입 둘다 삭제해야함
    //            // key 값에 따른 dictionary 타입의 데이터 베이스에서 삭제
    //            DDOManager.UnitDatas.UnitDataDic.Remove(key);
    //            // key 값에 따른 list 타입의 데이터 베이스에서 삭제
    //            for(int i = 0;i<DDOManager.UnitDatas.UnitDatas.Count;i++)
    //            {
    //                if(DDOManager.UnitDatas.UnitDatas[i].UserID == userID)
    //               {
    //                    if(DDOManager.UnitDatas.UnitDatas[i].PrototypeUnitID == unit.PrototypeUnitID && DDOManager.UnitDatas.UnitDatas[i].InstanceID == unit.InstanceID)
    //                    {
    //                        DDOManager.UnitDatas.UnitDatas.RemoveAt(i);
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //else
    //  {
    //        DDOManager.UnitDatas.UnitDataDic[key].HealthPoint = unit.HealthPoint;
    //   }
    //}
    private void UpdateUnitHealth(int userID)
    {
>>>>>>> 52df2dc3c80151fe82418f1e602f263b5cc1ea1e
        foreach (var unit in units)
        {
            var key = (unit.UserID, unit.PrototypeUnitID, unit.InstanceID);
            if (unit.HealthPoint <= 0 && unit.PrototypeUnitID == 100)
            {
<<<<<<< HEAD
                // 삭제시 데이터 베이스에서 list와 dictionary타입 둘다 삭제해야함
                // key 값에 따른 dictionary 타입의 데이터 베이스에서 삭제
                ddoManager.UnitDatas.UnitDataDic.Remove(key);
                // key 값에 따른 list 타입의 데이터 베이스에서 삭제
                for(int i = 0;i<ddoManager.UnitDatas.UnitDatas.Count;i++)
                {
                    if(ddoManager.UnitDatas.UnitDatas[i].UserID == userID)
                    {
                        if(ddoManager.UnitDatas.UnitDatas[i].PrototypeUnitID == unit.PrototypeUnitID && ddoManager.UnitDatas.UnitDatas[i].InstanceID == unit.InstanceID)
                        {
                            ddoManager.UnitDatas.UnitDatas.RemoveAt(i);
                            break;
                        }
                    }
                }
=======
                DDOManager.UnitDatas.UnitDataDic.Remove(key);
                DDOManager.UnitDatas.UnitDatas.RemoveAll(u => u.UserID == userID && u.PrototypeUnitID == unit.PrototypeUnitID && u.InstanceID == unit.InstanceID);
>>>>>>> 52df2dc3c80151fe82418f1e602f263b5cc1ea1e
            }
            else
            {
                ddoManager.UnitDatas.UnitDataDic[key].HealthPoint = unit.HealthPoint;
            }
        }
    }    


        // 4. 무기의 내구도 정보 업데이트 (UseWeaponData 및 WeaponData 사용)
<<<<<<< HEAD
        foreach(var useWeapon in ddoManager.UseWeaponDatas.UseWeaponDataDic.Values)
=======
       /* foreach(var useWeapon in DDOManager.UseWeaponDatas.UseWeaponDataDic.Values)
>>>>>>> 52df2dc3c80151fe82418f1e602f263b5cc1ea1e
        {
            // 사용한 무기의 key값 가져오기
            key = (userID, useWeapon.PrototypeWeaponID, useWeapon.InstanceID);
            // 사용한 무기의 내구도 감소
            // 기존 내구도 - (기본 5 + 시간 비례 추가(최소 0 / 최대 20))
<<<<<<< HEAD
            ddoManager.WeaponDatas.WeaponDataDic[key].Durability -= 5; // + 시간 비례식 필요 
        }
=======
            DDOManager.WeaponDatas.WeaponDataDic[key].Durability -= 5; // + 시간 비례식 필요 
        } */
>>>>>>> 52df2dc3c80151fe82418f1e602f263b5cc1ea1e

    private void UpdateWeaponDurability(int userID)
    {
        foreach (var useWeapon in DDOManager.UseWeaponDatas.UseWeaponDataDic.Values)
        {
            var key = (userID, useWeapon.PrototypeWeaponID, useWeapon.InstanceID);
            DDOManager.WeaponDatas.WeaponDataDic[key].Durability -= 5; // + 시간 비례식 필요
        }
    }

    /* // 5. 획득한 무기 정보 추가 -> 전투에서 모두 죽어 패배했어도 획득
     foreach(var newWeapon in newWeapons)
     {
         //PrototypeWeapon의 instanceCounter증가
         DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter++;

         //새로 획득한 무기의 InstanceID를 변경
         newWeapon.InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter;


         key = (userID, newWeapon.PrototypeWeaponID, newWeapon.InstanceID);


         //새로운 WeaponData 생성 -> 없을시 함수 종료시 nullreference
         WeaponData addWeapon = new WeaponData();
         addWeapon = newWeapon;

         //데이터 베이스에 중복된 무기가 있는지 검사
         if (!DDOManager.WeaponDatas.WeaponDataDic.ContainsKey(key))
         {
             //데이터 베이스에 list 및 dictionary에 추가
             DDOManager.WeaponDatas.WeaponDataDic.Add(key, newWeapon);
             DDOManager.WeaponDatas.WeaponDatas.Add(addWeapon);
         }
     }


     // **?? 5. 전투 데이터를 저장**
     DDOManager.SaveData();
 }*/

    private void AddNewWeapons(int userID)
    {
        foreach (var newWeapon in newWeapons)
        {
<<<<<<< HEAD
            //PrototypeWeapon의 instanceCounter증가
            ddoManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter++;

            //새로 획득한 무기의 InstanceID를 변경
            newWeapon.InstanceID = ddoManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter;
            
=======
            DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter++;
            newWeapon.InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeapon.PrototypeWeaponID].InstanceCounter;
>>>>>>> 52df2dc3c80151fe82418f1e602f263b5cc1ea1e

            var key = (userID, newWeapon.PrototypeWeaponID, newWeapon.InstanceID);

<<<<<<< HEAD

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


        // **?? 5. 전투 데이터를 저장**
        ddoManager.SaveData();
=======
            if (!DDOManager.WeaponDatas.WeaponDataDic.ContainsKey(key))
            {
                DDOManager.WeaponDatas.WeaponDataDic.Add(key, newWeapon);
                DDOManager.WeaponDatas.WeaponDatas.Add(newWeapon);
            }
        }
>>>>>>> 52df2dc3c80151fe82418f1e602f263b5cc1ea1e
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}