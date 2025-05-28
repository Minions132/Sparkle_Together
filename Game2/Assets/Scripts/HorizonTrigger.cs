using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody2D horizonObject;
    [SerializeField] private float horizontalForce = -50.0f;

    private bool hasMoved = false;

    public void ActivateTrap()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            if (horizonObject == null) return;

            Vector2 velocity = new Vector2(horizontalForce, 0f);
            horizonObject.velocity = velocity;
        }
    }
}
