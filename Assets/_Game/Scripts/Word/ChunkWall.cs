using UnityEngine;

public class ChunkWall : MonoBehaviour
{
    [Header("Element")] 
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject leftWall;
    
    public void Configire(int configuration)
    {
        frontWall.SetActive(IsKthBitSet(configuration, 0));
        rightWall.SetActive(IsKthBitSet(configuration, 1));
        backWall.SetActive(IsKthBitSet(configuration, 2));
        leftWall.SetActive(IsKthBitSet(configuration, 3));
    }
    
    public bool IsKthBitSet(int config,int k)
    {
        if ((config & (1 << k)) > 0)
            return false;
        return true;
    }
}
