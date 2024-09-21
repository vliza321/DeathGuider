using System;
using UnityEngine;
using UnityEngine.UI;

public class DateSystem : MonoBehaviour
{
    public event Action OnDateChanged;

    private DateTime currentDate;
    public Text dateText;  // 날짜를 표시할 텍스트

    void Start()
    {
        // 초기 날짜를 2024년 1월 1일로 설정
        currentDate = new DateTime(2024, 1, 1);
        UpdateDateText();
    }

    // 날짜를 변경하는 메서드 (버튼 클릭 시 호출됨)
    public void ChangeDate()
    {
        currentDate = currentDate.AddDays(1);
        UpdateDateText();
        OnDateChanged?.Invoke();
    }

    // 현재 날짜를 반환하는 메서드
    public DateTime GetCurrentDate()
    {
        return currentDate;
    }

    // 날짜를 텍스트에 표시하는 메서드
    private void UpdateDateText()
    {
        if (dateText != null)
        {
            dateText.text = currentDate.ToString("yyyy-MM-dd");
        }
    }
}
