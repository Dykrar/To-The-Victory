using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Data", menuName = "Map Data")]
public class LevelData : ScriptableObject
{
    public int level; //level number.
    public int mapWidth; // width of the map.
    public int mapHeight; // height of the map.
    public int seed; // seed used to generate the map.
    public int nPlayers; //number of players in the game.
    public bool isBeaten; //whether the level has been beaten by the player.
}
