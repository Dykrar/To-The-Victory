using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform

    public List<Vector3> vectorList;

    // Given vector to compare against the list
    public Vector3 givenVector;

    void Start()
    {
        
    }
        

    void Update()
    {

    }

    public void SearchForPlayer()
    {
        //if player position in highlighted tiles
    }

    public Vector3 FindClosestVector()
    {
        // Check if the list is empty
        if (vectorList.Count == 0)
        {
            Debug.LogError("Vector list is empty");
            return Vector3.zero;
        }

        // Initialize closest vector to the first vector in the list
        Vector3 closestVector = vectorList[0];
        float closestDistance = Vector3.Distance(givenVector, closestVector);

        // Loop through the rest of the vectors in the list
        for (int i = 1; i < vectorList.Count; i++)
        {
            Vector3 currentVector = vectorList[i];
            float currentDistance = Vector3.Distance(givenVector, currentVector);

            // Update closest vector if the current vector is closer
            if (currentDistance < closestDistance)
            {
                closestVector = currentVector;
                closestDistance = currentDistance;
            }
        }

        return closestVector;
    }

    private void MoveTowardsPlayer()
    {
        
    }

    private void AttackPlayer()
    {
        // Attack the player
    }

    private void Wander()
    {
       
    }
}
