using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WaterParticles : MonoBehaviour
{
    public static Action<Vector3[]> onWatersCollided;
    
    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int collisionAmount =  ps.GetCollisionEvents(other, collisionEvents);
        
        Vector3[] collisionPositions = new Vector3[collisionAmount];
        for (int i = 0; i < collisionAmount; i++)
            collisionPositions[i] = collisionEvents[i].intersection;
        
        onWatersCollided?.Invoke(collisionPositions);
    }
}