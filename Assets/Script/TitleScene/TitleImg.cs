using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleImg : MonoBehaviour
{
    public SpriteRenderer BG;
    public float width = Screen.width;
    public float height = Screen.height;
    public void Start()
    {
        float rate = 3840 / 2160;

        float screenRate = width / height;

        float baseScale = this.transform.lossyScale.x;

        if (screenRate > 1)
        {
            float scaleRate;
            // 화면의 가로가 더 길면
            scaleRate = width / 3840;

            this.transform.localScale = new Vector3(baseScale * scaleRate, baseScale * scaleRate, baseScale * scaleRate);
        }

        //화면 가로 비율이 더 길면
        if(screenRate > rate)
        {
            float scaleRate;
            // 화면의 가로가 더 길면
            scaleRate = width / 3840;
            
            this.transform.localScale = new Vector3(scaleRate, scaleRate, 0);
        }
        //화면 세로가 더 길면
        else
        {
            //세로에 맞춰 스케일 변경
            float scaleRate;
            // 화면의 가로가 더 길면
            scaleRate = height / 2160;


            this.transform.localScale = new Vector3(scaleRate, scaleRate, 0);
        }
    }
}
