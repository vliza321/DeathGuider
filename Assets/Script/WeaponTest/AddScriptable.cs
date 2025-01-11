using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScriptable : MonoBehaviour
{
    [SerializeField]
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
        }
        DDO = null;
    }

    public void OnButtonClick()
    {
        DDOManager.LocalUserDatas.LocalUserDataDic[0].UnitInstanceCounter=0;
        Debug.Log(DDOManager.LocalUserDatas.LocalUserDataDic[0].UnitInstanceCounter);
        DDOManager.SaveData();
    }
}
