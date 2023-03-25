using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile: MonoBehaviour {
    
    public bool isWalkable; // whether the tile can be walked on or not
    public GameObject highlightObject; // the GameObject that highlights this tile, if any
    public Vector2Int position; // the position of this tile in the grid
    public int cost; // the cost of traversing this tile

    public Tile(bool isWalkable, Vector2Int position) {
        this.isWalkable = isWalkable;
        this.position = position;
    }

    public void SetHighlight(bool isActive) {
        if (highlightObject != null) {
            highlightObject.SetActive(isActive);
        }
    }

    public bool IsWalkable() {
        return isWalkable;
    }

    
}

