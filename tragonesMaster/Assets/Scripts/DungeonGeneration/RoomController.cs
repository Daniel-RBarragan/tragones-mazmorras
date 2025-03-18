using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Datos de la sala en relacion a un grid
public class RoomInfo
{
    // Nombre de la sala
    public string Name;
    
    // Coordenadas de la sala
    public int X;
    public int Y;
}
public class RoomController : MonoBehaviour
{
    // Singleton de referencia
    public static RoomController Instance;
    
    // Nombre del área actual
    public string currentAreaName = "Basement";
    
    // Datos de la sala actualmente cargada
    public RoomInfo CurrentRoomInfo;
    
    // Sala actualmente cargada
    public Room currentRoom;
    
    // Cola con los datos de las salas por cargar
    private Queue<RoomInfo> RoomsToLoad = new Queue<RoomInfo>();
    
    // Lista con las salas ya cargadas
    public List<Room> loadedRooms = new List<Room>();
    
    // Bandera para saber si una sala está cargando
    public bool isLoadingRoom = false;
    
    // Bandera para saber si ya se cargó la sala del jefe / sala especial
    public bool loadedBossRoom = false;
    
    // Bandera para saber si ya se terminó de actualizar la sala
    public bool updatedRooms = false;
    
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Checar la cola
        UpdateRoomQueue();
    }
    
    // Metodo para checar si una sala existe
    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    }
    
    // Metodo para encontrar una sala
    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y);
    }
    
    // Metodo para actualizar elementos cuando el jugador entra a una sala
    public void OnPlayerEntry(Room room)
    {
        CameraController.Instance.currentRoom = room;
        currentRoom = room;
    }
    
    // Metodo para agregar la informacion de una sala a la cola de salas por cargar
    public void LoadRoom(string roomName, int x, int y)
    {
        // Checar si existe la sala
        if (DoesRoomExist(x, y))
        {
            return;  
        }
        
        // Inicializar variable de información de sala
        RoomInfo roomInfo = new RoomInfo();
        roomInfo.Name = roomName;
        roomInfo.X = x;
        roomInfo.Y = y;
        
        // Poner sala en la cola
        RoomsToLoad.Enqueue(roomInfo);
    }
    
    // Metodo para actualizar el estado de la cola
    public void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (RoomsToLoad.Count == 0)
        {
            // Cargar sala de jefe / sala especial cuando no haya nada en la cola
            if (!loadedBossRoom)
            {
                StartCoroutine(LoadBossRoom());
            } else if (loadedBossRoom && !updatedRooms)
            {
                // Eliminar las puertas innecesarias en cada sala
                foreach (var room in loadedRooms)
                {
                    room.RemoveIsolatedDoors();
                }
                updatedRooms = true;
            }
            return;
        }
        
        CurrentRoomInfo = RoomsToLoad.Dequeue();
        isLoadingRoom = true;
        
        StartCoroutine(LoadRooms(CurrentRoomInfo));
    }
    
    // Metodo para agregar sala a la lista de salas ya cargadas
    public void RegisterRoom(Room room)
    {
        // Checar si la sala ya existe para evitar duplicados
        if (!DoesRoomExist(CurrentRoomInfo.X, CurrentRoomInfo.Y))
        {
            // Cambiar la posicion de la sala que se acaba de cargar
            // La posicion se calcula multiplicando las coordenadas de la sala actual por el offset definido
            room.transform.position = new Vector3(CurrentRoomInfo.X * room.width, CurrentRoomInfo.Y * room.height, 0);
        
            // Poner valores de la variable
            room.x = CurrentRoomInfo.X;
            room.y = CurrentRoomInfo.Y;
            room.name = currentAreaName + "-" + CurrentRoomInfo.Name + " " +  room.x + ", " + room.y;
            room.transform.parent = transform;
        
            isLoadingRoom = false;
        
            // Poner la camara en la sala actual si no se han cargado mas salas
            if (loadedRooms.Count == 0)
            {
                CameraController.Instance.currentRoom = room;
            }
        
            // Agregar sala con informacion a la lista
            loadedRooms.Add(room);
            
        }
        else
        {
            // Si la sala ya existe, eliminarla e indicar que no se esta cargando nada
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }
    
    // Corutina para cargar las salas
    IEnumerator LoadRooms(RoomInfo roomInfo)
    {
        // Nombre de la sala por cargar
        string roomName = currentAreaName + roomInfo.Name;
        
        // Cargar la sala de manera asincrona
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        
        // Retornar nada para la corutina mientras la sala no se ha cargado
        while (loadRoom is { isDone: false })
        {
            yield return null;
        }
        
    }
    
    // Corutina para cargar la sala del jefe / sala especial
    IEnumerator LoadBossRoom()
    {
        loadedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (RoomsToLoad.Count == 0)
        {
            // Poner sala de jefe / sala especial en la última posicion
            Room bossRoom = loadedRooms[^1];
            
            // Guardar posicion de la sala del jefe / sala especial
            Vector2Int tempRoom = new Vector2Int(bossRoom.x, bossRoom.y);
            
            // Destruir el objeto de sala de jefe
            Destroy(bossRoom.gameObject);
            
            // Sala que se va a quitar de la lista
            var roomToRemove = loadedRooms.Single(r => r.x == tempRoom.x && r.y == tempRoom.y);
            
            // Eliminar sala de la lista
            loadedRooms.Remove(roomToRemove);
            
            // Cargar sala del jefe
            LoadRoom("End",  tempRoom.x, tempRoom.y);
            
        }
    }
}

