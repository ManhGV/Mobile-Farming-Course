#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Chunk))]
public class ChunkEditor : Editor
{
    private void OnSceneGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        
        Chunk chunk = (Chunk)target;
        Handles.Label(chunk.transform.position, chunk.name, style);
    }
}
#endif