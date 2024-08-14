using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoredPrisonerstat : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI proficiencyText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI crimeText;

    public string prisonerName;
    public int hp;
    public int proficiency;
    public int strength;
    public string crime;
    public int erosion;

    public void UpdateUI()
    {
        if (nameText != null) nameText.text = prisonerName;
        if (hpText != null) hpText.text = "HP: " + hp.ToString();
        if (proficiencyText != null) proficiencyText.text = "¼÷·Ãµµ: " + proficiency.ToString();
        if (strengthText != null) strengthText.text = "±Ù·Â: " + strength.ToString();
        if (crimeText != null) crimeText.text = "¹üÁË: " + crime;
    }

}
