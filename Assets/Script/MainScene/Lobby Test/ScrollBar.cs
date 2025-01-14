using UnityEngine;
using UnityEngine.UI;

public class ScrollBar : MonoBehaviour
{
    public ScrollRect scrollRect;

    void Start()
    {
        // ScrollRect의 verticalScrollbar 속성 강제로 설정
        if (scrollRect != null)
        {
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        }
    }
}
