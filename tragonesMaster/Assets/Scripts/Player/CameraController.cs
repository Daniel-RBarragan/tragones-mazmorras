using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Singleton de referencia
    public static CameraController Instance;

    // Referencia a la sala actual
    public Room currentRoom;
    
    // Velocidad de cambio de la camara
    public float changeSpeed;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizar posicion de la camara
        UpdateCameraPosition();
    }
    
    // Metodo para actualizar la posicion de la camara
    void UpdateCameraPosition()
    {
        // Dejar de ejecutar si la sala no existe
        if (!currentRoom)
        {
            return;
        }
        
        // Vector con posicion objetivo de la camara
        Vector3 targetPosition = GetCameraTargetPosition();
        
        // Cambiar la posicion de la camara
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, changeSpeed * Time.deltaTime);
    }
    
    // Metodo para obtener la posicion objetivo de la camara
    Vector3 GetCameraTargetPosition()
    {
        // Dejar de ejecutar si la sala no existe
        if (!currentRoom)
        {
            return Vector3.zero;
        }
        
        Vector3 targetPosition = currentRoom.GetRoomCenter();
        targetPosition.z = transform.position.z;
        return targetPosition;
    }
    
    // Metodo para checar si se esta cambiando de sala
    public bool IsSwitchingRoom()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
