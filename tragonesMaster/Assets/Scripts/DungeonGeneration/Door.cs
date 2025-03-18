using UnityEngine;

public class Door : MonoBehaviour
{
    // Enum para saber el tipo de puerta
    public enum DoorType
    {
        left,
        right,
        top,
        bottom
    }
    
    // Instancia del enum
    public DoorType doorType;
    
}
