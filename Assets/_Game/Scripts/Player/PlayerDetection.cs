using System;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("ChunkTrigger"))
        {
            other.GetComponent<ChunkGround>().TryUnlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out AppleTree tree))
            TriggeredAppleTree(tree);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out AppleTree tree))
            ExitedAppleTree(tree);
    }

    private void ExitedAppleTree(AppleTree component)
    {
        EventManager.OnExitedTreeZone?.Invoke(component);
    }

    private void TriggeredAppleTree(AppleTree component)
    {
        EventManager.OnEnteredTreeZone?.Invoke(component);
    }
}
