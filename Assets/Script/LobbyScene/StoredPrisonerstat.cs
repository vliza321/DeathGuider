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
<<<<<<< HEAD
    public TextMeshProUGUI erosionText;
    public Image headImage;
    public Image bodyImage;
=======
>>>>>>> parent of 1d91ec0 (Revert "TitleScene, Loading")

    public string prisonerName;
    public int hp;
    public int proficiency;
    public int strength;
    public string crime;
    public int erosion;

    public void SetPrisonerData(string name, int hp, int proficiency, int strength, string crime, int erosion, Sprite headSprite, Sprite bodySprite)
    {
        nameText.text = name;
        hpText.text = "HP: " + hp;
        proficiencyText.text = "¼÷·Ãµµ: " + proficiency;
        strengthText.text = "±Ù·Â: " + strength;
        crimeText.text = "¹üÁË: " + crime;
        erosionText.text = "Ä§½Ä: " + erosion; // Erosion ¾÷µ¥ÀÌÆ®
        headImage.sprite = headSprite;
        bodyImage.sprite = bodySprite;
    }

    public void UpdateUI()
    {
        if (nameText != null) nameText.text = prisonerName;
        if (hpText != null) hpText.text = "HP: " + hp.ToString();
        if (proficiencyText != null) proficiencyText.text = "¼÷·Ãµµ: " + proficiency.ToString();
        if (strengthText != null) strengthText.text = "±Ù·Â: " + strength.ToString();
        if (crimeText != null) crimeText.text = "¹üÁË: " + crime;
    }

}
