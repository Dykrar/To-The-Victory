using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {
    public static List<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int endPosition, Tile[,] tiles) {
        List<Vector2Int> path = new List<Vector2Int>();
        List<Vector2Int> openList = new List<Vector2Int>();
        List<Vector2Int> closedList = new List<Vector2Int>();
        openList.Add(startPosition);

        while (openList.Count > 0) {
            Vector2Int current = openList[0];
            openList.RemoveAt(0);
            closedList.Add(current);

            if (current == endPosition) {
                // Found path
                path.Add(current);
                while (current != startPosition) {
                    path.Insert(0, current);
                }
                return path;
            }
            /*
            List<Vector2Int> neighbors = GetNeighbors(current, tiles);
            foreach (Vector2Int neighbor in neighbors) {
                if (closedList.Contains(neighbor)) {
                    continue;
                }

                int newG = tiles[current.x, current.y].gCost + GetDistance(current, neighbor);
                if (newG < tiles[neighbor.x, neighbor.y].gCost || !openList.Contains(neighbor)) {
                    tiles[neighbor.x, neighbor.y].gCost = newG;
                    tiles[neighbor.x, neighbor.y].hCost = GetDistance(neighbor, endPosition);
                    tiles[neighbor.x, neighbor.y].parent = current;

                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                    }
                }
            }*/
        }

        // No path found
        return path;
    }

    private static List<Vector2Int> GetNeighbors(Vector2Int position, Tile[,] tiles) {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        int minX = Mathf.Max(0, position.x - 1);
        int maxX = Mathf.Min(tiles.GetLength(0) - 1, position.x + 1);
        int minY = Mathf.Max(0, position.y - 1);
        int maxY = Mathf.Min(tiles.GetLength(1) - 1, position.y + 1);

        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                if (x == position.x && y == position.y) {
                    continue;
                }
                if (tiles[x, y].IsWalkable()) {
                    neighbors.Add(new Vector2Int(x, y));
                }
            }
        }

        return neighbors;
    }

    private int GetDistance(Tile tileA, Tile tileB) {
        int distanceX = Mathf.Abs(tileA.position.x - tileB.position.x);
        int distanceY = Mathf.Abs(tileA.position.y - tileB.position.y);

        if (distanceX > distanceY) {
            return 14 * distanceY + 10 * (distanceX - distanceY) + tileB.cost;
        } else {
            return 14 * distanceX + 10 * (distanceY - distanceX) + tileB.cost;
        }
    }
}
