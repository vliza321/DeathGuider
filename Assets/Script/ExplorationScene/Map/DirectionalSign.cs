using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
using UnityEditor.SearchService;


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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        screenDiagonalAngle = MathF.Atan2(Screen.height, Screen.width);
        playerToObj.x = (targetObj.position.x - player.position.x);
        playerToObj.y = (targetObj.position.y - player.position.y);
        playerToObjAngle = MathF.Atan2(playerToObj.y, playerToObj.x);// * 180.0f / MathF.PI;// + 180.0f;
        absAngle = MathF.Abs(playerToObjAngle); 
        // 화면 밖에 있을때 예외 처리
        if(MathF.Abs(playerToObj.x) > MathF.Abs(Screen.width/200+0.64f) || MathF.Abs(playerToObj.y) > MathF.Abs(Screen.height/200+0.64f)) // 가로가 화면 밖에 있을때
        {
            cashingVector.x = 0;
            cashingVector.y = 0;
            cashingVector.z = -90 + (playerToObjAngle) * (180.0f) / MathF.PI;
            this.transform.eulerAngles = cashingVector;

            if(absAngle < screenDiagonalAngle)
            {
                cashingVector.x = (-playerToObj.x + (Screen.width / 200))/4;
                cashingVector.y = (-playerToObj.y + MathF.Sin(playerToObjAngle) * (Screen.width / 200)) / 4;
            }
            else 
            {      

                if (absAngle > screenDiagonalAngle && absAngle < MathF.PI - screenDiagonalAngle)
                {
                    //x좌표 오류 수정 요망

                    if (playerToObjAngle < 0)
                    {
                        cashingVector.x = (-playerToObj.x - MathF.Cos(playerToObjAngle) / MathF.Sin(playerToObjAngle) * (Screen.height / 200) ) / 4 ;
                        cashingVector.y = (-playerToObj.y - (Screen.height / 200)) / 4 + 0.032f;
                    }
                    else if (playerToObjAngle > 0)
                    {
                        cashingVector.x = (-playerToObj.x + MathF.Cos(playerToObjAngle) / MathF.Sin(playerToObjAngle) * (Screen.height / 200)) / 4;
                        cashingVector.y = (-playerToObj.y + (Screen.height / 200)) / 4 - 0.032f;
                    }
                }

                else
                {
                    cashingVector.x = (-playerToObj.x - (Screen.width / 200)) / 4;
                    cashingVector.y = (-playerToObj.y + MathF.Sin(playerToObjAngle) * (Screen.width / 200)) / 4;
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
            cashingVector.y = 0.128f*3;
            cashingVector.z = 0;
        }

        this.transform.localPosition = cashingVector;
    }
}
