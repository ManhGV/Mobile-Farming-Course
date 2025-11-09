using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject treeModePanel;
    
    [SerializeField] private GameObject treeButton;
    [SerializeField] private GameObject toolButtonsContainer;

    private void Awake()
    {
        EventManager.OnEnteredTreeZone += EnteredTreeZoneCallBack;
        EventManager.OnExitedTreeZone += ExitTreeZoneCallBack;
        EventManager.OnTreeModeStated += SetTreeMode;
        EventManager.OnTreeModeEnded += SetGameMode;
    }

    private void OnDestroy()
    {
        EventManager.OnEnteredTreeZone -= EnteredTreeZoneCallBack;
        EventManager.OnExitedTreeZone -= ExitTreeZoneCallBack;
        EventManager.OnTreeModeStated -= SetTreeMode;
        EventManager.OnTreeModeEnded -= SetGameMode;
    }

    private void Start()
    {
        SetGameMode();
    }

    private void EnteredTreeZoneCallBack(AppleTree tree)
    {
        treeButton.SetActive(true);
        toolButtonsContainer.SetActive(false);
    } 
    
    private void ExitTreeZoneCallBack(AppleTree tree)
    {
        treeButton.SetActive(false);
        toolButtonsContainer.SetActive(true);
    }

    private void SetGameMode()
    {
        treeModePanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    
    private void SetTreeMode(AppleTree tree)
    {
        treeModePanel.SetActive(true);
        gamePanel.SetActive(false);
    }
}
