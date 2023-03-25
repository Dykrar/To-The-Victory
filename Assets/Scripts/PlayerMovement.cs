using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private const string LEVELMANAGER = "LevelManager";
    [SerializeField]private LevelData mapData;
    [SerializeField]private float maxMoveDistance = 5f; // maximum number of tiles the player can move   
    private int xPosition;
    private int yPosition;
    private Vector2Int position;
    private List<Vector2> highlightedTiles;
    private bool highlighted = false;
    private bool receivePlayerInput = false;
    private bool canMove = false;
    public static EventHandler finishPlayerInput;
    public static EventHandler enableFightUI;  

    public FightManager fightManager;     
    public void Start()    
    {
        TurnManager.enableMov += EnableMovement;
        xPosition = (int)transform.position.x;
        yPosition = (int)transform.position.y;
        position = new Vector2Int(xPosition, yPosition);

        highlightedTiles = new List<Vector2>();

        GameObject levelManager = GameObject.Find(LEVELMANAGER);
        
        fightManager = levelManager.GetComponent<FightManager>();
    }

    void OnDestroy()
    {
        TurnManager.enableMov -= EnableMovement;
    }
    private void EnableMovement(object sender, EventArgs e)
    {
        receivePlayerInput = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && receivePlayerInput == true)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int playerLayer = LayerMask.GetMask("Player");
            int enemyLayer = LayerMask.GetMask("Enemy");            
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, playerLayer);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {                
              StartCoroutine(HighlightTiles());                
            }    
            
            if(highlighted && canMove)
            {
                RaycastHit2D hitEnemy = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, enemyLayer);

                Vector2Int clickedTile = new Vector2Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y)); 
                if (highlightedTiles.Contains(clickedTile) && clickedTile != position)
                {               
                    if (hitEnemy.collider != null)
                    {                
                        StartCoroutine(MoveAndFight(clickedTile));                
                    } 
                    else
                    {
                        MoveToTile(clickedTile);
                    }
                }   
            }
        }      
    }

    IEnumerator MoveAndFight(Vector2Int clickedTile)
    {
        GameObject enemyGameObject = null;
        Collider2D collider = Physics2D.OverlapPoint(clickedTile);
        if (collider != null)
        {
            enemyGameObject = collider.gameObject;
            Debug.Log("Found game object: " + enemyGameObject.name);
        }

        float randomCoord = UnityEngine.Random.Range(-0.5f, 0.5f);

        while (!highlightedTiles.Contains(clickedTile))
            {
                string border = UnityEngine.Random.Range(0, 4) switch {
                    0 => "top",
                    1 => "bottom",
                    2 => "left",
                    _ => "right",
                };

                randomCoord = UnityEngine.Random.Range(-0.5f, 0.5f);
                switch (border)
                {
                    case "top":
                        clickedTile = new Vector2Int(clickedTile.x + (int)Mathf.Round(randomCoord), clickedTile.y - 1);
                        break;
                    case "bottom":
                        clickedTile = new Vector2Int(clickedTile.x + (int)Mathf.Round(randomCoord), clickedTile.y + 1);
                        break;
                    case "left":
                        clickedTile = new Vector2Int(clickedTile.x - 1, clickedTile.y + (int)Mathf.Round(randomCoord));
                        break;
                    default:  // "right"
                        clickedTile = new Vector2Int(clickedTile.x + 1, clickedTile.y + (int)Mathf.Round(randomCoord));
                        break;
                }
                yield return null;
            }

        enableFightUI?.Invoke(this, EventArgs.Empty); 

        StartCoroutine(fightManager.OpenFightMenu(gameObject,enemyGameObject, true));

        ClearHighlightedTiles();     

        receivePlayerInput = false;
        finishPlayerInput?.Invoke(this, EventArgs.Empty);      
    }

    IEnumerator HighlightTiles() 
    { 
        // Populate highlightedTiles
        PopulateHighlightedTiles();

        // Iterate through all the tiles in the range and highlight them if they are valid
        Vector2Int minPos = position - new Vector2Int((int)maxMoveDistance, (int)maxMoveDistance);
        Vector2Int maxPos = position + new Vector2Int((int)maxMoveDistance, (int)maxMoveDistance);

        for (int x = minPos.x; x <= maxPos.x; x++) {
            for (int y = minPos.y; y <= maxPos.y; y++) {
                if (highlightedTiles.Contains(new Vector2Int(x, y))) {
                    SetHighlight(x, y, true);
                    yield return null;
                }
            }
        }
        
        highlighted = true;
        canMove = true;
    }

    void PopulateHighlightedTiles()
    {
        // calculate the range of tiles that can be highlighted
        Vector2Int minPos = position - new Vector2Int((int)maxMoveDistance, (int)maxMoveDistance);
        Vector2Int maxPos = position + new Vector2Int((int)maxMoveDistance, (int)maxMoveDistance);

        List<Vector2> availableTiles = new List<Vector2>();

        for(int y = minPos.y; y<= maxPos.y; y++)
        {    
            if(y < yPosition)
            {
                for(int n=0; n< maxMoveDistance; n++)
                {
                    for(int i=0; i<=n; i++)
                    {                        
                        for(int j=-i; j<=i; j++)
                        {
                            if( i == n)
                            {
                                Vector2 tile = position - new Vector2(j, (int)maxMoveDistance-i);
                                if(tile.x >= 0 && tile.x < mapData.mapWidth && tile.y >= 0)
                                {
                                    availableTiles.Add(tile);   
                                }
                            }
                        }
                    }
                }
            }
            if(y > yPosition)
            {
                for(int n = (int)maxMoveDistance; n >= 0; n--)
                {
                    for(int i=n; i>=0; i--)
                    {
                        for(int j=-i; j<=i; j++)
                        {
                            if( i == n)
                            {
                                Vector2 tile = position + new Vector2(j, (int)maxMoveDistance-i);
                                if(tile.x >= 0 && tile.x < mapData.mapWidth && tile.y < mapData.mapHeight)
                                {
                                    availableTiles.Add(tile);   
                                }
                            }
                        }
                    }
                }
            }  
        }

        highlightedTiles.Clear();
        highlightedTiles.AddRange(availableTiles);
    }

    void SetHighlight(int x, int y, bool active)
    {
        string tilesInRangeName = string.Format("tile_x{0}_y{1}", x, y);
        GameObject currentTileInRange = GameObject.Find(tilesInRangeName);
        Tile tile = currentTileInRange.GetComponent<Tile>();

        if (tile != null && tile.isWalkable) 
        {
            // highlight the tile
            tile.SetHighlight(active);
        }
    }    

    public void MoveToTile(Vector2 destination)
    {
        // Check that the destination is a valid tile
        if (highlightedTiles.Contains(destination))
        {
            // Get the tile game object at the destination
            string tileName = string.Format("tile_x{0}_y{1}", destination.x, destination.y);
            GameObject destinationTile = GameObject.Find(tileName);

            // Check that the destination tile is walkable
            Tile tile = destinationTile.GetComponent<Tile>();
            if (tile.isWalkable)
            {
                // Move the player to the destination
                transform.position = new Vector2(destinationTile.transform.position.x, destinationTile.transform.position.y);
                position = new Vector2Int((int)destination.x, (int)destination.y);

                // Clear highlighted tiles
                ClearHighlightedTiles();
                // Do any additional logic for when the player moves to a new tile
                // ...
                receivePlayerInput = false;
                finishPlayerInput?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void ClearHighlightedTiles()
    {
        // Clear highlighted tiles
        foreach (var tile in highlightedTiles)
        {
           SetHighlight((int)tile.x, (int)tile.y, false);
        }

        highlightedTiles.Clear();
        highlighted = false;
    }
}
