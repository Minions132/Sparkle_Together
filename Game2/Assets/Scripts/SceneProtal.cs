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
        Debug.Log("���ڼ��ش��͵ص�...");
        SpawnData.spawnID = targetSpawnID;
        SceneManager.LoadScene(targetScene);
    }
}
