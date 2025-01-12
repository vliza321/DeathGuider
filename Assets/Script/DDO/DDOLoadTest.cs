using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLoadTest : MonoBehaviour
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

    void temt()
    {
        int a = DDOManager.PrototypeUnitDatas.PrototypeUnitDataDic[100].Strength;
        var t = DDOManager.UnitDatas.UnitDatas;
        DDOManager.UnitDatas.UnitDataDic.Add((0, 100, DDOManager.LocalUserDatas.LocalUserDataDic[0].UnitInstanceCounter),new UnitData());
        DDOManager.UnitDatas.UnitDatas.Add(new UnitData());
    }
}
