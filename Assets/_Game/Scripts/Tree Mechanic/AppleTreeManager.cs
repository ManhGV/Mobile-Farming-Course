using System;
using UnityEngine;
using UnityEngine.UI;

public class AppleTreeManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider shakeSlider;
    
    [Header("Settings")] 
    private AppleTree lastTriggeredTree;

    private void Awake()
    {
        EventManager.OnEnteredTreeZone += EnteredTreeZoneCallBack;
    }

    private void OnDestroy()
    {
        EventManager.OnEnteredTreeZone += EnteredTreeZoneCallBack;
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
        EventManager.OnTreeModeStated?.Invoke(lastTriggeredTree);
        UpdateShakeSlider(0);
    }

    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }

    public void EndTreeMode()
    {
        EventManager.OnTreeModeEnded?.Invoke();
    }
}
