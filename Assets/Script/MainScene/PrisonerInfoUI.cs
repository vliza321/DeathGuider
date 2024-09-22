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

    public void ShowPrisonerInfo(Prisoner prisoner)
    {
        if (nameText != null) nameText.text = prisoner.name;
        if (hpText != null) hpText.text = "HP: " + prisoner.hp.ToString();
        if (proficiencyText != null) proficiencyText.text = "숙련도: " + prisoner.proficiency.ToString();
        if (strengthText != null) strengthText.text = "근력: " + prisoner.strength.ToString();
        if (crimeText != null) crimeText.text = "범죄: " + prisoner.crime;
        if (headImage != null) headImage.sprite = prisoner.head;
        if (bodyImage != null) bodyImage.sprite = prisoner.body;
    }
}