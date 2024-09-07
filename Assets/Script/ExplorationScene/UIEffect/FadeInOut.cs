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

    private IEnumerator m_FadeOut;
    private IEnumerator m_FadeIn;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 0.01f;
        vectorAlpha = 0.01f;
        m_FadeOut = FadeOut();
        m_FadeIn = FadeIn();
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFadeIn()
    {
        StartCoroutine(m_FadeIn);
    }

    public void StartFadeOut()
    {
        StartCoroutine(m_FadeOut);
        
    }

    IEnumerator FadeOut()
    {
        
        alpha = 0.01f;
        while (alpha < 1)
        {
            this.gameObject.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.black, alpha);
            alpha += vectorAlpha;
            yield return new WaitForSeconds(vectorAlpha);
        }
        SceneManager.LoadScene("Result");
        yield return true;
    }

    IEnumerator FadeIn()
    {
        
        alpha = 0.01f;
        while(alpha < 1)
        {
            this.gameObject.GetComponent<Image>().color = Color.Lerp(Color.black, Color.clear, alpha);
            alpha += vectorAlpha;
            yield return new WaitForSeconds(vectorAlpha);
        }
        yield return true ;
    }
}
