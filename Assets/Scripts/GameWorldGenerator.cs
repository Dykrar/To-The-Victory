using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameWorldGenerator : MonoBehaviour
{ 
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_plains;
    public GameObject prefab_forest;
    public GameObject prefab_mountains;

    public CameraControl cameraControl;

    public LevelData mapData;
    
    int seed;
    int map_width = 30;
    int map_height = 20;
    int tile_id;
    int mountainTileID = 2;
    int plainsTileID = 0;
    int forestTileID = 1;
 
    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();
 
    // recommend 4 to 20
    float magnification = 7.0f;
 
    int x_offset = 0; // <- +>
    int y_offset = 0; // v- +^
 
    public void StartGenerating(LevelData levelData)
    {
        GetMapData(levelData);     
        CreateTileset();
        CreateTileGroups();
        GenerateMap(levelData.seed);  
    }

    private void GetMapData(LevelData levelData)
    {                  
        seed = System.DateTime.Now.Millisecond;
        mapData.seed = seed;
        mapData.mapHeight = map_height;
        mapData.mapWidth = map_width;
    }

    void CreateTileset()
    {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/
 
        tileset = new Dictionary<int, GameObject>();   
        tileset.Add(0, prefab_plains); 
        tileset.Add(1, prefab_forest);
        tileset.Add(2, prefab_mountains);  
    }
 
    void CreateTileGroups()
    {
        /** Create empty gameobjects for grouping tiles of the same type, ie
            forest tiles **/
 
        tile_groups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }
 
    void GenerateMap(int seed)
    {
        /** Generate a 2D grid using the Perlin noise fuction, storing it as
            both raw ID values and tile gameobjects **/
        
        for(int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());
 
            for(int y = 0; y < map_height; y++)
            {
                tile_id = GetIdUsingPerlin(x, y, seed);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }       
    }
 
    int GetIdUsingPerlin(int x, int y, int seed)
    {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalised Perlin value
            to the number of tiles available. **/
        float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset) / magnification + seed,
            (y - y_offset) / magnification + seed
        );

        float clamp_perlin = Mathf.Clamp01(raw_perlin);
        float scaled_perlin = clamp_perlin * tileset.Count;

        if(scaled_perlin == tileset.Count)
        {
            scaled_perlin = (tileset.Count - 1);
        }                  
        
        return Mathf.FloorToInt(scaled_perlin);
    }
 
    void CreateTile(int tile_id, int x, int y)
    {
        /** Creates a new tile using the type id code, group it with common
            tiles, set it's position and store the gameobject. **/
 
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);
 
        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);
 
        tile_grid[x].Add(tile);

    }

    public bool CheckAvailabilityForSpawn(int x, int y, int seed, List <Vector3> positionSpawned)
    {  
       
        tile_id = GetIdUsingPerlin(x, y, seed);      
        if (tile_id == mountainTileID || positionSpawned.Contains(new Vector3(x, y, 0)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    
}

