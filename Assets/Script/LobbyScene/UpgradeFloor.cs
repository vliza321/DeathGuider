using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeFloor : MonoBehaviour
{
    public GameObject floorPrefab; // Ãþ ÇÁ¸®Æé
    public Transform parentTransform; // ÃþÀÇ ºÎ¸ð Æ®·£½ºÆû ¼³Á¤
    public int floorCount = 1; // Ãþ ¼ö

    public void CreateFloor()
    {
        // Ãþ »ý¼º
        GameObject newFloor = Instantiate(floorPrefab, parentTransform);

        // Ãþ ³ôÀÌ °è»ê
        RectTransform rectTransform = newFloor.GetComponent<RectTransform>();
        float FloorHeight = rectTransform.rect.height;

        // Ãþ À§Ä¡ ¼³Á¤
        rectTransform.anchoredPosition = new Vector2(0, floorCount * FloorHeight);

        // Ãþ ÀÌ¸§ ¼³Á¤
        newFloor.name = "Floor " + (floorCount + 1) + "Ãþ";

        // Ãþ ¼ö Áõ°¡
        floorCount++;
    }
}
