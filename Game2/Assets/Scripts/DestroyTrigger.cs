using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    
    public void DestroyObject()
    {
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
        else
        {
            Debug.LogWarning("Object to destroy is not assigned.");
        }
    }
}
