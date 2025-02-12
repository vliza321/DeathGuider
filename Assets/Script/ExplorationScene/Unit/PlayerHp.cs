using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHp : MonoBehaviour
{
    private float HitDelay;
    private float MaxHP;
    [SerializeField]
    private float HeartPoint;

    private string MonsterTagName = "Monster";
    private Vector3 cashingVector;
    // Start is called before the first frame update
    void Start()
    {
        cashingVector = Vector3.zero;
        HitDelay = 0.5f;
        MaxHP = 100;
        HeartPoint = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HitDelay > 0) { HitDelay -= Time.deltaTime; }
        cashingVector.x = transform.parent.position.x;
        cashingVector.y = transform.parent.position.y;
        cashingVector.z = transform.parent.position.z;
        this.transform.position = cashingVector;
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
        if (collision.gameObject.CompareTag(MonsterTagName))
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
            if (collision.gameObject.CompareTag(MonsterTagName))
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
