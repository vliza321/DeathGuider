using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FloorUIManager : MonoBehaviour
{
    public FloorSystem floorSystem;
    public GYMSystem gymSystem;
    public HealthSystem heathSystem;
    public ErosionSystem erosionSystem;
    public SmithSystem smithSystem;
    public BattleReadySystem battleReadySystem;
    public MoveCamera moveCamera;
    
    public GameObject floorPrisonerUI;
    public GameObject prisonerInfoUI;

    public GameObject laboratoryPrisonerChooseUI;
    public GameObject gymPrisonerChooseUI;
    public GameObject healthPrisonerChooseUI;
    public GameObject erosionPrisonerChooseUI;
    public GameObject smithPrisonerChooseUI;
    public GameObject managerPrisonerChooseUI;
    public GameObject dungeonPrisonerUI;

    private GameManager GameManager;
    private DontDestroyObjectManager DDOManager;

    public void Start()
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

    public void OpenFloorPrisonerUI()
    {
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
        laboratoryPrisonerChooseUI.SetActive(false);
        smithPrisonerChooseUI.SetActive(false);
        dungeonPrisonerUI.SetActive(false);
        floorSystem.DisplayFloorPrisoners();
        floorPrisonerUI.SetActive(true);
    }

    public void CloseFloorPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
    }

    public void openFloorPrisonerInfoUI(UnitData prisoner)
    {
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
        laboratoryPrisonerChooseUI.SetActive(false);
        dungeonPrisonerUI.SetActive(false);
        floorSystem.UpdatePrisonerInfoUI(prisoner);  // 죄수 정보 업데이트
        prisonerInfoUI.SetActive(true);              // 죄수 정보 UI 활성화
    }

    public void closeFloorPrisonerInfoUI()
    {
        prisonerInfoUI.SetActive(false);
    }

    public GameObject GetPrisonerInfoUI()
    {
        return prisonerInfoUI;
    }

    public void openGymPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
        laboratoryPrisonerChooseUI.SetActive(false);
        smithPrisonerChooseUI.SetActive(false);
        dungeonPrisonerUI.SetActive(false);
        gymSystem.DisplayGYMTrainingUI();
        gymSystem.DisplayGYMPrisoners();
        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 200));
        }
        gymPrisonerChooseUI.SetActive(true);
    }

    public void closeGymPrisonerUI()
    {
        gymPrisonerChooseUI.SetActive(false);
    }

    public void openHealthPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
        laboratoryPrisonerChooseUI.SetActive(false);
        smithPrisonerChooseUI.SetActive(false);
        dungeonPrisonerUI.SetActive(false);
        heathSystem.DisplayHealthRoomUI();
        heathSystem.DisplayHealthPrisoners();
        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 400));
        }
        healthPrisonerChooseUI.SetActive(true);
    }

    public void closeHealthPrisonerUI()
    {
        healthPrisonerChooseUI.SetActive(false);
    }

    public void openErosionPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        laboratoryPrisonerChooseUI.SetActive(false);
        smithPrisonerChooseUI.SetActive(false);
        dungeonPrisonerUI.SetActive(false);
        erosionSystem.DisplayErosionPrisoners();
        erosionSystem.DisplayErosionRoomUI();
        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 600));
        }
        erosionPrisonerChooseUI.SetActive(true);
    }

    public void closeErosionPrisonerUI()
    {
        erosionPrisonerChooseUI.SetActive(false);
    }

    public void openLaboratoryPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
        smithPrisonerChooseUI.SetActive(false);
        dungeonPrisonerUI.SetActive(false);
        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 800));
        }
        laboratoryPrisonerChooseUI.SetActive(true);
    }

    public void closeLaboratoryPrisonerUI()
    {
        laboratoryPrisonerChooseUI.SetActive(false);
    }

    public void openSmithPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
        laboratoryPrisonerChooseUI.SetActive(false);
        dungeonPrisonerUI.SetActive(false);
        smithSystem.GenerateHaveWeaponDatas();
        smithSystem.UpdateSmithEnhanceAndUI();
        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 1000));
        }
        smithPrisonerChooseUI.SetActive(true);
    }

    public void closeSmithPrisonerUI()
    {
        smithPrisonerChooseUI.SetActive(false);
    }

    public void openManagerPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
        laboratoryPrisonerChooseUI.SetActive(false);
        smithPrisonerChooseUI.SetActive(false);
        dungeonPrisonerUI.SetActive(false);
        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 1200));
        }
    }

    public void openDungeon1PrisonerUI()
    {
        int index = 0;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();
            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 1400));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void closeDungeonPriosnerUI()
    {
        dungeonPrisonerUI.SetActive(false);
    }

    public void openDungeon2PrisonerUI()
    {
        int index = 1;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();

            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);

            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 1600));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void openDungeon3PrisonerUI()
    {
        int index = 2;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();

            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 1800));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void openDungeon4PrisonerUI()
    {
        int index = 3;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();

            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 2000));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void openDungeon5PrisonerUI()
    {
        int index = 4;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();
            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 2200));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void openDungeon6PrisonerUI()
    {
        int index = 5;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();
            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 2400));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void openDungeon7PrisonerUI()
    {
        int index = 6;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();
            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 2600));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void openDungeon8PrisonerUI()
    {
        int index = 7;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();
            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 2800));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void openDungeon9PrisonerUI()
    {
        int index = 8;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();
            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 3000));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }

    public void openDungeon10PrisonerUI()
    {
        int index = 9;
        if (DDOManager.StageDatas.StageDatas[index].IsOpen == 1)
        {
            floorPrisonerUI.SetActive(false);
            prisonerInfoUI.SetActive(false);
            gymPrisonerChooseUI.SetActive(false);
            healthPrisonerChooseUI.SetActive(false);
            erosionPrisonerChooseUI.SetActive(false);
            laboratoryPrisonerChooseUI.SetActive(false);
            smithPrisonerChooseUI.SetActive(false);
            battleReadySystem.DisplayBattleReadyUnits();
            battleReadySystem.DisplayBattleReadyWeapons();
            battleReadySystem.StageName(index);
            battleReadySystem.StageProgress(index);
            battleReadySystem.setStageIndex(index);
            battleReadySystem.stagemonsterImage(index);
            if (moveCamera != null)
            {
                moveCamera.MoveToUI(new Vector2(0, 3200));
            }
            dungeonPrisonerUI.SetActive(true);
        }
    }
}
