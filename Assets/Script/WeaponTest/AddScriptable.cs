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


    public Queue<GameObject> bullet;
   
    public void Initialized()
    {
        bullet = new Queue<GameObject>(100);
        for(int b =0; b < bullet.Count; b++)
        {
            GameObject bulletClone = Instantiate(bulletPrefeb);
            bulletClone.SetActive(false);
            bullet.Enqueue(bulletClone);
        }
    }

    public GameObject bulletPrefeb;
    public Vector3 shootPoint;
    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefeb);
        bullet.transform.position = shootPoint;
    }


    public void destroyBullet()
    {
        if(this.transform.position.y > 100)
        {
            Destroy(this.gameObject);
        }
    }
}
