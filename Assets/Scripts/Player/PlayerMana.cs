using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMana : MonoBehaviour
{
    [SerializeField]
    private float maxMana; //max mana of player

    private float currentMana; //current mana of player

    [SerializeField]
    private GameObject manaPanel; //reference to mana panel

    [SerializeField]
    private TextMeshProUGUI manaText; //reference to mana text

    [SerializeField]
    private RectTransform manaBar; //reference to actual mana bar

    private float manaBarStartWidth; //starting width of mana bar (at max mana)

    private void Start()
    {
        currentMana = maxMana; //set current mana to max mana
        manaBarStartWidth = manaBar.sizeDelta.x; //starting size of the mana bar
    }

    public void LoseMana()
    {
        //on staff attack, lose some mana
    }
}
