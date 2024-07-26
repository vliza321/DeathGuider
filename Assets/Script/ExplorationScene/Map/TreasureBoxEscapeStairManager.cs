using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        TileSpriteImageStorage = GameObject.Find("TileSpriteImageStorage");

        TreasureBox = this.transform.GetChild(0).gameObject;
        EscapeStair = this.transform.GetChild(1).gameObject;

        treasureBoxConst.x = Random.Range(0, 20);
        treasureBoxConst.y = Random.Range(0, 20);
        escapeStairConst.x = Random.Range(0, 20);
        escapeStairConst.y = Random.Range(0, 20);

        RandConst = TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RandConst;
        stairPosition = Mathf.Abs((int)(RandConst.x) * (escapeStairConst.x % 4) - (int)(RandConst.y) * (escapeStairConst.y % 4)) % 100;
        boxPosition = Mathf.Abs((int)(RandConst.x) * (treasureBoxConst.x % 4) - (int)(RandConst.y) * (treasureBoxConst.y % 4)) % 100;


        //map 안의 set위치 설정
        EscapeStair.transform.GetChild(0).gameObject.transform.position = new Vector3((escapeStairConst.x - 10) * 12.8f + 0.64f, (escapeStairConst.y - 10) * 12.8f - 0.64f); ;
        TreasureBox.transform.GetChild(0).gameObject.transform.position = new Vector3((treasureBoxConst.x-10) * 12.8f + 0.64f, (treasureBoxConst.y-10) * 12.8f - 0.64f);

        //set 안의 tile위치 설정
        EscapeStair.transform.GetChild(0).gameObject.transform.position += new Vector3((int)((stairPosition % 10) - 5) * 1.28f, (int)((stairPosition / 10) - 5)* 1.28f);
        TreasureBox.transform.GetChild(0).gameObject.transform.position += new Vector3((int)((boxPosition % 10) - 5) * 1.28f, (int)((boxPosition / 10) - 5) * 1.28f);
    }
    // Start is called before the first frame update
    private void Start()
    {

    }

    public void EventSwapTile( int column, int row)
    {
        EscapeStair.transform.GetChild(0).gameObject.transform.position += new Vector3( 12.8f * (row), 12.8f * (-column));
        TreasureBox.transform.GetChild(0).gameObject.transform.position += new Vector3( 1.28f * (row), 12.8f * (-column));
    }
}
