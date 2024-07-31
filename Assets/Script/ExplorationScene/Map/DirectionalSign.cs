using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
using UnityEditor.SearchService;


public class DirectionalSign : MonoBehaviour
{
    public TreasureBoxEscapeStairManager TreasureBoxEscapeStairManager;
    public Transform player;
    public Transform targetObj;
    public Vector3 playerToObj;
    public float playerToObjDistance;
    public float playerToObjAngle;
    
    public Vector3 lastPosition;
    
    private float absAngle;

    public Transform Player
    {
        get
        {
            return player;
        }
        set
        {
            player = value;
        }
    }

    private void Awake()
    {
        lastPosition = new Vector3(0, 0, 0);
        absAngle = 0;
        playerToObjDistance = 0;
        try
        {
            TreasureBoxEscapeStairManager = targetObj.parent.transform.parent.GetComponent<TreasureBoxEscapeStairManager>();
            player = TreasureBoxEscapeStairManager.player.transform;
        }
        catch
        {
            GameObject[] Manager = GameObject.FindGameObjectsWithTag("Manager");
            foreach (GameObject manager in Manager)
            {
                if (manager.name == "TreasureBoxEscapeStairManager")
                {
                    TreasureBoxEscapeStairManager = manager.GetComponent<TreasureBoxEscapeStairManager>();
                    break;
                }
                Manager = null;
            }
            player = TreasureBoxEscapeStairManager.player.transform;
        }
        finally
        {
            //playerToObj = new Vector2(0.0f,0.0f);
        }
    }

    private void Start()
    {
        targetObj = this.transform.parent.transform;

        playerToObj = new Vector2(targetObj.position.x - player.position.x, targetObj.position.y - player.position.y);
        playerToObjAngle = MathF.Atan2(playerToObj.x, playerToObj.y) * 180.0f / MathF.PI;
        this.transform.eulerAngles = new Vector3(0, 0,-playerToObjAngle);
        playerToObjDistance = Vector2.Distance(targetObj.position,player.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerToObj = new Vector2(targetObj.position.x - player.position.x, targetObj.position.y - player.position.y);
        playerToObjDistance = Vector2.Distance(targetObj.position,player.position);
        playerToObjAngle = MathF.Atan2(playerToObj.x, playerToObj.y)*180.0f/MathF.PI;
        absAngle = MathF.Abs(playerToObjAngle); 
        // 화면 밖에 있을때 예외 처리
        if(MathF.Abs(playerToObj.x) > MathF.Abs(Screen.width/200+0.64f) || MathF.Abs(playerToObj.y) > MathF.Abs(Screen.height/200+0.64f)) // 가로가 화면 밖에 있을때
        {
            this.transform.eulerAngles = new Vector3(0, 0, -playerToObjAngle );
            if (absAngle == 90.0f ) // +y축 위 경우
            {
                lastPosition.y = 0.0f;
                if(playerToObjAngle < 0)
                {
                    lastPosition.x = +((Screen.height / 2) / 100 + playerToObj.y) / 4;
                }
                else
                {
                    lastPosition.x = -((Screen.height / 2) / 100 - playerToObj.y) / 4;
                }
            }
            else if (absAngle <= 45f) // 높이가 고정인 상황
            {
                lastPosition.x = (-playerToObj.x + (Screen.height / 2) / 100 * MathF.Tan(playerToObjAngle / 180 * MathF.PI))/4;
                lastPosition.y = (-playerToObj.y + (Screen.height / 2) / 100)/4;

            }
            else if (absAngle >  45f || absAngle < 90f) // 높이가 고정인 상황
            {
                lastPosition.x = (-playerToObj.x + (Screen.height / 2) / 100 * MathF.Tan(playerToObjAngle / 180 * MathF.PI)) / 4;
                lastPosition.y = (-playerToObj.y + (Screen.height / 2) / 100) / 4;

            }
            else // 너비가 고정인 상황
            {
                lastPosition.x = (-playerToObj.x + (Screen.width / 2) / 100)/4;
                lastPosition.y = (-playerToObj.y + ((Screen.width / 2) / 100) / MathF.Tan(playerToObjAngle / 180 * MathF.PI))/4;
            }
            
        }
        else 
        {
            this.transform.eulerAngles = new Vector3(0, 0, 180 );
            lastPosition.x = 0;
            lastPosition.y = 0.128f*3;
        }
        /*
        if (absAngle == 90f)
        {
            if (playerToObjAngle > 0)
            {
                lastPosition.x =  (Screen.width / 2) / 100 - playerToObj.x ;
                lastPosition.y = -playerToObj.y;
            }
            else
            {
                lastPosition.x = (Screen.width / 2) / 100 - playerToObj.x;
                lastPosition.y = -playerToObj.y;
            }
        }
        else if (absAngle < 45f || absAngle > 135f) // 높이가 고정인 상황
        {
            lastPosition.x = -playerToObj.x + (Screen.height/2) / 100 * MathF.Tan(playerToObjAngle / 180 * MathF.PI);
            lastPosition.y = -playerToObj.y + (Screen.height/2) / 100;

        }
        else // 너비가 고정인 상황
        {
            lastPosition.x = -playerToObj.x + (Screen.width / 2) / 100;
            lastPosition.y = -playerToObj.y + ((Screen.width / 2) / 100) / MathF.Tan(playerToObjAngle / 180 * MathF.PI); 
        }*/
        this.transform.localPosition = new Vector3 (lastPosition.x,lastPosition.y,0);
    }
}
