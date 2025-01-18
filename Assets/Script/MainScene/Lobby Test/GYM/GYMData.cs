using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GYMData
{
    public int InstanceID;
    public bool check;

    public GYMData(int instanceID)
    {
        InstanceID = instanceID;
        check = false;
    }
}