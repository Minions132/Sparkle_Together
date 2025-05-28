using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public Transform objectToMove;  
    public Transform targetPosition;
    private bool _isMoving;
    public bool _hasTriggered = false;

    public void TeleportMove()
    {
        _isMoving = true;
        objectToMove.position = targetPosition.position;
        _isMoving = false;
    }
}
