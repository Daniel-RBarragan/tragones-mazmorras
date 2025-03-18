using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{
    // Vector con la posicion del crawler
    public Vector2Int crawlerPosition { get; set; }
    
    // Constructor que toma como argumento un Vector2Int
    // De esta manera se pueden inicializar en la posicion 0,0 del mapa
    public DungeonCrawler(Vector2Int startPosition)
    {
        crawlerPosition = startPosition;
    }
    
    // Metodo para mover el crawler
    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        Direction moveTo = (Direction)Random.Range(0, directionMovementMap.Count);
        crawlerPosition += directionMovementMap[moveTo];
        return crawlerPosition;
    }
}
