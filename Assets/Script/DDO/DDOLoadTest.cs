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

    WeaponData WeaponSpawnTest()
    {
        int maxWeapon = 3; // 랭크 안에 만들어진 무기 개수
        int maxRank = 2; // 생성할 수 있는 무기 등급
        int newWeaponID = Random.Range(0, maxWeapon);
        int rank = Random.Range(0, maxRank + 1); 
        
        WeaponData newWeapon = new WeaponData();
        newWeapon.UserID = GameManager.SelectUserID;
        newWeapon.PrototypeWeaponID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeaponID + rank * 1000].ID;
        newWeapon.InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeaponID + rank * 1000].InstanceCounter;
        newWeapon.Name = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeaponID + rank * 1000].Name;
        newWeapon.AttackPoint = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeaponID + rank * 1000].AttackPoint;
        newWeapon.Durability = 100;
        newWeapon.Type = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeaponID + rank * 1000].Type;
        newWeapon.Enforce = 0;
        newWeapon.Crime = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeaponID + rank * 1000].Crime;
        newWeapon.Rank = rank;
        newWeapon.EffectID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[newWeaponID + rank * 1000].Type;
        newWeapon.ActivityStatus = 0;

        return newWeapon;
    }
    
    
    WeaponData weapon1;
    WeaponData weapon2;
    WeaponData weapon3;

    void WeaponCreate()
    {
        weapon1 = WeaponSpawnTest();
        weapon2 = WeaponSpawnTest();
        weapon3 = WeaponSpawnTest();
    }

    void WeaponBuy(WeaponData weapon)
    {
        //PrototypeWeapon의 instanceCounter증가
        DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].InstanceCounter++;

        //구매한 무기의 InstanceID를 변경
        weapon.InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].InstanceCounter;

        //데이터 베이스에 list 및 dictionary에 추가
        DDOManager.WeaponDatas.WeaponDatas.Add(weapon);
        DDOManager.WeaponDatas.WeaponDataDic.Add((weapon.UserID, weapon.PrototypeWeaponID, weapon.InstanceID), weapon);
    }

    void EnhanceWeapon(WeaponData weapon)
    {
        // 무기 강화 횟수가 5이상이면 탈출
        if(weapon.Enforce <= 5) { return; }

        //강화할 무기의 정보 변경
        weapon.AttackPoint += (int)(DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].AttackPoint * 0.2f);
        weapon.Enforce++;
    }


    void EnhanceWeapon(int UserID, int PrototypeWeaponID, int InstanceID)
    {
        //key 값에 맞는 무기 찾기
        WeaponData weapon = DDOManager.WeaponDatas.WeaponDataDic[(UserID, PrototypeWeaponID, InstanceID)];

        // 무기 강화 횟수가 5이상이면 탈출
        if (weapon.Enforce == 5) { return; }

        //강화할 무기의 정보 변경
        weapon.AttackPoint += (int)(DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].AttackPoint * 0.2f);
        weapon.Enforce++;
    }

    void WeaponEvolution(WeaponData weapon)
    {
        //무기가 진화될 대상의 ID 및 InstanceCounter 증가
        int EvolutionedWeaponPrototypeID = weapon.PrototypeWeaponID + 1000;
        DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[EvolutionedWeaponPrototypeID].InstanceCounter++;

        //진화할 무기의 정보 변경
        weapon.PrototypeWeaponID += EvolutionedWeaponPrototypeID;
        weapon.InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].InstanceCounter;
        weapon.Name = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].Name;
        weapon.AttackPoint = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].AttackPoint;
        weapon.Durability = 100;
        weapon.Enforce = 0;
        weapon.Rank++;
    }

    void WeaponEvolution(int UserID, int PrototypeWeaponID, int InstanceID)
    {
        //key 값에 맞는 무기 찾기
        WeaponData weapon = DDOManager.WeaponDatas.WeaponDataDic[(UserID, PrototypeWeaponID, InstanceID)];

        //무기가 진화될 대상의 ID 및 InstanceCounter 증가
        int EvolutionedWeaponPrototypeID = weapon.PrototypeWeaponID + 1000;
        DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[EvolutionedWeaponPrototypeID].InstanceCounter++;

        //진화할 무기의 정보 변경
        weapon.PrototypeWeaponID += EvolutionedWeaponPrototypeID;
        weapon.InstanceID = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].InstanceCounter;
        weapon.Name = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].Name;
        weapon.AttackPoint = DDOManager.PrototypeWeaponDatas.PrototypeWeaponDataDic[weapon.PrototypeWeaponID].AttackPoint;
        weapon.Durability = 100;
        weapon.Enforce = 0;
        weapon.Rank++;
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
