using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    public TextMeshProUGUI daysText;
    public Slider GoldBar;
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

        if (GoldBar != null)
        {
            GoldBar.minValue = 0;
            GoldBar.maxValue = long.MaxValue;
            UpdateGoldBar();
        }
    }

    public void UpdateDaysUI()
    {
        if (daysText != null)
        {
            daysText.text = $"{DDOManager.LocalUserDatas.LocalUserDataDic[0].Floor}¿œ";
        }
    }

    public void UpdateGoldBar()
    {
        if (GoldBar != null && DDOManager != null)
        {
            long currentGold = DDOManager.LocalUserDatas.LocalUserDataDic[0].Gold;
            GoldBar.value = Mathf.Clamp(currentGold, 0, long.MaxValue);
        }
    }
}
