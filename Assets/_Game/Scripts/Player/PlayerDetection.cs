using System;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [Header("Actions")] [CanBeNull]
    public static Action<AppleTree> onEnteredTreeZone;
    public static Action<AppleTree> onExitedTreeZone;
    
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("ChunkTrigger"))
        {
            other.GetComponent<Chunk>().TryUnlock();
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
        onExitedTreeZone?.Invoke(component);
    }

    private void TriggeredAppleTree(AppleTree component)
    {
        onEnteredTreeZone?.Invoke(component);
    }
}
