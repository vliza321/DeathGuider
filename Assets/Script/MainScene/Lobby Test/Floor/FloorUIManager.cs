using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUIManager : MonoBehaviour
{
    public FloorSystem floorSystem;
    public GYMSystem gymSystem;
    public HealthSystem heathSystem;
    public ErosionSystem erosionSystem;
    public MoveCamera moveCamera;

    public GameObject floorPrisonerUI;
    public GameObject prisonerInfoUI;

    public GameObject gymPrisonerChooseUI;
    public GameObject healthPrisonerChooseUI;
    public GameObject erosionPrisonerChooseUI;
    public GameObject smithPrisonerChooseUI;
    public GameObject managerPrisonerChooseUI;
    public GameObject dungeon1PrisonerUI;
    public GameObject dungeon2PrisonerUI;
    public GameObject dungeon3PrisonerUI;
    public GameObject dungeon4PrisonerUI;
    public GameObject dungeon5PrisonerUI;
    public GameObject dungeon6PrisonerUI;
    public GameObject dungeon7PrisonerUI;
    public GameObject dungeon8PrisonerUI;
    public GameObject dungeon9PrisonerUI;
    public GameObject dungeon10PrisonerUI;

    public void OpenFloorPrisonerUI()
    {
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
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
        gymSystem.DisplayGYMTrainingUI();
        gymSystem.DisplayGYMPrisoners();
        gymPrisonerChooseUI.SetActive(true);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 200));
        }
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
        heathSystem.DisplayHealthRoomUI();
        heathSystem.DisplayHealthPrisoners();
        healthPrisonerChooseUI.SetActive(true);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 400));
        }
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
        erosionSystem.DisplayErosionPrisoners();
        erosionSystem.DisplayErosionRoomUI();
        erosionPrisonerChooseUI.SetActive(true);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 600));
        }
    }

    public void closeErosionPrisonerUI()
    {
        erosionPrisonerChooseUI.SetActive(false);
    }

    public void openSmithPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);
        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 800));
        }
    }

    public void openManagerPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 1000));
        }
    }

    public void openDungeon1PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 1200));
        }
    }

    public void openDungeon2PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 1400));
        }
    }

    public void openDungeon3PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 1600));
        }
    }

    public void openDungeon4PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 1800));
        }
    }

    public void openDungeon5PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 2000));
        }
    }

    public void openDungeon6PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 2200));
        }
    }

    public void openDungeon7PrisonerUI()
    {
        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 2400));
        }
    }

    public void openDungeon8PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 2600));
        }
    }

    public void openDungeon9PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 2800));
        }
    }

    public void openDungeon10PrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
        prisonerInfoUI.SetActive(false);
        gymPrisonerChooseUI.SetActive(false);
        healthPrisonerChooseUI.SetActive(false);
        erosionPrisonerChooseUI.SetActive(false);

        if (moveCamera != null)
        {
            moveCamera.MoveToUI(new Vector2(0, 3000));
        }
    }
}
