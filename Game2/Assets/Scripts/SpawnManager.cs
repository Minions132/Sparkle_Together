using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }//����ģʽ����������ֻ��һ��SpawnManager��ȷ��SpawnManager�ڳ����л�ʱ���ᱻ����
    public GameObject playerPrefab;//���Ԥ���壬�������³�����ʵ�������
    private GameObject CurrentPlayer;//���浱ǰʾ������ң���������ActivateChaser,������Chaser�ҵ����
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
        //ȷ����ʼ�����ĳ�ʼ���������У�
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("Start");
            StartCoroutine(FindSpawnPointsAfterFrame());
        }
    }

    //OnEnable��OnDisable�������������������¼���
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
        yield return null;//�ȴ�һ֡��ȷ��������ȫ���أ������ڵ���FindObjectsOfTypeʱ�Ҳ���SpawnPoint

        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        string spawnID = SpawnData.spawnID;

        foreach (SpawnPoint point in spawnPoints)
        {
            if (point.spawnID == spawnID)
            {
                CurrentPlayer = Instantiate(playerPrefab, point.transform.position, Quaternion.identity);
                BindCameraToPlayer();//�������
                Debug.Log("�����������: " + point.spawnID);
                break;
            }
        }
        Invoke(nameof(ActivateChaser), 1f);
    }

    private void BindCameraToPlayer()
    {
        // ���ҳ����е� Virtual Camera
        VirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (VirtualCamera != null && CurrentPlayer != null)
        {
            // ���ø���Ŀ��͹۲�Ŀ��
            VirtualCamera.Follow = CurrentPlayer.transform;
            VirtualCamera.LookAt = CurrentPlayer.transform;
            Debug.Log("������Ѱ󶨵����");
        }
        else
        {
            Debug.LogError("δ�ҵ� Virtual Camera ����Ҷ���");
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
