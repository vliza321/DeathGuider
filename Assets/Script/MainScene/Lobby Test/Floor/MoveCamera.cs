using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MoveCamera : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;

    public float minY = 0;
    public float maxY = 3200;
    public float moveSpeed = 5f;

    private DontDestroyObjectManager DDOManager;
    private GameManager GameManager;
    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        GameObject[] DDO = GameObject.FindObjectsOfType<GameObject>(false);
        foreach (var ddo in DDO)
        {
            if (ddo.CompareTag("DDO") && ddo.name == "DDOManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                DDOManager = ddo.GetComponent<DontDestroyObjectManager>();
            }

            if (ddo.CompareTag("DDO") && ddo.name == "GameManager" && SceneManager.GetActiveScene() != ddo.scene)
            {
                GameManager = ddo.GetComponent<GameManager>();
            }
        }
        DDO = null;

        rectTransform = GetComponent<RectTransform>();
        UpdateMinY();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isMoving) return;

        Vector2 newPosition = rectTransform.anchoredPosition + new Vector2(0, eventData.delta.y);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        rectTransform.anchoredPosition = newPosition;
    }

    public void UpdateMinY()
    {
        if (DDOManager != null)
        {
            minY = -200 * (DDOManager.LocalUserDatas.LocalUserDataDic[GameManager.SelectUserID].Floor -1);
        }
    }

    public void MoveToUI(Vector2 targetPos)
    {
        targetPosition = targetPos;
        StartCoroutine(SmoothMove());
    }

    private IEnumerator SmoothMove()
    {
        isMoving = true;

        while (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) > 0.1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * moveSpeed
            );
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        isMoving = false;
    }
}
