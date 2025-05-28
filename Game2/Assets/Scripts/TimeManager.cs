using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager TMInstance;

    [Header("Time Settings")]
    public float dayDuration = 600f;
    public float currentTime;
    public bool isDaytime = true;

    [Header("Elements")]
    public Collider2D exitDoor;
    public Room[] allRooms;

    void Awake()
    {
        if (TMInstance == null)
        {
            TMInstance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }

        ResetAllRooms(); 
    }

    void Update()
    {
        if (isDaytime && currentTime < dayDuration)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= dayDuration) SwitchToNight();
        }
    }

  
    public void ResetAllRooms()
    {
        foreach (var room in allRooms)
        {
            room.isDangerous = false; 
            room.UpdateDangerState();
        }
    }

    void SwitchToNight()
    {
        isDaytime = false;
        currentTime = 0;
        exitDoor.enabled = false;
        MarkNewDangerRoom();
    }

    void MarkNewDangerRoom()
    {
        // 获取当前未危险的房间
        Room[] safeRooms = System.Array.FindAll(allRooms, r => !r.isDangerous);
        if (safeRooms.Length > 0)
        {
            int randomIndex = Random.Range(0, safeRooms.Length);
            safeRooms[randomIndex].SetAsDangerRoom();
        }
    }

    public void CompleteNight()
    {
        isDaytime = true;
        exitDoor.enabled = true;
    }
}