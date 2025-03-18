using System.Collections.Generic;
using UnityEngine;

// Enum con las direcciones
public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3
}

public class DungeonCrawlerController : MonoBehaviour
{
    // Lista de posiciones visitadas
    public static List<Vector2Int> VisitedPositions = new List<Vector2Int>();

    // Diccionario para asociar las direcciones del enum a las posiciones
    private static readonly Dictionary<Direction, Vector2Int> DirectionMovementMap =
        new Dictionary<Direction, Vector2Int>
        {
            { Direction.up, Vector2Int.up },
            { Direction.left, Vector2Int.left },
            { Direction.down, Vector2Int.down },
            { Direction.right, Vector2Int.right }
        };
    
    // Metodo que obtiene las posiciones de los crawlers para posteriormente cargar una sala en las posiciones
    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData)
    {
        // Lista de los crawlers
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();
        
        // Agregar crawlers a la lista
        // El numero de crawlers se define en DungeonGenerarionData
        for (int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }
        
        // Elegir al azar un numero de iteraciones entre las iteraciones minimas y maximas definidas en DungeonGenerationData
        int iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);
        
        // Mover los crawlers de acuerdo al numero de iteraciones
        for (int i = 0; i < iterations; i++)
        {
            foreach (DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPosition = dungeonCrawler.Move(DirectionMovementMap);
                VisitedPositions.Add(newPosition);
            }
        }

        return VisitedPositions;
    }
    
}
