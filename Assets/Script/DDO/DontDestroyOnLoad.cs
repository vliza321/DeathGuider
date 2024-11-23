using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (GameObject ddo in DDO)
        {
            if (ddo == this)
            {
                break;
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }
        DDO = null;
    }
}
