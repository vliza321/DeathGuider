using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusUIManager : MonoBehaviour
{
    public GameObject busPrisonerUI;

    public void OpenBusPrisonerUI()
    {
        busPrisonerUI.SetActive(true);
    }

    public void CloseBusPrisonerUI()
    {
        busPrisonerUI.SetActive(false);
    }
}
