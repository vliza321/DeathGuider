using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        bool isRegister = false;
        foreach (GameObject ddo in DDO)
        {
            if (ddo == this)
            {
                isRegister = true;
                break;
            }
        }
        DDO = null;
        if(!isRegister)
        {
            DontDestroyOnLoad(this);
        }
    }
}
