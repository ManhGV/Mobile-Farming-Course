using System;
using UnityEngine;

public class PlayerTreeAbility : MonoBehaviour
{
    [Header("Components")] 
    private PlayerAimator _playerAimator;
    
    [Header("Settings")]
    [SerializeField] private float distanceToTree;
    [Range(0f,1f)][SerializeField] private float shakeTheshold;
    private bool isActive;
    private Vector2 previousMousePosition;
    private bool isShaking;
    
    [Header("Elements")]
    private AppleTree currentTree;

    private void Awake()
    {
        _playerAimator = GetComponent<PlayerAimator>();
        EventManager.OnTreeModeStated += TreeModeStatedCallBack;
        EventManager.OnTreeModeEnded += TreeModeEndedCallBack;
    }

    private void OnDestroy()
    {
        EventManager.OnTreeModeEnded -= TreeModeEndedCallBack;
        EventManager.OnTreeModeStated -= TreeModeStatedCallBack;
    }

    private void Update()
    {
        if (isActive && !isShaking) 
            ManagerTreeShaking();
    }

    private void TreeModeStatedCallBack(AppleTree obj)
    {
        currentTree = obj;
        isActive = true;
        MoveTowardTree();
    }

    private void TreeModeEndedCallBack()
    {
        currentTree = null;
        isActive = false;
        isShaking = false;

        LeanTween.delayedCall(.1f, () => _playerAimator.StopShakeTreeAnimation());
    } 
    
    private void MoveTowardTree()
    {
        Vector3 treePos = currentTree.transform.position;
        Vector3 dir = transform.position - treePos;
        dir.y = 0;

        Vector3 targetPos = treePos + dir.normalized * distanceToTree;
        _playerAimator.ManageAnimations(-dir);
        LeanTween.move(gameObject, targetPos, .5f);
    }

    private void ManagerTreeShaking()
    {
        if(!Input.GetMouseButton(0))
        {
            currentTree.StopShaking();
            return;
        }
        float shakeMagnitude = Vector2.Distance(Input.mousePosition, previousMousePosition);

        if (ShouldShake(shakeMagnitude))
            Shake();
        else
            currentTree.StopShaking();
        
        previousMousePosition = Input.mousePosition;
    }

    private bool ShouldShake(float shakeMagnitude)
    {
        float screenPercent = shakeMagnitude / Screen.width;
        
        return screenPercent >= shakeTheshold;
    }

    private void Shake()
    {
        isShaking = true;
        currentTree.Shake();
        
        _playerAimator.PlayShakeTreeAnimation();
        LeanTween.delayedCall(.2f, () => isShaking = false);
    }
}
