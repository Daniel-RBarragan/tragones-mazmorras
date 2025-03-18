using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    // Offset entre cada sala
    public int width = 19;
    public int height = 11;
    
    // Coordenadas de la sala
    public int x;
    public int y;
    
    // Nombre de la sala
    public string name;
    
    // Variables para las puertas de la sala
    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;
    
    // Lista de las puertas de la sala
    public List<Door> doorsList = new List<Door>();
    
    // Bandera para checar si se han actualizado las puertas
    private bool _updatedDoors = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Checar si se comienza en la escena correcta
        if (RoomController.Instance == null)
        {
            Debug.LogError("Se empezo en la escena incorrecta");
            return;
        }
        
        // Arreglo de puertas en la sala
        Door[] doorsArray = GetComponentsInChildren<Door>();
        
        // Asignar tipo de puerta a cada puerta en la sala
        foreach (Door door in doorsArray)
        {
            // Agregar cada puerta a la lista
            doorsList.Add(door);
            
            switch (door.doorType)
            {
                case Door.DoorType.left:
                    leftDoor = door;
                    break;
                case Door.DoorType.right:
                    rightDoor = door;
                    break;
                case Door.DoorType.top:
                    topDoor = door;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = door;
                    break;
            }
        }
        
        // Registrar la sala cargada en la lista
        RoomController.Instance.RegisterRoom(this);

    }

    private void Update()
    {
        // Checar si es la sala del jefe / sala especial para actualizar puertas
        if (name.Contains("End") && !_updatedDoors)
        {
            RemoveIsolatedDoors();
            _updatedDoors = true;
        }
    }

    // Gizmos para alinear las salas
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
    
    // Metodo para obtener las coordenadas al centro de la sala
    public Vector3 GetRoomCenter()
    {
        return new Vector3(x * width,  y * height);
    }
    
    // Metodo para eliminar las puertas sin conexion a otra sala
    public void RemoveIsolatedDoors()
    {
        // Iterar por la lista de las puertas
        foreach (Door door in doorsList)
        {
            // Desactivar las puertas si no hay una sala en una direccion
            switch (door.doorType)
            {
                case Door.DoorType.left:
                    if (GetLeftRoom() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                
                case Door.DoorType.right:
                    if (GetRightRoom() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                
                case Door.DoorType.top:
                    if (GetTopRoom() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
                
                case Door.DoorType.bottom:
                    if (GetBottomRoom() == null)
                    {
                        door.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
    
    // Metodos para checar si hay una sala en una direccion dada
    public Room GetLeftRoom()
    {
        if (RoomController.Instance.DoesRoomExist(x - 1, y))
        {
            return RoomController.Instance.FindRoom(x - 1, y);
        }
        return null;
    }
    
    public Room GetRightRoom()
    {
        if (RoomController.Instance.DoesRoomExist(x + 1, y))
        {
            return RoomController.Instance.FindRoom(x + 1, y);
        }
        return null;
    }
    
    public Room GetTopRoom()
    {
        if (RoomController.Instance.DoesRoomExist(x, y + 1))
        {
            return RoomController.Instance.FindRoom(x, y + 1);
        }
        return null;
    }
    
    public Room GetBottomRoom()
    {
        if (RoomController.Instance.DoesRoomExist(x, y - 1))
        {
            return RoomController.Instance.FindRoom(x, y - 1);
        }
        return null;
    }
    
    // Metodo para detectar si el jugador entro a la sala
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collision");
            RoomController.Instance.OnPlayerEntry(this);
        }
    }
}
