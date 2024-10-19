using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetalliteTypeWeaponManager : Weapon
{
    private int setalliteCount;
    private GameObject baseSetallite;
    [SerializeField]
    private SetalliteTypeWeapon[] setallite;
    
    private Transform baseObject;
    private Transform effectPool;

    public SetalliteTypeWeaponManager(Transform BaseObjectTransform, Transform EffectPool) 
        : base(BaseObjectTransform, EffectPool)
    {
        baseSetallite = BaseObjectTransform.GetChild(0).gameObject;
        baseObject = BaseObjectTransform;
        effectPool = EffectPool;
    }

    public override void Init()
    {
        Vector3 cashingVector3 = new Vector3(0, 1, 0);
        float AngleColculateFloat;
        float PI = Mathf.PI;
        setalliteCount = 6;

        setallite = new SetalliteTypeWeapon[setalliteCount];
        setallite[0] = baseSetallite.GetComponent<SetalliteTypeWeapon>();

        
        GameObject instantiateSetallites = new GameObject();
        for (int i = 1; i < setalliteCount; i++)
        {
            instantiateSetallites = Instantiate(baseSetallite, new Vector3(0, 0, 0), Quaternion.identity);
            setallite[i] = instantiateSetallites.GetComponent<SetalliteTypeWeapon>();
            instantiateSetallites.transform.localScale =  baseSetallite.transform.parent.parent.localScale;
            instantiateSetallites.transform.parent = baseObject;
        }
        
        for(int st = 0; st < setallite.Length;st++)
        {
            AngleColculateFloat = (360.0f / setalliteCount) * st / 180.0f * PI;
            cashingVector3.x = -Mathf.Sin(AngleColculateFloat);
            cashingVector3.y = Mathf.Cos(AngleColculateFloat);

            setallite[st].Init(baseObject, cashingVector3, effectPool);
        }
        instantiateSetallites = null;
    }
    public override void Execute()
    {
        foreach(var st in setallite)
        {
            st.Execute(baseObject, effectPool);
        }
    }
}
