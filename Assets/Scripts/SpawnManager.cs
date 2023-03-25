using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{    
    // Reference to the prefabs to be spawned
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public GameObject healthPotionPrefab;
    public GameObject damageBoosterPrefab;
    public GameObject swordUpgradePrefab;
    public GameObject hpUpgradePrefab;
    
    // Reference to level data and the game world generator
    public LevelData mapData;
    public GameWorldGenerator gameWorldGenerator;

    // List to keep track of positions where objects have already been spawned
    public List<Vector3> positionSpawned = new List<Vector3>();

    // Used to alternate spawning of SwordUpgrade and HpUpgrade
    private bool swordOrHpUpgrade = false;

    // Method to spawn objects at the start of the game
    public void Spawn (int nPlayers)
    {        
        int map_width = mapData.mapWidth;
        int map_height = mapData.mapHeight;
        int map_seed = mapData.seed;
        
        // Spawn player at random corner
        int corner = Random.Range(0, 4); // 0 = bottom-left, 1 = bottom-right, 2 = top-right, 3 = top-left
        int x = 0, y = 0;
        
        bool positionFound = false;

        while (!positionFound)
        {
            if (corner == 0) // bottom-left corner
            {
                x = Random.Range(0, 10);
                y = Random.Range(0, 10);
            }
            else if (corner == 1) // bottom-right corner
            {
                x = Random.Range(map_width - 10, map_width);
                y = Random.Range(0, 10);
            }
            else if (corner == 2) // top-right corner
            {
                x = Random.Range(map_width - 10, map_width);
                y = Random.Range(map_height - 10, map_height);
            }
            else // top-left corner
            {
                x = Random.Range(0, 10);
                y = Random.Range(map_height - 10, map_height);
            }
            positionFound = gameWorldGenerator.CheckAvailabilityForSpawn(x, y, map_seed, positionSpawned);
        }
        GameObject player = Instantiate(playerPrefab, new Vector3(x, y, 0), Quaternion.identity);

        // Spawn enemies at random locations
        int num_enemies = nPlayers;
        for (int i = 0; i < num_enemies; i++)
        {
            Vector3 enemyPosition = GetRandomPosition(map_width, map_height, map_seed);
            positionSpawned.Add(enemyPosition);

            GameObject enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

            // Update the enemy's stats based on the current level
            Enemy enemyClass = enemy.GetComponent<Enemy>();
            if (enemyClass != null)
            {
                enemyClass.UpdateStats(mapData.level);
            }

            // Add the enemy to the TurnManager's list of enemies
            TurnManager turnManager = FindObjectOfType<TurnManager>();
            turnManager.enemies.Add(enemy);
        }

        // Spawn boss at random location
        Vector3 position = GetRandomPosition(map_width, map_height, map_seed);
        positionSpawned.Add(position);
        GameObject boss = Instantiate(bossPrefab, position, Quaternion.identity);
    }  

    // Method to spawn collectibles during the game
    public void SpawnCollectables()
    {
        int map_width = mapData.mapWidth;
        int map_height = mapData.mapHeight;
        int map_seed = mapData.seed;
   

        // Spawn SwordUpgrade or HpUpgrade every other round
        if (mapData.level % 2 == 0) // if the level is divisible by 2 (even)
        {
            if (swordOrHpUpgrade) // if swordOrHpUpgrade is true
            {
                Instantiate(swordUpgradePrefab, GetRandomPosition(map_width, map_height, map_seed), Quaternion.identity); // spawn swordUpgradePrefab
            }
            else // if swordOrHpUpgrade is false
            {
                Instantiate(hpUpgradePrefab, GetRandomPosition(map_width, map_height, map_seed), Quaternion.identity); // spawn hpUpgradePrefab
            }

            swordOrHpUpgrade = !swordOrHpUpgrade; // toggle swordOrHpUpgrade between true and false
        }      
    } 

    Vector3 GetRandomPosition(int map_width , int map_height, int map_seed)
    {
        bool positionFound = false;        
        int x = 0, y = 0;
        
        while (!positionFound)
        {
         // Generate random x and y coordinates within the bounds of the game world
        x = Random.Range(0, map_width);
        y = Random.Range(0, map_height);

        // Check if the generated position is already occupied by another game object
        positionFound = gameWorldGenerator.CheckAvailabilityForSpawn(x, y, map_seed, positionSpawned);
    }

    // Return the randomly generated position
    return new Vector3(x,y,0);       
            
    }

}
