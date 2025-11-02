using System;
using UnityEngine;

public class PlayerCropInteractor : MonoBehaviour
{
    [Header("Element")] 
    [SerializeField] private Material[] _materials;


    private void Update()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetVector("_PlayerPosition", transform.position);
        }
    }
}
