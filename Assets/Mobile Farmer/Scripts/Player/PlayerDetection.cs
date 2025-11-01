using System;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("ChunkTrigger"))
        {
            other.GetComponent<Chunk>().TryUnlock();
        }
    }
}
