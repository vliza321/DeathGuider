using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    private float alpha;
    private float vectorAlpha;

    private Vector2 screenSize;
    private bool isFadeOut;

    private RectTransform rt;
    private Image img;

    public float Alpha
    {
        get { return alpha; }
    }
    // Start is called before the first frame update
    void Start()
    {
        rt = this.GetComponent<RectTransform>();
        img = this.gameObject.GetComponent<Image>();
        screenSize = new Vector2(Screen.width, Screen.height);
        alpha = 0.01f;
        vectorAlpha = 0.66f;
        alpha = 0.01f;
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
        isFadeOut = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isFadeOut)
        {
            if (alpha < 0.99f)
            {
                rt.sizeDelta = screenSize;
                img.color = Color.Lerp(Color.clear, Color.black, alpha);
                alpha += vectorAlpha * Time.deltaTime;
            }
        }
    }

    public void StartFadeOut()
    {
        if(isFadeOut)
        {
            alpha = 0.01f;
            isFadeOut = false;
        }
    }
}
