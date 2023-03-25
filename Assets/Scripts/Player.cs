using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Declare public variables
    public int maxHP = 100; // starting health points
    public int currentHP;
    public int dmg = 30; // damage points

    public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize currentHP to maxHP and store starting position
        currentHP = maxHP;
        position = transform.position;
    }

    // Function to subtract damage from current HP
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        // Check if current HP is less than or equal to 0
        if (currentHP <= 0)
        {
            Die(); // Call the Die function if HP is 0 or less
        }
    }

    // Function to handle death
    private void Die()
    {
        // Code to handle death
    }

    // Function to deal damage to another player
    public void DealDamage(Player target)
    {
        target.TakeDamage(dmg);
    }

    // Function to heal player
    internal void Heal(int healedHP)
    {
        currentHP += healedHP;
        // Check if current HP is greater than max HP
        if (currentHP > maxHP) currentHP = maxHP;
    }

    // Function to upgrade sword damage
    internal void UpgradeSword(int extraDmg)
    {
        dmg += extraDmg;
    }

    // Function to upgrade max HP
    internal void UpgradeHp(int extraHP)
    {
        maxHP += extraHP;
    }

}
