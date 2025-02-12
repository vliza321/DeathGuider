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
        var userData = DDOManager.LocalUserDatas.LocalUserDataDic[userID];
        var battleData = DDOManager.ProgressDatas.ProgressDataDic[userID];

        battleResultText.text = battleData.IsVictory ? "Å½»ç ¼º°ø" : "Å½»ç ½ÇÆÐ";
        goldText.text = "°ñµå: " + userData.Gold;
        darkEssenseText.text = "¾îµÒ Á¤¼ö: " + userData.DarkEssense;
        deathEssenseText.text = "Á×À½ Á¤¼ö: " + userData.DeathEssense;
        expText.text = "°æÇèÄ¡: " + userData.Exp;
        battleTimeText.text = "ÀüÅõ ½Ã°£: " + battleData.BattleTime.ToString("F1") + "ÃÊ";
        explorationText.text = "Å½»ç ÁøÃ´µµ: " + battleData.ExplorationProgress;
    }
}
