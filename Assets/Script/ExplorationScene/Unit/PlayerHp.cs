using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    float HeartPoint;
    // Start is called before the first frame update
    void Start()
    {
        HeartPoint = 100;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
    }
    
    float GetHp()
    {
        return HeartPoint;
    }

    void SetHp(float hp)
    {
        HeartPoint += hp;
    }
    /*void SetHp<T>(ref T hp)
    {
        HeartPoint += hp;
    }
    void init
    */
}
