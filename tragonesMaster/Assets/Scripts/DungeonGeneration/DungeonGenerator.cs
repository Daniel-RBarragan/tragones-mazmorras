using System;
using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    
    // Lista con las posiciones de las salas a generar
    private List<Vector2Int> _dungeonRooms;

    private void Start()
    {
        // Se generan las salas
        _dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        
        // Se cargan las salas
        LoadRooms(_dungeonRooms);
        
    }
    
    // Metodo para cargar las salas
    private void LoadRooms(IEnumerable<Vector2Int> rooms)
    {
        // Se carga la primera sala
        RoomController.Instance.LoadRoom("Start", 0, 0);
        
        // Colocar salas en cada posicion dada por los crawlers
        foreach (Vector2Int roomLocation in rooms)
        {
            RoomController.Instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);
            
            /*
            // Checar para agregar sala de jefe o sala especial
            // Agregar sala especial en el ultimo lugar visitado
            // Se puede cambiar la ubicacion en donde aparece esta sala
            if (roomLocation == _dungeonRooms[^1] && !(roomLocation == Vector2Int.zero))
            {
                RoomController.Instance.LoadRoom("End", roomLocation.x, roomLocation.y);
            }
            else
            {
                
            }
            */
        }
    }
}
