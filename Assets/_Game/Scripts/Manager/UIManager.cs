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
        PlayerDetection.onEnteredTreeZone += EnteredTreeZoneCallBack;
        PlayerDetection.onExitedTreeZone += ExitTreeZoneCallBack;
        AppleTreeManager.onTreeModeStated += SetTreeMode;
        AppleTreeManager.onTreeModeEnded += SetGameMode;
    }

    private void OnDestroy()
    {
        PlayerDetection.onEnteredTreeZone -= EnteredTreeZoneCallBack;
        PlayerDetection.onExitedTreeZone -= ExitTreeZoneCallBack;
        AppleTreeManager.onTreeModeStated -= SetTreeMode;
        AppleTreeManager.onTreeModeEnded -= SetGameMode;
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
