using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    // Event handler for enabling player movement
    public static EventHandler enableMov;

    // Enum representing the game states
    public enum GameState { PlayerTurn, EnemyTurn, BossTurn }
    public GameState currentState;

    // Reference to the player and list of enemies
    public GameObject player;
    public List<GameObject> enemies = new List<GameObject>();

    // Index of the current enemy and flag for enabling player movement
    private int currentEnemyIndex;
    private bool movEnabled = false;

    private void Start()
    {
        // Subscribe to the event raised when the player has finished their turn
        PlayerMovement.finishPlayerInput += EndPlayerTurn;

        // Set the initial game state to PlayerTurn
        currentState = GameState.PlayerTurn;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.PlayerTurn:
                if (movEnabled == false)
                {
                    // Invoke the enableMov event if player movement is not already enabled
                    enableMov?.Invoke(this, EventArgs.Empty);
                    movEnabled = true;
                }
                break;
            case GameState.EnemyTurn:
                EndEnemyTurn();
                break;
            case GameState.BossTurn:
                EndBossTurn();
                break;
        }
    }

    public void EndPlayerTurn(object sender, EventArgs e)
    {
        // Set the game state to EnemyTurn and reset the currentEnemyIndex to 0
        currentState = GameState.EnemyTurn;
        currentEnemyIndex = 0;

        // Disable player movement
        movEnabled = false;
    }

    public void EndEnemyTurn()
    {
        // Increment the currentEnemyIndex and check if it's greater than or equal to the number of enemies
        currentEnemyIndex++;
        if (currentEnemyIndex >= enemies.Count)
        {
            // If so, set the game state to BossTurn
            currentState = GameState.BossTurn;
        }
    }

    public void EndBossTurn()
    {
        // Set the game state back to PlayerTurn
        currentState = GameState.PlayerTurn;
    }

    // Coroutine for waiting a certain amount of time before ending the turn
    IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(5f); // wait for 5 seconds        
    }
}
