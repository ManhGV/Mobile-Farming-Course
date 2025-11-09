using UnityEngine;

public class SOChunkGround : ScriptableObject
{
    [Header("Referends")]
    [SerializeField] private bool _isLocked;
    [SerializeField] private int _chungGroundPrice;
    [Tooltip("Phần trăm giaảm thời gian thu hoạch")][SerializeField] private float _timeHarvestReducePercent;
    
    [Header("Water")]
    [Tooltip("Thời gian nước tồn tại")] [SerializeField] private float _timeWaterExist;
    [Tooltip("Thời gian còn lại của tưới nước")][SerializeField] private float _currentTimeWaterExist;
    
    [Header("Material Chunk Ground")]
    [Tooltip("Material vùng trồng cây")][SerializeField] private Material _isMainChunkGround;
    [Tooltip("Material xung quanh vùng trồng cây")][SerializeField] private Material _isBolderChunkGround;
    
    #region GET - SET

    public bool IsLocked { get => _isLocked; set => _isLocked = value; }
    public int ChungGroundPrice { get => _chungGroundPrice; set => _chungGroundPrice = value; }
    public float TimeWaterExist { get => _timeWaterExist; set => _timeWaterExist = value; }
    public float CurrentTimeWaterExist { get => _currentTimeWaterExist; set => _currentTimeWaterExist = value; }
    public float TimeHarvestReducePercent { get => _timeHarvestReducePercent; set => _timeHarvestReducePercent = value; }
    
    #endregion
    
}