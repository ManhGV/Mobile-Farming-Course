#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChunkGround))]
public class ChunkEditor : Editor
{
    private void OnSceneGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        
        ChunkGround chunkGround = (ChunkGround)target;
        Handles.Label(chunkGround.transform.position, chunkGround.name, style);
    }
}
#endif