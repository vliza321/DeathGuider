using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUIManager : MonoBehaviour
{
    public GameObject floorPrisonerUI;
    public FloorSystem floorSystem;
    public GameObject prisonerInfoUI;

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

    // 새로운 함수로 Floor Prisoner Info UI 비활성화
    public void closeFloorPrisonerInfoUI()
    {
        prisonerInfoUI.SetActive(false);     // 죄수 정보 UI 비활성화
    }

    public GameObject GetPrisonerInfoUI()
    {
        return prisonerInfoUI;  // 죄수 정보 UI를 반환
    }
}
