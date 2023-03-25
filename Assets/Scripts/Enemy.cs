using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines the  behavior and stats of an enemy in the game.
/// </summary>
public class Enemy : MonoBehaviour
{
    // Constant fields that define initial values and incremental values for health points (HP) and damage points (dmg)
    private const int STARTING_HP = 10;
    private const int STARTING_DMG = 10;
    private const int LEVEL_HP_INC = 2;
    private const int LEVEL_DMG_INC = 1;
    private const int BOSS_HP_MULT = 2;
    private const int BOSS_DMG_MULT = 2;

    // Fields that store the enemy's current HP, dmg, and max HP, as well as whether the enemy is a boss or not
    private int _currentHP;
    private int _dmg;
    private int _maxHP;
    private bool _isBoss;

    // Reference to a LevelData scriptable object that holds the current game level
    public LevelData levelData;

    // Static field that keeps track of the number of enemies in the scene
    public static int enemyCount = 0;

    // Event that is triggered when all enemies in the scene are dead
    public static EventHandler AllEnemiesDead;

    // Called when the enemy object is created
    private void Awake()
    {
        enemyCount++;
    }

    // Called when the enemy object is initialized
    private void Start()
    {
        _isBoss = boss;
        UpdateStats(levelData.level);
    }

    // Called when the enemy object is destroyed
    private void OnDestroy()
    {
        enemyCount--;

        // Triggers the AllEnemiesDead event if there are no more enemies in the scene
        if (enemyCount <= 0)
        {
            AllEnemiesDead?.Invoke(this, EventArgs.Empty);
        }
    }

    // Coroutine that reduces the enemy's HP by the specified amount over time
    public IEnumerator ReceiveDamage(int amount)
    {
        yield return new WaitForSeconds(3f);
        _currentHP -= amount;

        // Calls the Die() method if the enemy's HP reaches 0 or less
        if (_currentHP <= 0)
        {
            Die();
        }
    }

    // Method that handles the death of the enemy
    private void Die()
    {
        // Code to handle enemy death, e.g. dropping items or triggering animations
        Destroy(gameObject);
    }

    // Method that updates the enemy's stats (HP and dmg) based on the current level
    public void UpdateStats(int level)
    {
        if (_isBoss)
        {
            // For a boss enemy, the max HP and dmg are higher and are multiplied by a constant value
            _maxHP = (STARTING_HP + (BOSS_HP_MULT * level)) * 2;
            _dmg = (STARTING_DMG + (BOSS_DMG_MULT * level)) * 2;
        }
        else
        {
            // For a regular enemy, the max HP and dmg are incremented by constant values
            _maxHP = STARTING_HP + (LEVEL_HP_INC * level);
            _dmg = STARTING_DMG + (LEVEL_DMG_INC * level);
        }

        // Sets the current HP to the updated max HP
        _currentHP = _maxHP;
    }

    // Properties that allow access to the private fields
    public int currentHP
    {
        get { return _currentHP; }
        private set { _currentHP = value; }
    }

    public int dmg
    {
        get { return _dmg; }
        private set { _dmg = value; }
    }

    public int maxHP
    {
        get { return _maxHP; }
       private set { _maxHP = value; }
    }

    public bool boss
    {
        get { return _isBoss; }
    }
}




