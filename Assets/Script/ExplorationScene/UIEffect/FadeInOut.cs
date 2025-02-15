using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    private float alpha;
    private float vectorAlpha;

    private Vector2 screenSize;
    private bool isFadeOut;

    private RectTransform rt;
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        rt = this.GetComponent<RectTransform>();
        img = this.gameObject.GetComponent<Image>();
        screenSize = new Vector2(Screen.width, Screen.height);
        alpha = 0.01f;
        vectorAlpha = 0.01f;
        StartFadeIn();
        alpha = 0.01f;
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFadeOut)
        {
            if(alpha < 1)
            {
                rt.sizeDelta = screenSize;
                img.color = Color.Lerp(Color.black, Color.clear, alpha);
                alpha += vectorAlpha;
            }
        }
        else
        {
            if(alpha < 1)
            {
                rt.sizeDelta = screenSize;
                img.color = Color.Lerp(Color.clear, Color.black, alpha);
                alpha += vectorAlpha;
            }
        }
    }

    public void StartFadeIn()
    {
        alpha = 0.01f;
        isFadeOut = true;
    }

    public void StartFadeOut()
    {
        alpha = 0.01f;
        isFadeOut = false;
    }
}
