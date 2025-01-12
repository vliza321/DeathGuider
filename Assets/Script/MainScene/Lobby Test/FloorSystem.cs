using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorSystem : MonoBehaviour
{
    public GameObject floorPrefab;
    public Transform parentTransform;
    public Button upgradeButton;
    private List<GameObject> floors = new List<GameObject>();

    void Start()
    {
        if (floorPrefab != null)
        {
            CreateFloor(new Vector2(0, -200)); // 첫 번째 층 생성
        }

        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnFloorUpgradeButtonClicked); // 버튼 클릭 이벤트 등록
        }
        else
        {
            Debug.LogWarning("Upgrade 버튼이 설정되지 않았습니다!");
        }
    }

    public void OnFloorUpgradeButtonClicked()
    {
        if (floors.Count > 0)
        {
            // 마지막 층의 위쪽에 새 층을 추가
            GameObject topFloor = floors[floors.Count - 1];
            Vector2 newPosition = new Vector2(
                topFloor.transform.position.x,
                topFloor.transform.position.y + 200 // 기존 층 위로 200만큼 이동
            );

            CreateFloor(newPosition);
        }
    }

    private void CreateFloor(Vector2 position)
    {
        if (floorPrefab != null)
        {
            // 새 층 생성 및 부모 Transform에 추가
            GameObject newFloor = Instantiate(floorPrefab, position, Quaternion.identity, parentTransform);
            floors.Add(newFloor); // 리스트에 추가
        }
        else
        {
            Debug.LogWarning("FloorPrefab이 설정되지 않았습니다!");
        }
    }
}
