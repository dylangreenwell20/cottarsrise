using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth; //max health of enemy

    private float currentHealth; //current health of enemy

    [SerializeField]
    private GameObject healthPanel; //reference to health panel

    [SerializeField]
    private TextMeshProUGUI healthText; //reference to health text

    [SerializeField]
    private RectTransform healthBar; //reference to actual health bar

    private float healthBarStartWidth; //starting width of health bar (at max health)

    private MeshRenderer mR; //mesh renderer of the enemy

    private CapsuleCollider cC; //capsule collider of the enemy

    public bool isDead; //is the health at 0 or not

    private CharacterStats characterStats; //reference to character stats script

    private EnemyLoot enemyLoot; //reference to enemy loot

    private void Start()
    {
        mR = GetComponent<MeshRenderer>(); //get mesh renderer
        cC = GetComponent<CapsuleCollider>(); //get capsule collider

        enemyLoot = GetComponentInParent<EnemyLoot>(); //get enemy loot script

        characterStats = GetComponent<CharacterStats>(); //get character stats
        maxHealth = characterStats.maxHealth; //get character max health
        currentHealth = maxHealth; //current health is max health
        healthBarStartWidth = healthBar.sizeDelta.x; //width of health bar
        UpdateUI(); //update the health bar UI
    }

    public void ApplyDamage(int damage)
    {
        if(isDead == true) //if enemy is dead
        {
            return; //return
        }

        int damageToTake = characterStats.DamageToTake(damage); //calculate damage with armour reduction applied

        currentHealth -= damageToTake; //take away damage from current health

        if(currentHealth <= 0) //if health is less than or equal to 0 after damage applied
        {
            currentHealth = 0; //set health to 0 to avoid negative health
            isDead = true; //enemy is dead
            //Destroy(gameObject); //destroy the enemy gameobject

            enemyLoot.SpawnItem(); //spawn loot item - in the future, a drop chance will be calculated first before spawning loot

            Animator animator = this.transform.parent.GetComponent<Animator>(); //get animator from parent

            animator.SetTrigger("Dead"); //play death animation

            Invoke(nameof(HideEnemy), 1.0f); //hide enemy after a second (when animation finishes)
        }

        UpdateUI(); //update the health bar UI
    }

    private void UpdateUI()
    {
        float percent = (currentHealth / maxHealth) * 100; //calculate the percent of the health bar which should be filled
        float newWidth = (percent / 100) * healthBarStartWidth; //new width of health bar

        healthBar.sizeDelta = new Vector2(newWidth, healthBar.sizeDelta.y); //calculate size of health bar
        healthText.text = currentHealth + "/" + maxHealth; //change health bar number
    }

    private void HideEnemy()
    {
        this.gameObject.SetActive(false); //hide enemy capsule
    }
}
