using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Apple : MonoBehaviour
{
    enum State
    {
        Ready,
        Growing
    }

    private State state;
    
    [Header("Elements")]
    [SerializeField] private Renderer renderer;
    private Rigidbody rig;

    [Header("Settings")]
    [SerializeField] private float shakeMultyploer;
    private Vector3 initPos;
    private Quaternion initRot;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        
        initPos = transform.position;
        initRot = transform.rotation;
    }

    private void Start()
    {
        state = State.Ready;
    }

    public void Shake(float shakeMagnitude)
    {
        float realShakeMagnitude = shakeMagnitude * shakeMultyploer;
        renderer.material.SetFloat("_Magnitude", realShakeMagnitude);
    }

    public bool IsFree()
    {
        return !rig.isKinematic;
    }

    public void Release()
    {
        rig.isKinematic = false;
        state = State.Growing;
        renderer.material.SetFloat("_Magnitude", 0);
    }

    public void Reset()
    {
        LeanTween.scale(gameObject, Vector3.zero, 1).setDelay(2).setOnComplete(ForceReset);
    }

    public bool IsReady() => state == State.Ready;
    
    private void ForceReset()
    {
        transform.position = initPos;
        transform.rotation = initRot;
        
        rig.isKinematic = true;
        float randomScaleTime = Random.Range(2f,5f);
        LeanTween.scale(gameObject, Vector3.one, randomScaleTime).setOnComplete(SetReady);
    }

    private void SetReady()
    {
        state = State.Ready;
    }
}
