using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class TreasureBoxEscapeStairManager : MonoBehaviour
{
    private Vector2Int treasureBoxConst; // 보물상자의 타일 맵 안에서 xy위치
    private Vector2Int escapeStairConst; // 탈출계단의 타일 맵 안에서 xy위치

    private Vector2Int RandConst; // 보물상자와 탈출계단의 타일 셋 안에서 xy위치

    private GameObject TreasureBox;
    private GameObject EscapeStair;

    private int stairPosition;
    private int boxPosition;

    private List<GameObject> TreasureBoxList;
    private List<GameObject> EscapeStairList;

    public List<GameObject> treasureBoxList
    {
        get { return TreasureBoxList; }        
    }
    public List<GameObject> escapeStairList
    {
        get { return EscapeStairList; }
    }

    private GameObject Player;
    public GameObject player
    {
        get { return Player; }
        set 
        {
            Player = value.gameObject;
            for(int i = 0;i<this.transform.childCount;i++)
            {
                for(int j =0;j<this.transform.GetChild(i).transform.childCount;j++ )
                {
                   this.transform.GetChild(i).transform.GetChild(j).transform.GetChild(0).GetComponent<DirectionalSign>().Player = value.transform;
                }
            }
        }
    }
    private string boxName = "Box";
    private string stairName = "Stair";
    private Vector3 cashingVector;
    private void Awake()
    {
        cashingVector = Vector3.zero;
        Player = GameObject.FindGameObjectWithTag("Player");

        TreasureBoxList = new List<GameObject>();
        EscapeStairList = new List<GameObject>();   

        RandConst = GameObject.Find("TileSpriteImageStorage").GetComponent<TileSpriteImageStorage>().randConst;

        TreasureBox = this.gameObject.transform.GetChild(0).gameObject;
        EscapeStair = this.gameObject.transform.GetChild(1).gameObject;

        for (int i  = 0; i < TreasureBox.transform.childCount; i++)
        {
            TreasureBoxList.Add(TreasureBox.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < EscapeStair.transform.childCount; i++)
        {
            EscapeStairList.Add(EscapeStair.transform.GetChild(i).gameObject);
        }

        foreach (var t in TreasureBoxList)
        {
            treasureBoxConst.x = Random.Range(0, 20);
            treasureBoxConst.y = Random.Range(0, 20);

            boxPosition = Mathf.Abs((int)(RandConst.x) * (treasureBoxConst.x % 4) - (int)(RandConst.y) * (treasureBoxConst.y % 4)) % 100;

            t.transform.position = new Vector3((treasureBoxConst.x - 10) * 12.8f + 0.64f, (treasureBoxConst.y - 10) * 12.8f - 0.64f);
            t.transform.position += new Vector3((int)((boxPosition % 10) - 5) * 1.28f, (int)((boxPosition / 10) - 5) * 1.28f);
        }
        foreach(var e in EscapeStairList)
        {
            escapeStairConst.x = Random.Range(0, 30);
            escapeStairConst.y = Random.Range(0, 30);

            stairPosition = Mathf.Abs((int)(RandConst.x) * (escapeStairConst.x % 4) - (int)(RandConst.y) * (escapeStairConst.y % 4)) % 100;

            e.transform.position = new Vector3((escapeStairConst.x - 15) * 12.8f + 0.64f, (escapeStairConst.y - 15) * 12.8f - 0.64f);
            e.transform.position += new Vector3((int)((stairPosition % 10) - 5) * 1.28f, (int)((stairPosition / 10) - 5) * 1.28f);
        }
    }

    public void AddInStairList(GameObject obj)
    {
        if(obj.tag == stairName)
        {
            EscapeStairList.Add(obj);
        }
    }
    public void AddInBoxList(GameObject obj)
    {
        if (obj.tag == boxName)
        {
            TreasureBoxList.Add(obj);
        }
    }

    public void RemoveGameObjectInBoxList(GameObject obj)
    {
        if (obj.tag == boxName)
        {
            TreasureBoxList.Remove(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public void RemoveGameObjectInStairList(GameObject obj)
    {
        if (obj.tag == stairName)
        {
            EscapeStairList.Remove(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public void EventSwapTile( int column, int row)
    {
        foreach(var e in EscapeStairList)
        {
            cashingVector.x = 12.8f * row;
            cashingVector.y = 12.8f * (-column);
            cashingVector.z = 0;
            e.transform.position += cashingVector;
        }

        foreach (var t in TreasureBoxList)
        {
            cashingVector.x = 12.8f * row;
            cashingVector.y = 12.8f * (-column);
            cashingVector.z = 0;
            t.transform.position += cashingVector;
        }
    }
}
