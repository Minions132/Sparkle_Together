using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class SceneProtal : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private string targetSpawnID;

    public void Teleport()
    {
        Debug.Log("正在加载传送地点...");
        SpawnData.spawnID = targetSpawnID;
        SceneManager.LoadScene(targetScene);
    }
}
