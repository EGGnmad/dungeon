using UnityEngine;

public class PhysicsTrap : TrapBase
{
    [SerializeField] private float minAttackSpeed;
    [SerializeField] private string dieMessage;
    
    private Rigidbody _rigid;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public override void Trigger()
    {
        _rigid.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && _rigid.velocity.magnitude > minAttackSpeed)
        {
            Vector3 dir = (collision.gameObject.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<PlayerAgent>()?.Die(dieMessage, dir);
        }
    }
}