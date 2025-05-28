using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTrigger : MonoBehaviour
{
    public Transform objectToMove;
    public Transform targetPosition;
    public float moveDuration = 2f;
    private bool _isMoving;
    public bool _hasTriggered = false;
    
    public void StartMove()
    {
        if (_isMoving) return; 
        _hasTriggered = true;
        StartCoroutine(SmoothMove());
    }

    IEnumerator SmoothMove()
    {
        _isMoving = true;
        Vector2 startPos = objectToMove.position;
        Vector2 targetPos = targetPosition.position;
        float elapsed = 0;

        while (elapsed < moveDuration)
        {
            objectToMove.position = Vector2.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        objectToMove.position = targetPos;
        _isMoving = false;
    }
}
