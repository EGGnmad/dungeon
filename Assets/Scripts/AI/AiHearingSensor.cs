using System.Linq;
using UnityEngine;

public class AiHearingSensor : MonoBehaviour, IAiSensor
{
    [Header("config")]
    [SerializeField] private float maxHearingRadius = 3f;
    [SerializeField] private LayerMask occlusionLayers;

    [Header("color")] 
    [SerializeField] private Color color;
    
    public event IAiSensor.AlertSensor onSensor;

    private void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, maxHearingRadius, occlusionLayers);
        Collider so = colliders.OrderBy(so => Vector3.Distance(transform.position, so.transform.position)).FirstOrDefault();

        if (so)
        {
            onSensor?.Invoke(so.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, maxHearingRadius);
    }
}
