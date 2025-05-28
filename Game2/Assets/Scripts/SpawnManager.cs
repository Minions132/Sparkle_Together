using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }//单例模式，就是有且只有一个SpawnManager，确保SpawnManager在场景切换时不会被销毁
    public GameObject playerPrefab;//玩家预制体，用来在新场景中实例化玩家
    private GameObject CurrentPlayer;//保存当前示例的玩家，用来传给ActivateChaser,才能让Chaser找到玩家
    private CinemachineVirtualCamera VirtualCamera;

    private void Awake()
    {
        Debug.Log("Awake");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //确保初始场景的初始化重生进行，
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("Start");
            StartCoroutine(FindSpawnPointsAfterFrame());
        }
    }

    //OnEnable和OnDisable是用来监听场景加载事件的
    private void OnEnable()
    {
        Debug.Log("OnEnable");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindSpawnPointsAfterFrame());
    }

    private IEnumerator FindSpawnPointsAfterFrame()
    {
        yield return null;//等待一帧，确保场景完全加载，避免在调用FindObjectsOfType时找不到SpawnPoint

        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        string spawnID = SpawnData.spawnID;

        foreach (SpawnPoint point in spawnPoints)
        {
            if (point.spawnID == spawnID)
            {
                CurrentPlayer = Instantiate(playerPrefab, point.transform.position, Quaternion.identity);
                BindCameraToPlayer();//绑定摄像机
                Debug.Log("玩家已重生于: " + point.spawnID);
                break;
            }
        }
        Invoke(nameof(ActivateChaser), 1f);
    }

    private void BindCameraToPlayer()
    {
        // 查找场景中的 Virtual Camera
        VirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (VirtualCamera != null && CurrentPlayer != null)
        {
            // 设置跟随目标和观察目标
            VirtualCamera.Follow = CurrentPlayer.transform;
            VirtualCamera.LookAt = CurrentPlayer.transform;
            Debug.Log("摄像机已绑定到玩家");
        }
        else
        {
            Debug.LogError("未找到 Virtual Camera 或玩家对象！");
        }
    }

    private void ActivateChaser()
    {
        Chaser chaser = FindObjectOfType<Chaser>();
        if (chaser != null)
        {
            chaser.SetPlayer(CurrentPlayer.transform);
            chaser.StartChase();       
        }
    }
}
