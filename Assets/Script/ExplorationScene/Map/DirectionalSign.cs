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
    private Vector3 lastPosition;
    


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
        playerToObj = new Vector2(targetObj.position.x - player.position.x, targetObj.position.y - player.position.y);
        playerToObjAngle = MathF.Atan2(playerToObj.y, playerToObj.x);// * 180.0f / MathF.PI;// + 180.0f;
        absAngle = MathF.Abs(playerToObjAngle); 
        // 화면 밖에 있을때 예외 처리
        if(MathF.Abs(playerToObj.x) > MathF.Abs(Screen.width/200+0.64f) || MathF.Abs(playerToObj.y) > MathF.Abs(Screen.height/200+0.64f)) // 가로가 화면 밖에 있을때
        {
            
            this.transform.eulerAngles = new Vector3(0, 0, -90 + (playerToObjAngle) * (180.0f) / MathF.PI);

            if(absAngle < screenDiagonalAngle)
            {
                lastPosition.x = (-playerToObj.x + (Screen.width / 200))/4;
                lastPosition.y = (-playerToObj.y + MathF.Sin(playerToObjAngle) * (Screen.width / 200)) / 4;
            }
            else 
            {      

                if (absAngle > screenDiagonalAngle && absAngle < MathF.PI - screenDiagonalAngle)
                {
                    //x좌표 오류 수정 요망

                    if (playerToObjAngle < 0)
                    {
                        lastPosition.x = (-playerToObj.x - MathF.Cos(playerToObjAngle) / MathF.Sin(playerToObjAngle) * (Screen.height / 200) ) / 4 ;
                        lastPosition.y = (-playerToObj.y - (Screen.height / 200)) / 4 + 0.032f;
                    }
                    else if (playerToObjAngle > 0)
                    {
                        lastPosition.x = (-playerToObj.x + MathF.Cos(playerToObjAngle) / MathF.Sin(playerToObjAngle) * (Screen.height / 200)) / 4;
                        lastPosition.y = (-playerToObj.y + (Screen.height / 200)) / 4 - 0.032f;
                    }
                }

                else
                {
                    lastPosition.x = (-playerToObj.x - (Screen.width / 200)) / 4;
                    lastPosition.y = (-playerToObj.y + MathF.Sin(playerToObjAngle) * (Screen.width / 200)) / 4;
                }
            }
        }
        // 오브젝트가 화면 안에 있을 때 예외 처리
        else 
        {
            this.transform.eulerAngles = new Vector3(0, 0, 180 );
            lastPosition.x = 0;
            lastPosition.y = 0.128f*3;
        }

        this.transform.localPosition = new Vector3 (lastPosition.x,lastPosition.y,0);
    }
}
