using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Water Particles")]
    public static Action<Vector3[]> OnWatersCollided;
    [Header("Speed Particle")]
    public static Action<Vector3[]> OnSeedsCollided;
    
    [Header("CropField")]
    public static Action<CropField> OnFullySown;
    public static Action<CropField> OnFullyWatered;
    public static Action<CropField> OnFullyHavert;
    
    [Header("CropTile")]
    public static Action<CropType> OnCropHavested;
    
    [Header("CropTree")]
    public static Action<AppleTree> OnTreeModeStated;
    public static Action OnTreeModeEnded;
    public static Action<AppleTree> OnEnteredTreeZone;
    public static Action<AppleTree> OnExitedTreeZone;
    
    
    
    [Header("SelectTool")]
    public static Action<Tool> OnToolSelected;
}
