using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script defines a public enum CollectableType that represents the different types of 
/// collectibles that can be obtained in the game. 
/// The three types defined are HealthPotion, SwordUpgrade, and HpUpgrade.
/// </summary>
public class Collectable : MonoBehaviour
{
    public enum CollectableType { HealthPotion, SwordUpgrade, HpUpgrade };
    public CollectableType type;

    // Constants for upgrade amounts
    private const int HEALTH_INCREASE_AMOUNT = 50;
    private const int SWORD_UPGRADE_AMOUNT = 20;
    private const int HP_UPGRADE_AMOUNT = 50;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply the effect of the collectable to the player
            switch (type)
            {
                case CollectableType.HealthPotion:
                    other.GetComponent<Player>().Heal(HEALTH_INCREASE_AMOUNT);
                    break;
                case CollectableType.SwordUpgrade:
                    Upgrade(other.GetComponent<Player>(), SWORD_UPGRADE_AMOUNT);
                    break;
                case CollectableType.HpUpgrade:
                    Upgrade(other.GetComponent<Player>(), HP_UPGRADE_AMOUNT);
                    break;
            }

            // Destroy the collectable game object
            Destroy(gameObject);
        }
    }

    // Upgrades the player's sword or HP
    private void Upgrade(Player player, int upgradeAmount)
    {
        if (type == CollectableType.SwordUpgrade)
        {
            player.UpgradeSword(upgradeAmount);
        }
        else if (type == CollectableType.HpUpgrade)
        {
            player.UpgradeHp(upgradeAmount);
        }
    }
}
