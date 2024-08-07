using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHp : MonoBehaviour
{
    float HitDelay;
    float MaxHP;
    [SerializeField]
    float HeartPoint;
    // Start is called before the first frame update
    void Start()
    {
        HitDelay = 20*Time.deltaTime;
        MaxHP = 100;
        HeartPoint = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HitDelay > 0) { HitDelay--; }
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            HeartPoint--;
            if (HeartPoint <= 0)
            {
                HeartPoint = MaxHP;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (HitDelay == 0)
        {
            if (collision.gameObject.CompareTag("Monster"))
            {
                HeartPoint--;
                if (HeartPoint <= 0)
                {
                    HeartPoint = MaxHP;
                }
            }
        }
    }
}
