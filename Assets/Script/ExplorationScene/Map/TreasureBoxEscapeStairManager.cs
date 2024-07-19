using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBoxEscapeStairManager : MonoBehaviour
{
    public Vector2 RandConstInSet; // 보물상자와 탈출계단의 타일 셋 안에서 xy위치
    public Vector2 RandConstInMap; // 보물상자와 탈출계단의 타일 맵 안에서 xy위치
    private GameObject TileSpriteImageStorage;
    public int treasureBoxConst;
    public int escapeStairConst;
    private void Awake()
    {
        TileSpriteImageStorage = GameObject.Find("TileSpriteImageStorage");
        //treasureBoxConst = Random.Range()
    }

    // Start is called before the first frame update
    private void Start()
    {
        RandConstInSet = TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RandConst;
        RandConstInMap = TileSpriteImageStorage.GetComponent<TileSpriteImageStorage>().RandConst;
    }
}
