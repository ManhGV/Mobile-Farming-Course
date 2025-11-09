using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolSeletor : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image[] toolImages;
    
    [Header("Settings")]
    [SerializeField] private Color selectToolColor;

    private Tool activeTool;

    private void Start()
    {
        SelectTool(0);
    }

    public void SelectTool(int indexTool)
    {
        activeTool = (Tool)indexTool;
        for (int i = 0; i < toolImages.Length; i++)
            toolImages[i].color = i == indexTool ? selectToolColor : Color.white;

        EventManager.OnToolSelected?.Invoke(activeTool);
    }

    public bool CanSow() => activeTool == Tool.Sow;
    public bool CanWater() => activeTool == Tool.Water;
    public bool CanHarvest() => activeTool == Tool.Harvest;
}
