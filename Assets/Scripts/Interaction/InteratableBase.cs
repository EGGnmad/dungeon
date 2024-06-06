using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteratableBase : MonoBehaviour
{
    protected bool canInteract = false;

    public abstract void Interact();
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Interact();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        
        canInteract = true;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        canInteract = false;
    }
}
