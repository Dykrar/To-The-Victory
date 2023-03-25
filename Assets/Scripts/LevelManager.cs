using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // References to other game components
    public LevelData levelData;
    public LevelDataModifier levelDataModifier;
    public GameWorldGenerator gameWorldGenerator;
    public SpawnManager spawnManager;
    public GameObject fightUI;

    // Flags for game state
    private bool levelIsBeaten = false;
    public bool fightUIEnabled = false;

    private void Start()
    {
        // Initialize the level
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        // Call a method to generate the level based on the LevelData
        GenerateLevel(levelData);

        // Call a method to spawn the players based on the LevelData
        SpawnPlayers(levelData);

        // Spawn collectables on the level
        SpawnCollectables();

        // Subscribe to events from other game components
        Enemy.AllEnemiesDead += OnAllEnemiesDead;
        PlayerMovement.enableFightUI += EnableFightUI;
    }

    private void SpawnCollectables()
    {
        // Call a method in the SpawnManager to spawn collectables on the level
        spawnManager.SpawnCollectables();
    }

    private void GenerateLevel(LevelData levelData)
    {
        // Call a method in the GameWorldGenerator to generate the level based on the LevelData
        gameWorldGenerator.StartGenerating(levelData);     
    }

    private void SpawnPlayers(LevelData levelData)
    {
        // Call a method in the SpawnManager to spawn players based on the LevelData
        spawnManager.Spawn(levelData.nPlayers);
    }

    private void OnAllEnemiesDead(object sender, EventArgs e)
    {
        // Set the levelIsBeaten flag to true
        levelIsBeaten = true;

        // Load the next level
        LoadNextLevel();
    }

    private void EnableFightUI(object sender, EventArgs e)
    {
        // Enable or disable the fight UI based on the fightUIEnabled flag
        if(fightUIEnabled == false) 
        {
            fightUI.SetActive(true);
            fightUIEnabled = true;
        }
        else 
        {
            fightUI.SetActive(false);
            fightUIEnabled = false;
        }
    }

    private void LoadNextLevel()
    {
        // Increment the level number in the LevelData
        levelData.level++;

        // Increase the map width and height by some amount
        levelData.mapWidth += 10;
        levelData.mapHeight += 10;

        // Increase the number of players
        levelData.nPlayers++;

        // Generate a new seed for the level
        levelData.seed = System.DateTime.Now.Millisecond;

        // Load the next level using the modified LevelData
        GenerateLevel(levelData);
        SpawnPlayers(levelData);
    }
}
