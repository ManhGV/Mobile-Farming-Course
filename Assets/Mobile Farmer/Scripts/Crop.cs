using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Transform cropRenderer;
    public void ScaleUp()
    {
        cropRenderer.localScale = Vector3.one;
    }
}
