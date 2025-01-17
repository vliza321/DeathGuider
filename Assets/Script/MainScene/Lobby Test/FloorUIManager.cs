using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUIManager : MonoBehaviour
{
    public FloorSystem floorSystem;
    public GYMSystem gymSystem;

    public GameObject floorPrisonerUI;
    public GameObject prisonerInfoUI;

    public GameObject gymPrisonerChooseUI;
    
    public void OpenFloorPrisonerUI()
    {
        floorSystem.DisplayFloorPrisoners();
        floorPrisonerUI.SetActive(true);
    }

    public void CloseFloorPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
    }

    public void openFloorPrisonerInfoUI(UnitData prisoner)
    {
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
        gymSystem.DisplayGYMTrainingUI();
        gymSystem.DisplayGYMPrisoners();
        gymPrisonerChooseUI.SetActive(true);
    }
    public void closeGymPrisonerUI()
    {
        gymPrisonerChooseUI.SetActive(false);
    }
}
