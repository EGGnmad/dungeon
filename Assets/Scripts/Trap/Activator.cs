using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Activator : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            onTriggerEnter.Invoke();
        }
    }
}