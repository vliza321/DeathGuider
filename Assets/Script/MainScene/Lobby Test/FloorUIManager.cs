using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUIManager : MonoBehaviour
{
    public GameObject floorPrisonerUI;
    public FloorSystem floorSystem;
    
    public void OpenFloorPrisonerUI()
    {
        floorSystem.DisplayFloorPrisoners();
        floorPrisonerUI.SetActive(true);
    }

    public void CloseFloorPrisonerUI()
    {
        floorPrisonerUI.SetActive(false);
    }
}
