using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultUI : MonoBehaviour
{
    private DontDestroyObjectManager DDOManager;
    private GameManager gameManager;

    public Text battleResultText;
    public Text goldText;
    public Text darkEssenseText;
    public Text deathEssenseText;
    public Text expText;
    public Text battleTimeText;
    public Text explorationText;

    void Start()
    {
        // resultManager를 찾아야서 결과 창 출력을 하는 방향성으로 변경 바람
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            if (ddo.name == "GameManager")
            {
                gameManager = ddo.GetComponent<GameManager>();
            }
        }

        LoadBattleResult();
    }

    void LoadBattleResult()
    {
        int userID = gameManager.SelectUserID;
        int stageID = gameManager.SelectStageID;
        var userData = DDOManager.LocalUserDatas.LocalUserDataDic[userID];
        // 오류 수정 : [userID,StageID] -> [(userID,stageID)]
        ProgressData battleData = DDOManager.ProgressDatas.ProgressDataDic[(userID,stageID)];


        //battleResultText.text = battleData.IsVictory ? "탐사 성공" : "탐사 실패";
        //획득한 재화만 출력하는걸로 수정 바람 
        /*goldText.text = "골드: " + userData.Gold;
        darkEssenseText.text = "어둠 정수: " + userData.DarkEssense;
        deathEssenseText.text = "죽음 정수: " + userData.DeathEssense;
        expText.text = "경험치: " + userData.Exp;*/

        //resultManager를 통해 해당 내용 출력
        /*battleTimeText.text = "전투 시간: " + battleData.BattleTime.ToString("F1") + "초";
        explorationText.text = "탐사 진척도: " + battleData.ExplorationProgress;*/
    }
}
