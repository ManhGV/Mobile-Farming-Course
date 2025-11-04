using System;
using UnityEngine;
using UnityEngine.UI;

public class AppleTreeManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider shakeSlider;
    
    [Header("Settings")] 
    private AppleTree lastTriggeredTree;
    
    [Header("Actions")]
    public static Action<AppleTree> onTreeModeStated;
    public static Action onTreeModeEnded;

    private void Awake()
    {
        PlayerDetection.onEnteredTreeZone += EnteredTreeZoneCallBack;
    }

    private void OnDestroy()
    {
        PlayerDetection.onEnteredTreeZone += EnteredTreeZoneCallBack;
    }

    private void EnteredTreeZoneCallBack(AppleTree obj)
    {
        lastTriggeredTree = obj;
    }

    public void TreeButtonClick()
    {
        if(lastTriggeredTree.IsReady())
            StartTreeMode();
    }

    private void StartTreeMode()
    {
        lastTriggeredTree.Initialize(this);
        onTreeModeStated?.Invoke(lastTriggeredTree);
        UpdateShakeSlider(0);
    }

    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }

    public void EndTreeMode()
    {
        onTreeModeEnded?.Invoke();
    }
}
