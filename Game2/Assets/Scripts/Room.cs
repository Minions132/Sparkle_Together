using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Settings")]
    public Transform doorPosition;
    [SerializeField] private GameObject dangerEffect;
    [SerializeField] private GameObject safeEffect;


    [Header("State")]
    public bool isDangerous; // ��Ϊ��ʱ״̬

    void Start()
    {
        UpdateDangerState();
    }

    public void SetAsDangerRoom()
    {
        isDangerous = true;
        UpdateDangerState();
    }

    public void UpdateDangerState()
    {
        SetEffectState(dangerEffect, isDangerous);
        SetEffectState(safeEffect, !isDangerous);
    }

    private void SetEffectState(GameObject effect, bool activeState)
    {
        if (effect != null)
        {
            effect.SetActive(activeState);
        }
    }

    //System.Collections.IEnumerator NightSequence()
    //{
        // ���������߼�
        //yield return new WaitForSeconds(1.5f);
        //TimeManager.TMInstance.CompleteNight();
    //}
}

