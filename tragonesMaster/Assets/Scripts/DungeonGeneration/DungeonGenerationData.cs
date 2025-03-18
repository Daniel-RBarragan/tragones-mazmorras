using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DungeonGenerationData/Dungeon Data")]

public class DungeonGenerationData : ScriptableObject
{
    // Numero de crawlers
    public int numberOfCrawlers;
    
    // Minimo y maximo de iteraciones
    public int iterationMin;
    public int iterationMax;
}
