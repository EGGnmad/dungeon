using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpikeTrap : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private AnimationCurve curve;
    
    private float _dt = 0f;
    private bool isUp = true;

    private void Update()
    {
        if (isUp) _dt += Time.deltaTime;
        else _dt -= Time.deltaTime;

        if (_dt >= 1f) isUp = false;
        else if (_dt <= 0f) isUp = true;

        float y = curve.Evaluate(_dt);
        Vector3 pos = transform.position;
        pos.y = y;
        
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<PlayerAgent>()?.Die("한박자 쉬고 두박자 쉬고 세박자마저 쉬면 안되죠~");
        }
    }
}
