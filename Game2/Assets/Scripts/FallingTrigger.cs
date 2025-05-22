using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody2D fallingObject; 

    private bool hasFallen = false;   

    public void ActivateTrap()
    {
        if (!hasFallen)
        {
            hasFallen = true;
            fallingObject.gravityScale = 2;
        }
    }
}
