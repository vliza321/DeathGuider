using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusButton : MonoBehaviour
{
    public Button busButton;
    public GameObject floorCanvas;
    public GameObject mainCanvas;
    public GameObject busPrisoner;

    void Start()
    {
        // 버튼에 리스너 추가
        busButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Floor Canvas 비활성화
        if (floorCanvas != null)
        {
            floorCanvas.SetActive(false);
        }

        // Main Canvas 비활성화
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
        }

        // Bus Canvas 활성화
        if (busPrisoner != null)
        {
            busPrisoner.SetActive(true);
        }
    }

    public void CloserButtonClick()
    {
        // Floor Canvas 활성화
        if (floorCanvas != null)
        {
            floorCanvas.SetActive(true);
        }

        // Main Canvas 활성화
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(true);
        }

        // Bus Canvas 비활성화
        if (busPrisoner != null)
        {
            busPrisoner.SetActive(false);
        }
    }
}