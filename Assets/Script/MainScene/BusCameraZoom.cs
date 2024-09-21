using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusCameraZoom : MonoBehaviour
{
    public Button zoomButton;
    public Button closeButton;
    public RectTransform targetUI;
    public Transform targetGround;
    public float scaleDuration = 0.5f;
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);
    public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    public Vector3 groundtargetScale = new Vector3(2.0f, 2.0f, 2.0f);
    public Vector3 groundtargetPosition = new Vector3(0f, 0f, 0f);

    private Vector3 originalScaleUI;
    private Vector3 originalPositionUI;
    private Vector3 originalScaleGround;
    private Vector3 originalPositionGround;
    private bool isScaling = false;

    void Start()
    {
        if (targetUI != null)
        {
            originalScaleUI = targetUI.localScale;
            originalPositionUI = targetUI.localPosition;
        }
        if (targetGround != null)
        {
            originalScaleGround = targetGround.localScale;
            originalPositionGround = targetGround.localPosition;
        }

        zoomButton.onClick.AddListener(OnZoomButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    void OnZoomButtonClick()
    {
        if (!isScaling)
        {
            if (targetUI != null)
            {
                StartCoroutine(ScaleAndMoveUI(targetUI, targetUI.localScale, targetScale, targetUI.localPosition, targetPosition, scaleDuration));
            }
            if (targetGround != null)
            {
                StartCoroutine(ScaleAndMoveObject(targetGround, targetGround.localScale, groundtargetScale, targetGround.localPosition, groundtargetPosition, scaleDuration));
            }
        }
    }

    void OnCloseButtonClick()
    {
        if (!isScaling)
        {
            if (targetUI != null)
            {
                StartCoroutine(ScaleAndMoveUI(targetUI, targetUI.localScale, originalScaleUI, targetUI.localPosition, originalPositionUI, scaleDuration));
            }
            if (targetGround != null)
            {
                StartCoroutine(ScaleAndMoveObject(targetGround, targetGround.localScale, originalScaleGround, targetGround.localPosition, originalPositionGround, scaleDuration));
            }
        }
    }

    private IEnumerator ScaleAndMoveUI(RectTransform uiElement, Vector3 startScale, Vector3 endScale, Vector3 startPos, Vector3 endPos, float duration)
    {
        isScaling = true;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            uiElement.localScale = Vector3.Lerp(startScale, endScale, t);
            uiElement.localPosition = Vector3.Lerp(startPos, endPos, t);
            time += Time.deltaTime;
            yield return null;
        }

        uiElement.localScale = endScale;
        uiElement.localPosition = endPos;
        isScaling = false;
    }

    private IEnumerator ScaleAndMoveObject(Transform obj, Vector3 startScale, Vector3 endScale, Vector3 startPos, Vector3 endPos, float duration)
    {
        isScaling = true;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            obj.localScale = Vector3.Lerp(startScale, endScale, t);
            obj.localPosition = Vector3.Lerp(startPos, endPos, t);
            time += Time.deltaTime;
            yield return null;
        }

        obj.localScale = endScale;
        obj.localPosition = endPos;
        isScaling = false;
    }
}