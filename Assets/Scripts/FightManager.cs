using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class FightManager : MonoBehaviour
{
    // Constants used to identify UI components
    private const string SLIDER = "Slider";
    private const string HP = "HP";
    private const string ATK = "ATK";

    // References to UI game objects
    public GameObject playerUI;
    public GameObject enemyUI;
    public GameObject playerTransformSlider;
    public GameObject playerTransformHP ;
    public GameObject playerTransformDMG;
    public GameObject enemyTransformSlider; 
    public GameObject enemyTransformHP; 
    public GameObject enemyTransformDMG;
    
    // References to UI components
    Slider playerSlider;
    Slider enemySlider;
    TextMeshProUGUI playerHP;
    TextMeshProUGUI playerATK;
    TextMeshProUGUI enemyHP;
    TextMeshProUGUI enemyATK;

    // References to player and enemy scripts
    Player playerScript;
    Enemy enemyScript;
    
    // Coroutine that opens the fight menu
    public IEnumerator OpenFightMenu(GameObject player,GameObject enemy, bool isPlayer)
    {
        // Associate the scripts
        AssociateScripts(player,enemy);
        
        // Wait for a short delay before starting the fight
        yield return new WaitForSeconds(1.5f);

        // Determine who attacks first and start the fight accordingly
        if(isPlayer)
        {
            PlayerAttacks(player, enemy);
        }
        else 
        {
            EnemyAttacks(player, enemy);
        }
    }

    // Player attacks the enemy
    public void PlayerAttacks(GameObject player,GameObject enemy)
    {
        // The enemy takes damage
        StartCoroutine(enemyScript.ReceiveDamage(playerScript.dmg));
        // Update the UI
        DefineUIValues();  
    }

    // Enemy attacks the player
    public void EnemyAttacks(GameObject player,GameObject enemy)
    {
        // The player takes damage
        playerScript.TakeDamage(enemyScript.dmg);
        // Update the UI
        DefineUIValues();    
    }

    // Associates the scripts for the player and enemy
    public void AssociateScripts(GameObject player,GameObject enemy)
    {
        playerScript = player.GetComponent<Player>();
        enemyScript = enemy.GetComponent<Enemy>();
        DefineUIValues();
    }

    // Updates the values displayed in the UI
    public void DefineUIValues()
    {        
        // Get references to the UI components
        playerSlider = playerTransformSlider.GetComponent<Slider>();
        enemySlider = enemyTransformSlider.GetComponent<Slider>();
        playerATK = playerTransformDMG.GetComponent<TextMeshProUGUI>();
        enemyATK = enemyTransformDMG.GetComponent<TextMeshProUGUI>();
        playerHP = playerTransformHP.GetComponent<TextMeshProUGUI>();
        enemyHP = enemyTransformHP.GetComponent<TextMeshProUGUI>();        

        // Set up the player slider
        playerSlider.minValue = 0;
        playerSlider.maxValue = playerScript.maxHP;
        playerSlider.value = playerScript.currentHP;

        // Set up the enemy slider
        enemySlider.minValue = 0;
        enemySlider.maxValue = enemyScript.maxHP;
        enemySlider.value = enemyScript.currentHP;
        
        // Set up the player UI text
        playerHP.text = "HP: " + playerScript.currentHP.ToString();
        playerATK.text = "ATK: " + playerScript.dmg.ToString();

        // Set up the enemy UI text
        enemyHP.text =  "HP: " + enemyScript.currentHP.ToString();
        enemyATK.text = "ATK: " + enemyScript.dmg.ToString();
    }
}

