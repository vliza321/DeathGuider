using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectionalSign : MonoBehaviour
{
    public TreasureBoxEscapeStairManager TreasureBoxEscapeStairManager;
    public Transform player;
    public Transform targetObj;
    public Vector2 playerToObj;
    public float playerToObjAngle;
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
        try
        {
            targetObj = this.transform.parent.transform;
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

    // Update is called once per frame
    void Update()
    {
        playerToObj = new Vector2(player.localPosition.x - targetObj.localPosition.x, player.localPosition.y - targetObj.localPosition.y);
        playerToObjAngle = MathF.Cos(Vector2.Distance(player.localPosition, targetObj.localPosition) / playerToObj.x);
        this.transform.localScale.Set(0.0f,0.0f,playerToObjAngle);
    }
}
