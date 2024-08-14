using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowUI : MonoBehaviour
{
    public GameObject uiPrefab; // 프리펩을 여기에 드래그하여 할당

    public void ShowUI2()
    {
        if (uiPrefab != null)
        {
            GameObject uiInstance = Instantiate(uiPrefab);
            uiInstance.SetActive(true);
        }
    }
}
