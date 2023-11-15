using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMana : MonoBehaviour
{
    [SerializeField]
    private float maxMana; //max mana of player

    public float currentMana; //current mana of player

    [SerializeField]
    private GameObject manaPanel; //reference to mana panel

    [SerializeField]
    private TextMeshProUGUI manaText; //reference to mana text

    [SerializeField]
    private RectTransform manaBar; //reference to actual mana bar

    private float manaBarStartWidth; //starting width of mana bar (at max mana)

    private bool isDead; //if player is currently alive or dead

    private bool hasRecentlyLostMana; //has the player recently lost mana (this is used after the player has taken mana damage)
    private bool canRegenAgain; //can the player regenerate mana again (this is used between passive regen ticks)
    private bool atMaxMana; //if the player is at max mana or not

    private float manaRegenAmount = 10; //amount of mana to  - 10 mana per tick currently
    private float restartManaRegenCD = 2f; //cooldown for restarting the passive mana regen after taking damage

    private Coroutine manaRegen; //variable to check if a coroutine for regen is already running
    private WaitForSeconds manaRegenTick = new WaitForSeconds(1); //cooldown used during passive mana regeneration (during regeneration ticks)


    private void Start()
    {
        currentMana = maxMana; //set current mana to max mana
        manaBarStartWidth = manaBar.sizeDelta.x; //starting size of the mana bar
        atMaxMana = true; //player will always start with max mana
        UpdateUI(); //update the ui
    }

    public void LoseMana(float manaDamage)
    {
        isDead = gameObject.GetComponent<PlayerHealth>().isDead; //check if the player is dead or alive

        if(isDead ) //if player is dead
        {
            return; //return function
        }

        currentMana -= manaDamage; //take away mana damage from total mana amount
        atMaxMana = false; //if the player has lost mana then they cannot be at the max mana amount

        if(manaRegen != null ) //if a mana regen coroutine is already created
        {
            StopCoroutine(manaRegen); //stop the regen coroutine to stop regenerating mana
        }

        manaRegen = StartCoroutine(RegenerateMana(restartManaRegenCD)); //start a coroutine to regenerate mana after 2 seconds of not attacking

        UpdateUI(); //update the ui
    }

    public void UpdateUI()
    {
        float percent = (currentMana / maxMana) * 100; //calculate percent of mana left
        float newWidth = (percent / 100) * manaBarStartWidth; //new width for the blue mana bar

        manaBar.sizeDelta = new Vector2(newWidth, manaBar.sizeDelta.y); //calculate width of the amount of mana left in the bar
        manaText.text = currentMana + "/" + maxMana; //update ui text to show mana value
    }

    private IEnumerator RegenerateMana(float cd)
    {
        yield return new WaitForSeconds(cd); //wait for a cooldown before starting mana regeneration

        while (!atMaxMana) //while player is not at max mana
        {
            currentMana += manaRegenAmount; //add mana regen amount to current mana

            if (currentMana > maxMana) //if current mana is greater than max mana
            {
                currentMana = maxMana; //set current mana to max mana value
                atMaxMana = true; //player is at max mana
            }

            UpdateUI(); //update the ui
            yield return manaRegenTick; //wait for a short cooldown between regen ticks
        }
        manaRegen = null; //stop mana regen coroutine
    }
}
