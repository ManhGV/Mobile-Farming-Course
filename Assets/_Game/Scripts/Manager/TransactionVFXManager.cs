using System;
using System.Collections;
using UnityEngine;

public class TransactionVFXManager : MonoBehaviour
{
    public static TransactionVFXManager instance;
    
    [Header("Elements")]
    [SerializeField] private ParticleSystem coinPS;
    [SerializeField] private RectTransform coinRectTransform;

    [Header("Settings")] 
    [SerializeField] private float moveSpeed;
    private int coinsAmount;
    private Camera _camera;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    [NaughtyAttributes.Button]
    public void a() => PlayCoinParticles(50);
    public void PlayCoinParticles(int amount)
    {
        if(coinPS.isPlaying)
            return;
        
        ParticleSystem.Burst brust = coinPS.emission.GetBurst(0);
        brust.count = amount;
        coinPS.emission.SetBurst(0, brust);
        
        ParticleSystem.MainModule coinMain = coinPS.main;
        coinMain.gravityModifier = 2;
        
        coinPS.Play();

        coinsAmount = amount;
        
        StartCoroutine(IEPlayCoinParticles());
    }

    private IEnumerator IEPlayCoinParticles()
    {
        yield return new WaitForSeconds(1f);
        ParticleSystem.MainModule coinMain = coinPS.main;
        coinMain.gravityModifier = 0;
        
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[coinsAmount];
        coinPS.GetParticles(particles);
        
        Vector3 dir = (coinRectTransform.position-_camera.transform.position).normalized;
        Vector3 targetPos = _camera.transform.position + dir * (Vector3.Distance(_camera.transform.position,coinPS.transform.position));
        
        while (coinPS.isPlaying)
        {
            coinPS.GetParticles(particles);
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].remainingLifetime <= 0)
                    continue;
                
                particles[i].position = Vector3.MoveTowards(particles[i].position, targetPos, moveSpeed * Time.deltaTime);
                if(Vector3.Distance(particles[i].position,targetPos) < 0.1f)
                {
                    // particles[i].remainingLifetime = 0;
                    particles[i].position = Vector3.up * 10000;
                    CashManager.instance.AddCoins(1);
                }
            }
            coinPS.SetParticles(particles);
            yield return null;
        }
    }
}
