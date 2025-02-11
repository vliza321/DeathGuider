using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLoadTest : MonoBehaviour
{
    [SerializeField]
    private DontDestroyObjectManager DDOManager;
    [SerializeField]
    private GameManager GameManager;

    void Start()
    {
        GameObject[] DDO = GameObject.FindGameObjectsWithTag("DDO");
        foreach (var ddo in DDO)
        {
            if (ddo.name == "DDOManager")
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }
            if (ddo.name == "GameManager")
            {
                GameManager = ddo.transform.gameObject.GetComponent<GameManager>();
            }
        }
        DDO = null;
    }

    void temt()
    {
        int a = DDOManager.PrototypeUnitDatas.PrototypeUnitDataDic[100].Strength;
        var t = DDOManager.UnitDatas.UnitDatas;
        DDOManager.UnitDatas.UnitDataDic.Add((0, 100, DDOManager.LocalUserDatas.LocalUserDataDic[0].UnitInstanceCounter),new UnitData());
        DDOManager.UnitDatas.UnitDatas.Add(new UnitData());
    }

    void GameManagerTest()
    {
        // 자유롭게 수정가능한 변수의 수정 예시
        GameManager.SelectUserID = 10;
        GameManager.SelectStageID = 1;

        // Monster 관련 정보 불러오기
        LinkedList<int> appearMonster = new LinkedList<int>();
        foreach(var am in DDOManager.AppearDatas.AppearDatas)
        {
            if(am.StageID == GameManager.SelectStageID)
            {
                appearMonster.AddLast(am.MonsterID);
            }
        }
        foreach(var am in appearMonster)
        {
            Sprite monsterSprite = GameManager.Monster[am].GetComponent<Sprite>(); // 몬스터 이미지 받아오기
        }

        //prototypeUnit 정보 불러오기
        int prototypeUnitID = 1; // 임의로 선정한 안내자의 prototypeUnitID
        GameObject guiderPrefeb = GameManager.PrototypeUnit[prototypeUnitID]; // 안내자 프리팹 받아오기

        //prototypeWeapon 정보 불러오기
        int prototypeWeaponID = 1; // 임의로 선정한 무기의 prototypeWeaponID
        GameObject weaponPrefeb = GameManager.PrototypeUnit[prototypeWeaponID]; // 무기 프리팹 받아오기

        //weaponImg 정보 불러오기
        Sprite weaponImg = GameManager.WeaponImg[prototypeWeaponID];

        //guiderHeadImg, guiderBodyImg 정보 불러오기
        Sprite guiderHeadImg = GameManager.GuiderHeadImg[prototypeUnitID];
        Sprite guiderBodyImg = GameManager.GuiderBodyImg[prototypeUnitID];

        //prisonerHeadImg, prisonerBodyImg, prisonerHeadAnim, prisonerBodyAnim 정보 불러오기
        int HeadID = 0;
        int BodyID = 4;
        Sprite prisonerHeadImg = GameManager.PrisonerHeadImg[HeadID];
        Sprite prisonerBodyImg = GameManager.PrisonerBodyImg[BodyID];
        RuntimeAnimatorController prisonerHeadAnim = GameManager.PrisonerHeadAnim[HeadID];
        RuntimeAnimatorController prisonerBodyAnim = GameManager.PrisonerBodyAnim[BodyID];

        //baseTileImg 정보 불러오기
        Sprite tileImg = GameManager.BaseTileImg[GameManager.SelectStageID];

    }

}
