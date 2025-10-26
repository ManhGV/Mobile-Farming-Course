using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Transform cropRenderer;
    [SerializeField] private ParticleSystem harvestedParticle;
    public void ScaleUp()
    {
        cropRenderer.gameObject.LeanScale(Vector3.one, 1).setEase(LeanTweenType.easeOutBack);
    }
    
    public void ScaleDown()
    {
        cropRenderer.gameObject.LeanScale(Vector3.zero, 1).setEase(LeanTweenType.easeOutBack).setOnComplete(OnDespawn);
        harvestedParticle.transform.parent = null;
        harvestedParticle.gameObject.SetActive(true);
    }

    private void OnDespawn()
    {
        Destroy(gameObject);
    }
}
