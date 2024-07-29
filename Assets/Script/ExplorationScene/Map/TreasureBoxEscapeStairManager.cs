using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class TreasureBoxEscapeStairManager : MonoBehaviour
{
    public Vector2Int treasureBoxConst; // 보물상자의 타일 맵 안에서 xy위치
    public Vector2Int escapeStairConst; // 탈출계단의 타일 맵 안에서 xy위치

    public Vector2Int RandConst; // 보물상자와 탈출계단의 타일 셋 안에서 xy위치


    public GameObject TileSpriteImageStorage;
    public GameObject TreasureBox;
    public GameObject EscapeStair;

    public int stairPosition;
    public int boxPosition;

    //Test
    private List<GameObject> TreasureBoxList;
    private List<GameObject> EscapeStairList;


    private void Awake()
    {
        TreasureBoxList = new List<GameObject>();
        EscapeStairList = new List<GameObject>();   

        TileSpriteImageStorage = GameObject.Find("TileSpriteImageStorage");
        RandConst = TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RandConst;

        for(int i  = 0; i < TreasureBox.transform.childCount; i++)
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
            escapeStairConst.x = Random.Range(0, 20);
            escapeStairConst.y = Random.Range(0, 20);

            stairPosition = Mathf.Abs((int)(RandConst.x) * (escapeStairConst.x % 4) - (int)(RandConst.y) * (escapeStairConst.y % 4)) % 100;

            e.transform.position = new Vector3((escapeStairConst.x - 10) * 12.8f + 0.64f, (escapeStairConst.y - 10) * 12.8f - 0.64f);
            e.transform.position += new Vector3((int)((stairPosition % 10) - 5) * 1.28f, (int)((stairPosition / 10) - 5) * 1.28f);
        }
    }

    public void RemoveGameObjectInList(GameObject obj)
    {
        if (obj.tag == "Box")
        {
            TreasureBoxList.Remove(obj);
            obj.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    public void EventSwapTile( int column, int row)
    {
        foreach(var e in EscapeStairList)
        {
            e.transform.position += new Vector3(12.8f * (row), 12.8f * (-column));
        }

        foreach (var t in TreasureBoxList)
        {
            t.transform.position += new Vector3(1.28f * (row), 12.8f * (-column));
        }
    }
}
