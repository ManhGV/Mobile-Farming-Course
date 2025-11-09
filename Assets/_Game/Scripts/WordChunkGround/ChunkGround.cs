using System;
using TMPro;
using UnityEngine;

public class ChunkGround : MonoBehaviour
{
    [Header("DATA - SO")]
    [SerializeField] private SOChunkGround _chunkGroundData;
    
    [Header("Lock - Unlock")]
    [SerializeField] private MeshRenderer _chunkGroundRenderer;
    [SerializeField] private GameObject _unlockedElements;
    [SerializeField] private GameObject _lockedElements;
    
    [Header("Element")]
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private BoxCollider boxTrigger;
    [SerializeField] private MeshFilter chunkFilter;
    private ChunkWall chunkWall;

    [Header("Setting")]
    [SerializeField] private int initialPrice; 
    private int currentPrice;
    private bool unlocked = false;
    private int congiguration;

    [Header("Actions")]
    public static Action onUnlocked;
    public static Action onPriceChanged;

    private void Awake()
    {
        chunkWall = GetComponent<ChunkWall>();
    }

    private void Start()
    {
        currentPrice = initialPrice;
        priceText.text = currentPrice.ToString();
    }

    public void Initialize(int wordDataChunkPrice)
    {
        
        currentPrice = wordDataChunkPrice;
        priceText.text = currentPrice.ToString();
        
        if(wordDataChunkPrice <= 0)
            Unlock(false);
    }
    
    public void TryUnlock()
    {
        if(CashManager.instance.GetCoint()<=0)
            return;
        currentPrice--;
        CashManager.instance.UseCoins(1);
        onUnlocked?.Invoke();
        
        priceText.text = currentPrice.ToString();
        if (currentPrice <= 0)
            Unlock();
    }

    private void Unlock(bool CanSave = true)
    {
        _unlockedElements.SetActive(true);
        _lockedElements.SetActive(false);
        boxTrigger.enabled = false;
        unlocked = true;
        if(CanSave) 
            onUnlocked?.Invoke();
    }

    public void UpdateWall(int configuration)
    {
        this.congiguration = configuration;
        chunkWall.Configire(configuration);
    }

    public void DisplayLockedElements()
    {
        _lockedElements.SetActive(true);
    }

    public void SetRenderer(Mesh chunkShape)
    {
        chunkFilter.mesh = chunkShape;
    }

    public bool IsUnlocked() => unlocked;
    
    public int GetCurrentPrice() => currentPrice;
    
    public int GetInitialPrice() => initialPrice;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5);
        
        Gizmos.color = new Color(0,0,0,0);
        Gizmos.DrawCube(transform.position, Vector3.one * 5);
    }
}
