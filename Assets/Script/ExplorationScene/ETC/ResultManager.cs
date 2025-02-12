using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    private float timer;
    private bool playerEscape;
    private FadeInOut fadeInOutUI;
    private GameManager gameManager;
    private DontDestroyObjectManager DDOManager;

    [SerializeField] private float gold;
    [SerializeField] private float darkEssense;
    [SerializeField] private int deathEssense;
    [SerializeField] private float exp;

    private float explorationProgress;
    private float currentExplorationProgress;

    void Start()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "GameManager")
            {
                gameManager = ddo.GetComponent<GameManager>();
            }
            if (ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
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

        timer = 0;
        playerEscape = false;
    }

    void Update()
    {
        if (!playerEscape) timer += Time.deltaTime;
    }

    public void PlayerEscape()
    {
        playerEscape = true;
        fadeInOutUI.StartFadeOut();
        SaveBattleResult(true);
        Invoke("LoadMainScene", 3f);
    }

    public void PlayerDefeated()
    {
        SaveBattleResult(false);
        Invoke("LoadMainScene", 3f);
    }

    private void SaveBattleResult(bool isVictory)
    {
        int userID = gameManager.SelectUserID;

        // **?? 1. 전투 결과 데이터 저장**
        DDOManager.ProgressDatas.ProgressDataDic[userID].IsVictory = isVictory;
        DDOManager.ProgressDatas.ProgressDataDic[userID].BattleTime = timer;
        DDOManager.ProgressDatas.ProgressDataDic[userID].ExplorationProgress = explorationProgress;
        DDOManager.ProgressDatas.ProgressDataDic[userID].CurrentExplorationProgress = currentExplorationProgress;

        // **?? 2. 획득한 재화 업데이트**
        DDOManager.LocalUserDatas.LocalUserDataDic[userID].Gold += (int)gold;
        DDOManager.LocalUserDatas.LocalUserDataDic[userID].DarkEssense += (int)darkEssense;
        DDOManager.LocalUserDatas.LocalUserDataDic[userID].DeathEssense += deathEssense;
        DDOManager.LocalUserDatas.LocalUserDataDic[userID].Exp += (int)exp;

        // **?? 3. 유닛의 체력 정보 업데이트 (HealthData 사용)**
        foreach (var unit in DDOManager.UnitParticipateDatas.UnitParticipateDataDic.Values)
        {
            if (unit.UserID == userID)
            {
                int unitInstanceID = unit.InstanceID;
                if (DDOManager.HealthSystem.HealthDataDic.ContainsKey(unitInstanceID))
                {
                    DDOManager.HealthSystem.HealthDataDic[unitInstanceID].InstanceID = unitInstanceID;
                }
                else
                {
                    DDOManager.HealthSystem.HealthDataDic.Add(unitInstanceID, new HealthData { InstanceID = unitInstanceID });
                }
            }
        }

        // **?? 4. 전투 데이터를 저장**
        DDOManager.SaveData();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}