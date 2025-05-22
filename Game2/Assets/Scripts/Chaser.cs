using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 10f;
    [SerializeField] private float chaseStartDelay = 1f;

    private Transform player;

    private bool isChasing = false;

    public void StartChase()
    {
        isChasing = true;
        Invoke(nameof(DelayedStart), chaseStartDelay);
        Debug.Log("开始追逐玩家");
    }

    private void DelayedStart()
    {
        isChasing = true;
    }

    public void SetPlayer(Transform newPlayer)
    {   
        player = newPlayer;
    }

    void Update()
    {
        if (!isChasing || player == null) return;
        Debug.Log("正在追逐玩家");
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * chaseSpeed * Time.deltaTime);
    }
}
