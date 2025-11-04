using System;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private GameObject treeCam;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Transform appleParent;
    private AppleTreeManager _treeManager;
    
    [Header("Settings")]
    [SerializeField] private float maxShakeMagnitude;
    [SerializeField] private float shakeIncrement;
    private float shakeSliderValue;
    private float shakeMagnitude;
    private bool isShaking;

    [Header("Actions")]
    public static Action<CropType> onAppleHarvested;

    public void Initialize(AppleTreeManager appleTreeManager)
    {
        EnableTreeCam();
        shakeSliderValue = 0;
        _treeManager = appleTreeManager;
    }
    
    public void EnableTreeCam()
    {
        treeCam.SetActive(true);
    }
    
    public void DisableTreeCam()
    {
        treeCam.SetActive(false);
    }

    public void Shake()
    {
        isShaking = true;
        TweenShake(maxShakeMagnitude);

        UpdateShakeSlider();
    }

    private void UpdateShakeSlider()
    {
        shakeSliderValue += shakeIncrement;
        _treeManager.UpdateShakeSlider(shakeMagnitude);

        for (int i = 0; i < appleParent.childCount; i++)
        {
            float applePercent = (float)i / appleParent.childCount;
            Apple currentApple = appleParent.GetChild(i).gameObject.GetComponent<Apple>();

            if (shakeSliderValue > applePercent && !currentApple.IsFree()) 
                ReleaseApple(currentApple);
        }

        if (shakeSliderValue >= 1)
            ExitTreeMode();
    }

    private void ReleaseApple(Apple currentApple)
    {
        currentApple.Release();
        onAppleHarvested?.Invoke(CropType.Apple);
    }

    public void StopShaking()
    {
        if(!isShaking)
            return;
        isShaking = false;
        
        TweenShake(0);
    }

    private void TweenShake(float targetMagnitude)
    {
        LeanTween.cancel(_renderer.gameObject);
        LeanTween.value(_renderer.gameObject, UpdateShakeMagnitude, shakeMagnitude, targetMagnitude, 1);
    }

    private void UpdateShakeMagnitude(float value)
    {
        shakeMagnitude = value;
        UpdateMaterials();
    }

    private void UpdateMaterials()
    {
        foreach (var rendererMaterial in _renderer.materials)
            rendererMaterial.SetFloat("_Magnitude", shakeMagnitude);

        foreach (Transform appleTF in appleParent)
        {   
            Apple apple = appleTF.GetComponent<Apple>();
            if(apple.IsFree())
                continue;
            
            apple.Shake(shakeMagnitude);
        }
    }

    private void ExitTreeMode()
    {
        _treeManager.EndTreeMode();
        DisableTreeCam();
        TweenShake(0);
        ResetApples();
    }

    private void ResetApples()
    {
        for (int i = 0; i < appleParent.childCount; i++)
        {
            appleParent.GetChild(i).GetComponent<Apple>().Reset();
        }
    }

    public bool IsReady()
    {
        for (int i = 0; i < appleParent.childCount; i++)
        {
            if (!appleParent.GetChild(i).GetComponent<Apple>().IsReady())
                return false;
        }

        return true;
    }
}
