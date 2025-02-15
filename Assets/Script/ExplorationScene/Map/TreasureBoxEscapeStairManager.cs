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

    private GameObject treasureBox;
    private GameObject escapeStair;

    private int stairPosition;
    private int boxPosition;

    [SerializeField]
    private List<GameObject> treasureBoxList;
    [SerializeField]
    private List<GameObject> escapeStairList;

    public List<GameObject> TreasureBoxList
    {
        get { return treasureBoxList; }        
    }
    public List<GameObject> EscapeStairList
    {
        get { return escapeStairList; }
    }

    private GameObject player;
    public GameObject Player
    {
        get { return player; }
        set 
        {
            player = value.gameObject;
            foreach(var t in treasureBoxList)
            {
                t.GetComponentInChildren<DirectionalSign>().Player = value.transform;
            }
            foreach (var s in escapeStairList)
            {
                s.GetComponentInChildren<DirectionalSign>().Player = value.transform;
            }
        }
    }
    private string boxName = "Box";
    private string stairName = "Stair";
    private Vector3 cashingVector;
    private void Awake()
    {
        cashingVector = Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");

        treasureBoxList = new List<GameObject>();
        escapeStairList = new List<GameObject>();   

        RandConst = GameObject.Find("TileSpriteImageStorage").GetComponent<TileSpriteImageStorage>().randConst;

        treasureBox = this.gameObject.transform.GetChild(0).gameObject;
        escapeStair = this.gameObject.transform.GetChild(1).gameObject;

        for (int i  = 0; i < treasureBox.transform.childCount; i++)
        {
            treasureBoxList.Add(treasureBox.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < escapeStair.transform.childCount; i++)
        {
            escapeStairList.Add(escapeStair.transform.GetChild(i).gameObject);
        }

        foreach (var t in treasureBoxList)
        {
            treasureBoxConst.x = Random.Range(0, 20);
            treasureBoxConst.y = Random.Range(0, 20);

            boxPosition = Mathf.Abs((int)(RandConst.x) * (treasureBoxConst.x % 4) - (int)(RandConst.y) * (treasureBoxConst.y % 4)) % 100;

            t.transform.position = new Vector3((treasureBoxConst.x - 10) * 12.8f + 0.64f, (treasureBoxConst.y - 10) * 12.8f - 0.64f);
            t.transform.position += new Vector3((int)((boxPosition % 10) - 5) * 1.28f, (int)((boxPosition / 10) - 5) * 1.28f);
        }
        foreach(var e in escapeStairList)
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
            escapeStairList.Add(obj);
        }
    }
    public void AddInBoxList(GameObject obj)
    {
        if (obj.tag == boxName)
        {
            treasureBoxList.Add(obj);
        }
    }

    public void RemoveGameObjectInBoxList(GameObject obj)
    {
        if (obj.tag == boxName)
        {
            treasureBoxList.Remove(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public void RemoveGameObjectInStairList(GameObject obj)
    {
        if (obj.tag == stairName)
        {
            escapeStairList.Remove(obj);
            obj.gameObject.SetActive(false);
        }
    }

    public void EventSwapTile(int row, int column)
    {
        foreach(var e in escapeStairList)
        {
            cashingVector.x = 12.8f * column;
            cashingVector.y = 12.8f * -row;
            cashingVector.z = 0;
            e.transform.position += cashingVector;
        }

        foreach (var t in treasureBoxList)
        {
            cashingVector.x = 12.8f * column;
            cashingVector.y = 12.8f * -row;
            cashingVector.z = 0;
            t.transform.position += cashingVector;
        }
    }
}
