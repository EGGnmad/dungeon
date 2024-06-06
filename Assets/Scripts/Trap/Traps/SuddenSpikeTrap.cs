using UnityEngine;

public class SuddenSpikeTrap : TrapBase
{
    [SerializeField] private float y;
    [SerializeField] private Vector3 force;
    [SerializeField] private string dieMessage;
    [SerializeField] private AudioSource sound;
    private bool _isToggled = false;
    
    public override void Trigger()
    {
        if (_isToggled) return;
        _isToggled = true;
        

        sound?.Play();
        transform.position += transform.up*y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<PlayerAgent>()?.Die(dieMessage, force);
        }
    }
}
