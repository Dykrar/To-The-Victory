using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataModifier : MonoBehaviour
{
    public LevelData levelData;
    public int widthIncrement;
    public int heightIncrement;
    public int playerIncrement;

    public void ModifyLevelData()
    {
        levelData.mapWidth += widthIncrement;
        levelData.mapHeight += heightIncrement;
        levelData.nPlayers += playerIncrement;
    }
}
