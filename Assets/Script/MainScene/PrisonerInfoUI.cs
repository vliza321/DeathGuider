using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrisonerInfoUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI proficiencyText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI crimeText;
    public Image headImage;
    public Image bodyImage;

    public Sprite[] headSprites; // HeadID에 해당하는 이미지 배열
    public Sprite[] bodySprites; // BodyID에 해당하는 이미지 배열

    public void ShowPrisonerInfo(PrototypeUnitData prototypeUnitData)
    {
        if (nameText != null) nameText.text = prototypeUnitData.Name;
        if (hpText != null) hpText.text = "HP: " + prototypeUnitData.MaxHealthPoint.ToString();
        if (proficiencyText != null) proficiencyText.text = "숙련도: " + prototypeUnitData.Handicraft.ToString();
        if (strengthText != null) strengthText.text = "근력: " + prototypeUnitData.Strength.ToString();
        if (crimeText != null) crimeText.text = "범죄: " + prototypeUnitData.Crime.ToString();

        if (headImage != null && prototypeUnitData.HeadID >= 0 && prototypeUnitData.HeadID < headSprites.Length)
            headImage.sprite = headSprites[prototypeUnitData.HeadID];

        if (bodyImage != null && prototypeUnitData.BodyID >= 0 && prototypeUnitData.BodyID < bodySprites.Length)
            bodyImage.sprite = bodySprites[prototypeUnitData.BodyID];
    }
}