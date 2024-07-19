using UnityEngine;
using UnityEngine.UI;

public class PrisonerDataUI : MonoBehaviour
{
    public GameObject prisonerUIImage; // 이미지 객체를 연결할 변수

    void Start()
    {
        // 이미지 객체를 초반 비공개
        if (prisonerUIImage != null)
        {
            prisonerUIImage.SetActive(false);
        }
    }

    public void showPrisonerUI()
    {
        // 이미지 객체의 활성화 상태를 설정
        prisonerUIImage.SetActive(true);
    }

    public void dasfdshowPrisonerUI()
    {
        // 이미지 객체의 활성화 상태를 설정
        prisonerUIImage.SetActive(false);
    }
}