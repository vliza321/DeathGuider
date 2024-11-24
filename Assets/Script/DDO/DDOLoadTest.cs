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
}
