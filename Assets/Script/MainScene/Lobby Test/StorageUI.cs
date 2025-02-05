using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    public TextMeshProUGUI daysText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI deathEssenceText;
    public TextMeshProUGUI darkEssenceText;

    private DontDestroyObjectManager DDOManager;
    void Start()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            DDO = null;
        }

        UpdateDaysUI();
        UpdateGold();
        UpdatedeathEssence();
        UpdatedarkEssence();
    }

    public void UpdateDaysUI()
    {
        if (daysText != null)
        {
            daysText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[0].Floor}¿œ";
        }
    }

    public void UpdateGold()
    {
        if(goldText == null || DDOManager == null)
        {
            return;
        }

        goldText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold:N0} G";
    }

    public void UpdatedeathEssence()
    {
        if (deathEssenceText == null || DDOManager == null)
        {
            return;
        }

        deathEssenceText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[0].DeathEssence:N0} D";
    }

    public void UpdatedarkEssence()
    {
        if (darkEssenceText == null || DDOManager == null)
        {
            return;
        }

        darkEssenceText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[0].DarkEssence:N0} K";
    }
}
