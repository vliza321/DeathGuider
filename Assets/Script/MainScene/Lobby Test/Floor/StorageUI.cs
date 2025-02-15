using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StorageUI : MonoBehaviour
{
    public TextMeshProUGUI daysText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI deathEssenceText;
    public TextMeshProUGUI darkEssenceText;

    private DontDestroyObjectManager DDOManager;
    private GameManager GameManager;
    void Start()
    {
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }

            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                GameManager = ddo.GetComponent<GameManager>();
            }
        }
        DDO = null;

        UpdateDaysUI();
        UpdateGold();
        UpdatedeathEssence();
        UpdatedarkEssence();
    }

    public void UpdateDaysUI()
    {
        if (daysText != null || DDOManager == null || GameManager == null)
        {
            daysText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Day}¿œ";
        }
    }

    public void UpdateGold()
    {
        if(goldText == null || DDOManager == null || GameManager == null)
        {
            return;
        }

        goldText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Gold:N0} G";
    }

    public void UpdatedeathEssence()
    {
        if (deathEssenceText == null || DDOManager == null || GameManager == null)
        {
            return;
        }

        deathEssenceText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].DeathEssence:N0} D";
    }

    public void UpdatedarkEssence()
    {
        if (darkEssenceText == null || DDOManager == null || GameManager == null)
        {
            return;
        }

        darkEssenceText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].DarkEssence:N0} K";
    }
}
