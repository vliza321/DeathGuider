using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
{
    protected MasterManager masterManager;
    public ManagerBase(MasterManager MasterManaer)
    {
        masterManager = MasterManaer;
    }

    public abstract void Awake();
    public abstract void Start();
    public abstract void Update();
    public abstract void OnNotify();
}
