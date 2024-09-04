using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTypeWeapon : MonoBehaviour
{
    private int baseCoolTime;
    private int coolTimer;
    [SerializeField]
    private AttackDirectional attackDirectional;
    [SerializeField]
    private ProjectileInWeapon[] projectile;
    private void Awake()
    {
        projectile = new ProjectileInWeapon[this.transform.childCount];
    }
    // Start is called before the first frame update
    void Start()
    {
        if (this.transform.parent.CompareTag("follower"))
        {
            attackDirectional = this.transform.parent.GetComponent<PlayerMove>().AttactDirectional;
        }
        else attackDirectional = this.transform.parent.parent.parent.GetChild(1).gameObject.GetComponent<AttackDirectional>();

        for (int a = 0; a < this.transform.childCount; a++)
        {
            projectile[a] = this.transform.GetChild(a).GetComponent<ProjectileInWeapon>();
            
        }
        baseCoolTime = 200;
        coolTimer = baseCoolTime;

        foreach (var p in projectile)
        {
            p.AttactDirection = attackDirectional.gameObject;
            p.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        coolTimer--;
        if (coolTimer < 0)
        {
            foreach(var p in projectile)
            {
                if(p.transform.gameObject.activeSelf == false)
                {
                    p.gameObject.SetActive(true);
                    p.Execute();
                    coolTimer = baseCoolTime;
                    break;
                }
            }
        }


    }

}
