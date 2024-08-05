using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    public GameObject Player;
    public GameObject MonsterObject;
    public Vector2 MonsterLocalScale;
    public Vector2 MonsterVelocityVector;

    public float MoveSpeeds;
    public float distance;
    Vector3 playerPos;
    public GameObject guider;

    public bool isKnockBack;
    public int knockBackTimer;
    float guiderMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        MonsterObject = this.gameObject;
        MonsterVelocityVector = new Vector2(0, 0);

        guider = Player.GetComponent<PlayerSwap>().guider;
        MoveSpeeds = guider.GetComponent<PlayerMove>().MoveSpeed / 5.0f;
        playerPos = guider.transform.position;
        isKnockBack = false;
        knockBackTimer = 10;
        guiderMoveSpeed = guider.GetComponent<PlayerMove>().MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        switch(isKnockBack)
        {
            case true: 
                knockBackTimer--;
                if(knockBackTimer < 1)
                {
                    isKnockBack = false;
                    knockBackTimer = 10;
                }

                break;
            case false:
                //guider = Player.GetComponent<PlayerSwap>().Guider;
                playerPos = guider.transform.position;
                MonsterLocalScale = MonsterObject.transform.position;
                distance = Vector3.Distance(guider.transform.position, MonsterLocalScale);
                MonsterVelocityVector.x = MonsterObject.transform.position.x - guider.transform.position.x;
                MonsterVelocityVector.y = MonsterObject.transform.position.y - guider.transform.position.y;

                if (distance < 0.5f) { MoveSpeeds = 0.10f; }
                if ((distance < 12.0f) && (distance >= 0.5f))
                {
                    MoveSpeeds = guiderMoveSpeed / 2.0f + 0.5f;
                }
                if (distance >= 12.0f) { MoveSpeeds = guiderMoveSpeed / 5.0f + 0.1f; }
                break;
        }
    }

    private void FixedUpdate()
    {
        float signX;
        float signY;
        if (Random.Range(0, 2) == 1) signX = 1;
        else signX = -1;
        if (Random.Range(0, 2) == 1) signY = 1;
        else signY = -1;
        if (distance > 30.0f) { this.transform.position = new Vector3(playerPos.x + signX * 12.8f * (Random.Range(6, 10) / 10.0f), playerPos.y + signY * 12.8f * (Random.Range(4, 8) / 10.0f), playerPos.z);  }
        
        MonsterVelocityVector = MonsterVelocityVector.normalized * MoveSpeeds * Time.fixedDeltaTime * (Mathf.Log10(knockBackTimer));

        MonsterObject.transform.position = new Vector2(MonsterObject.transform.position.x - MonsterVelocityVector.x, MonsterObject.transform.position.y - MonsterVelocityVector.y);

    }
}
