using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
//using UnityEditor.SearchService;


public class DirectionalSign : MonoBehaviour
{
    private TreasureBoxEscapeStairManager TreasureBoxEscapeStairManager;
    private Transform player;
    private Transform targetObj;
    private Vector3 playerToObj;

    private float playerToObjAngle;
    private float screenDiagonalAngle;

    private float absAngle;

    private Vector3 cashingVector;
    private float radian;
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
        cashingVector = new Vector3(0, 0, 0);
        absAngle = 0;
        playerToObjAngle = 0;
        screenDiagonalAngle = 0;
        try
        {
            TreasureBoxEscapeStairManager = targetObj.parent.transform.parent.GetComponent<TreasureBoxEscapeStairManager>();
            player = TreasureBoxEscapeStairManager.Player.transform;
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
            player = TreasureBoxEscapeStairManager.Player.transform;
        }
        finally
        {
            //playerToObj = new Vector2(0.0f,0.0f);
        }
        radian = 180 / Mathf.PI;
    }

    private void Start()
    {
        targetObj = this.transform.parent.transform;

        playerToObj = new Vector2(targetObj.position.x - player.position.x, targetObj.position.y - player.position.y);
        playerToObjAngle = MathF.Atan2(playerToObj.x, playerToObj.y) * radian;
        this.transform.eulerAngles = new Vector3(0, 0,-playerToObjAngle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float scale = this.transform.localScale.x;
        screenDiagonalAngle = MathF.Atan2(Screen.height, Screen.width);
        playerToObj.x = (targetObj.position.x - player.position.x);
        playerToObj.y = (targetObj.position.y - player.position.y);
        playerToObjAngle = MathF.Atan2(playerToObj.y, playerToObj.x);
        absAngle = (playerToObjAngle >= 0) ? playerToObjAngle : -playerToObjAngle;
        
        // 화면 밖에 있을때 예외 처리
        if ( (  ((playerToObj.x >= 0) ? playerToObj.x : -playerToObj.x) > ((Screen.width * 0.005f + 0.64f >= 0) ? Screen.width * 0.005f + 0.64f : -Screen.width * 0.005f + 0.64f))
            || ((playerToObj.y >= 0) ? playerToObj.y : -playerToObj.y) > ((Screen.width * 0.005f + 0.64f >= 0) ? Screen.width * 0.005f + 0.64f : -Screen.width * 0.005f + 0.64f))
        {
            cashingVector.x = 0;
            cashingVector.y = 0;
            cashingVector.z = -90 + (playerToObjAngle) * radian;
            this.transform.eulerAngles = cashingVector;

            if(absAngle < screenDiagonalAngle)
            {
                cashingVector.x = (-playerToObj.x + (Screen.width * 0.00475f)) * scale * 0.5f;
                cashingVector.y = (-playerToObj.y + MathF.Sin(playerToObjAngle) * (Screen.width * 0.00475f)) * scale * 0.5f;
            }
            else 
            {      

                if (absAngle > screenDiagonalAngle && absAngle < MathF.PI - screenDiagonalAngle)
                {
                    //x좌표 오류 수정 요망

                    if (playerToObjAngle < 0)
                    {
                        cashingVector.x = (-playerToObj.x - MathF.Cos(playerToObjAngle) / MathF.Sin(playerToObjAngle) * (Screen.height / 200) ) * scale * 0.5f;
                        cashingVector.y = (-playerToObj.y - (Screen.height * 0.00475f)) * scale * 0.5f + 0.032f;
                    }
                    else if (playerToObjAngle > 0)
                    {
                        cashingVector.x = (-playerToObj.x + MathF.Cos(playerToObjAngle) / MathF.Sin(playerToObjAngle) * (Screen.height / 200)) * scale * 0.5f;
                        cashingVector.y = (-playerToObj.y + (Screen.height * 0.00475f)) * scale * 0.5f - 0.032f;
                    }
                }

                else
                {
                    cashingVector.x = (-playerToObj.x - (Screen.width * 0.00475f)) * scale * 0.5f;
                    cashingVector.y = (-playerToObj.y + MathF.Sin(playerToObjAngle) * (Screen.width * 0.00475f)) * scale * 0.5f;
                }
            }
        }
        // 오브젝트가 화면 안에 있을 때 예외 처리
        else 
        {
            cashingVector.x = 0;
            cashingVector.y = 0;
            cashingVector.z = 180;
            this.transform.eulerAngles = cashingVector;

            cashingVector.x = 0;
            cashingVector.y = 0.128f*3 * scale * 01.75f;
            cashingVector.z = 0;
        }

        cashingVector.z = 1;
        this.transform.localPosition = cashingVector;
    }
}
