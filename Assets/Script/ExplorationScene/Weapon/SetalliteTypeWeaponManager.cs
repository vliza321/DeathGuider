using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetalliteTypeWeaponManager : MonoBehaviour
{
    public WeaponState setalliteTypeWeapon;
    public int setalliteCount;
    private GameObject setallite;

    
    private void Awake()
    {
        setalliteTypeWeapon = new WeaponState(60, 1, "ªÏ¿Œ", WeaponType.Setallite, 2);
        setallite = this.transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        Vector3 cashingVector3 = new Vector3(0, 1, 0);
        float AngleColculateFloat;
        float PI = Mathf.PI;
        setalliteCount = (setalliteTypeWeapon.Level / 10) + 1;
        GameObject instantiateSetallites = new GameObject();
        for (int i = 1; i < setalliteCount; i++)
        {
            instantiateSetallites = Instantiate(setallite, new Vector3(0, 0, 0), Quaternion.identity);
            instantiateSetallites.transform.localScale = setallite.transform.localScale;
            instantiateSetallites.transform.parent = this.transform;

            AngleColculateFloat = (360.0f / setalliteCount) * i / 180.0f * PI;
            cashingVector3.x = - Mathf.Sin(AngleColculateFloat);
            cashingVector3.y = Mathf.Cos(AngleColculateFloat);

            instantiateSetallites.GetComponent<SetalliteTypeWeapon>().Init(cashingVector3);
            //Debug.Log(AngleColculateFloat * 180.0f / PI +" "+ cashingVector3.x +" "+ cashingVector3.y);
        }
    }
}
