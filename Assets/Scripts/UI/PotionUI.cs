using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.Examples.ObjectSpin;

public class PotionUI : MonoBehaviour
{
    public TextMeshProUGUI healthPotionText; //health potion ui text
    public TextMeshProUGUI manaPotionText; //mana potion ui text

    private void Start()
    {
        UpdateHealthPotionUI(); //update health potion ui
        UpdateManaPotionUI(); //update mana potion ui
    }

    public void UpdateHealthPotionUI()
    {
        healthPotionText.text = Inventory.instance.healthPotCount.ToString(); //update ui with number of health pots
    }

    public void UpdateManaPotionUI()
    {
        manaPotionText.text = Inventory.instance.manaPotCount.ToString(); //update ui with number of mana pots
    }
}