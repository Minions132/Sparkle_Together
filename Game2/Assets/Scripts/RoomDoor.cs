using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomDoor : MonoBehaviour
{
    [SerializeField] private string RoomID;
    [SerializeField] private string targetSpawnID = "SpawnPoint_Room";

    public void Event()
    {
        Debug.Log("���ڽ��뷿��...");
        SpawnData.spawnID = targetSpawnID;
        SceneManager.LoadScene(RoomID);
    }
}
